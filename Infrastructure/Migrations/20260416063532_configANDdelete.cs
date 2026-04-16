using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamkeen.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class configANDdelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vendorProfiles_AspNetUsers_UserId1",
                table: "vendorProfiles");

            migrationBuilder.DropIndex(
                name: "IX_vendorProfiles_UserId1",
                table: "vendorProfiles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "vendorProfiles");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "vendorProfiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "vendorProfiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "vendorProfiles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_vendorProfiles_UserId1",
                table: "vendorProfiles",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_vendorProfiles_AspNetUsers_UserId1",
                table: "vendorProfiles",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
