using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MaxEndLabs.Data.Migrations.PostgresMigrations
{
    /// <inheritdoc />
    public partial class SeedUserOrdersOrderItemsProductsProductVariants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709"), 0, "a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d", "test@labs.com", true, "Bill Clinton", false, null, "TEST@LABS.COM", "TEST@LABS.COM", "AQAAAAIAAYagAAAAEP9JzX0YwKm6N+luPkuHrobdVlv+8FQxWWME12rmICorZgnbmrHziPC+5WxRG5/6aw==", null, false, "12345678-1234-1234-1234-123456789abc", false, "test@labs.com" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "IsPublished", "MainImageUrl", "Name", "Price", "Slug", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("1a2b3c4d-5e6f-7a8b-9c0d-e1f2a3b4c5d6"), new Guid("26a46720-ae3a-43bd-b5de-2e2dd4048dee"), new DateOnly(2026, 2, 10), "Designed by Olympic Champion Karlos Nasar with one purpose in mind—to break more world records. Now you can lift in the exact same knee sleeves as the greatest weightlifter of the generation.", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/nasar_sleeves_1.jpg", "Nasar 7mm Soft Knee Sleeves", 97.50m, "nasar-7mm-soft-knee-sleeves", new DateOnly(2026, 2, 10) },
                    { new Guid("3b89e3a0-8d5a-4e2b-9e45-8f6a9c1d3e2a"), new Guid("26a46720-ae3a-43bd-b5de-2e2dd4048dee"), new DateOnly(2026, 2, 10), "The extra soft leather makes this belt feel like it’s been yours for years, providing you with the support and strength you need in training and competition.", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/brownbelt1.png", "Elite Leather Belt", 110.00m, "elite-leather-belt", new DateOnly(2026, 2, 10) },
                    { new Guid("5e4d3c2b-1a0f-e9d8-c7b6-a5b4c3d2e1f0"), new Guid("26a46720-ae3a-43bd-b5de-2e2dd4048dee"), new DateOnly(2026, 2, 10), "Protecting the thumbs of weightlifters since 2019. If you’ve been to a weightlifting gym you’ve seen Weightlifting House Thumb Tape.", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/4.jpg", "Weightlifting Thumb Tape", 14.95m, "weightlifting-thumb-tape", new DateOnly(2026, 2, 10) },
                    { new Guid("7f1c2d9e-4b6a-485c-89a1-2d3e4f5a6b7c"), new Guid("26a46720-ae3a-43bd-b5de-2e2dd4048dee"), new DateOnly(2026, 2, 10), "Lasha’s 492kg total changed the game. Now it’s time to change yours with the Lasha Signature Leather Belt - in collaboration with Lasha Talakhadze.", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/50_1846030d-fcec-4ca4-88c8-25da74598781.jpg", "Lasha Signature Leather Belt", 135.00m, "lasha-signature-leather-belt", new DateOnly(2026, 2, 10) },
                    { new Guid("9e8d7c6b-5a4f-3e2d-1c9b-8a7b6c5d4e3f"), new Guid("26a46720-ae3a-43bd-b5de-2e2dd4048dee"), new DateOnly(2026, 2, 10), "Karlos took the existing WLHOUSE neoprene belt which he deemed perfect and added his own personalised branding and signature, allowing his fans who prefer velcro to still be a part of his collection.", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/nasar_neoprene_2.jpg?", "Nasar Neoprene Belt", 65.00m, "nasar-neoprene-belt", new DateOnly(2026, 2, 10) },
                    { new Guid("a1b2c3d4-e5f6-7a8b-9c0d-1a2b3c4d5e6f"), new Guid("6f026985-b5c2-4b05-9ac4-088b88982ec8"), new DateOnly(2026, 2, 10), "Inspired by the baggy, oversized looks of the ’90s, elevated with the soft, stretchy fabrics of today, featuring an elasticated waistband, cuffed legs and a soft fleece interior. ", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/BlackPants1.jpg", "90s Training Sweatpants", 65.00m, "90s-training-sweatpants", new DateOnly(2026, 2, 10) },
                    { new Guid("b4c5d6e7-f8a9-0b1c-2d3e-4f5a6b7c8d9e"), new Guid("cb8ceb6a-6623-4937-a6c4-777b4b498e1e"), new DateOnly(2026, 2, 10), "In collaboration with one of the greatest clean & jerkers of the generation comes the official WLHOUSE x Rahmat collab, featuring the Rahmat Unchained T-Shirt.", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/Rahmattee1.jpg", "Rahmat Unchained T-Shirt", 35.00m, "rahmat-unchained-tshirt", new DateOnly(2026, 2, 10) },
                    { new Guid("c3d2e1f0-a5b4-c7b6-e9d8-1a0f5e4d3c2b"), new Guid("cb8ceb6a-6623-4937-a6c4-777b4b498e1e"), new DateOnly(2026, 2, 10), "Inspired by the early ’90s training hall aesthetic, most memorably Ronnie Weller at the ’93 World Champs, featuring a WLHOUSE logo on the back that echoes Team Japan’s 1990 kit.", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/Jumperblack_red1.jpg", "90s Training Sweatshirt", 30.00m, "90s-training-sweatshirt", new DateOnly(2026, 2, 10) },
                    { new Guid("d6c5b4a3-2f1e-d0c9-b8a7-f6e5d4c3b2a1"), new Guid("26a46720-ae3a-43bd-b5de-2e2dd4048dee"), new DateOnly(2026, 2, 10), "Our Heavy Duty 7mm Knee Sleeves are designed to have an immediate impact on your leg strength. Built with tough neoprene and durable external materials they are the most powerful knee sleeves in weightlifting.", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/66.png", "Heavy Duty 7mm Knee Sleeves", 79.95m, "heavy-duty-7mm-knee-sleeves", new DateOnly(2026, 2, 10) },
                    { new Guid("e5d4c3b2-a1f0-e9d8-c7b6-a5b4c3d2e1f0"), new Guid("26a46720-ae3a-43bd-b5de-2e2dd4048dee"), new DateOnly(2026, 2, 10), "Lift with an optimal combination of warmth and support in our Chinese-style Weightlifting Knee Sleeves. At just 1.5mm thick, these sleeves offer warmth and comfort to the knee without compromising any mobility, perfect for snatches, cleans and deep squats.", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/93.png", "Chinese 1.5mm Knee Sleeves", 39.99m, "chinese-1.5mm-knee-sleeves", new DateOnly(2026, 2, 10) },
                    { new Guid("f0e1d2c3-b4a5-6b7c-8d9e-0f1a2b3c4d5e"), new Guid("26a46720-ae3a-43bd-b5de-2e2dd4048dee"), new DateOnly(2026, 2, 10), "Enjoy extra wrist support with the WLHOUSE Leather Wrist Wraps. Soft genuine leather and a stainless steel buckle provide strength and support where it’s needed.", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/88.png", "Leather Wrist Wraps", 35.00m, "leather-wrist-wraps", new DateOnly(2026, 2, 10) }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("e444796b-4dd0-410c-92fd-481a6dc3fd0d"), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "City", "CreatedAt", "OrderNumber", "Postcode", "Status", "StreetAddress", "TotalAmount", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { new Guid("11111111-2222-3333-4444-000000000001"), "Sofia", new DateTime(2026, 1, 2, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-20260401-11BC", "1000", 2, "1 Demo User Avenue", 99.99m, new DateTime(2026, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000002"), "Sofia", new DateTime(2026, 1, 3, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-20260402-12BC", "1000", 2, "1 Demo User Avenue", 66.00m, new DateTime(2026, 1, 3, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000003"), "Sofia", new DateTime(2026, 1, 4, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-20260403-13BC", "1000", 2, "1 Demo User Avenue", 99.99m, new DateTime(2026, 1, 4, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000004"), "Sofia", new DateTime(2026, 1, 5, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-20260404-14BC", "1000", 4, "1 Demo User Avenue", 66.00m, new DateTime(2026, 1, 5, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000005"), "Sofia", new DateTime(2026, 1, 6, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-20260405-15BC", "1000", 2, "1 Demo User Avenue", 99.99m, new DateTime(2026, 1, 6, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000006"), "Sofia", new DateTime(2026, 1, 7, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-20260406-16BC", "1000", 2, "1 Demo User Avenue", 66.00m, new DateTime(2026, 1, 7, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000007"), "Sofia", new DateTime(2026, 1, 8, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-20260407-17BC", "1000", 2, "1 Demo User Avenue", 99.99m, new DateTime(2026, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000008"), "Sofia", new DateTime(2026, 1, 9, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-20260408-18BC", "1000", 4, "1 Demo User Avenue", 66.00m, new DateTime(2026, 1, 9, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000009"), "Sofia", new DateTime(2026, 1, 10, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-20260409-19BC", "1000", 2, "1 Demo User Avenue", 99.99m, new DateTime(2026, 1, 10, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000010"), "Sofia", new DateTime(2026, 1, 11, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-202604010-20BC", "1000", 2, "1 Demo User Avenue", 66.00m, new DateTime(2026, 1, 11, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000011"), "Sofia", new DateTime(2026, 1, 12, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-202604011-21BC", "1000", 2, "1 Demo User Avenue", 99.99m, new DateTime(2026, 1, 12, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000012"), "Sofia", new DateTime(2026, 1, 13, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-202604012-22BC", "1000", 4, "1 Demo User Avenue", 66.00m, new DateTime(2026, 1, 13, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000013"), "Sofia", new DateTime(2026, 1, 14, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-202604013-23BC", "1000", 2, "1 Demo User Avenue", 99.99m, new DateTime(2026, 1, 14, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000014"), "Sofia", new DateTime(2026, 1, 15, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-202604014-24BC", "1000", 2, "1 Demo User Avenue", 66.00m, new DateTime(2026, 1, 15, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000015"), "Sofia", new DateTime(2026, 1, 16, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-202604015-25BC", "1000", 2, "1 Demo User Avenue", 99.99m, new DateTime(2026, 1, 16, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000016"), "Sofia", new DateTime(2026, 1, 17, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-202604016-26BC", "1000", 4, "1 Demo User Avenue", 66.00m, new DateTime(2026, 1, 17, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000017"), "Sofia", new DateTime(2026, 1, 18, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-202604017-27BC", "1000", 2, "1 Demo User Avenue", 99.99m, new DateTime(2026, 1, 18, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000018"), "Sofia", new DateTime(2026, 1, 19, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-202604018-28BC", "1000", 2, "1 Demo User Avenue", 66.00m, new DateTime(2026, 1, 19, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000019"), "Sofia", new DateTime(2026, 1, 20, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-202604019-29BC", "1000", 2, "1 Demo User Avenue", 99.99m, new DateTime(2026, 1, 20, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000020"), "Sofia", new DateTime(2026, 1, 21, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-202604020-30BC", "1000", 4, "1 Demo User Avenue", 66.00m, new DateTime(2026, 1, 21, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000021"), "Sofia", new DateTime(2026, 1, 22, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-202604021-31BC", "1000", 2, "1 Demo User Avenue", 99.99m, new DateTime(2026, 1, 22, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000022"), "Sofia", new DateTime(2026, 1, 23, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-202604022-32BC", "1000", 2, "1 Demo User Avenue", 66.00m, new DateTime(2026, 1, 23, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000023"), "Sofia", new DateTime(2026, 1, 24, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-202604023-33BC", "1000", 2, "1 Demo User Avenue", 99.99m, new DateTime(2026, 1, 24, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000024"), "Sofia", new DateTime(2026, 1, 25, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-202604024-34BC", "1000", 4, "1 Demo User Avenue", 66.00m, new DateTime(2026, 1, 25, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000025"), "Sofia", new DateTime(2026, 1, 26, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-202604025-35BC", "1000", 2, "1 Demo User Avenue", 99.99m, new DateTime(2026, 1, 26, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000026"), "Sofia", new DateTime(2026, 1, 27, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-202604026-36BC", "1000", 2, "1 Demo User Avenue", 66.00m, new DateTime(2026, 1, 27, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000027"), "Sofia", new DateTime(2026, 1, 28, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-202604027-37BC", "1000", 2, "1 Demo User Avenue", 99.99m, new DateTime(2026, 1, 28, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000028"), "Sofia", new DateTime(2026, 1, 29, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-202604028-38BC", "1000", 4, "1 Demo User Avenue", 66.00m, new DateTime(2026, 1, 29, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000029"), "Sofia", new DateTime(2026, 1, 30, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-202604029-39BC", "1000", 2, "1 Demo User Avenue", 99.99m, new DateTime(2026, 1, 30, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") },
                    { new Guid("11111111-2222-3333-4444-000000000030"), "Sofia", new DateTime(2026, 1, 31, 10, 0, 0, 0, DateTimeKind.Utc), "ORD-202604030-40BC", "1000", 2, "1 Demo User Avenue", 66.00m, new DateTime(2026, 1, 31, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") }
                });

            migrationBuilder.InsertData(
                table: "ProductVariants",
                columns: new[] { "Id", "IsDeleted", "Price", "ProductId", "VariantName" },
                values: new object[,]
                {
                    { new Guid("0c1d2e3f-4a5b-6c7d-8e9f-a0b1c2d3e4f5"), false, null, new Guid("5e4d3c2b-1a0f-e9d8-c7b6-a5b4c3d2e1f0"), "Color: Black and Gold" },
                    { new Guid("1b8c3a9d-7e42-4658-8d1f-b3a2c5e9d411"), false, null, new Guid("9e8d7c6b-5a4f-3e2d-1c9b-8a7b6c5d4e3f"), "Color: Black" },
                    { new Guid("2a3b4c5d-6e7f-8a9b-0c1d-e1f2a3b4c5d6"), false, null, new Guid("e5d4c3b2-a1f0-e9d8-c7b6-a5b4c3d2e1f0"), "Color: Blue" },
                    { new Guid("5c4d9e2a-1b8f-4a36-9d7c-8e1f2a3b4c55"), false, null, new Guid("1a2b3c4d-5e6f-7a8b-9c0d-e1f2a3b4c5d6"), "Color: Black and Green" },
                    { new Guid("6e7f8a9b-0c1d-2e3f-4a5b-c6d7e8f9a0b1"), false, null, new Guid("f0e1d2c3-b4a5-6b7c-8d9e-0f1a2b3c4d5e"), "Color: Blue" },
                    { new Guid("8d12e9b4-3a76-4f81-b51c-918d2a6c4f33"), false, null, new Guid("7f1c2d9e-4b6a-485c-89a1-2d3e4f5a6b7c"), "Color: White and Red" },
                    { new Guid("9e1f2a3b-4c5d-6e7f-8a9b-0c1d2e3f4a55"), false, null, new Guid("d6c5b4a3-2f1e-d0c9-b8a7-f6e5d4c3b2a1"), "Color: Black" },
                    { new Guid("a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d"), false, null, new Guid("c3d2e1f0-a5b4-c7b6-e9d8-1a0f5e4d3c2b"), "Size S / Color: Black" },
                    { new Guid("a7b8c9d0-e1f2-4a3b-4c5d-6e7f8a9b0c1d"), false, null, new Guid("b4c5d6e7-f8a9-0b1c-2d3e-4f5a6b7c8d9e"), "Size L / Color: Black" },
                    { new Guid("b2c3d4e5-f6a7-4b8c-9d0e-1f2a3b4c5d6e"), false, null, new Guid("c3d2e1f0-a5b4-c7b6-e9d8-1a0f5e4d3c2b"), "Size M / Color: Black" },
                    { new Guid("b8c9d0e1-f2a3-4b4c-5d6e-7f8a9b0c1d2e"), false, null, new Guid("b4c5d6e7-f8a9-0b1c-2d3e-4f5a6b7c8d9e"), "Size XL / Color: Black" },
                    { new Guid("c3d4e5f6-a7b8-4c9d-0e1f-2a3b4c5d6e7f"), false, null, new Guid("c3d2e1f0-a5b4-c7b6-e9d8-1a0f5e4d3c2b"), "Size L / Color: Black" },
                    { new Guid("c9d0e1f2-a3b4-4c5d-6e7f-8a9b0c1d2e3f"), false, null, new Guid("a1b2c3d4-e5f6-7a8b-9c0d-1a2b3c4d5e6f"), "Size S / Color: Black" },
                    { new Guid("d0e1f2a3-b4c5-4d6e-7f8a-9b0c1d2e3f4a"), false, null, new Guid("a1b2c3d4-e5f6-7a8b-9c0d-1a2b3c4d5e6f"), "Size M / Color: Black" },
                    { new Guid("d4e5f6a7-b8c9-4d0e-1f2a-3b4c5d6e7f8a"), false, null, new Guid("c3d2e1f0-a5b4-c7b6-e9d8-1a0f5e4d3c2b"), "Size XL / Color: Black" },
                    { new Guid("e1f2a3b4-c5d6-4e7f-8a9b-0c1d2e3f4a5b"), false, null, new Guid("a1b2c3d4-e5f6-7a8b-9c0d-1a2b3c4d5e6f"), "Size L / Color: Black" },
                    { new Guid("e5f6a7b8-c9d0-4e1f-2a3b-4c5d6e7f8a9b"), false, null, new Guid("b4c5d6e7-f8a9-0b1c-2d3e-4f5a6b7c8d9e"), "Size S / Color: Black" },
                    { new Guid("f2a3b4c5-d6e7-4f8a-9b0c-1d2e3f4a5b6c"), false, null, new Guid("a1b2c3d4-e5f6-7a8b-9c0d-1a2b3c4d5e6f"), "Size XL / Color: Black" },
                    { new Guid("f472b8d1-9c65-4d1a-a89e-32cf4b918a11"), false, null, new Guid("3b89e3a0-8d5a-4e2b-9e45-8f6a9c1d3e2a"), "Color: Brown" },
                    { new Guid("f6a7b8c9-d0e1-4f2a-3b4c-5d6e7f8a9b0c"), false, null, new Guid("b4c5d6e7-f8a9-0b1c-2d3e-4f5a6b7c8d9e"), "Size M / Color: Black" }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "OrderId", "ProductId", "ProductVariantId", "LineTotal", "Quantity", "UnitPrice" },
                values: new object[,]
                {
                    { new Guid("11111111-2222-3333-4444-000000000001"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26"), 99.99m, 1, 99.99m },
                    { new Guid("11111111-2222-3333-4444-000000000002"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8"), 40.00m, 1, 40.00m },
                    { new Guid("11111111-2222-3333-4444-000000000002"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265"), 26.00m, 1, 26.00m },
                    { new Guid("11111111-2222-3333-4444-000000000003"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26"), 99.99m, 1, 99.99m },
                    { new Guid("11111111-2222-3333-4444-000000000004"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8"), 40.00m, 1, 40.00m },
                    { new Guid("11111111-2222-3333-4444-000000000004"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265"), 26.00m, 1, 26.00m },
                    { new Guid("11111111-2222-3333-4444-000000000005"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26"), 99.99m, 1, 99.99m },
                    { new Guid("11111111-2222-3333-4444-000000000006"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8"), 40.00m, 1, 40.00m },
                    { new Guid("11111111-2222-3333-4444-000000000006"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265"), 26.00m, 1, 26.00m },
                    { new Guid("11111111-2222-3333-4444-000000000007"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26"), 99.99m, 1, 99.99m },
                    { new Guid("11111111-2222-3333-4444-000000000008"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8"), 40.00m, 1, 40.00m },
                    { new Guid("11111111-2222-3333-4444-000000000008"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265"), 26.00m, 1, 26.00m },
                    { new Guid("11111111-2222-3333-4444-000000000009"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26"), 99.99m, 1, 99.99m },
                    { new Guid("11111111-2222-3333-4444-000000000010"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8"), 40.00m, 1, 40.00m },
                    { new Guid("11111111-2222-3333-4444-000000000010"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265"), 26.00m, 1, 26.00m },
                    { new Guid("11111111-2222-3333-4444-000000000011"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26"), 99.99m, 1, 99.99m },
                    { new Guid("11111111-2222-3333-4444-000000000012"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8"), 40.00m, 1, 40.00m },
                    { new Guid("11111111-2222-3333-4444-000000000012"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265"), 26.00m, 1, 26.00m },
                    { new Guid("11111111-2222-3333-4444-000000000013"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26"), 99.99m, 1, 99.99m },
                    { new Guid("11111111-2222-3333-4444-000000000014"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8"), 40.00m, 1, 40.00m },
                    { new Guid("11111111-2222-3333-4444-000000000014"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265"), 26.00m, 1, 26.00m },
                    { new Guid("11111111-2222-3333-4444-000000000015"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26"), 99.99m, 1, 99.99m },
                    { new Guid("11111111-2222-3333-4444-000000000016"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8"), 40.00m, 1, 40.00m },
                    { new Guid("11111111-2222-3333-4444-000000000016"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265"), 26.00m, 1, 26.00m },
                    { new Guid("11111111-2222-3333-4444-000000000017"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26"), 99.99m, 1, 99.99m },
                    { new Guid("11111111-2222-3333-4444-000000000018"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8"), 40.00m, 1, 40.00m },
                    { new Guid("11111111-2222-3333-4444-000000000018"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265"), 26.00m, 1, 26.00m },
                    { new Guid("11111111-2222-3333-4444-000000000019"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26"), 99.99m, 1, 99.99m },
                    { new Guid("11111111-2222-3333-4444-000000000020"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8"), 40.00m, 1, 40.00m },
                    { new Guid("11111111-2222-3333-4444-000000000020"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265"), 26.00m, 1, 26.00m },
                    { new Guid("11111111-2222-3333-4444-000000000021"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26"), 99.99m, 1, 99.99m },
                    { new Guid("11111111-2222-3333-4444-000000000022"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8"), 40.00m, 1, 40.00m },
                    { new Guid("11111111-2222-3333-4444-000000000022"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265"), 26.00m, 1, 26.00m },
                    { new Guid("11111111-2222-3333-4444-000000000023"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26"), 99.99m, 1, 99.99m },
                    { new Guid("11111111-2222-3333-4444-000000000024"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8"), 40.00m, 1, 40.00m },
                    { new Guid("11111111-2222-3333-4444-000000000024"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265"), 26.00m, 1, 26.00m },
                    { new Guid("11111111-2222-3333-4444-000000000025"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26"), 99.99m, 1, 99.99m },
                    { new Guid("11111111-2222-3333-4444-000000000026"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8"), 40.00m, 1, 40.00m },
                    { new Guid("11111111-2222-3333-4444-000000000026"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265"), 26.00m, 1, 26.00m },
                    { new Guid("11111111-2222-3333-4444-000000000027"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26"), 99.99m, 1, 99.99m },
                    { new Guid("11111111-2222-3333-4444-000000000028"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8"), 40.00m, 1, 40.00m },
                    { new Guid("11111111-2222-3333-4444-000000000028"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265"), 26.00m, 1, 26.00m },
                    { new Guid("11111111-2222-3333-4444-000000000029"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26"), 99.99m, 1, 99.99m },
                    { new Guid("11111111-2222-3333-4444-000000000030"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8"), 40.00m, 1, 40.00m },
                    { new Guid("11111111-2222-3333-4444-000000000030"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265"), 26.00m, 1, 26.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("e444796b-4dd0-410c-92fd-481a6dc3fd0d"), new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000001"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000002"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000002"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000003"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000004"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000004"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000005"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000006"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000006"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000007"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000008"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000008"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000009"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000010"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000010"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000011"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000012"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000012"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000013"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000014"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000014"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000015"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000016"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000016"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000017"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000018"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000018"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000019"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000020"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000020"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000021"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000022"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000022"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000023"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000024"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000024"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000025"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000026"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000026"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000027"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000028"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000028"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000029"), new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000030"), new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("75feaf94-8a87-495e-910e-01276c94d0c8") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId", "ProductVariantId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-000000000030"), new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265") });

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("0c1d2e3f-4a5b-6c7d-8e9f-a0b1c2d3e4f5"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("1b8c3a9d-7e42-4658-8d1f-b3a2c5e9d411"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("2a3b4c5d-6e7f-8a9b-0c1d-e1f2a3b4c5d6"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("5c4d9e2a-1b8f-4a36-9d7c-8e1f2a3b4c55"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("6e7f8a9b-0c1d-2e3f-4a5b-c6d7e8f9a0b1"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("8d12e9b4-3a76-4f81-b51c-918d2a6c4f33"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("9e1f2a3b-4c5d-6e7f-8a9b-0c1d2e3f4a55"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("a7b8c9d0-e1f2-4a3b-4c5d-6e7f8a9b0c1d"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-4b8c-9d0e-1f2a3b4c5d6e"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("b8c9d0e1-f2a3-4b4c-5d6e-7f8a9b0c1d2e"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-a7b8-4c9d-0e1f-2a3b4c5d6e7f"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("c9d0e1f2-a3b4-4c5d-6e7f-8a9b0c1d2e3f"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("d0e1f2a3-b4c5-4d6e-7f8a-9b0c1d2e3f4a"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("d4e5f6a7-b8c9-4d0e-1f2a-3b4c5d6e7f8a"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("e1f2a3b4-c5d6-4e7f-8a9b-0c1d2e3f4a5b"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("e5f6a7b8-c9d0-4e1f-2a3b-4c5d6e7f8a9b"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("f2a3b4c5-d6e7-4f8a-9b0c-1d2e3f4a5b6c"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("f472b8d1-9c65-4d1a-a89e-32cf4b918a11"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("f6a7b8c9-d0e1-4f2a-3b4c-5d6e7f8a9b0c"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000001"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000002"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000003"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000004"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000005"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000006"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000007"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000008"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000009"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000010"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000011"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000012"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000013"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000014"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000015"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000016"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000017"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000018"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000019"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000020"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000021"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000022"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000023"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000024"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000025"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000026"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000027"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000028"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000029"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-000000000030"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("1a2b3c4d-5e6f-7a8b-9c0d-e1f2a3b4c5d6"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("3b89e3a0-8d5a-4e2b-9e45-8f6a9c1d3e2a"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("5e4d3c2b-1a0f-e9d8-c7b6-a5b4c3d2e1f0"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("7f1c2d9e-4b6a-485c-89a1-2d3e4f5a6b7c"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("9e8d7c6b-5a4f-3e2d-1c9b-8a7b6c5d4e3f"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7a8b-9c0d-1a2b3c4d5e6f"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b4c5d6e7-f8a9-0b1c-2d3e-4f5a6b7c8d9e"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c3d2e1f0-a5b4-c7b6-e9d8-1a0f5e4d3c2b"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d6c5b4a3-2f1e-d0c9-b8a7-f6e5d4c3b2a1"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e5d4c3b2-a1f0-e9d8-c7b6-a5b4c3d2e1f0"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f0e1d2c3-b4a5-6b7c-8d9e-0f1a2b3c4d5e"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("9d2b0228-4d0d-4c23-8b49-01a698857709"));
        }
    }
}
