using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SpacePark.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parkings",
                columns: table => new
                {
                    ParkingID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Length = table.Column<double>(type: "REAL", nullable: false),
                    HourlyRatePerMeter = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parkings", x => x.ParkingID);
                });

            migrationBuilder.CreateTable(
                name: "ParkingSpots",
                columns: table => new
                {
                    ParkingSpotID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParkingID = table.Column<int>(type: "INTEGER", nullable: false),
                    UserID = table.Column<int>(type: "INTEGER", nullable: true),
                    Vehicle = table.Column<string>(type: "TEXT", nullable: true),
                    VehicleLength = table.Column<double>(type: "REAL", nullable: false),
                    ParkingStarted = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingSpots", x => x.ParkingSpotID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parkings");

            migrationBuilder.DropTable(
                name: "ParkingSpots");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
