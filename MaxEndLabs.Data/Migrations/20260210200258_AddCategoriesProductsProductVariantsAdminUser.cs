using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MaxEndLabs.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoriesProductsProductVariantsAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDigital",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StockTracked",
                table: "Products");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "ProductVariants",
                type: "decimal(10,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d333796b-4dd0-410c-92fd-481a6dc3fd0d", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "06313180-fa45-42b5-ac33-1333f673455d", 0, "35b7be81-cad0-474e-8781-d36e28d2a329", "admin@maxendlabs.com", true, false, null, "ADMIN@MAXENDLABS.COM", "ADMIN", "AQAAAAIAAYagAAAAEEJveNlU2qlawcwIfv1zOPQlmfe9Ihsj2Cop/jB4mqzg1v2OFqb7RbeF5xMNI3DSRw==", null, false, "6afbe9b6-ab9a-4daa-9fe2-33610d75f108", false, "admin" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name", "ParentCategoryId", "Slug" },
                values: new object[,]
                {
                    { 1, null, "Upper Body", null, "upper-body" },
                    { 2, null, "Lower Body", null, "lower-body" },
                    { 3, null, "Shoes", null, "shoes" },
                    { 4, null, "Supplements", null, "supplements" },
                    { 5, null, "Accessories", null, "accessories" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "d333796b-4dd0-410c-92fd-481a6dc3fd0d", "06313180-fa45-42b5-ac33-1333f673455d" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "IsPublished", "MainImageUrl", "Name", "Price", "Slug", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2659), "The three words that motivated generations of weightlifters are back! As worn by Bulgaria’s -85kg snatch record holder from the ‘90s, Georgi Gardev. Oversized fit.", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/WBN1.jpg", "Why Be Normal T-shirt", 40m, "why-be-normal-tshirt", new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2663) },
                    { 2, 1, new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2667), "The official t-shirt of Olympic Champion Karlos Nasar. Made in collaboration with the greatest weightlifter of a generation, the Nasar ‘I Win, You Lose’ T-shirt.", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/6_155b8e55-8336-438c-a966-fdec261cd0cb.jpg", "Nasar I Win, You Lose T-shirt", 45m, "nasar-i-win-you-lose-tshirt", new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2667) },
                    { 3, 1, new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2670), "Celebrate the record-breaking power of Olympic Champion Karlos Nasar with this exclusive t-shirt. Featuring the iconic ‘404’ — marking the world and Olympic total record.", true, "https://karlosnasar.com/storage/products/371146981.jpg", "T-shirt 404", 30m, "tshirt-404", new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2671) },
                    { 4, 2, new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2674), "The official t-shirt of Olympic Champion Karlos Nasar. Made in collaboration with the greatest weightlifter of a generation, the Nasar ‘I Win, You Lose’ Shorts", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/18_75ea3d05-1e8b-4fbc-af35-a41fb16930b3.jpg", "Nasar I Win, You Lose Shorts", 62.95m, "nasar-i-win-you-lose-shorts", new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2674) },
                    { 5, 2, new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2676), "The official t-shirt of Olympic Champion Karlos Nasar. Made in collaboration with the greatest weightlifter of a generation, the Nasar ‘I Win, You Lose’ Leggings", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/13_2144736c-1165-42a7-87f7-c90db6a3c274.jpg", "Nasar I Win, You Lose Leggings", 70m, "nasar-i-win-you-lose-leggings", new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2677) },
                    { 6, 3, new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2678), "When Karlos Nasar shattered the world record, he did it in LUXIAOJUN lifters. That moment wasn’t just victory. It was domination. To celebrate his legacy and mindset, we present the Karlos Edition: built for lifters who don't just compete, they take over.", true, "https://luxiaojun.com/cdn/shop/files/LUXIAOJUN_PowerPro_Weightlifting_Shoes_Karlos_Edition_Pair.jpg", "LUXIAOJUN PowerPro Weightlifting Shoes (Karlos)", 195m, "luxiaojun-powerpro-weightlifting-shoes-karlos", new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2679) },
                    { 7, 4, new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2681), "Fuel your performance with Gold Standard 100% Whey — a premium protein powder built for muscle support and post-workout recovery. Featuring 24g of protein and 5.5g of BCAAs per serving, with whey protein isolate as the No. 1 ingredient.", true, "https://www.optimumnutrition.com/cdn/shop/files/on-1101490_Image_01.jpg", "GOLD STANDARD 100% WHEY", 99.99m, "gold-standard-100-whey", new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2681) },
                    { 8, 4, new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2683), "Boost your performance with Micronized Creatine Powder — designed to support muscle strength and power, explosive movements, and ATP recycling. Delivering 5g of creatine monohydrate per serving and stackable with Gold Standard 100% Whey for complete muscle support.", true, "https://www.optimumnutrition.com/cdn/shop/files/on-1102271_Image_01.png", "Micronized Creatine Powder", 30m, "micronized-creatine-powder", new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2684) },
                    { 9, 5, new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2686), "Designed by Olympic Champion Karlos Nasar to elevate your training. These classic quick-release straps offer performance and durability with Karlos’s signature branding.", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/nasar_house_1.jpg", "Nasar House Straps", 26m, "nasar-house-straps", new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2686) }
                });

            migrationBuilder.InsertData(
                table: "ProductVariants",
                columns: new[] { "Id", "Price", "ProductId", "VariantName" },
                values: new object[,]
                {
                    { 1, null, 1, "Size S / Color: Black" },
                    { 2, null, 1, "Size M / Color: Black" },
                    { 3, null, 1, "Size L / Color: Black" },
                    { 4, null, 1, "Size XL / Color: Black" },
                    { 5, null, 2, "Size S / Color: Black" },
                    { 6, null, 2, "Size M / Color: Black" },
                    { 7, null, 2, "Size L / Color: Black" },
                    { 8, null, 2, "Size XL / Color: Black" },
                    { 9, null, 3, "Size S / Color: Black" },
                    { 10, null, 3, "Size M / Color: Black" },
                    { 11, null, 3, "Size L / Color: Black" },
                    { 12, null, 3, "Size XL / Color: Black" },
                    { 13, null, 4, "Size S / Color: Black" },
                    { 14, null, 4, "Size M / Color: Black" },
                    { 15, null, 4, "Size L / Color: Black" },
                    { 16, null, 4, "Size XL / Color: Black" },
                    { 17, null, 5, "Size S / Color: Black" },
                    { 18, null, 5, "Size M / Color: Black" },
                    { 19, null, 5, "Size L / Color: Black" },
                    { 20, null, 5, "Size XL / Color: Black" },
                    { 21, null, 6, "Size 38 EU" },
                    { 22, null, 6, "Size 39 EU" },
                    { 23, null, 6, "Size 40 EU" },
                    { 24, null, 6, "Size 41 EU" },
                    { 25, null, 6, "Size 42 EU" },
                    { 26, null, 6, "Size 43 EU" },
                    { 27, null, 6, "Size 44 EU" },
                    { 28, null, 6, "Size 45 EU" },
                    { 29, null, 6, "Size 46 EU" },
                    { 30, null, 6, "Size 47 EU" },
                    { 31, null, 6, "Size 48 EU" },
                    { 32, 99.99m, 7, "Double Rich Chocolate / 2.26KG" },
                    { 33, 30m, 8, "Unflavored / 317g" },
                    { 34, null, 9, "Color: Black and Green" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "d333796b-4dd0-410c-92fd-481a6dc3fd0d", "06313180-fa45-42b5-ac33-1333f673455d" });

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d333796b-4dd0-410c-92fd-481a6dc3fd0d");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06313180-fa45-42b5-ac33-1333f673455d");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "ProductVariants",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDigital",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "StockTracked",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
