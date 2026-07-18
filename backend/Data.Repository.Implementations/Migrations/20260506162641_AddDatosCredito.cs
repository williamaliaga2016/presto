using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class AddDatosCredito : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "datos_credito",
                columns: table => new
                {
                    id_datos_credito = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    id_actividad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ubicacion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    id_tipo_operacion = table.Column<int>(type: "integer", nullable: true),
                    fines_generales = table.Column<bool>(type: "boolean", nullable: true),
                    nombre_proyecto = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    credito_segunda_vivienda = table.Column<bool>(type: "boolean", nullable: false),
                    inmobiliaria = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    vivienda_social = table.Column<bool>(type: "boolean", nullable: true),
                    dfl2 = table.Column<bool>(type: "boolean", nullable: true),
                    propietario_dfl2 = table.Column<bool>(type: "boolean", nullable: true),
                    recepcion_final_mayor_2_anios = table.Column<bool>(type: "boolean", nullable: true),
                    porcentaje_impuesto = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    monto_credito_afecto_impuesto = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    impuesto_a_pagar = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
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
                    table.PrimaryKey("PK_datos_credito", x => x.id_datos_credito);
                });

            migrationBuilder.CreateIndex(
                name: "IX_datos_credito_id_expediente",
                table: "datos_credito",
                column: "id_expediente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "datos_credito");
        }
    }
}
