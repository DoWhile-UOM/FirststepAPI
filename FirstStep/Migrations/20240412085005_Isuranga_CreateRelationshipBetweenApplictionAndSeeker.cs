using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstStep.Migrations
{
    /// <inheritdoc />
    public partial class Isuranga_CreateRelationshipBetweenApplictionAndSeeker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "seekeruser_id",
                table: "Applications");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_user_id",
                table: "Applications",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Seekers_user_id",
                table: "Applications",
                column: "user_id",
                principalTable: "Seekers",
                principalColumn: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_advertisement_id",
                table: "Applications",
                column: "advertisement_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Advertisements_advertisement_id",
                table: "Applications",
                column: "advertisement_id",
                principalTable: "Advertisements",
                principalColumn: "advertisement_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Seekers_user_id",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_user_id",
                table: "Applications");

            migrationBuilder.AddColumn<int>(
                name: "seekeruser_id",
                table: "Applications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_seekeruser_id",
                table: "Applications",
                column: "seekeruser_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Seekers_seekeruser_id",
                table: "Applications",
                column: "seekeruser_id",
                principalTable: "Seekers",
                principalColumn: "user_id");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Advertisements_advertisement_id",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_advertisement_id",
                table: "Applications");
        }
    }
}
