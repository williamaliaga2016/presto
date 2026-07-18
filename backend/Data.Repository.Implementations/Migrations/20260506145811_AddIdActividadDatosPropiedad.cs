using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class AddIdActividadDatosPropiedad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "id_actividad",
                table: "datos_propiedad",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "_ab12CD34EfGhIjKlMnOpQr");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "id_actividad",
                table: "datos_propiedad");
        }
    }
}
