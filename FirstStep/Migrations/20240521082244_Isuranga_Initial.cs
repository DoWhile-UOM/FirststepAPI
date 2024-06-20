using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Firststep.Migrations
{
    /// <inheritdoc />
    public partial class Isuranga_Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "OTPRequests",
                columns: table => new
                {
                    email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    otp = table.Column<int>(type: "int", nullable: false),
                    expiry_date_time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OTPRequests", x => x.email);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    skill_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    skill_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.skill_id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    user_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    refresh_token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    refresh_token_expiry = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.user_id);
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
                    linkedin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    field_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seekers", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_Seekers_JobFields_field_id",
                        column: x => x.field_id,
                        principalTable: "JobFields",
                        principalColumn: "field_id");
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
                name: "SeekerSkills",
                columns: table => new
                {
                    seekersuser_id = table.Column<int>(type: "int", nullable: false),
                    skillsskill_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeekerSkills", x => new { x.seekersuser_id, x.skillsskill_id });
                    table.ForeignKey(
                        name: "FK_SeekerSkills_Seekers_seekersuser_id",
                        column: x => x.seekersuser_id,
                        principalTable: "Seekers",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeekerSkills_Skills_skillsskill_id",
                        column: x => x.skillsskill_id,
                        principalTable: "Skills",
                        principalColumn: "skill_id",
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
                        name: "FK_AdvertisementProfessionKeywords_ProfessionKeywords_professionKeywordsprofession_id",
                        column: x => x.professionKeywordsprofession_id,
                        principalTable: "ProfessionKeywords",
                        principalColumn: "profession_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Advertisements",
                columns: table => new
                {
                    advertisement_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    job_number = table.Column<int>(type: "int", nullable: true),
                    title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    country = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    city = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    employeement_type = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    arrangement = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    experience = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    salary = table.Column<float>(type: "real", nullable: true),
                    currency_unit = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    posted_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    submission_deadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    expired_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    current_status = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    job_description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    hrManager_id = table.Column<int>(type: "int", nullable: false),
                    field_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisements", x => x.advertisement_id);
                    table.ForeignKey(
                        name: "FK_Advertisements_JobFields_field_id",
                        column: x => x.field_id,
                        principalTable: "JobFields",
                        principalColumn: "field_id");
                });

            migrationBuilder.CreateTable(
                name: "AdvertisementSeekers",
                columns: table => new
                {
                    savedAdvertisemntsadvertisement_id = table.Column<int>(type: "int", nullable: false),
                    savedSeekersuser_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementSeekers", x => new { x.savedAdvertisemntsadvertisement_id, x.savedSeekersuser_id });
                    table.ForeignKey(
                        name: "FK_AdvertisementSeekers_Advertisements_savedAdvertisemntsadvertisement_id",
                        column: x => x.savedAdvertisemntsadvertisement_id,
                        principalTable: "Advertisements",
                        principalColumn: "advertisement_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdvertisementSeekers_Seekers_savedSeekersuser_id",
                        column: x => x.savedSeekersuser_id,
                        principalTable: "Seekers",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    application_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    submitted_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CVurl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    doc1_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    doc2_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    advertisement_id = table.Column<int>(type: "int", nullable: false),
                    seeker_id = table.Column<int>(type: "int", nullable: false),
                    assigned_hrAssistant_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.application_Id);
                    table.ForeignKey(
                        name: "FK_Applications_Advertisements_advertisement_id",
                        column: x => x.advertisement_id,
                        principalTable: "Advertisements",
                        principalColumn: "advertisement_id");
                    table.ForeignKey(
                        name: "FK_Applications_Seekers_seeker_id",
                        column: x => x.seeker_id,
                        principalTable: "Seekers",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Companies",
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
                    certificate_of_incorporation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    company_applied_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    company_logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    company_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    company_city = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    company_province = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    company_business_scale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    company_registered_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    verified_system_admin_id = table.Column<int>(type: "int", nullable: true),
                    registration_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    company_admin_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.company_id);
                    table.ForeignKey(
                        name: "FK_Companies_SystemAdmins_verified_system_admin_id",
                        column: x => x.verified_system_admin_id,
                        principalTable: "SystemAdmins",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    company_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_Employees_Companies_company_id",
                        column: x => x.company_id,
                        principalTable: "Companies",
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
                name: "Revisions",
                columns: table => new
                {
                    revision_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    application_id = table.Column<int>(type: "int", nullable: false),
                    employee_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Revisions", x => x.revision_id);
                    table.ForeignKey(
                        name: "FK_Revisions_Applications_application_id",
                        column: x => x.application_id,
                        principalTable: "Applications",
                        principalColumn: "application_Id");
                    table.ForeignKey(
                        name: "FK_Revisions_Employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "Employees",
                        principalColumn: "user_id");
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
                name: "IX_Advertisements_hrManager_id",
                table: "Advertisements",
                column: "hrManager_id");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementSeekers_savedSeekersuser_id",
                table: "AdvertisementSeekers",
                column: "savedSeekersuser_id");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementSkills_skillsskill_id",
                table: "AdvertisementSkills",
                column: "skillsskill_id");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_advertisement_id",
                table: "Applications",
                column: "advertisement_id");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_assigned_hrAssistant_id",
                table: "Applications",
                column: "assigned_hrAssistant_id");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_seeker_id",
                table: "Applications",
                column: "seeker_id");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_company_admin_id",
                table: "Companies",
                column: "company_admin_id",
                unique: true,
                filter: "[company_admin_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_verified_system_admin_id",
                table: "Companies",
                column: "verified_system_admin_id");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_company_id",
                table: "Employees",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionKeywords_field_id",
                table: "ProfessionKeywords",
                column: "field_id");

            migrationBuilder.CreateIndex(
                name: "IX_Revisions_application_id",
                table: "Revisions",
                column: "application_id");

            migrationBuilder.CreateIndex(
                name: "IX_Revisions_employee_id",
                table: "Revisions",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_Seekers_field_id",
                table: "Seekers",
                column: "field_id");

            migrationBuilder.CreateIndex(
                name: "IX_SeekerSkills_skillsskill_id",
                table: "SeekerSkills",
                column: "skillsskill_id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementProfessionKeywords_Advertisements_advertisementsadvertisement_id",
                table: "AdvertisementProfessionKeywords",
                column: "advertisementsadvertisement_id",
                principalTable: "Advertisements",
                principalColumn: "advertisement_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_HRManagers_hrManager_id",
                table: "Advertisements",
                column: "hrManager_id",
                principalTable: "HRManagers",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_HRAssistants_assigned_hrAssistant_id",
                table: "Applications",
                column: "assigned_hrAssistant_id",
                principalTable: "HRAssistants",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_HRManagers_company_admin_id",
                table: "Companies",
                column: "company_admin_id",
                principalTable: "HRManagers",
                principalColumn: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_HRManagers_company_admin_id",
                table: "Companies");

            migrationBuilder.DropTable(
                name: "AdvertisementProfessionKeywords");

            migrationBuilder.DropTable(
                name: "AdvertisementSeekers");

            migrationBuilder.DropTable(
                name: "AdvertisementSkills");

            migrationBuilder.DropTable(
                name: "OTPRequests");

            migrationBuilder.DropTable(
                name: "Revisions");

            migrationBuilder.DropTable(
                name: "SeekerSkills");

            migrationBuilder.DropTable(
                name: "ProfessionKeywords");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Advertisements");

            migrationBuilder.DropTable(
                name: "HRAssistants");

            migrationBuilder.DropTable(
                name: "Seekers");

            migrationBuilder.DropTable(
                name: "JobFields");

            migrationBuilder.DropTable(
                name: "HRManagers");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "SystemAdmins");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
