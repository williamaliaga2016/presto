using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class Implement_Carga_Op_Bco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "carga_operacion_banco",
                columns: table => new
                {
                    id_carga_operacion_banco = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carga_operacion_banco", x => x.id_carga_operacion_banco);
                });

            migrationBuilder.CreateTable(
                name: "carga_operacion_banco_antecedente_comprador",
                columns: table => new
                {
                    id_carga_operacion_banco_antecedente_comprador = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_carga_operacion_banco = table.Column<int>(type: "integer", nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    rut = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: true),
                    tipo_comprador = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    razon_social = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    nombres = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    apellido_paterno = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    apellido_materno = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    fecha_nacimiento = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    genero = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    estado_civil = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    relacion_titular = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    direccion = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    region = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    comuna = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    telefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    nacionalidad = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    profesion = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carga_operacion_banco_antecedente_comprador", x => x.id_carga_operacion_banco_antecedente_comprador);
                });

            migrationBuilder.CreateTable(
                name: "carga_operacion_banco_datos_operacion",
                columns: table => new
                {
                    id_carga_operacion_banco_datos_operacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_carga_operacion_banco = table.Column<int>(type: "integer", nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    nro_mutuo = table.Column<long>(type: "bigint", nullable: true),
                    tipo_operacion = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    nro_registro = table.Column<long>(type: "bigint", nullable: true),
                    ult_clasif_al = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    segmento = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    canal_venta = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    nro_op_cartera = table.Column<long>(type: "bigint", nullable: true),
                    modelo_operacion = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    tipo_carpeta = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    propietario = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    inmobiliaria = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    glosa_producto = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carga_operacion_banco_datos_operacion", x => x.id_carga_operacion_banco_datos_operacion);
                });

            migrationBuilder.CreateIndex(
                name: "ix_carga_operacion_banco_id_expediente",
                table: "carga_operacion_banco",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "ix_cob_antecedente_comprador_id_carga_operacion_banco",
                table: "carga_operacion_banco_antecedente_comprador",
                column: "id_carga_operacion_banco");

            migrationBuilder.CreateIndex(
                name: "ix_cob_antecedente_comprador_id_expediente",
                table: "carga_operacion_banco_antecedente_comprador",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "ix_cob_antecedente_comprador_rut",
                table: "carga_operacion_banco_antecedente_comprador",
                column: "rut");

            migrationBuilder.CreateIndex(
                name: "ix_cob_datos_operacion_id_carga_operacion_banco",
                table: "carga_operacion_banco_datos_operacion",
                column: "id_carga_operacion_banco");

            migrationBuilder.CreateIndex(
                name: "ix_cob_datos_operacion_id_expediente",
                table: "carga_operacion_banco_datos_operacion",
                column: "id_expediente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "carga_operacion_banco");

            migrationBuilder.DropTable(
                name: "carga_operacion_banco_antecedente_comprador");

            migrationBuilder.DropTable(
                name: "carga_operacion_banco_datos_operacion");
        }
    }
}
