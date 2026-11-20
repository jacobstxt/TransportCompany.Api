namespace TransportCompany.Api.DTOs.Drivers
{
    public record DriverDto(
    int Id,
    string FirstName,
    string LastName,
    string LicenseNumber,
    string Phone,
    DateTime HiredAt,
    int TripsCount,
    int VehiclesCount);
}
