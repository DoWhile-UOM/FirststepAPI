using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class AddSeekerSkillRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SeekerSkills",
                columns: table => new
                {
                    seekersuser_id = table.Column<int>(type: "int", nullable: false),
                    skillsskill_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeekerSkills", x => new { x.seekersuser_id, x.skillsskill_id });
                    table.ForeignKey(
                        name: "FK_SeekerSkills_Seekers_seekersuser_id",
                        column: x => x.seekersuser_id,
                        principalTable: "Seekers",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeekerSkills_Skills_skillsskill_id",
                        column: x => x.skillsskill_id,
                        principalTable: "Skills",
                        principalColumn: "skill_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SeekerSkills_skillsskill_id",
                table: "SeekerSkills",
                column: "skillsskill_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeekerSkills");
        }
    }
}
