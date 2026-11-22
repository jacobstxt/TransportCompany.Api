using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportCompany.Api.Data;
using TransportCompany.Api.DTOs.Orders;
using TransportCompany.Api.Entities;
using TransportCompany.Api.Entities.Enums;

namespace TransportCompany.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(TransportDbContext db) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll([FromQuery] DateTime? from,[FromQuery] DateTime? to,CancellationToken ct)
        {
            if (from > to)
                return BadRequest("Параметр 'from' має бути не пізніше за 'to'.");

            var query = db.Orders.AsNoTracking();

            if (from is not null)
            {
                var start = from.Value.ToUniversalTime();
                query = query.Where(o => o.CreatedAt >= start);
            }

            if (to is not null)
            {
                var end = to.Value.ToUniversalTime();
                query = query.Where(o => o.CreatedAt <= end);
            }

            var orders = await query
                .OrderBy(o => o.CreatedAt)
                .Select(OrderMappings.ToDto)
                .ToListAsync(ct);

            return Ok(orders);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> GetById(int id, CancellationToken ct)
        {
            var order = await db.Orders
                .AsNoTracking()
                .Where(o => o.Id == id)
                .Select(OrderMappings.ToDto)
                .FirstOrDefaultAsync(ct);

            return order is null ? NotFound() : Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create(CreateOrderDto dto, CancellationToken ct)
        {
            if (!await db.Clients.AnyAsync(c => c.Id == dto.ClientId, ct))
                return BadRequest($"Клієнта {dto.ClientId} не існує.");

            var order = new Order
            {
                ClientId = dto.ClientId,
                PickupAddress = dto.PickupAddress,
                DeliveryAddress = dto.DeliveryAddress,
                CargoDescription = dto.CargoDescription,
                CargoWeightKg = dto.CargoWeightKg,
                Price = dto.Price
            };

            db.Orders.Add(order);
            await db.SaveChangesAsync(ct);

            var result = await db.Orders
                .AsNoTracking()
                .Where(o => o.Id == order.Id)
                .Select(OrderMappings.ToDto)
                .FirstAsync(ct);

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateOrderDto dto, CancellationToken ct)
        {
            var order = await db.Orders.FindAsync([id], ct);
            if (order is null) return NotFound();

            order.PickupAddress = dto.PickupAddress;
            order.DeliveryAddress = dto.DeliveryAddress;
            order.CargoDescription = dto.CargoDescription;
            order.CargoWeightKg = dto.CargoWeightKg;
            order.Price = dto.Price;
            order.Status = dto.Status;

            await db.SaveChangesAsync(ct);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var order = await db.Orders.FindAsync([id], ct);
            if (order is null) return NotFound();

            if (order.Status is OrderStatus.InTransit or OrderStatus.Delivered)
                return Conflict("Неможливо видалити замовлення, яке вже в дорозі або доставлене.");

            db.Orders.Remove(order);
            await db.SaveChangesAsync(ct);
            return NoContent();
        }




    }
}
