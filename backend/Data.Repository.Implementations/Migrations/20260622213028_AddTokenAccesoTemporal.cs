using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class AddTokenAccesoTemporal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "token_acceso_temporal",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    id_actividad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    id_usuario = table.Column<int>(type: "integer", nullable: false),
                    token = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    fecha_expiracion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    usado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    fecha_uso = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_token_acceso_temporal", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_token_acceso_unico",
                table: "token_acceso_temporal",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_token_expediente",
                table: "token_acceso_temporal",
                columns: new[] { "id_expediente", "id_actividad" });

            migrationBuilder.Sql(
                "COMMENT ON TABLE token_acceso_temporal IS 'Registra tokens UUID de un solo uso para acceso temporal de usuarios externos.';");
            migrationBuilder.Sql(
                "COMMENT ON COLUMN token_acceso_temporal.token IS 'UUID publico incluido en el link temporal; debe ser unico.';");
            migrationBuilder.Sql(
                "COMMENT ON COLUMN token_acceso_temporal.usado IS 'Indica si el token ya fue consumido exitosamente para emitir JWT temporal.';");
            migrationBuilder.Sql(
                "COMMENT ON COLUMN token_acceso_temporal.fecha_uso IS 'Fecha en la que el token fue consumido exitosamente.';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "token_acceso_temporal");
        }
    }
}
