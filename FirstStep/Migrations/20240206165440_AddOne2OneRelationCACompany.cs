using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class AddOne2OneRelationCACompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "company_admin_id",
                table: "Companies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_company_admin_id",
                table: "Companies",
                column: "company_admin_id",
                unique: true,
                filter: "[company_admin_id] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_HRManagers_company_admin_id",
                table: "Companies",
                column: "company_admin_id",
                principalTable: "HRManagers",
                principalColumn: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_HRManagers_company_admin_id",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_company_admin_id",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "company_admin_id",
                table: "Companies");
        }
    }
}
