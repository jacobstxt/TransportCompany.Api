using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportCompany.Api.Data;
using TransportCompany.Api.DTOs.Clients;
using TransportCompany.Api.Entities;

namespace TransportCompany.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController(TransportDbContext db) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetAll(CancellationToken ct)
        {
            var clients = await db.Clients
                .AsNoTracking()
                .OrderBy(c => c.Id)
                .Select(ClientMappings.ToDto)
                .ToListAsync(ct);

            return Ok(clients);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ClientDto>> GetById(int id, CancellationToken ct)
        {
            var client = await db.Clients
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(ClientMappings.ToDto)
                .FirstOrDefaultAsync(ct);

            return client is null ? NotFound() : Ok(client);
        }

        [HttpPost]
        public async Task<ActionResult<ClientDto>> Create(CreateClientDto dto, CancellationToken ct)
        {
            var client = new Client { FullName = dto.FullName, Phone = dto.Phone, Email = dto.Email };

            db.Clients.Add(client);
            await db.SaveChangesAsync(ct);

            var result = ClientMappings.ToDtoFunc(client);
            return CreatedAtAction(nameof(GetById), new { id = client.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateClientDto dto, CancellationToken ct)
        {
            var client = await db.Clients.FindAsync([id], ct);
            if (client is null) return NotFound();

            client.FullName = dto.FullName;
            client.Phone = dto.Phone;
            client.Email = dto.Email;

            await db.SaveChangesAsync(ct);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var client = await db.Clients.FindAsync([id], ct);
            if (client is null) return NotFound();

            if (await db.Orders.AnyAsync(o => o.ClientId == id, ct))
                return Conflict("Неможливо видалити клієнта, який має замовлення.");

            db.Clients.Remove(client);
            await db.SaveChangesAsync(ct);
            return NoContent();
        }
    }
}
