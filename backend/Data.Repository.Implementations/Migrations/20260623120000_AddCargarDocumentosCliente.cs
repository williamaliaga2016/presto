using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <summary>
    /// Crea la tabla de confirmacion documental requerida por la actividad Cargar Documentos Cliente.
    /// </summary>
    /// <inheritdoc />
    public partial class AddCargarDocumentosCliente : Migration
    {
        /// <summary>
        /// Crea la tabla e indice que almacenan la confirmacion documental de Cargar Documentos Cliente.
        /// </summary>
        /// <param name="migrationBuilder">Constructor de operaciones de migration EF Core.</param>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cargar_documentos_cliente",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    id_actividad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    documentos_adjuntos = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    observaciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    row_status = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cargar_documentos_cliente", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cargar_docs_cliente_expediente",
                table: "cargar_documentos_cliente",
                column: "id_expediente");
        }

        /// <summary>
        /// Revierte la tabla de confirmacion documental de Cargar Documentos Cliente.
        /// </summary>
        /// <param name="migrationBuilder">Constructor de operaciones de migration EF Core.</param>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cargar_documentos_cliente");
        }
    }
}
