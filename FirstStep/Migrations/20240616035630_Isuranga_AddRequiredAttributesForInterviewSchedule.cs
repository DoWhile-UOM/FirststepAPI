using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Firststep.Migrations
{
    /// <inheritdoc />
    public partial class Isuranga_AddRequiredAttributesForInterviewSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "end_time",
                table: "Appointments");

            migrationBuilder.AddColumn<bool>(
                name: "is_called",
                table: "Applications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "interview_duration",
                table: "Advertisements",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_called",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "interview_duration",
                table: "Advertisements");

            migrationBuilder.AddColumn<DateTime>(
                name: "end_time",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
