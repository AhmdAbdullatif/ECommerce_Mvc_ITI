using ECommerce_Mvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce_Mvc.Data.Config;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        var navigation = builder.Metadata.FindNavigation(nameof(Cart.Items));
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(c => c.Items)
            .WithOne(ci => ci.Cart)
            .HasForeignKey(c => c.CartId);

        builder.HasIndex(x => x.BuyerId).IsUnique();
    }
}
