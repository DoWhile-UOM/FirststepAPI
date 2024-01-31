using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNamesOfTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegisteredCompanys_Companys_company_id",
                table: "RegisteredCompanys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Companys",
                table: "Companys");

            migrationBuilder.RenameTable(
                name: "Companys",
                newName: "Companies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Companies",
                table: "Companies",
                column: "company_id");

            migrationBuilder.AddForeignKey(
                name: "FK_RegisteredCompanys_Companies_company_id",
                table: "RegisteredCompanys",
                column: "company_id",
                principalTable: "Companies",
                principalColumn: "company_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegisteredCompanys_Companies_company_id",
                table: "RegisteredCompanys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Companies",
                table: "Companies");

            migrationBuilder.RenameTable(
                name: "Companies",
                newName: "Companys");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Companys",
                table: "Companys",
                column: "company_id");

            migrationBuilder.AddForeignKey(
                name: "FK_RegisteredCompanys_Companys_company_id",
                table: "RegisteredCompanys",
                column: "company_id",
                principalTable: "Companys",
                principalColumn: "company_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
