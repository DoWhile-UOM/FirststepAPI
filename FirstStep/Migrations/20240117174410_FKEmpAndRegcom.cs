using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class FKEmpAndRegcom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_advertisement_Seekers_Advertisements_advertisement_id",
                table: "advertisement_Seekers");

            migrationBuilder.DropForeignKey(
                name: "FK_advertisement_Seekers_Seekers_seeker_id",
                table: "advertisement_Seekers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_advertisement_Seekers",
                table: "advertisement_Seekers");

            migrationBuilder.RenameTable(
                name: "advertisement_Seekers",
                newName: "AdvertisementSeekers");

            migrationBuilder.RenameColumn(
                name: "hrmanager_id",
                table: "Advertisements",
                newName: "hrManager_id");

            migrationBuilder.RenameIndex(
                name: "IX_advertisement_Seekers_seeker_id",
                table: "AdvertisementSeekers",
                newName: "IX_AdvertisementSeekers_seeker_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdvertisementSeekers",
                table: "AdvertisementSeekers",
                columns: new[] { "advertisement_id", "seeker_id" });

            migrationBuilder.CreateTable(
                name: "HRManagers",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRManagers", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_HRManagers_Employees_user_id",
                        column: x => x.user_id,
                        principalTable: "Employees",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_company_id",
                table: "Employees",
                column: "company_id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementSeekers_Advertisements_advertisement_id",
                table: "AdvertisementSeekers",
                column: "advertisement_id",
                principalTable: "Advertisements",
                principalColumn: "advertisement_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementSeekers_Seekers_seeker_id",
                table: "AdvertisementSeekers",
                column: "seeker_id",
                principalTable: "Seekers",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_RegisteredCompanys_company_id",
                table: "Employees",
                column: "company_id",
                principalTable: "RegisteredCompanys",
                principalColumn: "company_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementSeekers_Advertisements_advertisement_id",
                table: "AdvertisementSeekers");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementSeekers_Seekers_seeker_id",
                table: "AdvertisementSeekers");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_RegisteredCompanys_company_id",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "HRManagers");

            migrationBuilder.DropIndex(
                name: "IX_Employees_company_id",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdvertisementSeekers",
                table: "AdvertisementSeekers");

            migrationBuilder.RenameTable(
                name: "AdvertisementSeekers",
                newName: "advertisement_Seekers");

            migrationBuilder.RenameColumn(
                name: "hrManager_id",
                table: "Advertisements",
                newName: "hrmanager_id");

            migrationBuilder.RenameIndex(
                name: "IX_AdvertisementSeekers_seeker_id",
                table: "advertisement_Seekers",
                newName: "IX_advertisement_Seekers_seeker_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_advertisement_Seekers",
                table: "advertisement_Seekers",
                columns: new[] { "advertisement_id", "seeker_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_advertisement_Seekers_Advertisements_advertisement_id",
                table: "advertisement_Seekers",
                column: "advertisement_id",
                principalTable: "Advertisements",
                principalColumn: "advertisement_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_advertisement_Seekers_Seekers_seeker_id",
                table: "advertisement_Seekers",
                column: "seeker_id",
                principalTable: "Seekers",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
