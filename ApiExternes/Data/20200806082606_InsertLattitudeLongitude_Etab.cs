using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Data
{
    public partial class InsertLattitudeLongitude_Etab : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddLatitude",
                table: "Etablissements",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddLongitude",
                table: "Etablissements",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddLatitude",
                table: "Etablissements");

            migrationBuilder.DropColumn(
                name: "AddLongitude",
                table: "Etablissements");
        }
    }
}
