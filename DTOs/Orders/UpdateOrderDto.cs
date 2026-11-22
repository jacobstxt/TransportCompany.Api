using System.ComponentModel.DataAnnotations;
using TransportCompany.Api.Entities.Enums;

namespace TransportCompany.Api.DTOs.Orders
{
    public record UpdateOrderDto
    (
     [Required, MaxLength(250)]
    string PickupAddress,
     [Required, MaxLength(250)]
    string DeliveryAddress,
     [Required, MaxLength(500)]
    string CargoDescription,
     [Range(typeof(decimal), "1", "60000", ErrorMessage = "Вага від 1 до 60000 кг")]
    decimal CargoWeightKg,
     [Range(typeof(decimal), "1", "10000000", ErrorMessage = "Ціна має бути більшою за 0")]
    decimal Price,
     [EnumDataType(typeof(OrderStatus))]
    OrderStatus Status
    );
}
