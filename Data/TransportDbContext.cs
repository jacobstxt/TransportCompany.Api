using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace TransportCompany.Api.Data
{
    public class TransportDbContext(DbContextOptions<TransportDbContext> options): DbContext(options)
    {

    }
}
