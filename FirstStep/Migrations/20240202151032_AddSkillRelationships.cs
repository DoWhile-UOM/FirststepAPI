using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class AddSkillRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "field_id",
                table: "SeekerSkills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SeekerSkills_field_id",
                table: "SeekerSkills",
                column: "field_id");

            migrationBuilder.AddForeignKey(
                name: "FK_SeekerSkills_JobFields_field_id",
                table: "SeekerSkills",
                column: "field_id",
                principalTable: "JobFields",
                principalColumn: "field_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeekerSkills_JobFields_field_id",
                table: "SeekerSkills");

            migrationBuilder.DropIndex(
                name: "IX_SeekerSkills_field_id",
                table: "SeekerSkills");

            migrationBuilder.DropColumn(
                name: "field_id",
                table: "SeekerSkills");
        }
    }
}
