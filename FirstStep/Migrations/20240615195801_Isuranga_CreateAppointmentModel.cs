using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Firststep.Migrations
{
    /// <inheritdoc />
    public partial class Isuranga_CreateAppointmentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    appointment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    start_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    advertisement_id = table.Column<int>(type: "int", nullable: true),
                    seeker_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.appointment_id);
                    table.ForeignKey(
                        name: "FK_Appointment_Advertisements_advertisement_id",
                        column: x => x.advertisement_id,
                        principalTable: "Advertisements",
                        principalColumn: "advertisement_id");
                    table.ForeignKey(
                        name: "FK_Appointment_Companies_company_id",
                        column: x => x.company_id,
                        principalTable: "Companies",
                        principalColumn: "company_id");
                    table.ForeignKey(
                        name: "FK_Appointment_Seekers_seeker_id",
                        column: x => x.seeker_id,
                        principalTable: "Seekers",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_advertisement_id",
                table: "Appointment",
                column: "advertisement_id");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_company_id",
                table: "Appointment",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_seeker_id",
                table: "Appointment",
                column: "seeker_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointment");
        }
    }
}
