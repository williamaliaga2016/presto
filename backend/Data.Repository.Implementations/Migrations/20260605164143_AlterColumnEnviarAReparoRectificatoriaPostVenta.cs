using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class AlterColumnEnviarAReparoRectificatoriaPostVenta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "enviar_reparo_a",
                table: "rectificatoria_analisis_derivacion_reparo_postventa",
                type: "text",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "enviar_reparo_a",
                table: "rectificatoria_analisis_derivacion_reparo_postventa",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
