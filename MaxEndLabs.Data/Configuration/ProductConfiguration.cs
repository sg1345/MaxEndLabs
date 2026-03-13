using MaxEndLabs.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaxEndLabs.Data.Configuration
{
	public class ProductConfiguration : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> entity)
		{
			entity
				.HasQueryFilter(p => p.IsPublished == true);

			entity
				.HasData(SeedProduct());
		}

		private Product[] SeedProduct()
		{
			return
			[
				new Product
				{
					Id = 1,
					Name = "Why Be Normal T-shirt",
					Slug = "why-be-normal-tshirt",
					Description = "The three words that motivated generations of weightlifters are back! As worn by Bulgaria’s -85kg snatch record holder from the ‘90s, Georgi Gardev. Oversized fit.",
					CategoryId = 1,
					Price = 40m,
					MainImageUrl = "https://eustore.weightliftinghouse.com/cdn/shop/files/WBN1.jpg",
					CreatedAt = new DateOnly(2026, 2, 10),
					UpdatedAt = new DateOnly(2026, 2, 10),
					IsPublished = true
				},
				new Product
				{
					Id = 2,
					Name = "Nasar I Win, You Lose T-shirt",
					Slug = "nasar-i-win-you-lose-tshirt",
					Description = "The official t-shirt of Olympic Champion Karlos Nasar. Made in collaboration with the greatest weightlifter of a generation, the Nasar ‘I Win, You Lose’ T-shirt.",
					CategoryId = 1,
					Price = 45m,
					MainImageUrl = "https://eustore.weightliftinghouse.com/cdn/shop/files/6_155b8e55-8336-438c-a966-fdec261cd0cb.jpg",
					CreatedAt = new DateOnly(2026, 2, 10),
					UpdatedAt = new DateOnly(2026, 2, 10),
					IsPublished = true
				},
				new Product
				{
					Id = 3,
					Name = "T-shirt 404",
					Slug = "tshirt-404",
					Description = "Celebrate the record-breaking power of Olympic Champion Karlos Nasar with this exclusive t-shirt. Featuring the iconic ‘404’ — marking the world and Olympic total record.",
					CategoryId = 1,
					Price = 30m,
					MainImageUrl = "https://karlosnasar.com/storage/products/371146981.jpg",
					CreatedAt = new DateOnly(2026, 2, 10),
					UpdatedAt = new DateOnly(2026, 2, 10),
					IsPublished = true
				},
				new Product
				{
					Id = 4,
					Name = "Nasar I Win, You Lose Shorts",
					Slug = "nasar-i-win-you-lose-shorts",
					Description = "The official t-shirt of Olympic Champion Karlos Nasar. Made in collaboration with the greatest weightlifter of a generation, the Nasar ‘I Win, You Lose’ Shorts",
					CategoryId = 2,
					Price = 62.95m,
					MainImageUrl = "https://eustore.weightliftinghouse.com/cdn/shop/files/18_75ea3d05-1e8b-4fbc-af35-a41fb16930b3.jpg",
					CreatedAt = new DateOnly(2026, 2, 10),
					UpdatedAt = new DateOnly(2026, 2, 10),
					IsPublished = true
				},
				new Product
				{
					Id = 5,
					Name = "Nasar I Win, You Lose Leggings",
					Slug = "nasar-i-win-you-lose-leggings",
					Description = "The official t-shirt of Olympic Champion Karlos Nasar. Made in collaboration with the greatest weightlifter of a generation, the Nasar ‘I Win, You Lose’ Leggings",
					CategoryId = 2,
					Price = 70m,
					MainImageUrl = "https://eustore.weightliftinghouse.com/cdn/shop/files/13_2144736c-1165-42a7-87f7-c90db6a3c274.jpg",
					CreatedAt = new DateOnly(2026, 2, 10),
					UpdatedAt = new DateOnly(2026, 2, 10),
					IsPublished = true
				},
				new Product
				{
					Id = 6,
					Name = "LUXIAOJUN PowerPro Weightlifting Shoes (Karlos)",
					Slug = "luxiaojun-powerpro-weightlifting-shoes-karlos",
					Description = "When Karlos Nasar shattered the world record, he did it in LUXIAOJUN lifters. That moment wasn’t just victory. It was domination. To celebrate his legacy and mindset, we present the Karlos Edition: built for lifters who don't just compete, they take over.",
					CategoryId = 3,
					Price = 195m,
					MainImageUrl = "https://luxiaojun.com/cdn/shop/files/LUXIAOJUN_PowerPro_Weightlifting_Shoes_Karlos_Edition_Pair.jpg",
					CreatedAt = new DateOnly(2026, 2, 10),
					UpdatedAt = new DateOnly(2026, 2, 10),
					IsPublished = true
				},
				new Product
				{
					Id = 7,
					Name = "GOLD STANDARD 100% WHEY",
					Slug = "gold-standard-100-whey",
					Description = "Fuel your performance with Gold Standard 100% Whey — a premium protein powder built for muscle support and post-workout recovery. Featuring 24g of protein and 5.5g of BCAAs per serving, with whey protein isolate as the No. 1 ingredient.",
					CategoryId = 4,
					Price = 99.99m,
					MainImageUrl = "https://www.optimumnutrition.com/cdn/shop/files/on-1101490_Image_01.jpg",
					CreatedAt = new DateOnly(2026, 2, 10),
					UpdatedAt = new DateOnly(2026, 2, 10),
					IsPublished = true
				},
				new Product
				{
					Id = 8,
					Name = "Micronized Creatine Powder",
					Slug = "micronized-creatine-powder",
					Description = "Boost your performance with Micronized Creatine Powder — designed to support muscle strength and power, explosive movements, and ATP recycling. Delivering 5g of creatine monohydrate per serving and stackable with Gold Standard 100% Whey for complete muscle support.",
					CategoryId = 4,
					Price = 30m,
					MainImageUrl = "https://www.optimumnutrition.com/cdn/shop/files/on-1102271_Image_01.png",
					CreatedAt = new DateOnly(2026, 2, 10),
					UpdatedAt = new DateOnly(2026, 2, 10),
					IsPublished = true
				},
				new Product
				{
					Id = 9,
					Name = "Nasar House Straps",
					Slug = "nasar-house-straps",
					Description = "Designed by Olympic Champion Karlos Nasar to elevate your training. These classic quick-release straps offer performance and durability with Karlos’s signature branding.",
					CategoryId = 5,
					Price = 26m,
					MainImageUrl = "https://eustore.weightliftinghouse.com/cdn/shop/files/nasar_house_1.jpg",
					CreatedAt = new DateOnly(2026, 2, 10),
					UpdatedAt = new DateOnly(2026, 2, 10),
					IsPublished = true
				}
			];
		}
	}
}
