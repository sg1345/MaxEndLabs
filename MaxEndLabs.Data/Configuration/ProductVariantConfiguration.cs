using MaxEndLabs.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaxEndLabs.Data.Configuration
{
	public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
	{
		public void Configure(EntityTypeBuilder<ProductVariant> entity)
		{
			entity
				.HasData(SeedProductVariant());
		}

		private ProductVariant[] SeedProductVariant()
		{
			return
				[
					new ProductVariant
					{
						Id = 1,
						ProductId = 1,
						VariantName = "Size S / Color: Black"
					},
					new ProductVariant
					{
						Id = 2,
						ProductId = 1,
						VariantName = "Size M / Color: Black"
					},
					new ProductVariant
					{
						Id = 3,
						ProductId = 1,
						VariantName = "Size L / Color: Black"
					},
					new ProductVariant
					{
						Id = 4,
						ProductId = 1,
						VariantName = "Size XL / Color: Black"
					},
					new ProductVariant
					{
						Id = 5,
						ProductId = 2,
						VariantName = "Size S / Color: Black"
					},
					new ProductVariant
					{
						Id = 6,
						ProductId = 2,
						VariantName = "Size M / Color: Black"
					},
					new ProductVariant
					{
						Id = 7,
						ProductId = 2,
						VariantName = "Size L / Color: Black"
					},
					new ProductVariant
					{
						Id = 8,
						ProductId = 2,
						VariantName = "Size XL / Color: Black"
					},
					new ProductVariant
					{
						Id = 9,
						ProductId = 3,
						VariantName = "Size S / Color: Black"
					},
					new ProductVariant
					{
						Id = 10,
						ProductId = 3,
						VariantName = "Size M / Color: Black"
					},
					new ProductVariant
					{
						Id = 11,
						ProductId = 3,
						VariantName = "Size L / Color: Black"
					},
					new ProductVariant
					{
						Id = 12,
						ProductId = 3,
						VariantName = "Size XL / Color: Black"
					},
					new ProductVariant
					{
						Id = 13,
						ProductId = 4,
						VariantName = "Size S / Color: Black"
					},
					new ProductVariant
					{
						Id = 14,
						ProductId = 4,
						VariantName = "Size M / Color: Black"
					},
					new ProductVariant
					{
						Id = 15,
						ProductId = 4,
						VariantName = "Size L / Color: Black"
					},
					new ProductVariant
					{
						Id = 16,
						ProductId = 4,
						VariantName = "Size XL / Color: Black"
					},
					new ProductVariant
					{
						Id = 17,
						ProductId = 5,
						VariantName = "Size S / Color: Black"
					},
					new ProductVariant
					{
						Id = 18,
						ProductId = 5,
						VariantName = "Size M / Color: Black"
					},
					new ProductVariant
					{
						Id = 19,
						ProductId = 5,
						VariantName = "Size L / Color: Black"
					},
					new ProductVariant
					{
						Id = 20,
						ProductId = 5,
						VariantName = "Size XL / Color: Black"
					},
					new ProductVariant
					{
						Id = 21,
						ProductId = 6,
						VariantName = "Size 38 EU"
					},
					new ProductVariant
					{
						Id = 22,
						ProductId = 6,
						VariantName = "Size 39 EU"
					},
					new ProductVariant
					{
						Id = 23,
						ProductId = 6,
						VariantName = "Size 40 EU"
					},
					new ProductVariant
					{
						Id = 24,
						ProductId = 6,
						VariantName = "Size 41 EU"
					},
					new ProductVariant
					{
						Id = 25,
						ProductId = 6,
						VariantName = "Size 42 EU"
					},
					new ProductVariant
					{
						Id = 26,
						ProductId = 6,
						VariantName = "Size 43 EU"
					},
					new ProductVariant
					{
						Id = 27,
						ProductId = 6,
						VariantName = "Size 44 EU"
					},
					new ProductVariant
					{
						Id = 28,
						ProductId = 6,
						VariantName = "Size 45 EU"
					},
					new ProductVariant
					{
						Id = 29,
						ProductId = 6,
						VariantName = "Size 46 EU"
					},
					new ProductVariant
					{
						Id = 30,
						ProductId = 6,
						VariantName = "Size 47 EU"
					},
					new ProductVariant
					{
						Id = 31,
						ProductId = 6,
						VariantName = "Size 48 EU"
					},
					new ProductVariant
					{
						Id = 32,
						ProductId = 7,
						VariantName = "Double Rich Chocolate / 2.26KG",
						Price = 99.99m
					}, 
					new ProductVariant
					{
						Id = 33,
						ProductId = 8,
						VariantName = "Unflavored / 317g",
						Price = 30m
					},
					new ProductVariant
					{
						Id = 34,
						ProductId = 9,
						VariantName = "Color: Black and Green"
					}
				];
		}
	}
}
