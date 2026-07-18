using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class Implement_Tasacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tasacion",
                columns: table => new
                {
                    id_tasacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    is_enviar_reparo = table.Column<bool>(type: "boolean", nullable: false),
                    observaciones = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tasacion", x => x.id_tasacion);
                });

            migrationBuilder.CreateTable(
                name: "tasacion_detalle",
                columns: table => new
                {
                    id_tasacion_detalle = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_tasacion = table.Column<int>(type: "integer", nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    tipo_tasacion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    nro_tasacion_p1 = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    nro_tasacion_p2 = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    nro_tasacion_p3 = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    superficie_edificada = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    superficie_terreno = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    fecha_informe_tasacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    fecha_solicitud_tasacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    fecha_recepcion_tasacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    valor_tasacion_uf = table.Column<decimal>(type: "numeric(18,4)", nullable: true),
                    valor_tasacion_pesos = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    valor_liquidacion_uf = table.Column<decimal>(type: "numeric(18,4)", nullable: true),
                    valor_liquidacion_pesos = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    monto_asegurable_uf = table.Column<decimal>(type: "numeric(18,4)", nullable: true),
                    monto_asegurable_pesos = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tasacion_detalle", x => x.id_tasacion_detalle);
                });

            migrationBuilder.CreateIndex(
                name: "ix_tasacion_expediente",
                table: "tasacion",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "ix_tasacion_detalle_expediente",
                table: "tasacion_detalle",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "ix_tasacion_detalle_tasacion",
                table: "tasacion_detalle",
                column: "id_tasacion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tasacion_detalle");

            migrationBuilder.DropTable(
                name: "tasacion");
        }
    }
}
