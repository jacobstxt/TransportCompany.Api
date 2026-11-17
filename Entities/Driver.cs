namespace TransportCompany.Api.Entities
{
    public class Driver
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime HiredAt { get; set; }
        public ICollection<TripDetails> Trips { get; set; } = [];
        public ICollection<DriverVehicle> DriverVehicles { get; set; } = [];
    }
}
