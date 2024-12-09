using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DungeonMastersArchive.Migrations
{
    /// <inheritdoc />
    public partial class CreateIdentitySchema2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentCampaign",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentRole",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentCampaign",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CurrentRole",
                table: "AspNetUsers");
        }
    }
}
