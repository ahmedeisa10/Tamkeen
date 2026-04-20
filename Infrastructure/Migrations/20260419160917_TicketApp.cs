using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamkeen.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TicketApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TicketApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AppliedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketApplications_AspNetUsers_VendorId",
                        column: x => x.VendorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketApplications_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketApplications_TicketId_VendorId",
                table: "TicketApplications",
                columns: new[] { "TicketId", "VendorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketApplications_VendorId",
                table: "TicketApplications",
                column: "VendorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketApplications");
        }
    }
}
