using System.Linq.Expressions;
using TransportCompany.Api.Entities;

namespace TransportCompany.Api.DTOs.Orders
{
    public static class OrderMappings
    {
        public static readonly Expression<Func<Order, OrderDto>> ToDto =
            o => new OrderDto(o.Id, o.ClientId, o.Client.FullName, o.PickupAddress, o.DeliveryAddress,
                              o.CargoDescription, o.CargoWeightKg, o.Price, o.Status, o.CreatedAt,
                              o.TripDetails != null);
    }
}
