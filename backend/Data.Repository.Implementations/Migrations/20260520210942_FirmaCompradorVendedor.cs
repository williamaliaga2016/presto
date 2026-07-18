using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class FirmaCompradorVendedor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "firma_comprador",
                columns: table => new
                {
                    id_firma_comprador = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    observaciones = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_firma_comprador", x => x.id_firma_comprador);
                });

            migrationBuilder.CreateTable(
                name: "firma_comprador_detalle",
                columns: table => new
                {
                    id_firma_comprador_detalle = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_firma_comprador = table.Column<int>(type: "integer", nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    relacion_titular = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    rut = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nombres = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    apellido_paterno = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    apellido_materno = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    estado_civil = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    fecha_firma = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_firma_comprador_detalle", x => x.id_firma_comprador_detalle);
                });

            migrationBuilder.CreateTable(
                name: "firma_vendedor",
                columns: table => new
                {
                    id_firma_vendedor = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    observaciones = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_firma_vendedor", x => x.id_firma_vendedor);
                });

            migrationBuilder.CreateTable(
                name: "firma_vendedor_detalle",
                columns: table => new
                {
                    id_firma_vendedor_detalle = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_firma_vendedor = table.Column<int>(type: "integer", nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    relacion_titular = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    rut = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nombres = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    apellido_paterno = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    apellido_materno = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    estado_civil = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    fecha_firma = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_firma_vendedor_detalle", x => x.id_firma_vendedor_detalle);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "firma_comprador");

            migrationBuilder.DropTable(
                name: "firma_comprador_detalle");

            migrationBuilder.DropTable(
                name: "firma_vendedor");

            migrationBuilder.DropTable(
                name: "firma_vendedor_detalle");
        }
    }
}
