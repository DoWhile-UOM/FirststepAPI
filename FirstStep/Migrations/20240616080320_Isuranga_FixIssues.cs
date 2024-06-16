using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class Isuranga_FixIssues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "company_city",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "company_province",
                table: "Companies",
                newName: "registration_url");

            migrationBuilder.AlterColumn<int>(
                name: "interview_duration",
                table: "Advertisements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "current_status",
                table: "Advertisements",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(7)",
                oldMaxLength: 7);

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    appointment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    start_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    advertisement_id = table.Column<int>(type: "int", nullable: true),
                    seeker_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.appointment_id);
                    table.ForeignKey(
                        name: "FK_Appointments_Advertisements_advertisement_id",
                        column: x => x.advertisement_id,
                        principalTable: "Advertisements",
                        principalColumn: "advertisement_id");
                    table.ForeignKey(
                        name: "FK_Appointments_Companies_company_id",
                        column: x => x.company_id,
                        principalTable: "Companies",
                        principalColumn: "company_id");
                    table.ForeignKey(
                        name: "FK_Appointments_Seekers_seeker_id",
                        column: x => x.seeker_id,
                        principalTable: "Seekers",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_advertisement_id",
                table: "Appointments",
                column: "advertisement_id");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_company_id",
                table: "Appointments",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_seeker_id",
                table: "Appointments",
                column: "seeker_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.RenameColumn(
                name: "registration_url",
                table: "Companies",
                newName: "company_province");

            migrationBuilder.AddColumn<string>(
                name: "company_city",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "interview_duration",
                table: "Advertisements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "current_status",
                table: "Advertisements",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);
        }
    }
}
