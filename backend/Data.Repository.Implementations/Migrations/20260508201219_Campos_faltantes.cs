using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class Campos_faltantes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "banco_alzante",
                table: "carga_operacion_banco_datos_operacion",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "codigo_producto_comercial",
                table: "carga_operacion_banco_datos_operacion",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nombre_proyecto",
                table: "carga_operacion_banco_datos_operacion",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "nro_piloto",
                table: "carga_operacion_banco_datos_operacion",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "banco_alzante",
                table: "carga_operacion_banco_datos_operacion");

            migrationBuilder.DropColumn(
                name: "codigo_producto_comercial",
                table: "carga_operacion_banco_datos_operacion");

            migrationBuilder.DropColumn(
                name: "nombre_proyecto",
                table: "carga_operacion_banco_datos_operacion");

            migrationBuilder.DropColumn(
                name: "nro_piloto",
                table: "carga_operacion_banco_datos_operacion");
        }
    }
}
