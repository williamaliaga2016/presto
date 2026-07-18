using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class AddRectificatoriaFirmaPostVenta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rectificatoria_firma_post_venta",
                columns: table => new
                {
                    id_rectificatoria_firma_post_venta = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    id_usuario_solicitante = table.Column<int>(type: "integer", nullable: false),
                    is_subsanar = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_rectificatoria_firma_post_venta", x => x.id_rectificatoria_firma_post_venta);
                });

            migrationBuilder.CreateTable(
                name: "rectificatoria_firma_post_venta_detalle",
                columns: table => new
                {
                    id_rectificatoria_firma_post_venta_detalle = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_rectificatoria_firma_post_venta = table.Column<int>(type: "integer", nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    rol_compadecencia = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    rut = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
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
                    table.PrimaryKey("PK_rectificatoria_firma_post_venta_detalle", x => x.id_rectificatoria_firma_post_venta_detalle);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rectificatoria_firma_post_venta");

            migrationBuilder.DropTable(
                name: "rectificatoria_firma_post_venta_detalle");
        }
    }
}
