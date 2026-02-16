
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


            /* Starting Seeds */

            //User Section
            var adminRole = new IdentityRole
            {
                Id = "d333796b-4dd0-410c-92fd-481a6dc3fd0d",
                Name = "Admin",
                NormalizedName = "ADMIN"
            };

            builder.Entity<IdentityRole>().HasData(adminRole);

            var adminUser = new IdentityUser
            {
                Id = "06313180-fa45-42b5-ac33-1333f673455d",
                UserName = "admin@labs.com",
                NormalizedUserName = "ADMIN@LABS.COM",
                Email = "admin@labs.com",
                NormalizedEmail = "ADMIN@LABS.COM",
                EmailConfirmed = true,
                ConcurrencyStamp = "4bfdb153-a446-4968-a40f-34adc37a6f28",
                SecurityStamp = "c0142471-6ffd-44b4-b430-8b3c7acf8fbf"

			};

            adminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(adminUser, "admin");


			builder.Entity<IdentityUser>().HasData(adminUser);

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = adminUser.Id,
                    RoleId = adminRole.Id
                }
            );

            //Category Section
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Upper Body", Slug = "upper-body" },
                new Category { Id = 2, Name = "Lower Body", Slug = "lower-body" },
                new Category { Id = 3, Name = "Shoes", Slug = "shoes" },
                new Category { Id = 4, Name = "Supplements", Slug = "supplements" },
                new Category { Id = 5, Name = "Accessories", Slug = "accessories" }
            );

            //Product Section
            builder.Entity<Product>().HasData(
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
            );

            //ProductVariant Section 
            builder.Entity<ProductVariant>().HasData(
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
            );


        }
    }
}
