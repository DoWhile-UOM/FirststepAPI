using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class Isuranga_ChangePKofOTPRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OTPRequests",
                table: "OTPRequests");

            migrationBuilder.AlterColumn<string>(
                name: "otp",
                table: "OTPRequests",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OTPRequests",
                table: "OTPRequests",
                columns: new[] { "email", "otp" });
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
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OTPRequests",
                table: "OTPRequests",
                column: "email");
        }
    }
}
