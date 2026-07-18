using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class AddFiadorGaranteTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "fiador_garante",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    id_actividad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    rut = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    nombres = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    apellido_paterno = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    apellido_materno = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    fecha_nacimiento = table.Column<DateTime>(type: "date", nullable: false),
                    genero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    estado_civil = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nacionalidad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    profesion = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    direccion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    region = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    comuna = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    telefono_fijo = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    telefono_movil = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    relacion_titular = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    observaciones = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fiador_garante", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_fiador_garante_expediente",
                table: "fiador_garante",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "uq_expediente_rut_fiador",
                table: "fiador_garante",
                columns: new[] { "id_expediente", "rut" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fiador_garante");
        }
    }
}
