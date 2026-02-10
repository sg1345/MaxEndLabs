
namespace MaxEndLabs.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using Models;

    public class MaxEndLabsDbContext(DbContextOptions<MaxEndLabsDbContext> options) : IdentityDbContext(options)
    {
        public virtual DbSet<CartItem> CartItems { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductVariant> ProductVariants { get; set; } = null!;
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {

            //CartItem
            builder.Entity<CartItem>()
                .HasIndex(ci => new { ci.CartId, ci.ProductId, ci.ProductVariantId })
                .IsUnique();

            builder.Entity<CartItem>()
                .HasOne(ci => ci.ShoppingCart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CartItem>()
                .HasOne(ci => ci.ProductVariant)
                .WithMany(pv => pv.CartItems)
                .HasForeignKey(ci => ci.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);

            //Category
            builder
                .Entity<Category>()
                .HasIndex(c => c.Slug)
                .IsUnique();

            //Product
            builder
                .Entity<Product>()
                .HasIndex(p => p.Slug)
                .IsUnique();

            base.OnModelCreating(builder);
        }
    }
}
