using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class FKAd_and_HRManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_JobFields_field_id",
                table: "Advertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementSeekers_Advertisements_advertisement_id",
                table: "AdvertisementSeekers");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementSeekers_Seekers_seeker_id",
                table: "AdvertisementSeekers");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_RegisteredCompanys_company_id",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfessionKeywords_JobFields_field_id",
                table: "ProfessionKeywords");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_hrManager_id",
                table: "Advertisements",
                column: "hrManager_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_HRManagers_hrManager_id",
                table: "Advertisements",
                column: "hrManager_id",
                principalTable: "HRManagers",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_JobFields_field_id",
                table: "Advertisements",
                column: "field_id",
                principalTable: "JobFields",
                principalColumn: "field_id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementSeekers_Advertisements_advertisement_id",
                table: "AdvertisementSeekers",
                column: "advertisement_id",
                principalTable: "Advertisements",
                principalColumn: "advertisement_id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementSeekers_Seekers_seeker_id",
                table: "AdvertisementSeekers",
                column: "seeker_id",
                principalTable: "Seekers",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_RegisteredCompanys_company_id",
                table: "Employees",
                column: "company_id",
                principalTable: "RegisteredCompanys",
                principalColumn: "company_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessionKeywords_JobFields_field_id",
                table: "ProfessionKeywords",
                column: "field_id",
                principalTable: "JobFields",
                principalColumn: "field_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_HRManagers_hrManager_id",
                table: "Advertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_JobFields_field_id",
                table: "Advertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementSeekers_Advertisements_advertisement_id",
                table: "AdvertisementSeekers");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementSeekers_Seekers_seeker_id",
                table: "AdvertisementSeekers");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_RegisteredCompanys_company_id",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfessionKeywords_JobFields_field_id",
                table: "ProfessionKeywords");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_hrManager_id",
                table: "Advertisements");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_JobFields_field_id",
                table: "Advertisements",
                column: "field_id",
                principalTable: "JobFields",
                principalColumn: "field_id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessionKeywords_JobFields_field_id",
                table: "ProfessionKeywords",
                column: "field_id",
                principalTable: "JobFields",
                principalColumn: "field_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
