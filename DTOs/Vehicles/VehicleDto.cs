using TransportCompany.Api.Entities.Enums;

namespace TransportCompany.Api.DTOs.Vehicles
{
    public record VehicleDto
    (
       int Id,
       string PlateNumber,
       string Brand,
       int MileageKm,
       decimal CapacityKg,
       VehicleStatus Status,
       int TripsCount
    );
}
