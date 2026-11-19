using System.ComponentModel.DataAnnotations;

namespace TransportCompany.Api.DTOs.Clients
{
    public record CreateClientDto
    (
        [Required, MaxLength(150)]
        string FullName,
        [RegularExpression(@"^\+380\d{9}$", ErrorMessage = "Телефон має бути у форматі +380XXXXXXXXX")]
        string Phone,
        [EmailAddress, MaxLength(100)]
        string? Email
    );
}

