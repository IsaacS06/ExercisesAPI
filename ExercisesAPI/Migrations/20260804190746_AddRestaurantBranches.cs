using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExercisesAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRestaurantBranches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RestaurantBranches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(10,8)", precision: 10, scale: 8, nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(11,8)", precision: 11, scale: 8, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    OpeningHours = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantBranches", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "RestaurantBranches",
                columns: new[] { "Id", "Address", "CreatedAt", "IsActive", "Latitude", "Longitude", "Name", "OpeningHours", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "201 King Street, London, ON", new DateTime(2026, 8, 4, 19, 7, 46, 1, DateTimeKind.Utc).AddTicks(5730), true, 42.9832m, -81.2492m, "Downtown Branch", "Mon-Sun 10:00 AM - 10:00 PM", "+1-519-555-0101" },
                    { 2, "1680 Richmond St N, London, ON", new DateTime(2026, 8, 4, 19, 7, 46, 1, DateTimeKind.Utc).AddTicks(5735), true, 43.0200m, -81.2770m, "Masonville Branch", "Mon-Sun 10:00 AM - 10:00 PM", "+1-519-555-0102" },
                    { 3, "1105 Wellington Rd, London, ON", new DateTime(2026, 8, 4, 19, 7, 46, 1, DateTimeKind.Utc).AddTicks(5736), true, 42.9440m, -81.2300m, "White Oaks Branch", "Mon-Sun 10:00 AM - 10:00 PM", "+1-519-555-0103" },
                    { 4, "1265 Hyde Park Rd, London, ON", new DateTime(2026, 8, 4, 19, 7, 46, 1, DateTimeKind.Utc).AddTicks(5738), true, 43.0080m, -81.3320m, "Hyde Park Branch", "Mon-Sun 10:00 AM - 10:00 PM", "+1-519-555-0104" },
                    { 5, "1249 Oxford St E, London, ON", new DateTime(2026, 8, 4, 19, 7, 46, 1, DateTimeKind.Utc).AddTicks(5740), true, 42.9910m, -81.1890m, "Argyle Branch", "Mon-Sun 10:00 AM - 10:00 PM", "+1-519-555-0105" },
                    { 6, "785 Wonderland Rd S, London, ON", new DateTime(2026, 8, 4, 19, 7, 46, 1, DateTimeKind.Utc).AddTicks(5741), true, 42.9560m, -81.2870m, "Westmount Branch", "Mon-Sun 10:00 AM - 10:00 PM", "+1-519-555-0106" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RestaurantBranches");
        }
    }
}
