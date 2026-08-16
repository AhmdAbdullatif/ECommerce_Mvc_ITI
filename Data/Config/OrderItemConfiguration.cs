using ECommerce_Mvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce_Mvc.Data.Config;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(x => new { x.ProductId, x.OrderId });

        builder.Property(x => x.UnitPrice)
            .HasColumnType("decimal(18, 2)");

        builder.HasOne(oi => oi.Product)
            .WithMany()
            .HasForeignKey(oi => oi.ProductId);

        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.OrderedItems)
            .HasForeignKey(oi => oi.OrderId);
    }
}