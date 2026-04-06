using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MaxEndLabs.Data.Migrations
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
                    Id = table.Column<string>(type: "text", nullable: false),
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
                    Id = table.Column<string>(type: "text", nullable: false),
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                    RoleId = table.Column<string>(type: "text", nullable: false),
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
                    UserId = table.Column<string>(type: "text", nullable: false),
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
                    UserId = table.Column<string>(type: "text", nullable: false)
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
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
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
                    UserId = table.Column<string>(type: "text", nullable: false),
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
                name: "ShoppingCarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    VariantName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", nullable: true)
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CartId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    ProductVariantId = table.Column<int>(type: "integer", nullable: false),
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d333796b-4dd0-410c-92fd-481a6dc3fd0d", null, "Admin", "ADMIN" },
                    { "e444796b-4dd0-410c-92fd-481a6dc3fd0d", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "06313180-fa45-42b5-ac33-1333f673455d", 0, "4bfdb153-a446-4968-a40f-34adc37a6f28", "admin@labs.com", true, false, null, "ADMIN@LABS.COM", "ADMIN@LABS.COM", "AQAAAAIAAYagAAAAEFn+JSWstBTrgdKXnCbFFlyXn3VHglaT/4nEmFCUnSexw+jwdpB9hbPnuyX2myzcXw==", null, false, "c0142471-6ffd-44b4-b430-8b3c7acf8fbf", false, "admin@labs.com" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "Slug" },
                values: new object[,]
                {
                    { 1, "Upper Body", "upper-body" },
                    { 2, "Lower Body", "lower-body" },
                    { 3, "Shoes", "shoes" },
                    { 4, "Supplements", "supplements" },
                    { 5, "Accessories", "accessories" }
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
                    { 1, 1, new DateOnly(2026, 2, 10), "The three words that motivated generations of weightlifters are back! As worn by Bulgaria’s -85kg snatch record holder from the ‘90s, Georgi Gardev. Oversized fit.", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/WBN1.jpg", "Why Be Normal T-shirt", 40m, "why-be-normal-tshirt", new DateOnly(2026, 2, 10) },
                    { 2, 1, new DateOnly(2026, 2, 10), "The official t-shirt of Olympic Champion Karlos Nasar. Made in collaboration with the greatest weightlifter of a generation, the Nasar ‘I Win, You Lose’ T-shirt.", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/6_155b8e55-8336-438c-a966-fdec261cd0cb.jpg", "Nasar I Win, You Lose T-shirt", 45m, "nasar-i-win-you-lose-tshirt", new DateOnly(2026, 2, 10) },
                    { 3, 1, new DateOnly(2026, 2, 10), "Celebrate the record-breaking power of Olympic Champion Karlos Nasar with this exclusive t-shirt. Featuring the iconic ‘404’ — marking the world and Olympic total record.", true, "https://karlosnasar.com/storage/products/371146981.jpg", "T-shirt 404", 30m, "tshirt-404", new DateOnly(2026, 2, 10) },
                    { 4, 2, new DateOnly(2026, 2, 10), "The official t-shirt of Olympic Champion Karlos Nasar. Made in collaboration with the greatest weightlifter of a generation, the Nasar ‘I Win, You Lose’ Shorts", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/18_75ea3d05-1e8b-4fbc-af35-a41fb16930b3.jpg", "Nasar I Win, You Lose Shorts", 62.95m, "nasar-i-win-you-lose-shorts", new DateOnly(2026, 2, 10) },
                    { 5, 2, new DateOnly(2026, 2, 10), "The official t-shirt of Olympic Champion Karlos Nasar. Made in collaboration with the greatest weightlifter of a generation, the Nasar ‘I Win, You Lose’ Leggings", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/13_2144736c-1165-42a7-87f7-c90db6a3c274.jpg", "Nasar I Win, You Lose Leggings", 70m, "nasar-i-win-you-lose-leggings", new DateOnly(2026, 2, 10) },
                    { 6, 3, new DateOnly(2026, 2, 10), "When Karlos Nasar shattered the world record, he did it in LUXIAOJUN lifters. That moment wasn’t just victory. It was domination. To celebrate his legacy and mindset, we present the Karlos Edition: built for lifters who don't just compete, they take over.", true, "https://luxiaojun.com/cdn/shop/files/LUXIAOJUN_PowerPro_Weightlifting_Shoes_Karlos_Edition_Pair.jpg", "LUXIAOJUN PowerPro Weightlifting Shoes (Karlos)", 195m, "luxiaojun-powerpro-weightlifting-shoes-karlos", new DateOnly(2026, 2, 10) },
                    { 7, 4, new DateOnly(2026, 2, 10), "Fuel your performance with Gold Standard 100% Whey — a premium protein powder built for muscle support and post-workout recovery. Featuring 24g of protein and 5.5g of BCAAs per serving, with whey protein isolate as the No. 1 ingredient.", true, "https://www.optimumnutrition.com/cdn/shop/files/on-1101490_Image_01.jpg", "GOLD STANDARD 100% WHEY", 99.99m, "gold-standard-100-whey", new DateOnly(2026, 2, 10) },
                    { 8, 4, new DateOnly(2026, 2, 10), "Boost your performance with Micronized Creatine Powder — designed to support muscle strength and power, explosive movements, and ATP recycling. Delivering 5g of creatine monohydrate per serving and stackable with Gold Standard 100% Whey for complete muscle support.", true, "https://www.optimumnutrition.com/cdn/shop/files/on-1102271_Image_01.png", "Micronized Creatine Powder", 30m, "micronized-creatine-powder", new DateOnly(2026, 2, 10) },
                    { 9, 5, new DateOnly(2026, 2, 10), "Designed by Olympic Champion Karlos Nasar to elevate your training. These classic quick-release straps offer performance and durability with Karlos’s signature branding.", true, "https://eustore.weightliftinghouse.com/cdn/shop/files/nasar_house_1.jpg", "Nasar House Straps", 26m, "nasar-house-straps", new DateOnly(2026, 2, 10) }
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
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ProductVariants");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
