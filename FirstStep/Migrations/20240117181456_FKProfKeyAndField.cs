using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class FKProfKeyAndField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProfessionKeywords",
                columns: table => new
                {
                    profession_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    profession_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    field_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessionKeywords", x => x.profession_id);
                    table.ForeignKey(
                        name: "FK_ProfessionKeywords_JobFields_field_id",
                        column: x => x.field_id,
                        principalTable: "JobFields",
                        principalColumn: "field_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionKeywords_field_id",
                table: "ProfessionKeywords",
                column: "field_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfessionKeywords");
        }
    }
}
