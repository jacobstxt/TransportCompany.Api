using TransportCompany.Api.Entities.Enums;

namespace TransportCompany.Api.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string PickupAddress { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public string CargoDescription { get; set; } = string.Empty;
        public decimal CargoWeightKg { get; set; }
        public decimal Price { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.New;
        public DateTime CreatedAt { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;

        public TripDetails? TripDetails { get; set; }
    }
}
