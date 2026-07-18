using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class GeneraFiniquito : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "generar_finiquito",
                columns: table => new
                {
                    id_generar_finiquito = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    tipo_tasacion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    fojas_propiedad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    numero_propiedad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    año_propiedad = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    fojas_hipoteca = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    numero_hipoteca = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    año_hipoteca = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    fojas_prohibicion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    numero_prohibicion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    año_prohibicion = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    fojas_hipoteca_2grado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    numero_hipoteca_2grado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    año_hipoteca_2grado = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
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
                    table.PrimaryKey("PK_generar_finiquito", x => x.id_generar_finiquito);
                });

            migrationBuilder.CreateIndex(
                name: "ix_generar_finiquito_expediente",
                table: "generar_finiquito",
                column: "id_expediente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "generar_finiquito");
        }
    }
}
