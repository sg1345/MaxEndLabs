
using MaxEndLabs.Data.Configuration;

namespace MaxEndLabs.Data
{
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
        public virtual DbSet<Order> Orders { get; set; } = null!;
		public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder builder)
        {

	        //OrderItem
	        builder.Entity<OrderItem>()
		        .HasKey(oi => new { oi.OrderId, oi.ProductId, oi.ProductVariantId });

	        builder.Entity<OrderItem>()
		        .HasOne(oi => oi.Order)
		        .WithMany(o => o.OrderItems)
		        .HasForeignKey(oi => oi.OrderId)
		        .OnDelete(DeleteBehavior.Cascade);

	        builder.Entity<OrderItem>()
		        .HasOne(oi => oi.Product)
		        .WithMany(p => p.OrderItems)
		        .HasForeignKey(oi => oi.ProductId)
		        .OnDelete(DeleteBehavior.Restrict);

	        builder.Entity<OrderItem>()
		        .HasOne(oi => oi.ProductVariant)
		        .WithMany(pv => pv.OrderItems)
		        .HasForeignKey(oi => oi.ProductVariantId)
		        .OnDelete(DeleteBehavior.Restrict);



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
                .OnDelete(DeleteBehavior.Cascade);

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

            //Configurations

            builder.ApplyConfiguration(new IdentityRoleConfiguration());
            builder.ApplyConfiguration(new IdentityUserConfiguration());
            builder.ApplyConfiguration(new IdentityUserRoleConfiguration());

            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());
            builder.ApplyConfiguration(new ProductVariantConfiguration());
            builder.ApplyConfiguration(new CartItemConfiguration());

			//builder.ApplyConfigurationsFromAssembly(typeof(MaxEndLabsDbContext).Assembly);

		}
	}
}
