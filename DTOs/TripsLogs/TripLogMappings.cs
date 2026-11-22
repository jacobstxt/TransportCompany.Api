using System.Linq.Expressions;
using TransportCompany.Api.DTOs.TripsLogs;
using TransportCompany.Api.Entities;

public static class TripLogMappings
{
    public static readonly Expression<Func<TripLog, TripLogDto>> ToDto =
        l => new TripLogDto(l.Id, l.TripDetailsId, l.ActualTime, l.DriverComment);

    public static readonly Func<TripLog, TripLogDto> ToDtoFunc = ToDto.Compile();
}