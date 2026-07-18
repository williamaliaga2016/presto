using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class GestionRectificatoriaSolucionReparo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "gestion_rectificatoria",
            //    columns: table => new
            //    {
            //        id_gestion_rectificatoria = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
            //        enviar_tipo_reparo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //        observaciones = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
            //        is_active = table.Column<bool>(type: "boolean", nullable: false),
            //        row_status = table.Column<bool>(type: "boolean", nullable: false),
            //        created_by = table.Column<int>(type: "integer", nullable: false),
            //        created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //        modified_by = table.Column<int>(type: "integer", nullable: true),
            //        modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_gestion_rectificatoria", x => x.id_gestion_rectificatoria);
            //    });

            migrationBuilder.CreateTable(
                name: "gestion_rectificatoria_solucion_reparo",
                columns: table => new
                {
                    id_gestion_rectificatoria_solucion_reparo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_gestion_rectificatoria = table.Column<int>(type: "integer", nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    id_usuario_solicitante = table.Column<int>(type: "integer", nullable: false),
                    subsanar = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_gestion_rectificatoria_solucion_reparo", x => x.id_gestion_rectificatoria_solucion_reparo);
                });

            //migrationBuilder.CreateIndex(
            //    name: "ix_gestion_rectificatoria_expediente",
            //    table: "gestion_rectificatoria",
            //    column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "ix_gestion_rectificatoria_solucion_reparo_expediente",
                table: "gestion_rectificatoria_solucion_reparo",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "ix_gestion_rectificatoria_solucion_reparo_gestion_rectificatoria",
                table: "gestion_rectificatoria_solucion_reparo",
                column: "id_gestion_rectificatoria");

            migrationBuilder.CreateIndex(
                name: "ix_gestion_rectificatoria_solucion_reparo_usuario_solicitante",
                table: "gestion_rectificatoria_solucion_reparo",
                column: "id_usuario_solicitante");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "gestion_rectificatoria");

            migrationBuilder.DropTable(
                name: "gestion_rectificatoria_solucion_reparo");
        }
    }
}
