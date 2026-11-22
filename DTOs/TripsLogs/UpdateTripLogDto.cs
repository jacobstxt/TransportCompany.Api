using System.ComponentModel.DataAnnotations;

public record UpdateTripLogDto
(
    DateTime ActualTime,
    [MaxLength(1000)]
    string? DriverComment
);