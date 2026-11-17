using Microsoft.EntityFrameworkCore;
using TransportCompany.Api.Entities;

namespace TransportCompany.Api.Data
{
    public class TransportDbContext(DbContextOptions<TransportDbContext> options) : DbContext(options)
    {
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Driver> Drivers => Set<Driver>();
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<DriverVehicle> DriverVehicles => Set<DriverVehicle>();
        public DbSet<TripDetails> TripDetails => Set<TripDetails>();
        public DbSet<TripLog> TripLogs => Set<TripLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TransportDbContext).Assembly);
        }
    }
}