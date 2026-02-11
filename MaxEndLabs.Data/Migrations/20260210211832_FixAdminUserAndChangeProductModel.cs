using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaxEndLabs.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixAdminUserAndChangeProductModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "UpdatedAt",
                table: "Products",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreatedAt",
                table: "Products",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06313180-fa45-42b5-ac33-1333f673455d",
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "4bfdb153-a446-4968-a40f-34adc37a6f28", "admin@labs.com", "ADMIN@LABS.COM", "ADMIN@LABS.COM", "AQAAAAIAAYagAAAAEDeNUYMTsaz6jxiBG/ArPf9+DwC9M6GINzbW7TMwHjJ4VyjOzaur3xS4tT6+2jdQ+Q==", "c0142471-6ffd-44b4-b430-8b3c7acf8fbf", "admin@labs.com" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateOnly(2026, 2, 10), new DateOnly(2026, 2, 10) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateOnly(2026, 2, 10), new DateOnly(2026, 2, 10) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateOnly(2026, 2, 10), new DateOnly(2026, 2, 10) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateOnly(2026, 2, 10), new DateOnly(2026, 2, 10) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateOnly(2026, 2, 10), new DateOnly(2026, 2, 10) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateOnly(2026, 2, 10), new DateOnly(2026, 2, 10) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateOnly(2026, 2, 10), new DateOnly(2026, 2, 10) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateOnly(2026, 2, 10), new DateOnly(2026, 2, 10) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateOnly(2026, 2, 10), new DateOnly(2026, 2, 10) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Products",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Products",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06313180-fa45-42b5-ac33-1333f673455d",
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "35b7be81-cad0-474e-8781-d36e28d2a329", "admin@maxendlabs.com", "ADMIN@MAXENDLABS.COM", "ADMIN", "AQAAAAIAAYagAAAAEEJveNlU2qlawcwIfv1zOPQlmfe9Ihsj2Cop/jB4mqzg1v2OFqb7RbeF5xMNI3DSRw==", "6afbe9b6-ab9a-4daa-9fe2-33610d75f108", "admin" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2659), new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2663) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2667), new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2667) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2670), new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2671) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2674), new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2674) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2676), new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2677) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2678), new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2679) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2681), new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2681) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2683), new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2684) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2686), new DateTime(2026, 2, 10, 20, 2, 57, 945, DateTimeKind.Utc).AddTicks(2686) });
        }
    }
}
