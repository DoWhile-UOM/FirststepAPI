using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrationToFitIot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    application_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    submitted_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.application_Id);
                });

            migrationBuilder.CreateTable(
                name: "Companys",
                columns: table => new
                {
                    company_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    business_reg_no = table.Column<int>(type: "int", nullable: false),
                    company_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company_email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company_website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    company_phone_number = table.Column<int>(type: "int", nullable: false),
                    verification_status = table.Column<bool>(type: "bit", nullable: false),
                    business_reg_certificate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    certificate_of_incorporation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companys", x => x.company_id);
                });

            migrationBuilder.CreateTable(
                name: "JobFields",
                columns: table => new
                {
                    field_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    field_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobFields", x => x.field_id);
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
                name: "SeekerSkill",
                columns: table => new
                {
                    seeker_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    skillName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeekerSkill", x => x.seeker_Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    user_type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "RegisteredCompanys",
                columns: table => new
                {
                    company_id = table.Column<int>(type: "int", nullable: false),
                    company_logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    company_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company_city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company_province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company_business_scale = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company_registered_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredCompanys", x => x.company_id);
                    table.ForeignKey(
                        name: "FK_RegisteredCompanys_Companys_company_id",
                        column: x => x.company_id,
                        principalTable: "Companys",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        principalColumn: "field_id");
                });

            migrationBuilder.CreateTable(
                name: "Seekers",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    phone_number = table.Column<int>(type: "int", nullable: false),
                    bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    university = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CVurl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    profile_picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    linkedin = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "Employees",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    is_HRM = table.Column<bool>(type: "bit", nullable: false),
                    company_id = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Advertisements",
                columns: table => new
                {
                    advertisement_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    job_number = table.Column<int>(type: "int", nullable: true),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    employeement_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    arrangement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_experience_required = table.Column<bool>(type: "bit", nullable: false),
                    salary = table.Column<float>(type: "real", nullable: false),
                    posted_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    submission_deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    current_status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    job_overview = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    job_responsibilities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    job_qualifications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    job_benefits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    job_other_details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    hrManager_id = table.Column<int>(type: "int", nullable: false),
                    field_id = table.Column<int>(type: "int", nullable: false),
                    HRAssistantuser_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisements", x => x.advertisement_id);
                    table.ForeignKey(
                        name: "FK_Advertisements_HRAssistants_HRAssistantuser_id",
                        column: x => x.HRAssistantuser_id,
                        principalTable: "HRAssistants",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_Advertisements_HRManagers_hrManager_id",
                        column: x => x.hrManager_id,
                        principalTable: "HRManagers",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_Advertisements_JobFields_field_id",
                        column: x => x.field_id,
                        principalTable: "JobFields",
                        principalColumn: "field_id");
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
                name: "AdvertisementProfessionKeywords",
                columns: table => new
                {
                    advertisementsadvertisement_id = table.Column<int>(type: "int", nullable: false),
                    professionKeywordsprofession_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementProfessionKeywords", x => new { x.advertisementsadvertisement_id, x.professionKeywordsprofession_id });
                    table.ForeignKey(
                        name: "FK_AdvertisementProfessionKeywords_Advertisements_advertisementsadvertisement_id",
                        column: x => x.advertisementsadvertisement_id,
                        principalTable: "Advertisements",
                        principalColumn: "advertisement_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdvertisementProfessionKeywords_ProfessionKeywords_professionKeywordsprofession_id",
                        column: x => x.professionKeywordsprofession_id,
                        principalTable: "ProfessionKeywords",
                        principalColumn: "profession_id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementProfessionKeywords_professionKeywordsprofession_id",
                table: "AdvertisementProfessionKeywords",
                column: "professionKeywordsprofession_id");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_field_id",
                table: "Advertisements",
                column: "field_id");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_HRAssistantuser_id",
                table: "Advertisements",
                column: "HRAssistantuser_id");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_hrManager_id",
                table: "Advertisements",
                column: "hrManager_id");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementSeeker_savedSeekersuser_id",
                table: "AdvertisementSeeker",
                column: "savedSeekersuser_id");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_company_id",
                table: "Employees",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionKeywords_field_id",
                table: "ProfessionKeywords",
                column: "field_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertisementProfessionKeywords");

            migrationBuilder.DropTable(
                name: "AdvertisementSeeker");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "CompanyAdmins");

            migrationBuilder.DropTable(
                name: "Revisions");

            migrationBuilder.DropTable(
                name: "SeekerSkill");

            migrationBuilder.DropTable(
                name: "SystemAdmins");

            migrationBuilder.DropTable(
                name: "ProfessionKeywords");

            migrationBuilder.DropTable(
                name: "Advertisements");

            migrationBuilder.DropTable(
                name: "Seekers");

            migrationBuilder.DropTable(
                name: "HRAssistants");

            migrationBuilder.DropTable(
                name: "HRManagers");

            migrationBuilder.DropTable(
                name: "JobFields");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "RegisteredCompanys");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Companys");
        }
    }
}
