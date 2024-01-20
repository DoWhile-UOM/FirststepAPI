using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class FKAdAndPK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvertisementProfessionKeywords",
                columns: table => new
                {
                    advertisement_id = table.Column<int>(type: "int", nullable: false),
                    profession_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementProfessionKeywords", x => new { x.advertisement_id, x.profession_id });
                    table.ForeignKey(
                        name: "FK_AdvertisementProfessionKeywords_Advertisements_advertisement_id",
                        column: x => x.advertisement_id,
                        principalTable: "Advertisements",
                        principalColumn: "advertisement_id");
                    table.ForeignKey(
                        name: "FK_AdvertisementProfessionKeywords_ProfessionKeywords_profession_id",
                        column: x => x.profession_id,
                        principalTable: "ProfessionKeywords",
                        principalColumn: "profession_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementProfessionKeywords_profession_id",
                table: "AdvertisementProfessionKeywords",
                column: "profession_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertisementProfessionKeywords");
        }
    }
}
