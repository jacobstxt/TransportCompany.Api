using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportCompany.Api.Entities;

namespace TransportCompany.Api.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.PickupAddress).HasMaxLength(250).IsRequired();
        builder.Property(o => o.DeliveryAddress).HasMaxLength(250).IsRequired();
        builder.Property(o => o.CargoDescription).HasMaxLength(500).IsRequired();
        builder.Property(o => o.CargoWeightKg).HasPrecision(10, 2);
        builder.Property(o => o.Price).HasPrecision(12, 2);
        builder.Property(o => o.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(o => o.CreatedAt).HasDefaultValueSql("now()");

        // для запиту "замовлення за період"
        builder.HasIndex(o => o.CreatedAt);

        // 1:M Client -> Orders
        builder.HasOne(o => o.Client)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}