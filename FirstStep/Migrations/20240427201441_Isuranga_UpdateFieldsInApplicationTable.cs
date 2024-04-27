using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class Isuranga_UpdateFieldsInApplicationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Seekers_user_id",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Applications",
                newName: "seeker_id");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_user_id",
                table: "Applications",
                newName: "IX_Applications_seeker_id");

            migrationBuilder.AddColumn<string>(
                name: "CVurl",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "doc1_url",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "doc2_url",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Seekers_seeker_id",
                table: "Applications",
                column: "seeker_id",
                principalTable: "Seekers",
                principalColumn: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Seekers_seeker_id",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "CVurl",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "doc1_url",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "doc2_url",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "seeker_id",
                table: "Applications",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_seeker_id",
                table: "Applications",
                newName: "IX_Applications_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Seekers_user_id",
                table: "Applications",
                column: "user_id",
                principalTable: "Seekers",
                principalColumn: "user_id");
        }
    }
}
