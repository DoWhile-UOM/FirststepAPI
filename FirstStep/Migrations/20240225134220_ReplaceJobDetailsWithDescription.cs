using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceJobDetailsWithDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "job_benefits",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "job_other_details",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "job_overview",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "job_qualifications",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "job_responsibilities",
                table: "Advertisements");

            migrationBuilder.AddColumn<string>(
                name: "job_description",
                table: "Advertisements",
                type: "nvarchar(2500)",
                maxLength: 2500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "job_description",
                table: "Advertisements");

            migrationBuilder.AddColumn<string>(
                name: "job_benefits",
                table: "Advertisements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "job_other_details",
                table: "Advertisements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "job_overview",
                table: "Advertisements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "job_qualifications",
                table: "Advertisements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "job_responsibilities",
                table: "Advertisements",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
