using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportCompany.Api.Entities;

namespace TransportCompany.Api.Data.Configurations;

public class TripLogConfiguration : IEntityTypeConfiguration<TripLog>
{
    public void Configure(EntityTypeBuilder<TripLog> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.DriverComment).HasMaxLength(1000);

        // 1:M TripDetails -> Logs
        builder.HasOne(l => l.TripDetails)
            .WithMany(t => t.Logs)
            .HasForeignKey(l => l.TripDetailsId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}