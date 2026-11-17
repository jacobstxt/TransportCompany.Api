using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportCompany.Api.Entities;

namespace TransportCompany.Api.Data.Configurations;

public class DriverVehicleConfiguration : IEntityTypeConfiguration<DriverVehicle>
{
    public void Configure(EntityTypeBuilder<DriverVehicle> builder)
    {
        // складений ключ
        builder.HasKey(dv => new { dv.DriverId, dv.VehicleId });

        builder.Property(dv => dv.AssignedAt).HasDefaultValueSql("now()");

        // N:M Driver <-> Vehicle через join-таблицю
        builder.HasOne(dv => dv.Driver)
            .WithMany(d => d.DriverVehicles)
            .HasForeignKey(dv => dv.DriverId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(dv => dv.Vehicle)
            .WithMany(v => v.DriverVehicles)
            .HasForeignKey(dv => dv.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}