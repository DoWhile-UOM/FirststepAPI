using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class RemoveHRMfromAdvertisement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_HRAssistants_HRAssistantuser_id",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_HRAssistantuser_id",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "HRAssistantuser_id",
                table: "Advertisements");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HRAssistantuser_id",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_HRAssistantuser_id",
                table: "Advertisements",
                column: "HRAssistantuser_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_HRAssistants_HRAssistantuser_id",
                table: "Advertisements",
                column: "HRAssistantuser_id",
                principalTable: "HRAssistants",
                principalColumn: "user_id");
        }
    }
}
