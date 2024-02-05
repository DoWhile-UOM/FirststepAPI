using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRegCompanyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_RegisteredCompanys_company_id",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "RegisteredCompanys");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "company_business_scale",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "company_city",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "company_description",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "company_logo",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "company_province",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "company_registered_date",
                table: "Companies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "verified_system_admin_id",
                table: "Companies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_verified_system_admin_id",
                table: "Companies",
                column: "verified_system_admin_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_SystemAdmins_verified_system_admin_id",
                table: "Companies",
                column: "verified_system_admin_id",
                principalTable: "SystemAdmins",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Companies_company_id",
                table: "Employees",
                column: "company_id",
                principalTable: "Companies",
                principalColumn: "company_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_SystemAdmins_verified_system_admin_id",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Companies_company_id",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Companies_verified_system_admin_id",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "company_business_scale",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "company_city",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "company_description",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "company_logo",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "company_province",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "company_registered_date",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "verified_system_admin_id",
                table: "Companies");

            migrationBuilder.CreateTable(
                name: "RegisteredCompanys",
                columns: table => new
                {
                    company_id = table.Column<int>(type: "int", nullable: false),
                    verified_system_admin_id = table.Column<int>(type: "int", nullable: false),
                    company_business_scale = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company_city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company_logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    company_province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company_registered_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredCompanys", x => x.company_id);
                    table.ForeignKey(
                        name: "FK_RegisteredCompanys_Companies_company_id",
                        column: x => x.company_id,
                        principalTable: "Companies",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegisteredCompanys_SystemAdmins_verified_system_admin_id",
                        column: x => x.verified_system_admin_id,
                        principalTable: "SystemAdmins",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredCompanys_verified_system_admin_id",
                table: "RegisteredCompanys",
                column: "verified_system_admin_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_RegisteredCompanys_company_id",
                table: "Employees",
                column: "company_id",
                principalTable: "RegisteredCompanys",
                principalColumn: "company_id");
        }
    }
}
