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
				.HasIndex(p => p.Slug)
				.IsUnique();

			entity
				.HasQueryFilter(p => p.IsPublished == true);

			entity
				.HasData(SeedProduct());

            entity
                .Property(p => p.Price)
                .HasPrecision(10, 2);
        }

		private Product[] SeedProduct()
		{
			return
			[
				new Product
				{
					Id = Guid.Parse("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), //1
					Name = "Why Be Normal T-shirt",
					Slug = "why-be-normal-tshirt",
					Description = "The three words that motivated generations of weightlifters are back! As worn by Bulgaria’s -85kg snatch record holder from the ‘90s, Georgi Gardev. Oversized fit.",
					CategoryId = Guid.Parse("cb8ceb6a-6623-4937-a6c4-777b4b498e1e"),
					Price = 40m,
					MainImageUrl = "https://eustore.weightliftinghouse.com/cdn/shop/files/WBN1.jpg",
					CreatedAt = new DateOnly(2026, 2, 10),
					UpdatedAt = new DateOnly(2026, 2, 10),
					IsPublished = true
				},
				new Product
				{
					Id = Guid.Parse("1dec9339-ab58-4186-90c3-c6eb7f971893"), //2
					Name = "Nasar I Win, You Lose T-shirt",
					Slug = "nasar-i-win-you-lose-tshirt",
					Description = "The official t-shirt of Olympic Champion Karlos Nasar. Made in collaboration with the greatest weightlifter of a generation, the Nasar ‘I Win, You Lose’ T-shirt.",
					CategoryId = Guid.Parse("cb8ceb6a-6623-4937-a6c4-777b4b498e1e"),
					Price = 45m,
					MainImageUrl = "https://eustore.weightliftinghouse.com/cdn/shop/files/6_155b8e55-8336-438c-a966-fdec261cd0cb.jpg",
					CreatedAt = new DateOnly(2026, 2, 10),
					UpdatedAt = new DateOnly(2026, 2, 10),
					IsPublished = true
				},
				new Product
				{
					Id = Guid.Parse("e28b6ccc-c7d5-49d6-95ae-1c6e665c6ced"), //3
					Name = "T-shirt 404",
					Slug = "tshirt-404",
					Description = "Celebrate the record-breaking power of Olympic Champion Karlos Nasar with this exclusive t-shirt. Featuring the iconic ‘404’ — marking the world and Olympic total record.",
					CategoryId = Guid.Parse("cb8ceb6a-6623-4937-a6c4-777b4b498e1e"),
					Price = 30m,
					MainImageUrl = "https://karlosnasar.com/storage/products/371146981.jpg",
					CreatedAt = new DateOnly(2026, 2, 10),
					UpdatedAt = new DateOnly(2026, 2, 10),
					IsPublished = true
				},
				new Product
				{
					Id = Guid.Parse("96ddcde3-5ff2-4fd4-b685-f216945568a3"), //4
					Name = "Nasar I Win, You Lose Shorts",
					Slug = "nasar-i-win-you-lose-shorts",
					Description = "The official t-shirt of Olympic Champion Karlos Nasar. Made in collaboration with the greatest weightlifter of a generation, the Nasar ‘I Win, You Lose’ Shorts",
					CategoryId = Guid.Parse("6f026985-b5c2-4b05-9ac4-088b88982ec8"), 
					Price = 62.95m,
					MainImageUrl = "https://eustore.weightliftinghouse.com/cdn/shop/files/18_75ea3d05-1e8b-4fbc-af35-a41fb16930b3.jpg",
					CreatedAt = new DateOnly(2026, 2, 10),
					UpdatedAt = new DateOnly(2026, 2, 10),
					IsPublished = true
				},
				new Product
				{
					Id = Guid.Parse("467ea21c-0fcf-4dde-801a-17e77ee4a5b2"), //5
					Name = "Nasar I Win, You Lose Leggings",
					Slug = "nasar-i-win-you-lose-leggings",
					Description = "The official t-shirt of Olympic Champion Karlos Nasar. Made in collaboration with the greatest weightlifter of a generation, the Nasar ‘I Win, You Lose’ Leggings",
					CategoryId = Guid.Parse("6f026985-b5c2-4b05-9ac4-088b88982ec8"),
					Price = 70m,
					MainImageUrl = "https://eustore.weightliftinghouse.com/cdn/shop/files/13_2144736c-1165-42a7-87f7-c90db6a3c274.jpg",
					CreatedAt = new DateOnly(2026, 2, 10),
					UpdatedAt = new DateOnly(2026, 2, 10),
					IsPublished = true
				},
				new Product
				{
					Id = Guid.Parse("ac8a1ca5-d799-435f-8b6b-f4161ac2e284"), //6
					Name = "LUXIAOJUN PowerPro Weightlifting Shoes (Karlos)",
					Slug = "luxiaojun-powerpro-weightlifting-shoes-karlos",
					Description = "When Karlos Nasar shattered the world record, he did it in LUXIAOJUN lifters. That moment wasn’t just victory. It was domination. To celebrate his legacy and mindset, we present the Karlos Edition: built for lifters who don't just compete, they take over.",
					CategoryId = Guid.Parse("a9c9243a-aca9-4d3c-8f7a-4bdeb4578682"),
					Price = 195m,
					MainImageUrl = "https://luxiaojun.com/cdn/shop/files/LUXIAOJUN_PowerPro_Weightlifting_Shoes_Karlos_Edition_Pair.jpg",
					CreatedAt = new DateOnly(2026, 2, 10),
					UpdatedAt = new DateOnly(2026, 2, 10),
					IsPublished = true
				},
				new Product
				{
					Id = Guid.Parse("a8bcb2ba-bd47-4370-a443-505776baebb3"), //7
					Name = "GOLD STANDARD 100% WHEY",
					Slug = "gold-standard-100-whey",
					Description = "Fuel your performance with Gold Standard 100% Whey — a premium protein powder built for muscle support and post-workout recovery. Featuring 24g of protein and 5.5g of BCAAs per serving, with whey protein isolate as the No. 1 ingredient.",
					CategoryId = Guid.Parse("f1f47314-cdc4-4104-a9f2-a2ccf38e9623"),
					Price = 99.99m,
					MainImageUrl = "https://www.optimumnutrition.com/cdn/shop/files/on-1101490_Image_01.jpg",
					CreatedAt = new DateOnly(2026, 2, 10),
					UpdatedAt = new DateOnly(2026, 2, 10),
					IsPublished = true
				},
				new Product
				{
					Id = Guid.Parse("c7f9f6ad-8512-49b0-bb48-980c3922fe60"), //8
					Name = "Micronized Creatine Powder",
					Slug = "micronized-creatine-powder",
					Description = "Boost your performance with Micronized Creatine Powder — designed to support muscle strength and power, explosive movements, and ATP recycling. Delivering 5g of creatine monohydrate per serving and stackable with Gold Standard 100% Whey for complete muscle support.",
					CategoryId = Guid.Parse("f1f47314-cdc4-4104-a9f2-a2ccf38e9623"),
					Price = 30m,
					MainImageUrl = "https://www.optimumnutrition.com/cdn/shop/files/on-1102271_Image_01.png",
					CreatedAt = new DateOnly(2026, 2, 10),
					UpdatedAt = new DateOnly(2026, 2, 10),
					IsPublished = true
				},
				new Product
				{
					Id = Guid.Parse("c4ed28d4-92cb-4532-8949-044be6a66e28"), //9
					Name = "Nasar House Straps",
					Slug = "nasar-house-straps",
					Description = "Designed by Olympic Champion Karlos Nasar to elevate your training. These classic quick-release straps offer performance and durability with Karlos’s signature branding.",
					CategoryId = Guid.Parse("26a46720-ae3a-43bd-b5de-2e2dd4048dee"), 
					Price = 26m,
					MainImageUrl = "https://eustore.weightliftinghouse.com/cdn/shop/files/nasar_house_1.jpg",
					CreatedAt = new DateOnly(2026, 2, 10),
					UpdatedAt = new DateOnly(2026, 2, 10),
					IsPublished = true
				},//
                new Product
                {
                    Id = Guid.Parse("3b89e3a0-8d5a-4e2b-9e45-8f6a9c1d3e2a"),
                    Name = "Elite Leather Belt",
                    Slug = "elite-leather-belt",
                    Description = "The extra soft leather makes this belt feel like it’s been yours for years, providing you with the support and strength you need in training and competition.",
                    CategoryId = Guid.Parse("26a46720-ae3a-43bd-b5de-2e2dd4048dee"), // Accessories
                    Price = 110.00m,
                    MainImageUrl = "https://eustore.weightliftinghouse.com/cdn/shop/files/brownbelt1.png",
                    CreatedAt = new DateOnly(2026, 2, 10),
                    UpdatedAt = new DateOnly(2026, 2, 10),
                    IsPublished = true
                },
                new Product
                {
                    Id = Guid.Parse("7f1c2d9e-4b6a-485c-89a1-2d3e4f5a6b7c"),
                    Name = "Lasha Signature Leather Belt",
                    Slug = "lasha-signature-leather-belt",
                    Description = "Lasha’s 492kg total changed the game. Now it’s time to change yours with the Lasha Signature Leather Belt - in collaboration with Lasha Talakhadze.",
                    CategoryId = Guid.Parse("26a46720-ae3a-43bd-b5de-2e2dd4048dee"), // Accessories
                    Price = 135.00m,
                    MainImageUrl = "https://eustore.weightliftinghouse.com/cdn/shop/files/50_1846030d-fcec-4ca4-88c8-25da74598781.jpg",
                    CreatedAt = new DateOnly(2026, 2, 10),
                    UpdatedAt = new DateOnly(2026, 2, 10),
                    IsPublished = true
                },//
                new Product
                {
                    Id = Guid.Parse("9e8d7c6b-5a4f-3e2d-1c9b-8a7b6c5d4e3f"),
                    Name = "Nasar Neoprene Belt",
                    Slug = "nasar-neoprene-belt",
                    Description = "Karlos took the existing WLHOUSE neoprene belt which he deemed perfect and added his own personalised branding and signature, allowing his fans who prefer velcro to still be a part of his collection.",
                    CategoryId = Guid.Parse("26a46720-ae3a-43bd-b5de-2e2dd4048dee"), // Accessories
                    Price = 65.00m,
                    MainImageUrl = "https://eustore.weightliftinghouse.com/cdn/shop/files/nasar_neoprene_2.jpg?",
                    CreatedAt = new DateOnly(2026, 2, 10),
                    UpdatedAt = new DateOnly(2026, 2, 10),
                    IsPublished = true
                },
                new Product
                {
                    Id = Guid.Parse("1a2b3c4d-5e6f-7a8b-9c0d-e1f2a3b4c5d6"),
                    Name = "Nasar 7mm Soft Knee Sleeves",
                    Slug = "nasar-7mm-soft-knee-sleeves",
                    Description = "Designed by Olympic Champion Karlos Nasar with one purpose in mind—to break more world records. Now you can lift in the exact same knee sleeves as the greatest weightlifter of the generation.",
                    CategoryId = Guid.Parse("26a46720-ae3a-43bd-b5de-2e2dd4048dee"), // Accessories
                    Price = 97.50m,
                    MainImageUrl = "https://eustore.weightliftinghouse.com/cdn/shop/files/nasar_sleeves_1.jpg",
                    CreatedAt = new DateOnly(2026, 2, 10),
                    UpdatedAt = new DateOnly(2026, 2, 10),
                    IsPublished = true
                },
                new Product
                {
                    Id = Guid.Parse("d6c5b4a3-2f1e-d0c9-b8a7-f6e5d4c3b2a1"),
                    Name = "Heavy Duty 7mm Knee Sleeves",
                    Slug = "heavy-duty-7mm-knee-sleeves",
                    Description = "Our Heavy Duty 7mm Knee Sleeves are designed to have an immediate impact on your leg strength. Built with tough neoprene and durable external materials they are the most powerful knee sleeves in weightlifting.",
                    CategoryId = Guid.Parse("26a46720-ae3a-43bd-b5de-2e2dd4048dee"), // Accessories
                    Price = 79.95m,
                    MainImageUrl = "https://eustore.weightliftinghouse.com/cdn/shop/files/66.png",
                    CreatedAt = new DateOnly(2026, 2, 10),
                    UpdatedAt = new DateOnly(2026, 2, 10),
                    IsPublished = true
                },
                new Product
                {
                    Id = Guid.Parse("e5d4c3b2-a1f0-e9d8-c7b6-a5b4c3d2e1f0"),
                    Name = "Chinese 1.5mm Knee Sleeves",
                    Slug = "chinese-1.5mm-knee-sleeves",
                    Description = "Lift with an optimal combination of warmth and support in our Chinese-style Weightlifting Knee Sleeves. At just 1.5mm thick, these sleeves offer warmth and comfort to the knee without compromising any mobility, perfect for snatches, cleans and deep squats.",
                    CategoryId = Guid.Parse("26a46720-ae3a-43bd-b5de-2e2dd4048dee"), // Accessories
                    Price = 39.99m,
                    MainImageUrl = "https://eustore.weightliftinghouse.com/cdn/shop/files/93.png",
                    CreatedAt = new DateOnly(2026, 2, 10),
                    UpdatedAt = new DateOnly(2026, 2, 10),
                    IsPublished = true
                },
                new Product
                {
                    Id = Guid.Parse("f0e1d2c3-b4a5-6b7c-8d9e-0f1a2b3c4d5e"),
                    Name = "Leather Wrist Wraps",
                    Slug = "leather-wrist-wraps",
                    Description = "Enjoy extra wrist support with the WLHOUSE Leather Wrist Wraps. Soft genuine leather and a stainless steel buckle provide strength and support where it’s needed.",
                    CategoryId = Guid.Parse("26a46720-ae3a-43bd-b5de-2e2dd4048dee"), // Accessories
                    Price = 35.00m,
                    MainImageUrl = "https://eustore.weightliftinghouse.com/cdn/shop/files/88.png",
                    CreatedAt = new DateOnly(2026, 2, 10),
                    UpdatedAt = new DateOnly(2026, 2, 10),
                    IsPublished = true
                },
                new Product
                {
                    Id = Guid.Parse("5e4d3c2b-1a0f-e9d8-c7b6-a5b4c3d2e1f0"),
                    Name = "Weightlifting Thumb Tape",
                    Slug = "weightlifting-thumb-tape",
                    Description = "Protecting the thumbs of weightlifters since 2019. If you’ve been to a weightlifting gym you’ve seen Weightlifting House Thumb Tape.",
                    CategoryId = Guid.Parse("26a46720-ae3a-43bd-b5de-2e2dd4048dee"), // Accessories
                    Price = 14.95m,
                    MainImageUrl = "https://eustore.weightliftinghouse.com/cdn/shop/files/4.jpg",
                    CreatedAt = new DateOnly(2026, 2, 10),
                    UpdatedAt = new DateOnly(2026, 2, 10),
                    IsPublished = true
                },
                new Product
                {
                    Id = Guid.Parse("c3d2e1f0-a5b4-c7b6-e9d8-1a0f5e4d3c2b"),
                    Name = "90s Training Sweatshirt",
                    Slug = "90s-training-sweatshirt",
                    Description = "Inspired by the early ’90s training hall aesthetic, most memorably Ronnie Weller at the ’93 World Champs, featuring a WLHOUSE logo on the back that echoes Team Japan’s 1990 kit.",
                    CategoryId = Guid.Parse("cb8ceb6a-6623-4937-a6c4-777b4b498e1e"), // Upper Body
                    Price = 30.00m,
                    MainImageUrl = "https://eustore.weightliftinghouse.com/cdn/shop/files/Jumperblack_red1.jpg",
                    CreatedAt = new DateOnly(2026, 2, 10),
                    UpdatedAt = new DateOnly(2026, 2, 10),
                    IsPublished = true
                },
                new Product
                {
                    Id = Guid.Parse("b4c5d6e7-f8a9-0b1c-2d3e-4f5a6b7c8d9e"),
                    Name = "Rahmat Unchained T-Shirt",
                    Slug = "rahmat-unchained-tshirt",
                    Description = "In collaboration with one of the greatest clean & jerkers of the generation comes the official WLHOUSE x Rahmat collab, featuring the Rahmat Unchained T-Shirt.",
                    CategoryId = Guid.Parse("cb8ceb6a-6623-4937-a6c4-777b4b498e1e"), // Upper Body
                    Price = 35.00m,
                    MainImageUrl = "https://eustore.weightliftinghouse.com/cdn/shop/files/Rahmattee1.jpg",
                    CreatedAt = new DateOnly(2026, 2, 10),
                    UpdatedAt = new DateOnly(2026, 2, 10),
                    IsPublished = true
                },
                new Product
                {
                    Id = Guid.Parse("a1b2c3d4-e5f6-7a8b-9c0d-1a2b3c4d5e6f"),
                    Name = "90s Training Sweatpants",
                    Slug = "90s-training-sweatpants",
                    Description = "Inspired by the baggy, oversized looks of the ’90s, elevated with the soft, stretchy fabrics of today, featuring an elasticated waistband, cuffed legs and a soft fleece interior. ",
                    CategoryId = Guid.Parse("6f026985-b5c2-4b05-9ac4-088b88982ec8"), // Lower Body
                    Price = 65.00m,
                    MainImageUrl = "https://eustore.weightliftinghouse.com/cdn/shop/files/BlackPants1.jpg",
                    CreatedAt = new DateOnly(2026, 2, 10),
                    UpdatedAt = new DateOnly(2026, 2, 10),
                    IsPublished = true
                }
            ];
		}
	}
}
