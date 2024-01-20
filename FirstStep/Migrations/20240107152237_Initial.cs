using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Advertisements",
                columns: table => new
                {
                    job_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    job_number = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    location_province = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    location_city = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employeement_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    arrangement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_experience_required = table.Column<bool>(type: "bit", nullable: false),
                    salary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    posted_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    submission_deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    current_status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    job_overview = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    job_responsibilities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    job_qualifications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    job_benefits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    job_other_details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    hrmanager_id = table.Column<int>(type: "int", nullable: false),
                    field_id = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisements", x => x.job_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Advertisements");
        }
    }
}
