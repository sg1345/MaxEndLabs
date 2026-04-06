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
				.HasIndex(c => c.Slug)
				.IsUnique();

			entity
				.HasData(SeedCategory());
		}

		private Category[] SeedCategory()
		{
			return
			[
				new Category { Id = Guid.Parse("cb8ceb6a-6623-4937-a6c4-777b4b498e1e"), Name = "Upper Body", Slug = "upper-body" },
				new Category { Id = Guid.Parse("6f026985-b5c2-4b05-9ac4-088b88982ec8"), Name = "Lower Body", Slug = "lower-body" },
				new Category { Id = Guid.Parse("a9c9243a-aca9-4d3c-8f7a-4bdeb4578682"), Name = "Shoes", Slug = "shoes" },
				new Category { Id = Guid.Parse("f1f47314-cdc4-4104-a9f2-a2ccf38e9623"), Name = "Supplements", Slug = "supplements" },
				new Category { Id = Guid.Parse("26a46720-ae3a-43bd-b5de-2e2dd4048dee"), Name = "Accessories", Slug = "accessories" }
			];
		}
	}
}
