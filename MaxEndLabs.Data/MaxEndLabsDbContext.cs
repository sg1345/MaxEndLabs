namespace MaxEndLabs.Data
{
    using Configuration;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using Models;

    public class MaxEndLabsDbContext : IdentityDbContext<ApplicationUser>
    {
        public MaxEndLabsDbContext(DbContextOptions<MaxEndLabsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CartItem> CartItems { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductVariant> ProductVariants { get; set; } = null!;
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
		public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
			//TODO: configurations for OrderItem and Order
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
            
	        //Order
	        builder.Entity<Order>()
		        .HasIndex(o => o.OrderNumber)
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
		}
	}
}
