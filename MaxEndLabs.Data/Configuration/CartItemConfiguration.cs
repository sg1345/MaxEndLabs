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
				.HasQueryFilter(ci => ci.IsPublished == true);
		}
	}
}
