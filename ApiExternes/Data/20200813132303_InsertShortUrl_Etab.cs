using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Data
{
    public partial class InsertShortUrl_Etab : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SiteWebShortUrl",
                table: "Etablissements",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ValidShortUrl",
                table: "Etablissements",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SiteWebShortUrl",
                table: "Etablissements");

            migrationBuilder.DropColumn(
                name: "ValidShortUrl",
                table: "Etablissements");
        }
    }
}
