using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class FixCorregirReparoGenerarMemoEscritura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "observaciones",
                table: "corregir_reparo_generar_memo_escritura",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<bool>(
                name: "is_subsanar",
                table: "corregir_reparo_generar_memo_escritura",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "id_usuario_solicitante",
                table: "corregir_reparo_generar_memo_escritura",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "id_usuario_solicitante",
                table: "corregir_reparo_generar_memo_escritura");

            migrationBuilder.AlterColumn<string>(
                name: "observaciones",
                table: "corregir_reparo_generar_memo_escritura",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_subsanar",
                table: "corregir_reparo_generar_memo_escritura",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");
        }
    }
}
