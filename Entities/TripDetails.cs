namespace TransportCompany.Api.Entities
{
    public class TripDetails
    {
        public int Id { get; set; }
        public DateTime PlannedDeparture { get; set; }
        public DateTime PlannedArrival { get; set; }
        public decimal DistanceKm { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public int DriverId { get; set; }
        public Driver Driver { get; set; } = null!;
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
        public ICollection<TripLog> Logs { get; set; } = [];
    }
}
