using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class AddDatosVendedorTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "datos_vendedor",
                columns: table => new
                {
                    id_datos_vendedor = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    id_actividad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    enviar_a_reparo = table.Column<bool>(type: "boolean", nullable: true),
                    observaciones = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_datos_vendedor", x => x.id_datos_vendedor);
                });

            migrationBuilder.CreateTable(
                name: "datos_vendedor_detalle",
                columns: table => new
                {
                    id_datos_vendedor_detalle = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    tipo_vendedor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    rut = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    nombres = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    apellido_paterno = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    apellido_materno = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    fecha_nacimiento = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    genero = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    estado_civil = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    nacionalidad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    profesion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    razon_social = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    relacion_titular = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    direccion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    region = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    comuna = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    telefono = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    telefono_comercial = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    telefono_movil = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    email2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_datos_vendedor_detalle", x => x.id_datos_vendedor_detalle);
                });

            migrationBuilder.CreateIndex(
                name: "IX_datos_vendedor_id_expediente",
                table: "datos_vendedor",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "IX_datos_vendedor_detalle_id_expediente",
                table: "datos_vendedor_detalle",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "IX_datos_vendedor_detalle_rut",
                table: "datos_vendedor_detalle",
                column: "rut");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "datos_vendedor");

            migrationBuilder.DropTable(
                name: "datos_vendedor_detalle");
        }
    }
}
