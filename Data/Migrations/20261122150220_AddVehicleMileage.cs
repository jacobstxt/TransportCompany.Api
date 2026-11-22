using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportCompany.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleMileage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MileageKm",
                table: "Vehicles",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MileageKm",
                table: "Vehicles");
        }
    }
}
