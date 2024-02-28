using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class AddAdvertisementApplicationRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "advertisement_id",
                table: "Applications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_advertisement_id",
                table: "Applications",
                column: "advertisement_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Advertisements_advertisement_id",
                table: "Applications",
                column: "advertisement_id",
                principalTable: "Advertisements",
                principalColumn: "advertisement_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Advertisements_advertisement_id",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_advertisement_id",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "advertisement_id",
                table: "Applications");
        }
    }
}
