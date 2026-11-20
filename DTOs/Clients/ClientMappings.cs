using System.Linq.Expressions;
using TransportCompany.Api.DTOs.Clients;
using TransportCompany.Api.Entities;

public static class ClientMappings
{
    public static readonly Expression<Func<Client, ClientDto>> ToDto =
        c => new ClientDto(c.Id, c.FullName, c.Phone, c.Email, c.CreatedAt, c.Orders.Count);

    public static readonly Func<Client, ClientDto> ToDtoFunc = ToDto.Compile();
}