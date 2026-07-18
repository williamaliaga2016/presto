using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class FixCorregirReparo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id_usuario_solicitante",
                table: "corregir_reparo_cierre_copias_notaria",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_corregir_reparo_cierre_copias_notaria_expediente",
                table: "corregir_reparo_cierre_copias_notaria",
                column: "id_expediente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_corregir_reparo_cierre_copias_notaria_expediente",
                table: "corregir_reparo_cierre_copias_notaria");

            migrationBuilder.DropColumn(
                name: "id_usuario_solicitante",
                table: "corregir_reparo_cierre_copias_notaria");
        }
    }
}
