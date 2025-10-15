using Microsoft.EntityFrameworkCore.Migrations;

namespace EquipShare.Migrations
{
    public partial class AddOwnerReceivableAmountToBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OwnerReceivableAmount",
                table: "Bookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerReceivableAmount",
                table: "Bookings");
        }
    }
}
