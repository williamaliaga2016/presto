using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class CambiarTipoTasacionAString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "tipo_tasacion",
                table: "tasacion_detalle",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldMaxLength: 50,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "tipo_tasacion",
                table: "tasacion_detalle",
                type: "boolean",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
