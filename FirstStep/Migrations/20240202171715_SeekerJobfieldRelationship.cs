using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class SeekerJobfieldRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "field_id",
                table: "Seekers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Seekers_field_id",
                table: "Seekers",
                column: "field_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Seekers_JobFields_field_id",
                table: "Seekers",
                column: "field_id",
                principalTable: "JobFields",
                principalColumn: "field_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seekers_JobFields_field_id",
                table: "Seekers");

            migrationBuilder.DropIndex(
                name: "IX_Seekers_field_id",
                table: "Seekers");

            migrationBuilder.DropColumn(
                name: "field_id",
                table: "Seekers");
        }
    }
}
