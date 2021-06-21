using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Data
{
    public partial class UpdateEtablissementsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Heures_Fermeture",
                table: "Etablissements");

            migrationBuilder.DropColumn(
                name: "Heures_Ouverture",
                table: "Etablissements");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "Heures_Fermeture",
                table: "Etablissements",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Heures_Ouverture",
                table: "Etablissements",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
