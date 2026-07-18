using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class FixCorregirDetalles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "observaciones",
                table: "corregir_reparo_estudio_titulos",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "id_usuario_solicitante",
                table: "corregir_reparo_estudio_titulos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "is_subsanar",
                table: "corregir_reparo_estudio_titulos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "ix_reingresar_escritura_cbr_expediente",
                table: "reingresar_escritura_cbr",
                column: "id_expediente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_reingresar_escritura_cbr_expediente",
                table: "reingresar_escritura_cbr");

            migrationBuilder.DropColumn(
                name: "id_usuario_solicitante",
                table: "corregir_reparo_estudio_titulos");

            migrationBuilder.DropColumn(
                name: "is_subsanar",
                table: "corregir_reparo_estudio_titulos");

            migrationBuilder.AlterColumn<string>(
                name: "observaciones",
                table: "corregir_reparo_estudio_titulos",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);
        }
    }
}
