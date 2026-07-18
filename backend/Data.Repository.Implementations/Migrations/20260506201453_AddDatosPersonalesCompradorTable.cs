using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class AddDatosPersonalesCompradorTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "datos_personales_comprador",
                columns: table => new
                {
                    id_datos_personales_comprador = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    rut = table.Column<string>(type: "text", nullable: false),
                    direccion = table.Column<string>(type: "text", nullable: false),
                    direccion_env_esc = table.Column<string>(type: "text", nullable: false),
                    region = table.Column<string>(type: "text", nullable: false),
                    region_env_esc = table.Column<string>(type: "text", nullable: false),
                    comuna = table.Column<string>(type: "text", nullable: false),
                    comuna_env_esc = table.Column<string>(type: "text", nullable: false),
                    direccion_env_div = table.Column<string>(type: "text", nullable: false),
                    region_env_div = table.Column<string>(type: "text", nullable: false),
                    comuna_env_div = table.Column<string>(type: "text", nullable: false),
                    tipo_dir_dividendo = table.Column<int>(type: "integer", nullable: false),
                    telefono = table.Column<string>(type: "text", nullable: false),
                    telefono_comercial = table.Column<string>(type: "text", nullable: true),
                    telefono_movil = table.Column<string>(type: "text", nullable: true),
                    profesion = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: false),
                    email2 = table.Column<string>(type: "text", nullable: true),
                    id_relacion_titular = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_datos_personales_comprador", x => x.id_datos_personales_comprador);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "datos_personales_comprador");
        }
    }
}
