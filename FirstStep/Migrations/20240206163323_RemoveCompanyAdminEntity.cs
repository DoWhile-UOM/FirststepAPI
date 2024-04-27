using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCompanyAdminEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyAdmins");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
