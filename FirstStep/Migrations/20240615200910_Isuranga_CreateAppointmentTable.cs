using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Firststep.Migrations
{
    /// <inheritdoc />
    public partial class Isuranga_CreateAppointmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Advertisements_advertisement_id",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Companies_company_id",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Seekers_seeker_id",
                table: "Appointment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointment",
                table: "Appointment");

            migrationBuilder.RenameTable(
                name: "Appointment",
                newName: "Appointments");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_seeker_id",
                table: "Appointments",
                newName: "IX_Appointments_seeker_id");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_company_id",
                table: "Appointments",
                newName: "IX_Appointments_company_id");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_advertisement_id",
                table: "Appointments",
                newName: "IX_Appointments_advertisement_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments",
                column: "appointment_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Advertisements_advertisement_id",
                table: "Appointments",
                column: "advertisement_id",
                principalTable: "Advertisements",
                principalColumn: "advertisement_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Companies_company_id",
                table: "Appointments",
                column: "company_id",
                principalTable: "Companies",
                principalColumn: "company_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Seekers_seeker_id",
                table: "Appointments",
                column: "seeker_id",
                principalTable: "Seekers",
                principalColumn: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Advertisements_advertisement_id",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Companies_company_id",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Seekers_seeker_id",
                table: "Appointments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments");

            migrationBuilder.RenameTable(
                name: "Appointments",
                newName: "Appointment");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_seeker_id",
                table: "Appointment",
                newName: "IX_Appointment_seeker_id");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_company_id",
                table: "Appointment",
                newName: "IX_Appointment_company_id");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_advertisement_id",
                table: "Appointment",
                newName: "IX_Appointment_advertisement_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointment",
                table: "Appointment",
                column: "appointment_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Advertisements_advertisement_id",
                table: "Appointment",
                column: "advertisement_id",
                principalTable: "Advertisements",
                principalColumn: "advertisement_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Companies_company_id",
                table: "Appointment",
                column: "company_id",
                principalTable: "Companies",
                principalColumn: "company_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Seekers_seeker_id",
                table: "Appointment",
                column: "seeker_id",
                principalTable: "Seekers",
                principalColumn: "user_id");
        }
    }
}
