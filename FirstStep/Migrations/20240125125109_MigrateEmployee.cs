using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class MigrateEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Applications",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "role",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "applicationId",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "submittedDate",
                table: "Applications",
                newName: "submitted_date");

            migrationBuilder.RenameColumn(
                name: "reviewDate",
                table: "Applications",
                newName: "review_date");

            migrationBuilder.RenameColumn(
                name: "phoneNumber",
                table: "Applications",
                newName: "phone_number");

            migrationBuilder.AddColumn<bool>(
                name: "is_HRM",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "application_Id",
                table: "Applications",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Applications",
                table: "Applications",
                column: "application_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Applications",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "is_HRM",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "application_Id",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "submitted_date",
                table: "Applications",
                newName: "submittedDate");

            migrationBuilder.RenameColumn(
                name: "review_date",
                table: "Applications",
                newName: "reviewDate");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "Applications",
                newName: "phoneNumber");

            migrationBuilder.AddColumn<string>(
                name: "role",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "applicationId",
                table: "Applications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Applications",
                table: "Applications",
                column: "applicationId");
        }
    }
}
