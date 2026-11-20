namespace TransportCompany.Api.DTOs.Drivers
{
    public record DriverVehicleDto
    (
     int VehicleId,
     string PlateNumber,
     string Brand,
     string Model,
     DateTime AssignedAt
    );
}
