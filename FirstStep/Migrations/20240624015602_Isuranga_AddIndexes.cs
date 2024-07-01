using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Firststep.Migrations
{
    /// <inheritdoc />
    public partial class Isuranga_AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "skill_name",
                table: "Skills",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "Revisions",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "profession_name",
                table: "ProfessionKeywords",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "Applications",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Users_email",
                table: "Users",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_skill_name",
                table: "Skills",
                column: "skill_name");

            migrationBuilder.CreateIndex(
                name: "IX_Revisions_status",
                table: "Revisions",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionKeywords_profession_name",
                table: "ProfessionKeywords",
                column: "profession_name");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_status",
                table: "Applications",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_title_current_status",
                table: "Advertisements",
                columns: new[] { "title", "current_status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Skills_skill_name",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Revisions_status",
                table: "Revisions");

            migrationBuilder.DropIndex(
                name: "IX_ProfessionKeywords_profession_name",
                table: "ProfessionKeywords");

            migrationBuilder.DropIndex(
                name: "IX_Applications_status",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_title_current_status",
                table: "Advertisements");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "skill_name",
                table: "Skills",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "Revisions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "profession_name",
                table: "ProfessionKeywords",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);
        }
    }
}
