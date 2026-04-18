using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamkeen.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class cityANDgovernate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Governorate",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Governorate",
                table: "Tickets");
        }
    }
}
