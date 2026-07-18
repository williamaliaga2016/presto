using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class AddValidacionRectificatoriaLegalPostventaTabla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "validacion_rectificatoria_legal_postventa",
                columns: table => new
                {
                    id_validacion_rectificatoria_legal_postventa = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    id_usuario_solicitante = table.Column<int>(type: "integer", nullable: false),
                    is_subsanar = table.Column<bool>(type: "boolean", nullable: false),
                    observaciones = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    require_documentacion = table.Column<string>(type: "text", nullable: false),
                    realiza_pago = table.Column<string>(type: "text", nullable: false),
                    encargado_firma = table.Column<bool>(type: "boolean", nullable: false),
                    requiere_inscripcion_cbr = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_validacion_rectificatoria_legal_postventa", x => x.id_validacion_rectificatoria_legal_postventa);
                });

            migrationBuilder.CreateTable(
                name: "validacion_rectificatoria_legal_postventa_datos_personales",
                columns: table => new
                {
                    id_validacion_rectificatoria_legal_postventa_datos_personales = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_validacion_rectificatoria_legal_postventa = table.Column<int>(type: "integer", nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    rut = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: true),
                    fecha_nacimiento = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    genero = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    nombres = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    apellido_paterno = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    apellido_materno = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    nacionalidad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    relacion_titular = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    profesion = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    direccion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    estado_civil = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    region = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    comuna = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    rol_comparecencia = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_validacion_rectificatoria_legal_postventa_datos_personales", x => x.id_validacion_rectificatoria_legal_postventa_datos_personales);
                });

            migrationBuilder.CreateIndex(
                name: "ix_validacion_rectificatoria_legal_postventa_expediente",
                table: "validacion_rectificatoria_legal_postventa",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "ix_id_validacion_rectificatoria_legal_postventa_datos_personales_expediente",
                table: "validacion_rectificatoria_legal_postventa_datos_personales",
                column: "id_expediente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "validacion_rectificatoria_legal_postventa");

            migrationBuilder.DropTable(
                name: "validacion_rectificatoria_legal_postventa_datos_personales");
        }
    }
}
