using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACar3.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminCredentials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: BCrypt.Net.BCrypt.HashPassword("admin123"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
