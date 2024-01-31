using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationClassAttributeRenamed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "lastReviewDate",
                table: "Applications",
                newName: "reviewDate");

            migrationBuilder.RenameColumn(
                name: "lastComment",
                table: "Applications",
                newName: "comment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "reviewDate",
                table: "Applications",
                newName: "lastReviewDate");

            migrationBuilder.RenameColumn(
                name: "comment",
                table: "Applications",
                newName: "lastComment");
        }
    }
}
