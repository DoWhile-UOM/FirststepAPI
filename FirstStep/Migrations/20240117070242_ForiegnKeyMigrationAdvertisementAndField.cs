using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class ForiegnKeyMigrationAdvertisementAndField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "job_id",
                table: "Advertisements",
                newName: "advertisement_id");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_field_id",
                table: "Advertisements",
                column: "field_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_JobFields_field_id",
                table: "Advertisements",
                column: "field_id",
                principalTable: "JobFields",
                principalColumn: "field_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_JobFields_field_id",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_field_id",
                table: "Advertisements");

            migrationBuilder.RenameColumn(
                name: "advertisement_id",
                table: "Advertisements",
                newName: "job_id");
        }
    }
}
