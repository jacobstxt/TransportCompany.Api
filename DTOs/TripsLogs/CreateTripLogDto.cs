using System.ComponentModel.DataAnnotations;

public record CreateTripLogDto
(
    [Range(1, int.MaxValue)]
    int TripDetailsId,
    DateTime ActualTime,
    [MaxLength(1000)]
    string? DriverComment
);