using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class AddAdvertisementRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementSeeker_Advertisements_savedAdvertisemntsadvertisement_id",
                table: "AdvertisementSeeker");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementSeeker_Seekers_savedSeekersuser_id",
                table: "AdvertisementSeeker");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdvertisementSeeker",
                table: "AdvertisementSeeker");

            migrationBuilder.RenameTable(
                name: "AdvertisementSeeker",
                newName: "AdvertisementSeekers");

            migrationBuilder.RenameIndex(
                name: "IX_AdvertisementSeeker_savedSeekersuser_id",
                table: "AdvertisementSeekers",
                newName: "IX_AdvertisementSeekers_savedSeekersuser_id");

            migrationBuilder.AddColumn<int>(
                name: "company_id",
                table: "Advertisements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdvertisementSeekers",
                table: "AdvertisementSeekers",
                columns: new[] { "savedAdvertisemntsadvertisement_id", "savedSeekersuser_id" });

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_company_id",
                table: "Advertisements",
                column: "company_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_Companies_company_id",
                table: "Advertisements",
                column: "company_id",
                principalTable: "Companies",
                principalColumn: "company_id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementSeekers_Advertisements_savedAdvertisemntsadvertisement_id",
                table: "AdvertisementSeekers",
                column: "savedAdvertisemntsadvertisement_id",
                principalTable: "Advertisements",
                principalColumn: "advertisement_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementSeekers_Seekers_savedSeekersuser_id",
                table: "AdvertisementSeekers",
                column: "savedSeekersuser_id",
                principalTable: "Seekers",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_Companies_company_id",
                table: "Advertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementSeekers_Advertisements_savedAdvertisemntsadvertisement_id",
                table: "AdvertisementSeekers");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementSeekers_Seekers_savedSeekersuser_id",
                table: "AdvertisementSeekers");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_company_id",
                table: "Advertisements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdvertisementSeekers",
                table: "AdvertisementSeekers");

            migrationBuilder.DropColumn(
                name: "company_id",
                table: "Advertisements");

            migrationBuilder.RenameTable(
                name: "AdvertisementSeekers",
                newName: "AdvertisementSeeker");

            migrationBuilder.RenameIndex(
                name: "IX_AdvertisementSeekers_savedSeekersuser_id",
                table: "AdvertisementSeeker",
                newName: "IX_AdvertisementSeeker_savedSeekersuser_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdvertisementSeeker",
                table: "AdvertisementSeeker",
                columns: new[] { "savedAdvertisemntsadvertisement_id", "savedSeekersuser_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementSeeker_Advertisements_savedAdvertisemntsadvertisement_id",
                table: "AdvertisementSeeker",
                column: "savedAdvertisemntsadvertisement_id",
                principalTable: "Advertisements",
                principalColumn: "advertisement_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementSeeker_Seekers_savedSeekersuser_id",
                table: "AdvertisementSeeker",
                column: "savedSeekersuser_id",
                principalTable: "Seekers",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
