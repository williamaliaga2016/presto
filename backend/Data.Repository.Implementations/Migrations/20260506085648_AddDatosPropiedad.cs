using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class AddDatosPropiedad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "datos_propiedad",
                columns: table => new
                {
                    id_datos_propiedad = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    tipo_propiedad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    estado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    tipo_venta = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    tipo_construccion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    tipo_direccion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    direccion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    villa_condominio = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    numero = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    numero_casa_habitantes = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    conjunto = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    manzana = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    lote = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    region = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    comuna = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    existe_rol_avaluo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    rol_avaluo_1 = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    rol_avaluo_2 = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    valor_avaluo_pesos = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    enviar_reparo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
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
                    table.PrimaryKey("PK_datos_propiedad", x => x.id_datos_propiedad);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "datos_propiedad");
        }
    }
}
