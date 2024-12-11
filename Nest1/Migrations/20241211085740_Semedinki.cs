using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nest1.Migrations
{
    /// <inheritdoc />
    public partial class Semedinki : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConfirmationKey",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmationKey",
                table: "AspNetUsers");
        }
    }
}
