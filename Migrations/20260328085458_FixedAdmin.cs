using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACar3.Migrations
{
    /// <inheritdoc />
    public partial class FixedAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$R9h/lIPzHZ7vGTEyNm9I6eVUAIh5S8.9mB.Dsn.L8Y8E1m6tXN.7G");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$Y3gxD1k0PKPdReAYKlA2v.qE9fkoinkO6fDx0tfXHeX2Tth2xOD8.");
        }
    }
}
