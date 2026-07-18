using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class AddAsignarFirmasPeritosAbogados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.CreateTable(
                name: "asignar_firmas_peritos_abogados",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    id_actividad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "ACT_ASIGNAR_FIRMAS"),
                    tipo_cliente = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    codigo_ejecutivo_solicitante = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    oficina_solicitante = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    tipo_solicitud_avaluo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    tipo_tramite_eett = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    nombre_firma_supervisor = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    telefono_firma = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    email_firma = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    valor_avaluo = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    valor_total_consignar = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    opciones_recaudo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    numero_recaudo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    banco = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    nombre_abogado = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    telefono_abogado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    valor_estudio_titulos = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    tipo_cuenta_abogado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    numero_cuenta_abogado = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    requiere_envio_notificacion = table.Column<bool>(type: "boolean", nullable: true),
                    checklist_documentos_solicitar = table.Column<string>(type: "text", nullable: true),
                    observaciones = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    row_status = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asignar_firmas_peritos_abogados", x => x.id);
                });

            

            migrationBuilder.CreateIndex(
                name: "idx_afpa_expediente",
                table: "asignar_firmas_peritos_abogados",
                column: "id_expediente");

           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "asignar_firmas_peritos_abogados");

        }
    }
}
