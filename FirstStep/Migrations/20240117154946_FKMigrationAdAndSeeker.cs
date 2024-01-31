using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class FKMigrationAdAndSeeker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "job_number",
                table: "Advertisements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "advertisement_Seekers",
                columns: table => new
                {
                    advertisement_id = table.Column<int>(type: "int", nullable: false),
                    seeker_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_advertisement_Seekers", x => new { x.advertisement_id, x.seeker_id });
                    table.ForeignKey(
                        name: "FK_advertisement_Seekers_Advertisements_advertisement_id",
                        column: x => x.advertisement_id,
                        principalTable: "Advertisements",
                        principalColumn: "advertisement_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_advertisement_Seekers_Seekers_seeker_id",
                        column: x => x.seeker_id,
                        principalTable: "Seekers",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_advertisement_Seekers_seeker_id",
                table: "advertisement_Seekers",
                column: "seeker_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "advertisement_Seekers");

            migrationBuilder.AlterColumn<int>(
                name: "job_number",
                table: "Advertisements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
