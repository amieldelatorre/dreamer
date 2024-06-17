using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dreamer.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddIsEnabledFieldToJwt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "Jwts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Jwts");
        }
    }
}
