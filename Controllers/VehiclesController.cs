using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportCompany.Api.Data;
using TransportCompany.Api.DTOs.Vehicles;
using TransportCompany.Api.Entities;
using TransportCompany.Api.Entities.Enums;

namespace TransportCompany.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController(TransportDbContext db) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetAll(CancellationToken ct)
        {
            var vehicles = await db.Vehicles
                .AsNoTracking()
                .OrderBy(v => v.Id)
                .Select(VehicleMappings.ToDto)
                .ToListAsync(ct);

            return Ok(vehicles);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<VehicleDto>> GetById(int id, CancellationToken ct)
        {
            var vehicle = await db.Vehicles
                .AsNoTracking()
                .Where(v => v.Id == id)
                .Select(VehicleMappings.ToDto)
                .FirstOrDefaultAsync(ct);

            return vehicle is null ? NotFound() : Ok(vehicle);
        }

        [HttpPost]
        public async Task<ActionResult<VehicleDto>> Create(CreateVehicleDto dto, CancellationToken ct)
        {
            if (await db.Vehicles.AnyAsync(v => v.PlateNumber == dto.PlateNumber, ct))
                return Conflict($"Авто з номером {dto.PlateNumber} вже існує.");

            var vehicle = new Vehicle { PlateNumber = dto.PlateNumber, Brand = dto.Brand, Model = dto.Model,MileageKm = dto.MileageKm, CapacityKg = dto.CapacityKg };

            db.Vehicles.Add(vehicle);
            await db.SaveChangesAsync(ct);

            var result = VehicleMappings.ToDtoFunc(vehicle);
            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, result);
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateVehicleDto dto, CancellationToken ct)
        {
            var vehicle = await db.Vehicles.FindAsync([id], ct);
            if (vehicle is null) return NotFound();

            if (await db.Vehicles.AnyAsync(v => v.PlateNumber == dto.PlateNumber && v.Id != id, ct))
                return Conflict($"Авто з номером {dto.PlateNumber} вже існує.");

            vehicle.PlateNumber = dto.PlateNumber;
            vehicle.Brand = dto.Brand;
            vehicle.Model = dto.Model;
            vehicle.MileageKm = dto.MileageKm;
            vehicle.CapacityKg = dto.CapacityKg;
            vehicle.Status = dto.Status;

            await db.SaveChangesAsync(ct);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var vehicle = await db.Vehicles.FindAsync([id], ct);
            if (vehicle is null) return NotFound();

            if (await db.TripDetails.AnyAsync(t => t.VehicleId == id, ct))
                return Conflict("Неможливо видалити транспорт, який має поїздки.");


            db.Vehicles.Remove(vehicle);
            await db.SaveChangesAsync(ct);
            return NoContent();
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetAvailable(CancellationToken ct)
        {
            var vehicles = await db.Vehicles
                .AsNoTracking()
                .Where(v => v.Status == VehicleStatus.Available)
                .OrderBy(v => v.Id)
                .Select(VehicleMappings.ToDto)
                .ToListAsync(ct);

            return Ok(vehicles);
        }


    }
}
