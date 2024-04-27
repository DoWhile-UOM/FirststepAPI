using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCompanyClassv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "comment",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "company_applied_date",
                table: "Companies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "comment",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "company_applied_date",
                table: "Companies");
        }
    }
}
