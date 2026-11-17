using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportCompany.Api.Entities;

namespace TransportCompany.Api.Data.Configurations;

public class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.FirstName).HasMaxLength(50).IsRequired();
        builder.Property(d => d.LastName).HasMaxLength(50).IsRequired();
        builder.Property(d => d.LicenseNumber).HasMaxLength(20).IsRequired();
        builder.Property(d => d.Phone).HasMaxLength(20).IsRequired();

        builder.HasIndex(d => d.LicenseNumber).IsUnique();
    }
}