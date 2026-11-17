using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportCompany.Api.Entities;

namespace TransportCompany.Api.Data.Configurations;

public class TripDetailsConfiguration : IEntityTypeConfiguration<TripDetails>
{
    public void Configure(EntityTypeBuilder<TripDetails> builder)
    {
        builder.ToTable(t => t.HasCheckConstraint(
            "CK_TripDetails_PlannedDates",
            "\"PlannedArrival\" > \"PlannedDeparture\""));

        builder.HasKey(t => t.Id);

        builder.Property(t => t.DistanceKm).HasPrecision(10, 2);

        // 1:1 Order <-> TripDetails (FK у TripDetails, EF сам зробить унікальний індекс)
        builder.HasOne(t => t.Order)
            .WithOne(o => o.TripDetails)
            .HasForeignKey<TripDetails>(t => t.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // 1:M Driver -> Trips
        builder.HasOne(t => t.Driver)
            .WithMany(d => d.Trips)
            .HasForeignKey(t => t.DriverId)
            .OnDelete(DeleteBehavior.Restrict);

        // 1:M Vehicle -> Trips
        builder.HasOne(t => t.Vehicle)
            .WithMany(v => v.Trips)
            .HasForeignKey(t => t.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}