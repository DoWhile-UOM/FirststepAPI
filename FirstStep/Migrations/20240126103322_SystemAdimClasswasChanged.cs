using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class SystemAdimClasswasChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemAdmis_Users_user_id",
                table: "SystemAdmis");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SystemAdmis",
                table: "SystemAdmis");

            migrationBuilder.RenameTable(
                name: "SystemAdmis",
                newName: "SystemAdmins");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SystemAdmins",
                table: "SystemAdmins",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemAdmins_Users_user_id",
                table: "SystemAdmins",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemAdmins_Users_user_id",
                table: "SystemAdmins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SystemAdmins",
                table: "SystemAdmins");

            migrationBuilder.RenameTable(
                name: "SystemAdmins",
                newName: "SystemAdmis");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SystemAdmis",
                table: "SystemAdmis",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemAdmis_Users_user_id",
                table: "SystemAdmis",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
