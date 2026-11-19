namespace TransportCompany.Api.DTOs.Clients
{
    public record ClientDto
    (
        int Id, 
        string FullName,
        string Phone,
        string? Email,
        DateTime CreatedAt,
        int OrdersCount
    );
}
