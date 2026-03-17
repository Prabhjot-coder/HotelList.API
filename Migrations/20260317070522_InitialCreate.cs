using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication4.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hotels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotels", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "Address", "CreatedDate", "EmailAddress", "HotelName", "Rating" },
                values: new object[,]
                {
                    { 1, "123 Main St", new DateTime(2026, 3, 17, 12, 35, 18, 768, DateTimeKind.Local).AddTicks(4843), "pkaur@gmail.com", "Hotel A", 4 },
                    { 2, "456 Main St", new DateTime(2026, 3, 17, 12, 35, 18, 769, DateTimeKind.Local).AddTicks(9681), "agam@gmail.com", "Hotel B", 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hotels");
        }
    }
}
