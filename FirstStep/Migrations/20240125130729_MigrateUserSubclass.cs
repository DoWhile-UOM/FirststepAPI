using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class MigrateUserSubclass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_HRManagers_hrManager_id",
                table: "Advertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementSeekers_Seekers_seeker_id",
                table: "AdvertisementSeekers");

            migrationBuilder.DropTable(
                name: "HRManagers");

            migrationBuilder.DropTable(
                name: "Seekers");

            migrationBuilder.DropTable(
                name: "SystemAdmins");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "CVurl",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "bio",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "company_id",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_HRM",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "linkedin",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "phone_number",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "profile_picture",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "university",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_company_id",
                table: "Users",
                column: "company_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_Users_hrManager_id",
                table: "Advertisements",
                column: "hrManager_id",
                principalTable: "Users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementSeekers_Users_seeker_id",
                table: "AdvertisementSeekers",
                column: "seeker_id",
                principalTable: "Users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_RegisteredCompanys_company_id",
                table: "Users",
                column: "company_id",
                principalTable: "RegisteredCompanys",
                principalColumn: "company_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_Users_hrManager_id",
                table: "Advertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementSeekers_Users_seeker_id",
                table: "AdvertisementSeekers");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_RegisteredCompanys_company_id",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_company_id",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CVurl",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "bio",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "company_id",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "description",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "is_HRM",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "linkedin",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "phone_number",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "profile_picture",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "university",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    is_HRM = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_Employees_RegisteredCompanys_company_id",
                        column: x => x.company_id,
                        principalTable: "RegisteredCompanys",
                        principalColumn: "company_id");
                    table.ForeignKey(
                        name: "FK_Employees_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seekers",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    CVurl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    linkedin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone_number = table.Column<int>(type: "int", nullable: false),
                    profile_picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    university = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seekers", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_Seekers_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "FK_Advertisements_HRManagers_hrManager_id",
                table: "Advertisements",
                column: "hrManager_id",
                principalTable: "HRManagers",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementSeekers_Seekers_seeker_id",
                table: "AdvertisementSeekers",
                column: "seeker_id",
                principalTable: "Seekers",
                principalColumn: "user_id");
        }
    }
}
