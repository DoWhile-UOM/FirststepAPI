using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class RenameSkills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SeekerSkill",
                table: "SeekerSkill");

            migrationBuilder.RenameTable(
                name: "SeekerSkill",
                newName: "SeekerSkills");

            migrationBuilder.RenameColumn(
                name: "skillName",
                table: "SeekerSkills",
                newName: "skill_name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeekerSkills",
                table: "SeekerSkills",
                column: "seeker_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SeekerSkills",
                table: "SeekerSkills");

            migrationBuilder.RenameTable(
                name: "SeekerSkills",
                newName: "SeekerSkill");

            migrationBuilder.RenameColumn(
                name: "skill_name",
                table: "SeekerSkill",
                newName: "skillName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeekerSkill",
                table: "SeekerSkill",
                column: "seeker_Id");
        }
    }
}
