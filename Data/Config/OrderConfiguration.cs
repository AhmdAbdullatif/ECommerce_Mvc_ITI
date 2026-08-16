using ECommerce_Mvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce_Mvc.Data.Config;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        var navigation = builder.Metadata.FindNavigation(nameof(Order.OrderedItems));
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(o => o.OrderedItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId);

        builder.HasOne(o => o.User)
            .WithMany()
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.OwnsOne(o => o.ShipToAddress, a =>
        {
            a.WithOwner();

            a.Property(a => a.Street)
                .HasMaxLength(30)
                .IsRequired();

            a.Property(a => a.State)
                .HasMaxLength(30);

            a.Property(a => a.Country)
                .HasMaxLength(30)
                .IsRequired();

            a.Property(a => a.City)
                .HasMaxLength(30)
                .IsRequired();
        });
    }
}
