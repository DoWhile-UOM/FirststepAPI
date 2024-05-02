using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class Isuranga_ChangeExperienceInAdvertisementTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_experience_required",
                table: "Advertisements");

            migrationBuilder.AddColumn<string>(
                name: "experience",
                table: "Advertisements",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "experience",
                table: "Advertisements");

            migrationBuilder.AddColumn<bool>(
                name: "is_experience_required",
                table: "Advertisements",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
