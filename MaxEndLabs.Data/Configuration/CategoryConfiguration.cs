using MaxEndLabs.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaxEndLabs.Data.Configuration
{
	public class CategoryConfiguration : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> entity)
		{
			entity
				.HasData(SeedCategory());
		}

		private Category[] SeedCategory()
		{
			return
			[
				new Category { Id = 1, Name = "Upper Body", Slug = "upper-body" },
				new Category { Id = 2, Name = "Lower Body", Slug = "lower-body" },
				new Category { Id = 3, Name = "Shoes", Slug = "shoes" },
				new Category { Id = 4, Name = "Supplements", Slug = "supplements" },
				new Category { Id = 5, Name = "Accessories", Slug = "accessories" }
			];
		}
	}
}
