using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class GestionRectificatoriaSolucionReparocorreciondecampos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "descontabilizar_operacion",
                table: "gestion_rectificatoria_solucion_reparo",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "modificar_datos_memo",
                table: "gestion_rectificatoria_solucion_reparo",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "descontabilizar_operacion",
                table: "gestion_rectificatoria_solucion_reparo");

            migrationBuilder.DropColumn(
                name: "modificar_datos_memo",
                table: "gestion_rectificatoria_solucion_reparo");
        }
    }
}
