using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class Impleent_Antecedendes_Vendedor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "carga_operacion_banco_antecedente_vendedor",
                columns: table => new
                {
                    id_carga_operacion_banco_antecedente_vendedor = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_carga_operacion_banco = table.Column<int>(type: "integer", nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    rut = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: true),
                    tipo_vendedor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    razon_social = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    nombres = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    apellido_paterno = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    apellido_materno = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fecha_nacimiento = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    genero = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    estado_civil = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    relacion_titular = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    direccion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    region = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    comuna = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    nacionalidad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    profesion = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carga_operacion_banco_antecedente_vendedor", x => x.id_carga_operacion_banco_antecedente_vendedor);
                });

            migrationBuilder.CreateIndex(
                name: "ix_cob_antecedente_vendedor_id_carga_operacion_banco",
                table: "carga_operacion_banco_antecedente_vendedor",
                column: "id_carga_operacion_banco");

            migrationBuilder.CreateIndex(
                name: "ix_cob_antecedente_vendedor_id_expediente",
                table: "carga_operacion_banco_antecedente_vendedor",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "ix_cob_antecedente_vendedor_rut",
                table: "carga_operacion_banco_antecedente_vendedor",
                column: "rut");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "carga_operacion_banco_antecedente_vendedor");
        }
    }
}
