namespace TransportCompany.Api.DTOs.Trips
{
    public record TripDto(
        int Id,
        int OrderId,
        string PickupAddress,
        string DeliveryAddress,
        int DriverId,
        string DriverName,
        int VehicleId,
        string PlateNumber,
        DateTime PlannedDeparture,
        DateTime PlannedArrival,
        decimal DistanceKm,
        int LogsCount);
}