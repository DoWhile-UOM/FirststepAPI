using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class Isuranga_CreateRelationshipBetweenRevisinAndApplicationAndSeeker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "application_id",
                table: "Revisions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "employee_id",
                table: "Revisions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "assigned_hrAssistant_id",
                table: "Applications",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "submission_deadline",
                table: "Advertisements",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_Revisions_application_id",
                table: "Revisions",
                column: "application_id");

            migrationBuilder.CreateIndex(
                name: "IX_Revisions_employee_id",
                table: "Revisions",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_assigned_hrAssistant_id",
                table: "Applications",
                column: "assigned_hrAssistant_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_HRAssistants_assigned_hrAssistant_id",
                table: "Applications",
                column: "assigned_hrAssistant_id",
                principalTable: "HRAssistants",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Revisions_Applications_application_id",
                table: "Revisions",
                column: "application_id",
                principalTable: "Applications",
                principalColumn: "application_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Revisions_Employees_employee_id",
                table: "Revisions",
                column: "employee_id",
                principalTable: "Employees",
                principalColumn: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_HRAssistants_assigned_hrAssistant_id",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Revisions_Applications_application_id",
                table: "Revisions");

            migrationBuilder.DropForeignKey(
                name: "FK_Revisions_Employees_employee_id",
                table: "Revisions");

            migrationBuilder.DropIndex(
                name: "IX_Revisions_application_id",
                table: "Revisions");

            migrationBuilder.DropIndex(
                name: "IX_Revisions_employee_id",
                table: "Revisions");

            migrationBuilder.DropIndex(
                name: "IX_Applications_assigned_hrAssistant_id",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "application_id",
                table: "Revisions");

            migrationBuilder.DropColumn(
                name: "employee_id",
                table: "Revisions");

            migrationBuilder.DropColumn(
                name: "assigned_hrAssistant_id",
                table: "Applications");

            migrationBuilder.AlterColumn<DateTime>(
                name: "submission_deadline",
                table: "Advertisements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
