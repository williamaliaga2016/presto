using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDatosPropiedadFromCalculoGeneracionDocumento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "comuna",
                table: "calculo_generacion_documento");

            migrationBuilder.DropColumn(
                name: "direccion",
                table: "calculo_generacion_documento");

            migrationBuilder.DropColumn(
                name: "region",
                table: "calculo_generacion_documento");

            migrationBuilder.DropColumn(
                name: "rol_avaluo",
                table: "calculo_generacion_documento");

            migrationBuilder.DropColumn(
                name: "tipo_direccion",
                table: "calculo_generacion_documento");

            migrationBuilder.DropColumn(
                name: "tipo_propiedad",
                table: "calculo_generacion_documento");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "comuna",
                table: "calculo_generacion_documento",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "direccion",
                table: "calculo_generacion_documento",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "region",
                table: "calculo_generacion_documento",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rol_avaluo",
                table: "calculo_generacion_documento",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tipo_direccion",
                table: "calculo_generacion_documento",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tipo_propiedad",
                table: "calculo_generacion_documento",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
