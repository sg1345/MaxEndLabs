using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaxEndLabs.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCartItemIsPublished : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "CartItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e444796b-4dd0-410c-92fd-481a6dc3fd0d", null, "User", "USER" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06313180-fa45-42b5-ac33-1333f673455d",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFOWsd0unT2+CYny7I44LMv8YnK0rGAa7YvaUKJJscUnG1wUOc1njjs2hbfUaM7leA==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e444796b-4dd0-410c-92fd-481a6dc3fd0d");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "CartItems");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06313180-fa45-42b5-ac33-1333f673455d",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEONm9efY3gU8RPElJ0+y8/dItPQm5jdcOdihNtzdoNwSWWDRX2BS84HW1EorGkInBg==");
        }
    }
}
