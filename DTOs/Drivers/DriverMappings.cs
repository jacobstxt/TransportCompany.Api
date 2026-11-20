using System.Linq.Expressions;
using TransportCompany.Api.DTOs.Drivers;
using TransportCompany.Api.Entities;

public static class DriverMappings
{
    public static readonly Expression<Func<Driver, DriverDto>> ToDto =
        d => new DriverDto(d.Id, d.FirstName, d.LastName, d.LicenseNumber, d.Phone,
                           d.HiredAt, d.Trips.Count, d.DriverVehicles.Count);

    public static readonly Func<Driver, DriverDto> ToDtoFunc = ToDto.Compile();
}