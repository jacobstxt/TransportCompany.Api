using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportCompany.Api.Entities;

namespace TransportCompany.Api.Data.Configurations
{
    public class VehicleConfiguration: IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasKey(v => v.Id);

            builder.Property(v => v.PlateNumber).HasMaxLength(20).IsRequired();
            builder.HasIndex(v => v.PlateNumber).IsUnique();

            builder.Property(v => v.Brand).HasMaxLength(50).IsRequired();
            builder.Property(v => v.Model).HasMaxLength(50).IsRequired();
            builder.Property(v => v.CapacityKg).HasPrecision(10, 2);

            builder.Property(v => v.Status).HasConversion<string>().HasMaxLength(20);
        }
    }
}
