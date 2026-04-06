using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MaxEndLabs.Data.Migrations.PostgresMigrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Slug = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderNumber = table.Column<string>(type: "character varying(19)", maxLength: 19, nullable: false),
                    StreetAddress = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Postcode = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    MainImageUrl = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    UpdatedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    VariantName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariants_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CartId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductVariantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_ProductVariants_ProductVariantId",
                        column: x => x.ProductVariantId,
                        principalTable: "ProductVariants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CartItems_ShoppingCarts_CartId",
                        column: x => x.CartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductVariantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    LineTotal = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => new { x.OrderId, x.ProductId, x.ProductVariantId });
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_ProductVariants_ProductVariantId",
                        column: x => x.ProductVariantId,
                        principalTable: "ProductVariants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("d333796b-4dd0-410c-92fd-481a6dc3fd0d"), null, "Admin", "ADMIN" },
                    { new Guid("e444796b-4dd0-410c-92fd-481a6dc3fd0d"), null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("06313180-fa45-42b5-ac33-1333f673455d"), 0, "4bfdb153-a446-4968-a40f-34adc37a6f28", "admin@labs.com", true, "Dimitar Ivanov Berbatov", false, null, "ADMIN@LABS.COM", "ADMIN@LABS.COM", "AQAAAAIAAYagAAAAEP9JzX0YwKm6N+luPkuHrobdVlv+8FQxWWME12rmICorZgnbmrHziPC+5WxRG5/6aw==", null, false, "c0142471-6ffd-44b4-b430-8b3c7acf8fbf", false, "admin@labs.com" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "Slug" },
                values: new object[,]
                {
                    { new Guid("26a46720-ae3a-43bd-b5de-2e2dd4048dee"), "Accessories", "accessories" },
                    { new Guid("6f026985-b5c2-4b05-9ac4-088b88982ec8"), "Lower Body", "lower-body" },
                    { new Guid("a9c9243a-aca9-4d3c-8f7a-4bdeb4578682"), "Shoes", "shoes" },
                    { new Guid("cb8ceb6a-6623-4937-a6c4-777b4b498e1e"), "Upper Body", "upper-body" },
                    { new Guid("f1f47314-cdc4-4104-a9f2-a2ccf38e9623"), "Supplements", "supplements" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("d333796b-4dd0-410c-92fd-481a6dc3fd0d"), new Guid("06313180-fa45-42b5-ac33-1333f673455d") });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "IsPublished", "MainImageUrl", "Name", "Price", "Slug", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), new Guid("cb8ceb6a-6623-4937-a6c4-777b4b498e1e"), new DateOnly(2026, 2, 10), "The three words that motivated generations of weightlifters are back! As worn by Bulgaria’s -85kg snatch record holder from the ‘90s, Georgi Gardev. Oversized fit.", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/WBN1.jpg", "Why Be Normal T-shirt", 40m, "why-be-normal-tshirt", new DateOnly(2026, 2, 10) },
                    { new Guid("1dec9339-ab58-4186-90c3-c6eb7f971893"), new Guid("cb8ceb6a-6623-4937-a6c4-777b4b498e1e"), new DateOnly(2026, 2, 10), "The official t-shirt of Olympic Champion Karlos Nasar. Made in collaboration with the greatest weightlifter of a generation, the Nasar ‘I Win, You Lose’ T-shirt.", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/6_155b8e55-8336-438c-a966-fdec261cd0cb.jpg", "Nasar I Win, You Lose T-shirt", 45m, "nasar-i-win-you-lose-tshirt", new DateOnly(2026, 2, 10) },
                    { new Guid("467ea21c-0fcf-4dde-801a-17e77ee4a5b2"), new Guid("6f026985-b5c2-4b05-9ac4-088b88982ec8"), new DateOnly(2026, 2, 10), "The official t-shirt of Olympic Champion Karlos Nasar. Made in collaboration with the greatest weightlifter of a generation, the Nasar ‘I Win, You Lose’ Leggings", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/13_2144736c-1165-42a7-87f7-c90db6a3c274.jpg", "Nasar I Win, You Lose Leggings", 70m, "nasar-i-win-you-lose-leggings", new DateOnly(2026, 2, 10) },
                    { new Guid("96ddcde3-5ff2-4fd4-b685-f216945568a3"), new Guid("6f026985-b5c2-4b05-9ac4-088b88982ec8"), new DateOnly(2026, 2, 10), "The official t-shirt of Olympic Champion Karlos Nasar. Made in collaboration with the greatest weightlifter of a generation, the Nasar ‘I Win, You Lose’ Shorts", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/18_75ea3d05-1e8b-4fbc-af35-a41fb16930b3.jpg", "Nasar I Win, You Lose Shorts", 62.95m, "nasar-i-win-you-lose-shorts", new DateOnly(2026, 2, 10) },
                    { new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), new Guid("f1f47314-cdc4-4104-a9f2-a2ccf38e9623"), new DateOnly(2026, 2, 10), "Fuel your performance with Gold Standard 100% Whey — a premium protein powder built for muscle support and post-workout recovery. Featuring 24g of protein and 5.5g of BCAAs per serving, with whey protein isolate as the No. 1 ingredient.", true, "https://www.optimumnutrition.com/cdn/shop/files/on-1101490_Image_01.jpg", "GOLD STANDARD 100% WHEY", 99.99m, "gold-standard-100-whey", new DateOnly(2026, 2, 10) },
                    { new Guid("ac8a1ca5-d799-435f-8b6b-f4161ac2e284"), new Guid("a9c9243a-aca9-4d3c-8f7a-4bdeb4578682"), new DateOnly(2026, 2, 10), "When Karlos Nasar shattered the world record, he did it in LUXIAOJUN lifters. That moment wasn’t just victory. It was domination. To celebrate his legacy and mindset, we present the Karlos Edition: built for lifters who don't just compete, they take over.", true, "https://luxiaojun.com/cdn/shop/files/LUXIAOJUN_PowerPro_Weightlifting_Shoes_Karlos_Edition_Pair.jpg", "LUXIAOJUN PowerPro Weightlifting Shoes (Karlos)", 195m, "luxiaojun-powerpro-weightlifting-shoes-karlos", new DateOnly(2026, 2, 10) },
                    { new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), new Guid("26a46720-ae3a-43bd-b5de-2e2dd4048dee"), new DateOnly(2026, 2, 10), "Designed by Olympic Champion Karlos Nasar to elevate your training. These classic quick-release straps offer performance and durability with Karlos’s signature branding.", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/nasar_house_1.jpg", "Nasar House Straps", 26m, "nasar-house-straps", new DateOnly(2026, 2, 10) },
                    { new Guid("c7f9f6ad-8512-49b0-bb48-980c3922fe60"), new Guid("f1f47314-cdc4-4104-a9f2-a2ccf38e9623"), new DateOnly(2026, 2, 10), "Boost your performance with Micronized Creatine Powder — designed to support muscle strength and power, explosive movements, and ATP recycling. Delivering 5g of creatine monohydrate per serving and stackable with Gold Standard 100% Whey for complete muscle support.", true, "https://www.optimumnutrition.com/cdn/shop/files/on-1102271_Image_01.png", "Micronized Creatine Powder", 30m, "micronized-creatine-powder", new DateOnly(2026, 2, 10) },
                    { new Guid("e28b6ccc-c7d5-49d6-95ae-1c6e665c6ced"), new Guid("cb8ceb6a-6623-4937-a6c4-777b4b498e1e"), new DateOnly(2026, 2, 10), "Celebrate the record-breaking power of Olympic Champion Karlos Nasar with this exclusive t-shirt. Featuring the iconic ‘404’ — marking the world and Olympic total record.", true, "https://karlosnasar.com/storage/products/371146981.jpg", "T-shirt 404", 30m, "tshirt-404", new DateOnly(2026, 2, 10) }
                });

            migrationBuilder.InsertData(
                table: "ProductVariants",
                columns: new[] { "Id", "IsDeleted", "Price", "ProductId", "VariantName" },
                values: new object[,]
                {
                    { new Guid("0135a402-3bfe-4cb0-9ae6-96a18e8dadef"), false, null, new Guid("1dec9339-ab58-4186-90c3-c6eb7f971893"), "Size L / Color: Black" },
                    { new Guid("04eb7d5e-ad31-417a-8544-470f559583c2"), false, null, new Guid("ac8a1ca5-d799-435f-8b6b-f4161ac2e284"), "Size 45 EU" },
                    { new Guid("077ce4ee-d228-421e-8c67-b6936a511e36"), false, null, new Guid("ac8a1ca5-d799-435f-8b6b-f4161ac2e284"), "Size 40 EU" },
                    { new Guid("0bedd38e-210c-4552-9de8-8ba385901981"), false, null, new Guid("ac8a1ca5-d799-435f-8b6b-f4161ac2e284"), "Size 42 EU" },
                    { new Guid("1cb5d36e-0e0c-401e-a6dc-a1d287e8afb6"), false, null, new Guid("ac8a1ca5-d799-435f-8b6b-f4161ac2e284"), "Size 44 EU" },
                    { new Guid("21b6185b-b955-46c4-a48f-1e8b037fa265"), false, null, new Guid("c4ed28d4-92cb-4532-8949-044be6a66e28"), "Color: Black and Green" },
                    { new Guid("21f64de2-0991-4363-baff-16aa1b8064f4"), false, null, new Guid("1dec9339-ab58-4186-90c3-c6eb7f971893"), "Size M / Color: Black" },
                    { new Guid("2ceab2fb-6d88-421a-98b4-ba293000fa8e"), false, null, new Guid("ac8a1ca5-d799-435f-8b6b-f4161ac2e284"), "Size 46 EU" },
                    { new Guid("2e64045a-0c1b-4122-9100-14df928f025a"), false, null, new Guid("ac8a1ca5-d799-435f-8b6b-f4161ac2e284"), "Size 39 EU" },
                    { new Guid("3415ca9a-14ab-44a8-8cfe-cfbaa687fddd"), false, null, new Guid("467ea21c-0fcf-4dde-801a-17e77ee4a5b2"), "Size S / Color: Black" },
                    { new Guid("38657209-0631-4310-8839-7a8badc05367"), false, null, new Guid("1dec9339-ab58-4186-90c3-c6eb7f971893"), "Size XL / Color: Black" },
                    { new Guid("389068f2-e83d-4d11-8d7c-db2760966585"), false, null, new Guid("e28b6ccc-c7d5-49d6-95ae-1c6e665c6ced"), "Size XL / Color: Black" },
                    { new Guid("3c793526-7537-49f4-8fff-aa25dd25ad48"), false, null, new Guid("467ea21c-0fcf-4dde-801a-17e77ee4a5b2"), "Size L / Color: Black" },
                    { new Guid("4147f8f0-6c0a-4005-a916-20cef7685120"), false, null, new Guid("e28b6ccc-c7d5-49d6-95ae-1c6e665c6ced"), "Size S / Color: Black" },
                    { new Guid("430e4616-4746-4fdb-a227-5584361edd68"), false, null, new Guid("1dec9339-ab58-4186-90c3-c6eb7f971893"), "Size S / Color: Black" },
                    { new Guid("4c8177a6-5c5d-4ce5-9955-65626c6262a3"), false, null, new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), "Size L / Color: Black" },
                    { new Guid("51b55327-3e64-406b-b2b6-341bb4562ce1"), false, null, new Guid("96ddcde3-5ff2-4fd4-b685-f216945568a3"), "Size XL / Color: Black" },
                    { new Guid("53c5dc85-a298-4627-b65a-bcae0f94682a"), false, null, new Guid("467ea21c-0fcf-4dde-801a-17e77ee4a5b2"), "Size XL / Color: Black" },
                    { new Guid("694e0600-e0ca-456a-9ec9-9dd3b25651d9"), false, null, new Guid("ac8a1ca5-d799-435f-8b6b-f4161ac2e284"), "Size 47 EU" },
                    { new Guid("6f665e33-c4a9-4548-aa78-accdad51909a"), false, null, new Guid("96ddcde3-5ff2-4fd4-b685-f216945568a3"), "Size M / Color: Black" },
                    { new Guid("75feaf94-8a87-495e-910e-01276c94d0c8"), false, null, new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), "Size S / Color: Black" },
                    { new Guid("7b0f801b-5af3-4c03-9406-e4262f5e3c16"), false, 30m, new Guid("c7f9f6ad-8512-49b0-bb48-980c3922fe60"), "Unflavored / 317g" },
                    { new Guid("7dcf0d49-527c-4c7a-9cd6-89ce6cab3a26"), false, 99.99m, new Guid("a8bcb2ba-bd47-4370-a443-505776baebb3"), "Double Rich Chocolate / 2.26KG" },
                    { new Guid("7e905a23-8f10-4074-aade-7c29693e6e93"), false, null, new Guid("ac8a1ca5-d799-435f-8b6b-f4161ac2e284"), "Size 48 EU" },
                    { new Guid("7f8adfae-2530-4920-b71c-4a58ce6a428d"), false, null, new Guid("96ddcde3-5ff2-4fd4-b685-f216945568a3"), "Size L / Color: Black" },
                    { new Guid("8a2e6649-bd1a-4147-acf0-5e20405d315d"), false, null, new Guid("ac8a1ca5-d799-435f-8b6b-f4161ac2e284"), "Size 43 EU" },
                    { new Guid("8c3c3fd0-d1cb-49e6-8d2b-e7c04a059917"), false, null, new Guid("ac8a1ca5-d799-435f-8b6b-f4161ac2e284"), "Size 41 EU" },
                    { new Guid("91367123-74d9-435e-91ce-98debdeaf9ff"), false, null, new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), "Size XL / Color: Black" },
                    { new Guid("b347cf2f-c0cd-416f-a330-e75614b11c60"), false, null, new Guid("ac8a1ca5-d799-435f-8b6b-f4161ac2e284"), "Size 38 EU" },
                    { new Guid("c3adf1e3-384b-4fbd-9601-4ad589f5cd8d"), false, null, new Guid("96ddcde3-5ff2-4fd4-b685-f216945568a3"), "Size S / Color: Black" },
                    { new Guid("d57befc8-55a9-41c1-b5f6-7a47381de37f"), false, null, new Guid("e28b6ccc-c7d5-49d6-95ae-1c6e665c6ced"), "Size M / Color: Black" },
                    { new Guid("e2994b78-47b5-4aaa-afec-3d1a093b5a43"), false, null, new Guid("e28b6ccc-c7d5-49d6-95ae-1c6e665c6ced"), "Size L / Color: Black" },
                    { new Guid("f94fc295-e46a-41f1-b016-0055ebfbde22"), false, null, new Guid("467ea21c-0fcf-4dde-801a-17e77ee4a5b2"), "Size M / Color: Black" },
                    { new Guid("fac660c5-b882-426d-bc27-5c20a6634402"), false, null, new Guid("02c314ef-6c38-41cd-ac00-aa3a986e3f46"), "Size M / Color: Black" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId_ProductId_ProductVariantId",
                table: "CartItems",
                columns: new[] { "CartId", "ProductId", "ProductVariantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductVariantId",
                table: "CartItems",
                column: "ProductVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug",
                table: "Categories",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductVariantId",
                table: "OrderItems",
                column: "ProductVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNumber",
                table: "Orders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Slug",
                table: "Products",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ProductId",
                table: "ProductVariants",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_UserId",
                table: "ShoppingCarts",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "ProductVariants");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
