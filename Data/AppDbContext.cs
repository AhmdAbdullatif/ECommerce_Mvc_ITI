using ECommerce_Mvc.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Emit;

namespace ECommerce_Mvc.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<SellerRequest> SellerRequests { get; set; }
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


        string realSellerId = "a6f76fb7-5b39-4ecb-9639-f3bba5117dd5";

        builder.Entity<Product>().HasData(
         new
         {
             Id = 1,
             CategoryId = 1, // تأكد أن لديك قسم (Category) يمتلك Id رقم 1
             Name = "Laptop",
             Description = "High performance laptop",
             Quantity = 10,
             Price = 15000m,
             PictureUri = "laptop.png",
             CreatedAtUtc = new DateTime(2023, 10, 1, 0, 0, 0, DateTimeKind.Utc), // يفضل تثبيت الوقت في السِيد
             UserId = realSellerId,
             SellerId = realSellerId // يمكنك وضع null هنا إذا أردت لأننا جعلناه يقبل null
         }
        );



        // ApplicationUser -> SellerRequest
        builder.Entity<SellerRequest>()
            .HasOne(sr => sr.User)
            .WithMany(u => u.SellerRequests)
            .HasForeignKey(sr => sr.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // ApplicationUser -> Product
        builder.Entity<Product>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.SellerId)
            .OnDelete(DeleteBehavior.Restrict);
    }

}