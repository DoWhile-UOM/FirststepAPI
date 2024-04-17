using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class Isuranga_ChangePKAsSinglePK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OTPRequests",
                table: "OTPRequests");

            migrationBuilder.AlterColumn<int>(
                name: "otp",
                table: "OTPRequests",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OTPRequests",
                table: "OTPRequests",
                column: "email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OTPRequests",
                table: "OTPRequests");

            migrationBuilder.AlterColumn<string>(
                name: "otp",
                table: "OTPRequests",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OTPRequests",
                table: "OTPRequests",
                columns: new[] { "email", "otp" });
        }
    }
}
