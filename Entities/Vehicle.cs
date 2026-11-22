using TransportCompany.Api.Entities.Enums;

namespace TransportCompany.Api.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string PlateNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int MileageKm { get; set; }
        public decimal CapacityKg { get; set; }
        public VehicleStatus Status { get; set; } = VehicleStatus.Available;
        public ICollection<TripDetails> Trips { get; set; } = [];
        public ICollection<DriverVehicle> DriverVehicles { get; set; } = [];
    }
}
