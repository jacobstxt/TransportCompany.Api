using System.ComponentModel.DataAnnotations;

namespace TransportCompany.Api.DTOs.Trips
{
    public record CreateTripDto
    (
        [Range(1, int.MaxValue)] int OrderId,
        [Range(1, int.MaxValue)] int DriverId,
        [Range(1, int.MaxValue)] int VehicleId,
        DateTime PlannedDeparture,
        DateTime PlannedArrival,
        [Range(typeof(decimal), "1", "20000", ErrorMessage = "Відстань від 1 до 20000 км")]
        decimal DistanceKm
    );
}