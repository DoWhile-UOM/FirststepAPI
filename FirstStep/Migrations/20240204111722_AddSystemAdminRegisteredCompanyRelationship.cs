using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class AddSystemAdminRegisteredCompanyRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "verified_system_admin_id",
                table: "RegisteredCompanys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredCompanys_verified_system_admin_id",
                table: "RegisteredCompanys",
                column: "verified_system_admin_id");

            migrationBuilder.AddForeignKey(
                name: "FK_RegisteredCompanys_SystemAdmins_verified_system_admin_id",
                table: "RegisteredCompanys",
                column: "verified_system_admin_id",
                principalTable: "SystemAdmins",
                principalColumn: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegisteredCompanys_SystemAdmins_verified_system_admin_id",
                table: "RegisteredCompanys");

            migrationBuilder.DropIndex(
                name: "IX_RegisteredCompanys_verified_system_admin_id",
                table: "RegisteredCompanys");

            migrationBuilder.DropColumn(
                name: "verified_system_admin_id",
                table: "RegisteredCompanys");
        }
    }
}
