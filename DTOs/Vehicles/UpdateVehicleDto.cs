using System.ComponentModel.DataAnnotations;
using TransportCompany.Api.Entities.Enums;

namespace TransportCompany.Api.DTOs.Vehicles
{
    public record UpdateVehicleDto
    (
        [Required, MaxLength(20)]
        [RegularExpression(@"^[A-ZА-ЯІ]{2}\d{4}[A-ZА-ЯІ]{2}$", ErrorMessage = "Номер має бути у форматі AA1234BB")]
        string PlateNumber,
        [Required, MaxLength(50)]
        string Brand,
        [Required, MaxLength(50)]
        string Model,
        [Range(0, 5_000_000)] 
        int MileageKm,
        [Range(typeof(decimal), "1", "60000", ErrorMessage = "Вантажопідйомність має бути від 1 до 60000 кг")]
        decimal CapacityKg,
        [EnumDataType(typeof(VehicleStatus))]
        VehicleStatus Status
    );
}