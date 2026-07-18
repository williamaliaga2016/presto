using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class CorrecionDatosOperacionCamposCorreccion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "enviar_a_reparo",
                table: "datos_operacion",
                newName: "enviar_reparo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "enviar_reparo",
                table: "datos_operacion",
                newName: "enviar_a_reparo");
        }
    }
}
