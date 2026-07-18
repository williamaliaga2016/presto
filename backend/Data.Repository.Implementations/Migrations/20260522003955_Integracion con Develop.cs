using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class IntegracionconDevelop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE IF EXISTS verificar_reparo_datos_operacion_detalle CASCADE;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "verificar_reparo_datos_operacion_detalle",
                columns: table => new
                {
                    id_verificar_reparo_datos_operacion_detalle = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fecha_ingreso = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    observaciones = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    solicitante = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    subsanar = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_verificar_reparo_datos_operacion_detalle", x => x.id_verificar_reparo_datos_operacion_detalle);
                });

            migrationBuilder.CreateIndex(
                name: "ix_verificar_reparo_datos_operacion_detalle_expediente",
                table: "verificar_reparo_datos_operacion_detalle",
                column: "id_expediente");
        }
    }
}