using TransportCompany.Api.Entities.Enums;

namespace TransportCompany.Api.DTOs.Orders
{
    public record OrderDto(
    int Id,
    int ClientId,
    string ClientName,
    string PickupAddress,
    string DeliveryAddress,
    string CargoDescription,
    decimal CargoWeightKg,
    decimal Price,
    OrderStatus Status,
    DateTime CreatedAt,
    bool HasTrip);
}
