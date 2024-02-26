using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class AddSkillAdvertisementRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_Companies_company_id",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_company_id",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "company_id",
                table: "Advertisements");

            migrationBuilder.CreateTable(
                name: "AdvertisementSkills",
                columns: table => new
                {
                    advertisementsadvertisement_id = table.Column<int>(type: "int", nullable: false),
                    skillsskill_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementSkills", x => new { x.advertisementsadvertisement_id, x.skillsskill_id });
                    table.ForeignKey(
                        name: "FK_AdvertisementSkills_Advertisements_advertisementsadvertisement_id",
                        column: x => x.advertisementsadvertisement_id,
                        principalTable: "Advertisements",
                        principalColumn: "advertisement_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdvertisementSkills_Skills_skillsskill_id",
                        column: x => x.skillsskill_id,
                        principalTable: "Skills",
                        principalColumn: "skill_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementSkills_skillsskill_id",
                table: "AdvertisementSkills",
                column: "skillsskill_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertisementSkills");

            migrationBuilder.AddColumn<int>(
                name: "company_id",
                table: "Advertisements",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
        }
    }
}
