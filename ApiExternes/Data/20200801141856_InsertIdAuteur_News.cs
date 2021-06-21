using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Data
{
    public partial class InsertIdAuteur_News : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Horaires_Etablissements_EtablissementId",
                table: "Horaires");

            migrationBuilder.DropForeignKey(
                name: "FK_PhotosEtablissements_Etablissements_EtablissementId",
                table: "PhotosEtablissements");

            migrationBuilder.AlterColumn<int>(
                name: "EtablissementId",
                table: "PhotosEtablissements",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomAuteur",
                table: "News",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EtablissementId",
                table: "Horaires",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Horaires_Etablissements_EtablissementId",
                table: "Horaires",
                column: "EtablissementId",
                principalTable: "Etablissements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhotosEtablissements_Etablissements_EtablissementId",
                table: "PhotosEtablissements",
                column: "EtablissementId",
                principalTable: "Etablissements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Horaires_Etablissements_EtablissementId",
                table: "Horaires");

            migrationBuilder.DropForeignKey(
                name: "FK_PhotosEtablissements_Etablissements_EtablissementId",
                table: "PhotosEtablissements");

            migrationBuilder.DropColumn(
                name: "NomAuteur",
                table: "News");

            migrationBuilder.AlterColumn<int>(
                name: "EtablissementId",
                table: "PhotosEtablissements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "EtablissementId",
                table: "Horaires",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Horaires_Etablissements_EtablissementId",
                table: "Horaires",
                column: "EtablissementId",
                principalTable: "Etablissements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PhotosEtablissements_Etablissements_EtablissementId",
                table: "PhotosEtablissements",
                column: "EtablissementId",
                principalTable: "Etablissements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
