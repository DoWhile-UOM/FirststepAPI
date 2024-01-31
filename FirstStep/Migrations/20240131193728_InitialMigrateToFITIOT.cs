using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrateToFITIOT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Seekerskill",
                table: "Seekerskill");

            migrationBuilder.RenameTable(
                name: "Seekerskill",
                newName: "SeekerSkill");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeekerSkill",
                table: "SeekerSkill",
                column: "skillNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SeekerSkill",
                table: "SeekerSkill");

            migrationBuilder.RenameTable(
                name: "SeekerSkill",
                newName: "Seekerskill");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Seekerskill",
                table: "Seekerskill",
                column: "skillNo");
        }
    }
}
