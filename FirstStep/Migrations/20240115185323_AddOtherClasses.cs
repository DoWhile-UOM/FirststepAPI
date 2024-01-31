using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class AddOtherClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Users_user_id",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Seeker_Users_user_id",
                table: "Seeker");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Seeker",
                table: "Seeker");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employee",
                table: "Employee");

            migrationBuilder.RenameTable(
                name: "Seeker",
                newName: "Seekers");

            migrationBuilder.RenameTable(
                name: "Employee",
                newName: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "user_type",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Seekers",
                table: "Seekers",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "user_id");

            migrationBuilder.CreateTable(
                name: "Companys",
                columns: table => new
                {
                    company_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    company_registration_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company_email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company_website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    company_phone_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    verification_status = table.Column<bool>(type: "bit", nullable: false),
                    company_reg_document = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companys", x => x.company_id);
                });

            migrationBuilder.CreateTable(
                name: "JobFields",
                columns: table => new
                {
                    field_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    field_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobFields", x => x.field_id);
                });

            migrationBuilder.CreateTable(
                name: "RegisteredCompanys",
                columns: table => new
                {
                    company_id = table.Column<int>(type: "int", nullable: false),
                    company_logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    company_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company_city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company_province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company_business_scale = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company_registered_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredCompanys", x => x.company_id);
                    table.ForeignKey(
                        name: "FK_RegisteredCompanys_Companys_company_id",
                        column: x => x.company_id,
                        principalTable: "Companys",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Users_user_id",
                table: "Employees",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seekers_Users_user_id",
                table: "Seekers",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Users_user_id",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Seekers_Users_user_id",
                table: "Seekers");

            migrationBuilder.DropTable(
                name: "JobFields");

            migrationBuilder.DropTable(
                name: "RegisteredCompanys");

            migrationBuilder.DropTable(
                name: "Companys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Seekers",
                table: "Seekers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "user_type",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Seekers",
                newName: "Seeker");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Employee");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Seeker",
                table: "Seeker",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employee",
                table: "Employee",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Users_user_id",
                table: "Employee",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seeker_Users_user_id",
                table: "Seeker",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
