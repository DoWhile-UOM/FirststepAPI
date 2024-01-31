using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class AddUserEmpSubClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertisementSeekers");

            migrationBuilder.DropColumn(
                name: "comment",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "review_date",
                table: "Applications");

            migrationBuilder.AddColumn<int>(
                name: "HRAssistantuser_id",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AdvertisementSeeker",
                columns: table => new
                {
                    savedAdvertisemntsadvertisement_id = table.Column<int>(type: "int", nullable: false),
                    savedSeekersuser_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementSeeker", x => new { x.savedAdvertisemntsadvertisement_id, x.savedSeekersuser_id });
                    table.ForeignKey(
                        name: "FK_AdvertisementSeeker_Advertisements_savedAdvertisemntsadvertisement_id",
                        column: x => x.savedAdvertisemntsadvertisement_id,
                        principalTable: "Advertisements",
                        principalColumn: "advertisement_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdvertisementSeeker_Seekers_savedSeekersuser_id",
                        column: x => x.savedSeekersuser_id,
                        principalTable: "Seekers",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyAdmins",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyAdmins", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_CompanyAdmins_HRManagers_user_id",
                        column: x => x.user_id,
                        principalTable: "HRManagers",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HRAssistants",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRAssistants", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_HRAssistants_Employees_user_id",
                        column: x => x.user_id,
                        principalTable: "Employees",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Revisions",
                columns: table => new
                {
                    revision_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Revisions", x => x.revision_id);
                });

            migrationBuilder.CreateTable(
                name: "SystemAdmins",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemAdmins", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_SystemAdmins_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_HRAssistantuser_id",
                table: "Advertisements",
                column: "HRAssistantuser_id");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementSeeker_savedSeekersuser_id",
                table: "AdvertisementSeeker",
                column: "savedSeekersuser_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_HRAssistants_HRAssistantuser_id",
                table: "Advertisements",
                column: "HRAssistantuser_id",
                principalTable: "HRAssistants",
                principalColumn: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_HRAssistants_HRAssistantuser_id",
                table: "Advertisements");

            migrationBuilder.DropTable(
                name: "AdvertisementSeeker");

            migrationBuilder.DropTable(
                name: "CompanyAdmins");

            migrationBuilder.DropTable(
                name: "HRAssistants");

            migrationBuilder.DropTable(
                name: "Revisions");

            migrationBuilder.DropTable(
                name: "SystemAdmins");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_HRAssistantuser_id",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "HRAssistantuser_id",
                table: "Advertisements");

            migrationBuilder.AddColumn<string>(
                name: "comment",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "review_date",
                table: "Applications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AdvertisementSeekers",
                columns: table => new
                {
                    advertisement_id = table.Column<int>(type: "int", nullable: false),
                    seeker_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementSeekers", x => new { x.advertisement_id, x.seeker_id });
                    table.ForeignKey(
                        name: "FK_AdvertisementSeekers_Advertisements_advertisement_id",
                        column: x => x.advertisement_id,
                        principalTable: "Advertisements",
                        principalColumn: "advertisement_id");
                    table.ForeignKey(
                        name: "FK_AdvertisementSeekers_Seekers_seeker_id",
                        column: x => x.seeker_id,
                        principalTable: "Seekers",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementSeekers_seeker_id",
                table: "AdvertisementSeekers",
                column: "seeker_id");
        }
    }
}
