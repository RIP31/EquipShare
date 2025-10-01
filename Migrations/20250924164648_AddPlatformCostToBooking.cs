using Microsoft.EntityFrameworkCore.Migrations;

namespace EquipShare.Migrations
{
    public partial class AddPlatformCostToBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PlatformCost",
                table: "Bookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlatformCost",
                table: "Bookings");
        }
    }
}
