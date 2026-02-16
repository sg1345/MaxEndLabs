using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaxEndLabs.Data.Migrations
{
    /// <inheritdoc />
    public partial class ShoppingCartUserIdRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_AspNetUsers_UserId",
                table: "ShoppingCarts");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ShoppingCarts",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06313180-fa45-42b5-ac33-1333f673455d",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENrqN12KXKRQKJDXAE2E/b2i2xW9BqGq6x1mnpOFQtnPJDzYU5ZmTTzMi2GA/62BDw==");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_AspNetUsers_UserId",
                table: "ShoppingCarts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_AspNetUsers_UserId",
                table: "ShoppingCarts");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ShoppingCarts",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06313180-fa45-42b5-ac33-1333f673455d",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENimES3qMUHG/Awl1RGOASxmBhXXpkYC7TWWaz/ZgO77N471Y8TAnRzyabYlZFvSng==");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_AspNetUsers_UserId",
                table: "ShoppingCarts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
