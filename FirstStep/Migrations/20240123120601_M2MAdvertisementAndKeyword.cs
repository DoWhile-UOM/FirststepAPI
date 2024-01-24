using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class M2MAdvertisementAndKeyword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementProfessionKeywords_Advertisements_advertisement_id",
                table: "AdvertisementProfessionKeywords");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementProfessionKeywords_ProfessionKeywords_profession_id",
                table: "AdvertisementProfessionKeywords");

            migrationBuilder.RenameColumn(
                name: "profession_id",
                table: "AdvertisementProfessionKeywords",
                newName: "professionKeywordsprofession_id");

            migrationBuilder.RenameColumn(
                name: "advertisement_id",
                table: "AdvertisementProfessionKeywords",
                newName: "advertisementsadvertisement_id");

            migrationBuilder.RenameIndex(
                name: "IX_AdvertisementProfessionKeywords_profession_id",
                table: "AdvertisementProfessionKeywords",
                newName: "IX_AdvertisementProfessionKeywords_professionKeywordsprofession_id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementProfessionKeywords_Advertisements_advertisementsadvertisement_id",
                table: "AdvertisementProfessionKeywords",
                column: "advertisementsadvertisement_id",
                principalTable: "Advertisements",
                principalColumn: "advertisement_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementProfessionKeywords_ProfessionKeywords_professionKeywordsprofession_id",
                table: "AdvertisementProfessionKeywords",
                column: "professionKeywordsprofession_id",
                principalTable: "ProfessionKeywords",
                principalColumn: "profession_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementProfessionKeywords_Advertisements_advertisementsadvertisement_id",
                table: "AdvertisementProfessionKeywords");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementProfessionKeywords_ProfessionKeywords_professionKeywordsprofession_id",
                table: "AdvertisementProfessionKeywords");

            migrationBuilder.RenameColumn(
                name: "professionKeywordsprofession_id",
                table: "AdvertisementProfessionKeywords",
                newName: "profession_id");

            migrationBuilder.RenameColumn(
                name: "advertisementsadvertisement_id",
                table: "AdvertisementProfessionKeywords",
                newName: "advertisement_id");

            migrationBuilder.RenameIndex(
                name: "IX_AdvertisementProfessionKeywords_professionKeywordsprofession_id",
                table: "AdvertisementProfessionKeywords",
                newName: "IX_AdvertisementProfessionKeywords_profession_id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementProfessionKeywords_Advertisements_advertisement_id",
                table: "AdvertisementProfessionKeywords",
                column: "advertisement_id",
                principalTable: "Advertisements",
                principalColumn: "advertisement_id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementProfessionKeywords_ProfessionKeywords_profession_id",
                table: "AdvertisementProfessionKeywords",
                column: "profession_id",
                principalTable: "ProfessionKeywords",
                principalColumn: "profession_id");
        }
    }
}
