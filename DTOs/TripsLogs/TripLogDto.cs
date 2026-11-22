namespace TransportCompany.Api.DTOs.TripsLogs
{
    public record TripLogDto(
    int Id,
    int TripDetailsId,
    DateTime ActualTime,
    string? DriverComment);
}
