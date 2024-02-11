using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCountryAndCityInAdvertisement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "location_province",
                table: "Advertisements",
                newName: "country");

            migrationBuilder.RenameColumn(
                name: "location_city",
                table: "Advertisements",
                newName: "city");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "country",
                table: "Advertisements",
                newName: "location_province");

            migrationBuilder.RenameColumn(
                name: "city",
                table: "Advertisements",
                newName: "location_city");
        }
    }
}
