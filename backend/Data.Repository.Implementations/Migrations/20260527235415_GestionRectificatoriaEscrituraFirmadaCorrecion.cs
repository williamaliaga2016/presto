using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class GestionRectificatoriaEscrituraFirmadaCorrecion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id_rectificatoria_firma_comprado_vendedor",
                table: "gestion_rectificatoria_escritura_firmada",
                newName: "id_rectificatoria_firma_comprador_vendedor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id_rectificatoria_firma_comprador_vendedor",
                table: "gestion_rectificatoria_escritura_firmada",
                newName: "id_rectificatoria_firma_comprado_vendedor");
        }
    }
}
