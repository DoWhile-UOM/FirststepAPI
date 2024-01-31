using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class AddUserSubClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_RegisteredCompanys_company_id",
                table: "Users");

            migrationBuilder.DropTable(
                name: "RegisteredCompanys");

            migrationBuilder.RenameColumn(
                name: "user_type",
                table: "Users",
                newName: "Discriminator");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Companys",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "company_business_scale",
                table: "Companys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "company_city",
                table: "Companys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "company_description",
                table: "Companys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "company_logo",
                table: "Companys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "company_province",
                table: "Companys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "company_registered_date",
                table: "Companys",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companys_company_id",
                table: "Users",
                column: "company_id",
                principalTable: "Companys",
                principalColumn: "company_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companys_company_id",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "company_business_scale",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "company_city",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "company_description",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "company_logo",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "company_province",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "company_registered_date",
                table: "Companys");

            migrationBuilder.RenameColumn(
                name: "Discriminator",
                table: "Users",
                newName: "user_type");

            migrationBuilder.CreateTable(
                name: "RegisteredCompanys",
                columns: table => new
                {
                    company_id = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_RegisteredCompanys_Companys_company_id",
                        column: x => x.company_id,
                        principalTable: "Companys",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Users_RegisteredCompanys_company_id",
                table: "Users",
                column: "company_id",
                principalTable: "RegisteredCompanys",
                principalColumn: "company_id");
        }
    }
}
