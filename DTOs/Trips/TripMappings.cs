using System.Linq.Expressions;
using TransportCompany.Api.Entities;

namespace TransportCompany.Api.DTOs.Trips
{
    public static class TripMappings
    {
        public static readonly Expression<Func<TripDetails, TripDto>> ToDto =
            t => new TripDto(
                t.Id,
                t.OrderId, t.Order.PickupAddress, t.Order.DeliveryAddress,
                t.DriverId, t.Driver.FirstName + " " + t.Driver.LastName,
                t.VehicleId, t.Vehicle.PlateNumber,
                t.PlannedDeparture, t.PlannedArrival, t.DistanceKm,
                t.Logs.Count);
    }
}