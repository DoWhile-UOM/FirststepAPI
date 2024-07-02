using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Firststep.Migrations
{
    /// <inheritdoc />
    public partial class Isuranga_AddAdvertisementCoordinates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "latitude",
                table: "Seekers",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "longitude",
                table: "Seekers",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "latitude",
                table: "Advertisements",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "longitude",
                table: "Advertisements",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "latitude",
                table: "Seekers");

            migrationBuilder.DropColumn(
                name: "longitude",
                table: "Seekers");

            migrationBuilder.DropColumn(
                name: "latitude",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "longitude",
                table: "Advertisements");
        }
    }
}
