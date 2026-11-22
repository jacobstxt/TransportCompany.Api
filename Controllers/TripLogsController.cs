using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportCompany.Api.Data;
using TransportCompany.Api.DTOs.TripsLogs;
using TransportCompany.Api.Entities;

namespace TransportCompany.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripLogsController(TransportDbContext db) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripLogDto>>> GetAll([FromQuery] int? tripId, CancellationToken ct)
        {
            var query = db.TripLogs.AsNoTracking();

            if (tripId is not null)
                query = query.Where(l => l.TripDetailsId == tripId);

            var tripLogs = await query
                .OrderBy(l => l.ActualTime)
                .Select(TripLogMappings.ToDto)
                .ToListAsync(ct);

            return Ok(tripLogs);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TripLogDto>> GetById(int id, CancellationToken ct)
        {
            var tripLog = await db.TripLogs
                .AsNoTracking()
                .Where(l => l.Id == id)
                .Select(TripLogMappings.ToDto)
                .FirstOrDefaultAsync(ct);

            return tripLog is null ? NotFound() : Ok(tripLog);
        }

        [HttpPost]
        public async Task<ActionResult<TripLogDto>> Create(CreateTripLogDto dto, CancellationToken ct)
        {
            if (!await db.TripDetails.AnyAsync(t => t.Id == dto.TripDetailsId, ct))
                return BadRequest($"Поїздки {dto.TripDetailsId} не існує.");

            var tripLog = new TripLog
            {
                TripDetailsId = dto.TripDetailsId,
                ActualTime = dto.ActualTime.ToUniversalTime(),
                DriverComment = dto.DriverComment
            };

            db.TripLogs.Add(tripLog);
            await db.SaveChangesAsync(ct);

            var result = TripLogMappings.ToDtoFunc(tripLog);
            return CreatedAtAction(nameof(GetById), new { id = tripLog.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateTripLogDto dto, CancellationToken ct)
        {
            var tripLog = await db.TripLogs.FindAsync([id], ct);
            if (tripLog is null) return NotFound();

            tripLog.ActualTime = dto.ActualTime.ToUniversalTime();
            tripLog.DriverComment = dto.DriverComment;

            await db.SaveChangesAsync(ct);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var tripLog = await db.TripLogs.FindAsync([id], ct);
            if (tripLog is null) return NotFound();

            // від TripLog нічого не залежить, тому перевірок не потрібно
            db.TripLogs.Remove(tripLog);
            await db.SaveChangesAsync(ct);
            return NoContent();
        }
    }
}