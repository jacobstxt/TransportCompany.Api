using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportCompany.Api.Data;
using TransportCompany.Api.DTOs.Drivers;
using TransportCompany.Api.Entities;

namespace TransportCompany.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController(TransportDbContext db) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverDto>>> GetAll(CancellationToken ct)
        {
            var drivers = await db.Drivers
                .AsNoTracking()
                .OrderBy(d => d.Id)
                .Select(DriverMappings.ToDto)
                .ToListAsync(ct);

            return Ok(drivers);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DriverDto>> GetById(int id, CancellationToken ct)
        {
            var driver = await db.Drivers           
                .AsNoTracking()
                .Where(d => d.Id == id)
                .Select(DriverMappings.ToDto)
                .FirstOrDefaultAsync(ct);

            return driver is null ? NotFound() : Ok(driver);
        }

        [HttpPost]
        public async Task<ActionResult<DriverDto>> Create(CreateDriverDto dto, CancellationToken ct)
        {
            if (await db.Drivers.AnyAsync(d => d.LicenseNumber == dto.LicenseNumber, ct))
                return Conflict($"Водій з посвідченням {dto.LicenseNumber} вже існує.");

            var driver = new Driver { FirstName = dto.FirstName, LastName = dto.LastName, LicenseNumber = dto.LicenseNumber, Phone = dto.Phone, HiredAt = dto.HiredAt.ToUniversalTime() };

            db.Drivers.Add(driver);
            await db.SaveChangesAsync(ct);

            var result = DriverMappings.ToDtoFunc(driver);
            return CreatedAtAction(nameof(GetById), new { id = driver.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateDriverDto dto, CancellationToken ct)
        {
            var driver = await db.Drivers.FindAsync([id], ct);
            if (driver is null) return NotFound();

            if (await db.Drivers.AnyAsync(d => d.LicenseNumber == dto.LicenseNumber && d.Id != id, ct))
                return Conflict($"Водій з посвідченням {dto.LicenseNumber} вже існує.");

            driver.FirstName = dto.FirstName;
            driver.LastName = dto.LastName;
            driver.LicenseNumber = dto.LicenseNumber;
            driver.Phone = dto.Phone;
            driver.HiredAt = dto.HiredAt.ToUniversalTime();

            await db.SaveChangesAsync(ct);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var driver = await db.Drivers.FindAsync([id], ct);
            if (driver is null) return NotFound();

            if (await db.TripDetails.AnyAsync(t => t.DriverId == id, ct))
                return Conflict("Неможливо видалити водія, який має поїздки.");

            db.Drivers.Remove(driver);
            await db.SaveChangesAsync(ct);
            return NoContent();
        }

        // GET /api/drivers/5/vehicles: до яких авто допущений водій
        [HttpGet("{driverId:int}/vehicles")]
        public async Task<ActionResult<IEnumerable<DriverVehicleDto>>> GetVehicles(int driverId, CancellationToken ct)
        {
            if (!await db.Drivers.AnyAsync(d => d.Id == driverId, ct))
                return NotFound();

            var vehicles = await db.DriverVehicles
                .AsNoTracking()
                .Where(dv => dv.DriverId == driverId)
                .OrderBy(dv => dv.AssignedAt)
                .Select(dv => new DriverVehicleDto(
                    dv.VehicleId, dv.Vehicle.PlateNumber, dv.Vehicle.Brand, dv.Vehicle.Model, dv.AssignedAt))
                .ToListAsync(ct);

            return Ok(vehicles);
        }

        // POST /api/drivers/5/vehicles/2: допустити водія до авто
        [HttpPost("{driverId:int}/vehicles/{vehicleId:int}")]
        public async Task<IActionResult> AssignVehicle(int driverId, int vehicleId, CancellationToken ct)
        {
            if (!await db.Drivers.AnyAsync(d => d.Id == driverId, ct))
                return NotFound($"Водія {driverId} не знайдено.");
            if (!await db.Vehicles.AnyAsync(v => v.Id == vehicleId, ct))
                return NotFound($"Авто {vehicleId} не знайдено.");
            if (await db.DriverVehicles.AnyAsync(dv => dv.DriverId == driverId && dv.VehicleId == vehicleId, ct))
                return Conflict("Водій уже допущений до цього авто.");

            db.DriverVehicles.Add(new DriverVehicle { DriverId = driverId, VehicleId = vehicleId });
            await db.SaveChangesAsync(ct);
            return NoContent();
        }

        // DELETE /api/drivers/5/vehicles/2: зняти допуск
        [HttpDelete("{driverId:int}/vehicles/{vehicleId:int}")]
        public async Task<IActionResult> UnassignVehicle(int driverId, int vehicleId, CancellationToken ct)
        {
            var link = await db.DriverVehicles.FindAsync([driverId, vehicleId], ct);
            if (link is null) return NotFound();

            db.DriverVehicles.Remove(link);
            await db.SaveChangesAsync(ct);
            return NoContent();
        }
    }
}
