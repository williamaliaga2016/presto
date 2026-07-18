using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class AddRegistrarFechaRegistroCbrTabla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "registrar_fecha_registro_cbr",
                columns: table => new
                {
                    id_registrar_fecha_registro_cbr = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    fecha_registro_cbr = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    observaciones = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_registrar_fecha_registro_cbr", x => x.id_registrar_fecha_registro_cbr);
                });

            migrationBuilder.CreateIndex(
                name: "ix_registrar_fecha_registro_cbr_expediente",
                table: "registrar_fecha_registro_cbr",
                column: "id_expediente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "registrar_fecha_registro_cbr");
        }
    }
}
