using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class RelationshipBetweenSeekerandapplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.AddColumn<int>(
                name: "seekeruser_id",
                table: "Applications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "Applications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_seekeruser_id",
                table: "Applications",
                column: "seekeruser_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Seekers_seekeruser_id",
                table: "Applications",
                column: "seekeruser_id",
                principalTable: "Seekers",
                principalColumn: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Seekers_seekeruser_id",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_seekeruser_id",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "seekeruser_id",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "Applications");

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    document_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    document_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    document_extension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    document_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    document_path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    document_size = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    document_type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.document_id);
                });
        }
    }
}
