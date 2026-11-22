using System.Linq.Expressions;
using TransportCompany.Api.DTOs.Clients;
using TransportCompany.Api.Entities;

namespace TransportCompany.Api.DTOs.Vehicles;

public static class VehicleMappings
{
    public static readonly Expression<Func<Vehicle, VehicleDto>> ToDto =
        v => new VehicleDto(v.Id, v.PlateNumber, v.Brand, v.MileageKm, v.CapacityKg, v.Status, v.Trips.Count);

    public static readonly Func<Vehicle, VehicleDto> ToDtoFunc = ToDto.Compile();

}

