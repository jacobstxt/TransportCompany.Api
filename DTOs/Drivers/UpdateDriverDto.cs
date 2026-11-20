using System.ComponentModel.DataAnnotations;

namespace TransportCompany.Api.DTOs.Drivers
{
    public record UpdateDriverDto
    (
        [Required, MaxLength(50)]
        string FirstName,
        [Required, MaxLength(50)]
        string LastName,
        [Required, MaxLength(20)]
        [RegularExpression(@"^[A-ZА-ЯІ]{3}\d{6}$", ErrorMessage = "Номер посвідчення у форматі ABC123456")]
        string LicenseNumber,
        [Required, MaxLength(20)]
        [RegularExpression(@"^\+380\d{9}$", ErrorMessage = "Телефон має бути у форматі +380XXXXXXXXX")]
        string Phone,
        DateTime HiredAt
    );
}
