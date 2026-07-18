using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class AddRevisarDatosOperacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "revisar_datos_operacion",
                columns: table => new
                {
                    id_revisar_datos_operacion = table.Column<int>(type: "integer", nullable: false)
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
                    table.PrimaryKey("PK_revisar_datos_operacion", x => x.id_revisar_datos_operacion);
                });

            migrationBuilder.CreateTable(
                name: "revisar_datos_operacion_propiedad",
                columns: table => new
                {
                    id_revisar_datos_operacion_propiedad = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_revisar_datos_operacion = table.Column<int>(type: "integer", nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    tipo_propiedad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    estado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    tipo_venta = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    tipo_construccion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    tipo_direccion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    direccion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    villa_condominio = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    numero = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    numero_casa_habitantes = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    conjunto = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    manzana = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    lote = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    region = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    comuna = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    existe_rol_avaluo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    rol_avaluo_1 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    rol_avaluo_2 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    valor_avaluo_pesos = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    enviar_reparo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_revisar_datos_operacion_propiedad", x => x.id_revisar_datos_operacion_propiedad);
                });

            migrationBuilder.CreateIndex(
                name: "IX_revisar_datos_operacion_id_expediente",
                table: "revisar_datos_operacion",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "IX_revisar_datos_operacion_propiedad_id_expediente",
                table: "revisar_datos_operacion_propiedad",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "IX_revisar_datos_operacion_propiedad_id_revisar_datos_operacion",
                table: "revisar_datos_operacion_propiedad",
                column: "id_revisar_datos_operacion");

            migrationBuilder.Sql(@"
DROP FUNCTION IF EXISTS public.usp_select_revisar_datos_operacion(bigint);

CREATE OR REPLACE FUNCTION public.usp_select_revisar_datos_operacion(
    p_id_expediente bigint
)
RETURNS TABLE (
    id_revisar_datos_operacion integer,
    id_expediente bigint,
    is_active boolean,
    row_status boolean,
    created_by integer,
    created_date timestamp without time zone,
    modified_by integer,
    modified_date timestamp without time zone
)
LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT
        rdo.id_revisar_datos_operacion,
        rdo.id_expediente,
        rdo.is_active,
        rdo.row_status,
        rdo.created_by,
        rdo.created_date,
        rdo.modified_by,
        rdo.modified_date
    FROM revisar_datos_operacion rdo
    WHERE rdo.id_expediente = p_id_expediente
      AND rdo.is_active = true
      AND rdo.row_status = true
    ORDER BY rdo.id_revisar_datos_operacion DESC
    LIMIT 1;
END;
$function$;
");

            migrationBuilder.Sql(@"
DROP FUNCTION IF EXISTS public.usp_select_revisar_datos_operacion_propiedad(bigint, integer);

CREATE OR REPLACE FUNCTION public.usp_select_revisar_datos_operacion_propiedad(
    p_id_expediente bigint,
    p_id_revisar_datos_operacion integer DEFAULT 0
)
RETURNS TABLE (
    id_revisar_datos_operacion_propiedad integer,
    id_revisar_datos_operacion integer,
    id_expediente bigint,
    tipo_propiedad character varying,
    estado character varying,
    tipo_venta character varying,
    tipo_construccion character varying,
    tipo_direccion character varying,
    direccion character varying,
    villa_condominio character varying,
    numero character varying,
    numero_casa_habitantes character varying,
    conjunto character varying,
    manzana character varying,
    lote character varying,
    region character varying,
    comuna character varying,
    existe_rol_avaluo character varying,
    rol_avaluo_1 character varying,
    rol_avaluo_2 character varying,
    valor_avaluo_pesos numeric,
    enviar_reparo character varying,
    observaciones character varying,
    is_active boolean,
    row_status boolean,
    created_by integer,
    created_date timestamp without time zone,
    modified_by integer,
    modified_date timestamp without time zone
)
LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT
        rdop.id_revisar_datos_operacion_propiedad,
        rdop.id_revisar_datos_operacion,
        rdop.id_expediente,
        rdop.tipo_propiedad,
        rdop.estado,
        rdop.tipo_venta,
        rdop.tipo_construccion,
        rdop.tipo_direccion,
        rdop.direccion,
        rdop.villa_condominio,
        rdop.numero,
        rdop.numero_casa_habitantes,
        rdop.conjunto,
        rdop.manzana,
        rdop.lote,
        rdop.region,
        rdop.comuna,
        rdop.existe_rol_avaluo,
        rdop.rol_avaluo_1,
        rdop.rol_avaluo_2,
        rdop.valor_avaluo_pesos,
        rdop.enviar_reparo,
        rdop.observaciones,
        rdop.is_active,
        rdop.row_status,
        rdop.created_by,
        rdop.created_date,
        rdop.modified_by,
        rdop.modified_date
    FROM revisar_datos_operacion_propiedad rdop
    WHERE rdop.id_expediente = p_id_expediente
      AND (COALESCE(p_id_revisar_datos_operacion, 0) = 0 OR rdop.id_revisar_datos_operacion = p_id_revisar_datos_operacion)
      AND rdop.is_active = true
      AND rdop.row_status = true
    ORDER BY rdop.id_revisar_datos_operacion_propiedad DESC
    LIMIT 1;
END;
$function$;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS public.usp_select_revisar_datos_operacion_propiedad(bigint, integer);");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS public.usp_select_revisar_datos_operacion(bigint);");

            migrationBuilder.DropTable(
                name: "revisar_datos_operacion");

            migrationBuilder.DropTable(
                name: "revisar_datos_operacion_propiedad");
        }
    }
}
