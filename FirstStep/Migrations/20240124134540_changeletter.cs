using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class changeletter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_seekerskill",
                table: "seekerskill");

            migrationBuilder.RenameTable(
                name: "seekerskill",
                newName: "Seekerskill");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Seekerskill",
                table: "Seekerskill",
                column: "skillNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Seekerskill",
                table: "Seekerskill");

            migrationBuilder.RenameTable(
                name: "Seekerskill",
                newName: "seekerskill");

            migrationBuilder.AddPrimaryKey(
                name: "PK_seekerskill",
                table: "seekerskill",
                column: "skillNo");
        }
    }
}
