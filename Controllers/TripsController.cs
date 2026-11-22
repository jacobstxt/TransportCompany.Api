using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportCompany.Api.Data;
using TransportCompany.Api.DTOs.Trips;
using TransportCompany.Api.Entities;
using TransportCompany.Api.Entities.Enums;

namespace TransportCompany.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController(TransportDbContext db) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripDto>>> GetAll(CancellationToken ct)
        {
            var trips = await db.TripDetails
                .AsNoTracking()
                .OrderBy(t => t.PlannedDeparture)
                .Select(TripMappings.ToDto)
                .ToListAsync(ct);

            return Ok(trips);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TripDto>> GetById(int id, CancellationToken ct)
        {
            var trip = await db.TripDetails
                .AsNoTracking()
                .Where(t => t.Id == id)
                .Select(TripMappings.ToDto)
                .FirstOrDefaultAsync(ct);

            return trip is null ? NotFound() : Ok(trip);
        }

        [HttpPost]
        public async Task<ActionResult<TripDto>> Create(CreateTripDto dto, CancellationToken ct)
        {
            var order = await db.Orders.FirstOrDefaultAsync(o => o.Id == dto.OrderId, ct);
            if (order is null)
                return BadRequest($"Замовлення {dto.OrderId} не існує.");

            if (await db.TripDetails.AnyAsync(t => t.OrderId == dto.OrderId, ct))
                return Conflict("Для цього замовлення поїздка вже існує.");

            if (order.Status != OrderStatus.New)
                return Conflict($"Поїздку можна створити лише для нового замовлення (зараз: {order.Status}).");

            var departure = dto.PlannedDeparture.ToUniversalTime();
            var arrival = dto.PlannedArrival.ToUniversalTime();

            var error = await ValidateTripAsync(dto.DriverId, dto.VehicleId, departure, arrival,
                                                order.CargoWeightKg, excludeTripId: 0, ct);
            if (error is not null) return error;

            var trip = new TripDetails
            {
                OrderId = dto.OrderId,
                DriverId = dto.DriverId,
                VehicleId = dto.VehicleId,
                PlannedDeparture = departure,
                PlannedArrival = arrival,
                DistanceKm = dto.DistanceKm
            };

            db.TripDetails.Add(trip);
            order.Status = OrderStatus.Assigned;
            await db.SaveChangesAsync(ct);

            var result = await db.TripDetails
                .AsNoTracking()
                .Where(t => t.Id == trip.Id)
                .Select(TripMappings.ToDto)
                .FirstAsync(ct);

            return CreatedAtAction(nameof(GetById), new { id = trip.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateTripDto dto, CancellationToken ct)
        {
            var trip = await db.TripDetails.FindAsync([id], ct);
            if (trip is null) return NotFound();

            var departure = dto.PlannedDeparture.ToUniversalTime();
            var arrival = dto.PlannedArrival.ToUniversalTime();

            var cargoWeight = await db.Orders
                .Where(o => o.Id == trip.OrderId)
                .Select(o => o.CargoWeightKg)
                .FirstAsync(ct);

            var error = await ValidateTripAsync(dto.DriverId, dto.VehicleId, departure, arrival,
                                                cargoWeight, excludeTripId: id, ct);
            if (error is not null) return error;

            trip.DriverId = dto.DriverId;
            trip.VehicleId = dto.VehicleId;
            trip.PlannedDeparture = departure;
            trip.PlannedArrival = arrival;
            trip.DistanceKm = dto.DistanceKm;

            await db.SaveChangesAsync(ct);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var trip = await db.TripDetails
                .Include(t => t.Order)
                .FirstOrDefaultAsync(t => t.Id == id, ct);
            if (trip is null) return NotFound();

            if (await db.TripLogs.AnyAsync(l => l.TripDetailsId == id, ct))
                return Conflict("Неможливо видалити поїздку, яка вже має записи в журналі.");

            if (trip.Order.Status == OrderStatus.Assigned)
                trip.Order.Status = OrderStatus.New;   

            db.TripDetails.Remove(trip);
            await db.SaveChangesAsync(ct);
            return NoContent();
        }


        private async Task<ActionResult?> ValidateTripAsync(
            int driverId, int vehicleId, DateTime departure, DateTime arrival,
            decimal cargoWeightKg, int excludeTripId, CancellationToken ct)
        {
            if (arrival <= departure)
                return BadRequest("Час прибуття має бути пізніше за час відправлення.");

            if (!await db.Drivers.AnyAsync(d => d.Id == driverId, ct))
                return BadRequest($"Водія {driverId} не існує.");

            var vehicle = await db.Vehicles.AsNoTracking().FirstOrDefaultAsync(v => v.Id == vehicleId, ct);
            if (vehicle is null)
                return BadRequest($"Авто {vehicleId} не існує.");

            if (vehicle.Status == VehicleStatus.Maintenance)
                return Conflict("Авто на обслуговуванні.");

            if (cargoWeightKg > vehicle.CapacityKg)
                return Conflict($"Вага вантажу ({cargoWeightKg} кг) перевищує вантажопідйомність авто ({vehicle.CapacityKg} кг).");

            if (!await db.DriverVehicles.AnyAsync(dv => dv.DriverId == driverId && dv.VehicleId == vehicleId, ct))
                return Conflict("Водій не допущений до цього авто.");

            if (await db.TripDetails.AnyAsync(t => t.Id != excludeTripId && t.VehicleId == vehicleId
                    && t.PlannedDeparture < arrival && t.PlannedArrival > departure, ct))
                return Conflict("Авто вже зайняте іншою поїздкою в цей час.");

            if (await db.TripDetails.AnyAsync(t => t.Id != excludeTripId && t.DriverId == driverId
                    && t.PlannedDeparture < arrival && t.PlannedArrival > departure, ct))
                return Conflict("Водій уже має іншу поїздку в цей час.");

            return null;
        }
    }
}