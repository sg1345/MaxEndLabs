using Microsoft.EntityFrameworkCore;
using MaxEndLabs.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaxEndLabs.Data.Configuration
{
	public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
	{
		public void Configure(EntityTypeBuilder<CartItem> entity)
		{
			entity
				.HasIndex(ci => new { ci.CartId, ci.ProductId, ci.ProductVariantId })
				.IsUnique();

			entity
				.HasOne(ci => ci.ShoppingCart)
				.WithMany(c => c.CartItems)
				.HasForeignKey(ci => ci.CartId)
				.OnDelete(DeleteBehavior.Cascade);

			entity
				.HasOne(ci => ci.Product)
				.WithMany()
				.HasForeignKey(ci => ci.ProductId)
				.OnDelete(DeleteBehavior.Restrict);

			entity
				.HasOne(ci => ci.ProductVariant)
				.WithMany(pv => pv.CartItems)
				.HasForeignKey(ci => ci.ProductVariantId)
				.OnDelete(DeleteBehavior.Cascade);

			entity
				.HasQueryFilter(ci => ci.IsPublished == true);
		}
	}
}
