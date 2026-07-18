using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class GestionRectificatoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "reparo_calculo_doc_detalle");

            //migrationBuilder.DropTable(
            //    name: "rol_avaluo_calculo_doc_detalle");

            //migrationBuilder.DropTable(
            //    name: "verificar_reparo_datos_operacion_detalle");
            migrationBuilder.Sql("DROP TABLE IF EXISTS reparo_calculo_doc_detalle;");
            migrationBuilder.Sql("DROP TABLE IF EXISTS rol_avaluo_calculo_doc_detalle;");
            migrationBuilder.Sql("DROP TABLE IF EXISTS verificar_reparo_datos_operacion_detalle;");

            migrationBuilder.AlterColumn<string>(
                name: "observaciones",
                table: "verificar_reparo_datos_operacion",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "id_usuario_solicitante",
            //    table: "verificar_reparo_datos_operacion",
            //    type: "integer",
            //    nullable: false,
            //    defaultValue: 0);
            migrationBuilder.Sql(@"
    DO $$ 
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                      WHERE table_name = 'verificar_reparo_datos_operacion' 
                      AND column_name = 'id_usuario_solicitante') THEN
            ALTER TABLE verificar_reparo_datos_operacion 
            ADD COLUMN id_usuario_solicitante integer NOT NULL DEFAULT 0;
        END IF;
    END $$;
");

            //migrationBuilder.AddColumn<bool>(
            //    name: "is_subsanar",
            //    table: "verificar_reparo_datos_operacion",
            //    type: "boolean",
            //    nullable: false,
            //    defaultValue: false);
            migrationBuilder.Sql(@"
    DO $$ 
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                      WHERE table_name = 'verificar_reparo_datos_operacion' 
                      AND column_name = 'is_subsanar') THEN
            ALTER TABLE verificar_reparo_datos_operacion 
            ADD COLUMN is_subsanar boolean NOT NULL DEFAULT false;
        END IF;
    END $$;
");

            migrationBuilder.AlterColumn<string>(
                name: "tipo_tasacion",
                table: "tasacion_detalle",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.Sql(@"
    ALTER TABLE revisar_ingreso_datos_credito
    ALTER COLUMN ubicacion TYPE boolean
    USING (
        CASE
            WHEN lower(ubicacion) IN ('true', 't', '1', 'si')
            THEN true
            ELSE false
        END
    );
");

            migrationBuilder.AlterColumn<bool>(
                name: "ubicacion",
                table: "datos_operacion_datos_credito",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "observaciones",
                table: "corregir_reparo_visado",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "id_usuario_solicitante",
            //    table: "corregir_reparo_visado",
            //    type: "integer",
            //    nullable: false,
            //    defaultValue: 0);
            migrationBuilder.Sql(@"
    DO $$ 
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                      WHERE table_name = 'corregir_reparo_visado' 
                      AND column_name = 'id_usuario_solicitante') THEN
            ALTER TABLE corregir_reparo_visado 
            ADD COLUMN id_usuario_solicitante integer NOT NULL DEFAULT 0;
        END IF;
    END $$;
");

            //migrationBuilder.AddColumn<bool>(
            //    name: "is_subsanar",
            //    table: "corregir_reparo_visado",
            //    type: "boolean",
            //    nullable: false,
            //    defaultValue: false);
            migrationBuilder.Sql(@"
    DO $$ 
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                      WHERE table_name = 'corregir_reparo_visado' 
                      AND column_name = 'is_subsanar') THEN
            ALTER TABLE corregir_reparo_visado 
            ADD COLUMN is_subsanar boolean NOT NULL DEFAULT false;
        END IF;
    END $$;
");

            migrationBuilder.AlterColumn<string>(
                name: "observaciones",
                table: "corregir_reparo_generar_memo_escritura",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            //migrationBuilder.AddColumn<int>(
            //    name: "id_usuario_solicitante",
            //    table: "corregir_reparo_generar_memo_escritura",
            //    type: "integer",
            //    nullable: false,
            //    defaultValue: 0);
            migrationBuilder.Sql(@"
    DO $$ 
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                      WHERE table_name = 'corregir_reparo_generar_memo_escritura' 
                      AND column_name = 'id_usuario_solicitante') THEN
            ALTER TABLE corregir_reparo_generar_memo_escritura 
            ADD COLUMN id_usuario_solicitante integer NOT NULL DEFAULT 0;
        END IF;
    END $$;
");

            migrationBuilder.AlterColumn<string>(
                name: "observaciones",
                table: "corregir_reparo_generar_borrador_escritura",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "observaciones",
                table: "corregir_reparo_calculo_doc",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            //migrationBuilder.AddColumn<string>(
            //    name: "existe_rol_avaluo",
            //    table: "corregir_reparo_calculo_doc",
            //    type: "character varying(10)",
            //    maxLength: 10,
            //    nullable: true);
            migrationBuilder.Sql(@"
    DO $$ 
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                      WHERE table_name = 'corregir_reparo_calculo_doc' 
                      AND column_name = 'existe_rol_avaluo') THEN
            ALTER TABLE corregir_reparo_calculo_doc 
            ADD COLUMN existe_rol_avaluo character varying(10) NULL;
        END IF;
    END $$;
");

            //migrationBuilder.AddColumn<int>(
            //    name: "id_usuario_solicitante",
            //    table: "corregir_reparo_calculo_doc",
            //    type: "integer",
            //    nullable: false,
            //    defaultValue: 0);
            migrationBuilder.Sql(@"
    DO $$ 
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                      WHERE table_name = 'corregir_reparo_calculo_doc' 
                      AND column_name = 'id_usuario_solicitante') THEN
            ALTER TABLE corregir_reparo_calculo_doc 
            ADD COLUMN id_usuario_solicitante integer NOT NULL DEFAULT 0;
        END IF;
    END $$;
");

            //migrationBuilder.AddColumn<bool>(
            //    name: "is_subsanar",
            //    table: "corregir_reparo_calculo_doc",
            //    type: "boolean",
            //    nullable: false,
            //    defaultValue: false);
            migrationBuilder.Sql(@"
    DO $$ 
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                      WHERE table_name = 'corregir_reparo_calculo_doc' 
                      AND column_name = 'is_subsanar') THEN
            ALTER TABLE corregir_reparo_calculo_doc 
            ADD COLUMN is_subsanar boolean NOT NULL DEFAULT false;
        END IF;
    END $$;
");

            //migrationBuilder.AddColumn<string>(
            //    name: "rol_avaluo_editado",
            //    table: "corregir_reparo_calculo_doc",
            //    type: "character varying(200)",
            //    maxLength: 200,
            //    nullable: true);
            migrationBuilder.Sql(@"
    DO $$ 
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                      WHERE table_name = 'corregir_reparo_calculo_doc' 
                      AND column_name = 'rol_avaluo_editado') THEN
            ALTER TABLE corregir_reparo_calculo_doc 
            ADD COLUMN rol_avaluo_editado character varying(200) NULL;
        END IF;
    END $$;
");

            //migrationBuilder.AddColumn<decimal>(
            //    name: "valor_avaluo_pesos",
            //    table: "corregir_reparo_calculo_doc",
            //    type: "numeric",
            //    nullable: true);
            migrationBuilder.Sql(@"
    DO $$ 
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                      WHERE table_name = 'corregir_reparo_calculo_doc' 
                      AND column_name = 'valor_avaluo_pesos') THEN
            ALTER TABLE corregir_reparo_calculo_doc 
            ADD COLUMN valor_avaluo_pesos numeric NULL;
        END IF;
    END $$;
");

            //migrationBuilder.CreateTable(
            //    name: "aprobacion_comercial_legal_cdr",
            //    columns: table => new
            //    {
            //        id_aprobacion_comercial_legal_cdr = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
            //        enviar_a_reparo = table.Column<bool>(type: "boolean", nullable: false),
            //        observaciones = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
            //        is_active = table.Column<bool>(type: "boolean", nullable: false),
            //        row_status = table.Column<bool>(type: "boolean", nullable: false),
            //        created_by = table.Column<int>(type: "integer", nullable: false),
            //        created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //        modified_by = table.Column<int>(type: "integer", nullable: true),
            //        modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_aprobacion_comercial_legal_cdr", x => x.id_aprobacion_comercial_legal_cdr);
            //    });
            migrationBuilder.Sql(@"
    CREATE TABLE IF NOT EXISTS aprobacion_comercial_legal_cdr (
        id_aprobacion_comercial_legal_cdr integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        enviar_a_reparo boolean NOT NULL,
        observaciones character varying(2000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_aprobacion_comercial_legal_cdr"" PRIMARY KEY (id_aprobacion_comercial_legal_cdr)
    );
");

            //migrationBuilder.CreateTable(
            //    name: "cierre_copias_notaria",
            //    columns: table => new
            //    {
            //        id_cierre_copias_notaria = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
            //        enviar_a_reparo = table.Column<bool>(type: "boolean", nullable: true),
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
            //        table.PrimaryKey("PK_cierre_copias_notaria", x => x.id_cierre_copias_notaria);
            //    });
            migrationBuilder.Sql(@"
    CREATE TABLE IF NOT EXISTS cierre_copias_notaria (
        id_cierre_copias_notaria integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        enviar_a_reparo boolean,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_cierre_copias_notaria"" PRIMARY KEY (id_cierre_copias_notaria)
    );
");

            //migrationBuilder.CreateTable(
            //    name: "corregir_carta_resguardo",
            //    columns: table => new
            //    {
            //        id_corregir_carta_resguardo = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
            //        id_usuario_solicitante = table.Column<int>(type: "integer", nullable: false),
            //        is_subsanar = table.Column<bool>(type: "boolean", nullable: false),
            //        observaciones = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
            //        is_active = table.Column<bool>(type: "boolean", nullable: false),
            //        row_status = table.Column<bool>(type: "boolean", nullable: false),
            //        created_by = table.Column<int>(type: "integer", nullable: false),
            //        created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //        modified_by = table.Column<int>(type: "integer", nullable: true),
            //        modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_corregir_carta_resguardo", x => x.id_corregir_carta_resguardo);
            //    });
            migrationBuilder.Sql(@"
    CREATE TABLE IF NOT EXISTS corregir_carta_resguardo (
        id_corregir_carta_resguardo integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        id_usuario_solicitante integer NOT NULL,
        is_subsanar boolean NOT NULL,
        observaciones character varying(2000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_corregir_carta_resguardo"" PRIMARY KEY (id_corregir_carta_resguardo)
    );
");

            //migrationBuilder.CreateTable(
            //    name: "corregir_notaria_reparo_abogados",
            //    columns: table => new
            //    {
            //        id_corregir_notaria_reparo_abogados = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
            //        id_usuario_solicitante = table.Column<int>(type: "integer", nullable: false),
            //        is_subsanar = table.Column<bool>(type: "boolean", nullable: false),
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
            //        table.PrimaryKey("PK_corregir_notaria_reparo_abogados", x => x.id_corregir_notaria_reparo_abogados);
            //    });
            migrationBuilder.Sql(@"
    CREATE TABLE IF NOT EXISTS corregir_notaria_reparo_abogados (
        id_corregir_notaria_reparo_abogados integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        id_usuario_solicitante integer NOT NULL,
        is_subsanar boolean NOT NULL,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_corregir_notaria_reparo_abogados"" PRIMARY KEY (id_corregir_notaria_reparo_abogados)
    );
");

            //migrationBuilder.CreateTable(
            //    name: "corregir_reparo_cdr",
            //    columns: table => new
            //    {
            //        id_corregir_reparo_cdr = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
            //        id_usuario_solicitante = table.Column<int>(type: "integer", nullable: false),
            //        is_subsanar = table.Column<bool>(type: "boolean", nullable: false),
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
            //        table.PrimaryKey("PK_corregir_reparo_cdr", x => x.id_corregir_reparo_cdr);
            //    });
            migrationBuilder.Sql(@"
    CREATE TABLE IF NOT EXISTS corregir_reparo_cdr (
        id_corregir_reparo_cdr integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        id_usuario_solicitante integer NOT NULL,
        is_subsanar boolean NOT NULL,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_corregir_reparo_cdr"" PRIMARY KEY (id_corregir_reparo_cdr)
    );
");

            //migrationBuilder.CreateTable(
            //    name: "corregir_reparo_cierre_copias_notaria",
            //    columns: table => new
            //    {
            //        id_corregir_reparo_cierre_copias_notaria = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
            //        is_subsanar = table.Column<bool>(type: "boolean", nullable: false),
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
            //        table.PrimaryKey("PK_corregir_reparo_cierre_copias_notaria", x => x.id_corregir_reparo_cierre_copias_notaria);
            //    });
            migrationBuilder.Sql(@"
    CREATE TABLE IF NOT EXISTS corregir_reparo_cierre_copias_notaria (
        id_corregir_reparo_cierre_copias_notaria integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        is_subsanar boolean NOT NULL,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_corregir_reparo_cierre_copias_notaria"" PRIMARY KEY (id_corregir_reparo_cierre_copias_notaria)
    );
");

            //migrationBuilder.CreateTable(
            //    name: "corregir_reparo_control_credito",
            //    columns: table => new
            //    {
            //        id_corregir_reparo_control_credito = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_realizar_control_credito = table.Column<int>(type: "integer", nullable: false),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
            //        id_usuario_solicitante = table.Column<int>(type: "integer", nullable: false),
            //        subsanar = table.Column<bool>(type: "boolean", nullable: false),
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
            //        table.PrimaryKey("PK_corregir_reparo_control_credito", x => x.id_corregir_reparo_control_credito);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "corregir_reparo_inst_pago",
            //    columns: table => new
            //    {
            //        id_corregir_reparo_inst_pago = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
            //        id_usuario_solicitante = table.Column<int>(type: "integer", nullable: false),
            //        is_subsanar = table.Column<bool>(type: "boolean", nullable: false),
            //        observaciones = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
            //        enviar_a_reparo = table.Column<bool>(type: "boolean", nullable: false),
            //        is_active = table.Column<bool>(type: "boolean", nullable: false),
            //        row_status = table.Column<bool>(type: "boolean", nullable: false),
            //        created_by = table.Column<int>(type: "integer", nullable: false),
            //        created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //        modified_by = table.Column<int>(type: "integer", nullable: true),
            //        modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_corregir_reparo_inst_pago", x => x.id_corregir_reparo_inst_pago);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "corregir_reparo_liquidacion",
            //    columns: table => new
            //    {
            //        id_corregir_reparo_liquidacion = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
            //        id_usuario_solicitante = table.Column<int>(type: "integer", nullable: false),
            //        is_subsanar = table.Column<bool>(type: "boolean", nullable: false),
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
            //        table.PrimaryKey("PK_corregir_reparo_liquidacion", x => x.id_corregir_reparo_liquidacion);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "corregir_reparo_prefiniquito",
            //    columns: table => new
            //    {
            //        id_corregir_reparo_prefiniquito = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
            //        id_usuario_solicitante = table.Column<int>(type: "integer", nullable: false),
            //        is_subsanar = table.Column<bool>(type: "boolean", nullable: false),
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
            //        table.PrimaryKey("PK_corregir_reparo_prefiniquito", x => x.id_corregir_reparo_prefiniquito);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "entregar_carpeta",
            //    columns: table => new
            //    {
            //        id_entregar_carpeta = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
            //        enviar_a_reparo = table.Column<bool>(type: "boolean", nullable: true),
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
            //        table.PrimaryKey("PK_entregar_carpeta", x => x.id_entregar_carpeta);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "firma_banco_acreedor_cg",
            //    columns: table => new
            //    {
            //        id_firma_banco_acreedor_cg = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
            //        observaciones = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
            //        is_active = table.Column<bool>(type: "boolean", nullable: false),
            //        row_status = table.Column<bool>(type: "boolean", nullable: false),
            //        created_by = table.Column<int>(type: "integer", nullable: false),
            //        created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //        modified_by = table.Column<int>(type: "integer", nullable: true),
            //        modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_firma_banco_acreedor_cg", x => x.id_firma_banco_acreedor_cg);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "generar_carta_resguardo",
            //    columns: table => new
            //    {
            //        id_generar_carta_resguardo = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
            //        generar_carta = table.Column<bool>(type: "boolean", nullable: true),
            //        tipo_carta = table.Column<string>(type: "text", nullable: true),
            //        enviar_a_reparo = table.Column<bool>(type: "boolean", nullable: true),
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
            //        table.PrimaryKey("PK_generar_carta_resguardo", x => x.id_generar_carta_resguardo);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "generar_finiquito",
            //    columns: table => new
            //    {
            //        id_generar_finiquito = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
            //        fojas_propiedad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //        numero_propiedad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //        año_propiedad = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
            //        fojas_hipoteca = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //        numero_hipoteca = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //        año_hipoteca = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
            //        fojas_prohibicion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //        numero_prohibicion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //        año_prohibicion = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
            //        fojas_hipoteca_2grado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //        numero_hipoteca_2grado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //        año_hipoteca_2grado = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
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
            //        table.PrimaryKey("PK_generar_finiquito", x => x.id_generar_finiquito);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "generar_recursos_pagos_cbr",
            //    columns: table => new
            //    {
            //        id_generar_recursos_pagos_cbr = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
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
            //        table.PrimaryKey("PK_generar_recursos_pagos_cbr", x => x.id_generar_recursos_pagos_cbr);
            //    });

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

            //migrationBuilder.CreateTable(
            //    name: "realizar_control_credito",
            //    columns: table => new
            //    {
            //        id_realizar_control_credito = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
            //        id_usuario_solicitante = table.Column<int>(type: "integer", nullable: false),
            //        enviar_reparo = table.Column<bool>(type: "boolean", nullable: false),
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
            //        table.PrimaryKey("PK_realizar_control_credito", x => x.id_realizar_control_credito);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "realizar_revision_previo_firma_banco",
            //    columns: table => new
            //    {
            //        id_realizar_revision_previo_firma_banco = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
            //        enviar_a_reparo = table.Column<bool>(type: "boolean", nullable: true),
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
            //        table.PrimaryKey("PK_realizar_revision_previo_firma_banco", x => x.id_realizar_revision_previo_firma_banco);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "recepcionar_matriz",
            //    columns: table => new
            //    {
            //        id_recepcionar_matriz = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
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
            //        table.PrimaryKey("PK_recepcionar_matriz", x => x.id_recepcionar_matriz);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "recibir_instruccion_pago",
            //    columns: table => new
            //    {
            //        id_recibir_instruccion_pago = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
            //        enviar_a_reparo = table.Column<bool>(type: "boolean", nullable: true),
            //        condicion_especial_desembolso = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
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
            //        table.PrimaryKey("PK_recibir_instruccion_pago", x => x.id_recibir_instruccion_pago);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "registrar_escritura_cbr",
            //    columns: table => new
            //    {
            //        id_registrar_escritura_cbr = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
            //        conservador = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
            //        numero_caratula = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
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
            //        table.PrimaryKey("PK_registrar_escritura_cbr", x => x.id_registrar_escritura_cbr);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "registrar_firma_apoderado_banco",
            //    columns: table => new
            //    {
            //        id_registrar_firma_apoderado_banco = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        fecha_firma = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
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
            //        table.PrimaryKey("PK_registrar_firma_apoderado_banco", x => x.id_registrar_firma_apoderado_banco);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "reingresar_escritura_cbr",
            //    columns: table => new
            //    {
            //        id_reingresar_escritura_cbr = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
            //        id_usuario_solicitante = table.Column<int>(type: "integer", nullable: false),
            //        is_subsanar = table.Column<bool>(type: "boolean", nullable: false),
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
            //        table.PrimaryKey("PK_reingresar_escritura_cbr", x => x.id_reingresar_escritura_cbr);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "revisar_desembolso",
            //    columns: table => new
            //    {
            //        id_revisar_desembolso = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
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
            //        table.PrimaryKey("PK_revisar_desembolso", x => x.id_revisar_desembolso);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "revisar_inscripcion_cbr",
            //    columns: table => new
            //    {
            //        id_revisar_inscripcion_cbr = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
            //        id_usuario_solicitante = table.Column<int>(type: "integer", nullable: false),
            //        is_enviar_reparo = table.Column<bool>(type: "boolean", nullable: false),
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
            //        table.PrimaryKey("PK_revisar_inscripcion_cbr", x => x.id_revisar_inscripcion_cbr);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "revisar_liquidacion",
            //    columns: table => new
            //    {
            //        id_revisar_liquidacion = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
            //        id_usuario_solicitante = table.Column<int>(type: "integer", nullable: false),
            //        is_enviar_reparo = table.Column<bool>(type: "boolean", nullable: false),
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
            //        table.PrimaryKey("PK_revisar_liquidacion", x => x.id_revisar_liquidacion);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "valorizar_cbr",
            //    columns: table => new
            //    {
            //        id_valorizar_cbr = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
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
            //        table.PrimaryKey("PK_valorizar_cbr", x => x.id_valorizar_cbr);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "verificar_correccion_escritura",
            //    columns: table => new
            //    {
            //        id_verificar_correccion_escritura = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
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
            //        table.PrimaryKey("PK_verificar_correccion_escritura", x => x.id_verificar_correccion_escritura);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "verificar_reparo_cbr",
            //    columns: table => new
            //    {
            //        id_verificar_reparo_cbr = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        id_expediente = table.Column<long>(type: "bigint", nullable: false),
            //        enviar_a_reparo = table.Column<bool>(type: "boolean", nullable: true),
            //        enviar_reparo_a = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
            //        estatus_reparo = table.Column<bool>(type: "boolean", nullable: true),
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
            //        table.PrimaryKey("PK_verificar_reparo_cbr", x => x.id_verificar_reparo_cbr);
            //    });
            // En lugar de cada CreateTable, usa CREATE IF NOT EXISTS
            migrationBuilder.Sql(@"
    CREATE TABLE IF NOT EXISTS aprobacion_comercial_legal_cdr (
        id_aprobacion_comercial_legal_cdr integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        enviar_a_reparo boolean NOT NULL,
        observaciones character varying(2000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_aprobacion_comercial_legal_cdr"" PRIMARY KEY (id_aprobacion_comercial_legal_cdr)
    );

    CREATE TABLE IF NOT EXISTS cierre_copias_notaria (
        id_cierre_copias_notaria integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        enviar_a_reparo boolean,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_cierre_copias_notaria"" PRIMARY KEY (id_cierre_copias_notaria)
    );

    CREATE TABLE IF NOT EXISTS corregir_carta_resguardo (
        id_corregir_carta_resguardo integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        id_usuario_solicitante integer NOT NULL,
        is_subsanar boolean NOT NULL,
        observaciones character varying(2000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_corregir_carta_resguardo"" PRIMARY KEY (id_corregir_carta_resguardo)
    );

    CREATE TABLE IF NOT EXISTS corregir_notaria_reparo_abogados (
        id_corregir_notaria_reparo_abogados integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        id_usuario_solicitante integer NOT NULL,
        is_subsanar boolean NOT NULL,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_corregir_notaria_reparo_abogados"" PRIMARY KEY (id_corregir_notaria_reparo_abogados)
    );

    CREATE TABLE IF NOT EXISTS corregir_reparo_cdr (
        id_corregir_reparo_cdr integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        id_usuario_solicitante integer NOT NULL,
        is_subsanar boolean NOT NULL,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_corregir_reparo_cdr"" PRIMARY KEY (id_corregir_reparo_cdr)
    );

    CREATE TABLE IF NOT EXISTS corregir_reparo_cierre_copias_notaria (
        id_corregir_reparo_cierre_copias_notaria integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        is_subsanar boolean NOT NULL,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_corregir_reparo_cierre_copias_notaria"" PRIMARY KEY (id_corregir_reparo_cierre_copias_notaria)
    );

    CREATE TABLE IF NOT EXISTS corregir_reparo_control_credito (
        id_corregir_reparo_control_credito integer GENERATED BY DEFAULT AS IDENTITY,
        id_realizar_control_credito integer NOT NULL,
        id_expediente bigint NOT NULL,
        id_usuario_solicitante integer NOT NULL,
        subsanar boolean NOT NULL,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_corregir_reparo_control_credito"" PRIMARY KEY (id_corregir_reparo_control_credito)
    );

    CREATE TABLE IF NOT EXISTS corregir_reparo_inst_pago (
        id_corregir_reparo_inst_pago integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        id_usuario_solicitante integer NOT NULL,
        is_subsanar boolean NOT NULL,
        observaciones character varying(1000),
        enviar_a_reparo boolean NOT NULL,
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_corregir_reparo_inst_pago"" PRIMARY KEY (id_corregir_reparo_inst_pago)
    );

    CREATE TABLE IF NOT EXISTS corregir_reparo_liquidacion (
        id_corregir_reparo_liquidacion integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        id_usuario_solicitante integer NOT NULL,
        is_subsanar boolean NOT NULL,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_corregir_reparo_liquidacion"" PRIMARY KEY (id_corregir_reparo_liquidacion)
    );

    CREATE TABLE IF NOT EXISTS corregir_reparo_prefiniquito (
        id_corregir_reparo_prefiniquito integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        id_usuario_solicitante integer NOT NULL,
        is_subsanar boolean NOT NULL,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_corregir_reparo_prefiniquito"" PRIMARY KEY (id_corregir_reparo_prefiniquito)
    );

    CREATE TABLE IF NOT EXISTS entregar_carpeta (
        id_entregar_carpeta integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        enviar_a_reparo boolean,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_entregar_carpeta"" PRIMARY KEY (id_entregar_carpeta)
    );

    CREATE TABLE IF NOT EXISTS firma_banco_acreedor_cg (
        id_firma_banco_acreedor_cg integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        observaciones character varying(2000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_firma_banco_acreedor_cg"" PRIMARY KEY (id_firma_banco_acreedor_cg)
    );

    CREATE TABLE IF NOT EXISTS generar_carta_resguardo (
        id_generar_carta_resguardo integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        generar_carta boolean,
        tipo_carta text,
        enviar_a_reparo boolean,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_generar_carta_resguardo"" PRIMARY KEY (id_generar_carta_resguardo)
    );

    CREATE TABLE IF NOT EXISTS generar_finiquito (
        id_generar_finiquito integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        fojas_propiedad character varying(50) NOT NULL,
        numero_propiedad character varying(50) NOT NULL,
        año_propiedad character varying(10) NOT NULL,
        fojas_hipoteca character varying(50) NOT NULL,
        numero_hipoteca character varying(50) NOT NULL,
        año_hipoteca character varying(10) NOT NULL,
        fojas_prohibicion character varying(50) NOT NULL,
        numero_prohibicion character varying(50) NOT NULL,
        año_prohibicion character varying(10) NOT NULL,
        fojas_hipoteca_2grado character varying(50) NOT NULL,
        numero_hipoteca_2grado character varying(50) NOT NULL,
        año_hipoteca_2grado character varying(10) NOT NULL,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_generar_finiquito"" PRIMARY KEY (id_generar_finiquito)
    );

    CREATE TABLE IF NOT EXISTS generar_recursos_pagos_cbr (
        id_generar_recursos_pagos_cbr integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_generar_recursos_pagos_cbr"" PRIMARY KEY (id_generar_recursos_pagos_cbr)
    );

    CREATE TABLE IF NOT EXISTS gestion_rectificatoria (
        id_gestion_rectificatoria integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        enviar_tipo_reparo character varying(100) NOT NULL,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_gestion_rectificatoria"" PRIMARY KEY (id_gestion_rectificatoria)
    );

    CREATE TABLE IF NOT EXISTS realizar_control_credito (
        id_realizar_control_credito integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        id_usuario_solicitante integer NOT NULL,
        enviar_reparo boolean NOT NULL,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_realizar_control_credito"" PRIMARY KEY (id_realizar_control_credito)
    );

    CREATE TABLE IF NOT EXISTS realizar_revision_previo_firma_banco (
        id_realizar_revision_previo_firma_banco integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        enviar_a_reparo boolean,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_realizar_revision_previo_firma_banco"" PRIMARY KEY (id_realizar_revision_previo_firma_banco)
    );

    CREATE TABLE IF NOT EXISTS recepcionar_matriz (
        id_recepcionar_matriz integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_recepcionar_matriz"" PRIMARY KEY (id_recepcionar_matriz)
    );

    CREATE TABLE IF NOT EXISTS recibir_instruccion_pago (
        id_recibir_instruccion_pago integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        enviar_a_reparo boolean,
        condicion_especial_desembolso character varying(1000),
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_recibir_instruccion_pago"" PRIMARY KEY (id_recibir_instruccion_pago)
    );

    CREATE TABLE IF NOT EXISTS registrar_escritura_cbr (
        id_registrar_escritura_cbr integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        conservador character varying(1000),
        numero_caratula character varying(1000),
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_registrar_escritura_cbr"" PRIMARY KEY (id_registrar_escritura_cbr)
    );

    CREATE TABLE IF NOT EXISTS registrar_firma_apoderado_banco (
        id_registrar_firma_apoderado_banco integer GENERATED BY DEFAULT AS IDENTITY,
        fecha_firma timestamp without time zone NOT NULL,
        id_expediente bigint NOT NULL,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_registrar_firma_apoderado_banco"" PRIMARY KEY (id_registrar_firma_apoderado_banco)
    );

    CREATE TABLE IF NOT EXISTS reingresar_escritura_cbr (
        id_reingresar_escritura_cbr integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        id_usuario_solicitante integer NOT NULL,
        is_subsanar boolean NOT NULL,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_reingresar_escritura_cbr"" PRIMARY KEY (id_reingresar_escritura_cbr)
    );

    CREATE TABLE IF NOT EXISTS revisar_desembolso (
        id_revisar_desembolso integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_revisar_desembolso"" PRIMARY KEY (id_revisar_desembolso)
    );

    CREATE TABLE IF NOT EXISTS revisar_inscripcion_cbr (
        id_revisar_inscripcion_cbr integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        id_usuario_solicitante integer NOT NULL,
        is_enviar_reparo boolean NOT NULL,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_revisar_inscripcion_cbr"" PRIMARY KEY (id_revisar_inscripcion_cbr)
    );

    CREATE TABLE IF NOT EXISTS revisar_liquidacion (
        id_revisar_liquidacion integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        id_usuario_solicitante integer NOT NULL,
        is_enviar_reparo boolean NOT NULL,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_revisar_liquidacion"" PRIMARY KEY (id_revisar_liquidacion)
    );

    CREATE TABLE IF NOT EXISTS valorizar_cbr (
        id_valorizar_cbr integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_valorizar_cbr"" PRIMARY KEY (id_valorizar_cbr)
    );

    CREATE TABLE IF NOT EXISTS verificar_correccion_escritura (
        id_verificar_correccion_escritura integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_verificar_correccion_escritura"" PRIMARY KEY (id_verificar_correccion_escritura)
    );

    CREATE TABLE IF NOT EXISTS verificar_reparo_cbr (
        id_verificar_reparo_cbr integer GENERATED BY DEFAULT AS IDENTITY,
        id_expediente bigint NOT NULL,
        enviar_a_reparo boolean,
        enviar_reparo_a character varying(1000),
        estatus_reparo boolean,
        observaciones character varying(1000),
        is_active boolean NOT NULL,
        row_status boolean NOT NULL,
        created_by integer NOT NULL,
        created_date timestamp without time zone NOT NULL,
        modified_by integer,
        modified_date timestamp without time zone,
        CONSTRAINT ""PK_verificar_reparo_cbr"" PRIMARY KEY (id_verificar_reparo_cbr)
    );
");
            //migrationBuilder.CreateIndex(
            //    name: "ix_aprobacion_comercial_legal_cdr",
            //    table: "aprobacion_comercial_legal_cdr",
            //    column: "id_expediente");

            //migrationBuilder.CreateIndex(
            //    name: "ix_corregir_carta_resguardo_expediente",
            //    table: "corregir_carta_resguardo",
            //    column: "id_expediente");

            //migrationBuilder.CreateIndex(
            //    name: "ix_corregir_notaria_reparo_abogados_expediente",
            //    table: "corregir_notaria_reparo_abogados",
            //    column: "id_expediente");

            //migrationBuilder.CreateIndex(
            //    name: "ix_corregir_reparo_cdr_expediente",
            //    table: "corregir_reparo_cdr",
            //    column: "id_expediente");

            //migrationBuilder.CreateIndex(
            //    name: "ix_corregir_reparo_control_credito_expediente",
            //    table: "corregir_reparo_control_credito",
            //    column: "id_expediente");

            //migrationBuilder.CreateIndex(
            //    name: "ix_corregir_reparo_inst_pago_expediente",
            //    table: "corregir_reparo_inst_pago",
            //    column: "id_expediente");

            //migrationBuilder.CreateIndex(
            //    name: "ix_corregir_reparo_prefiniquito_expediente",
            //    table: "corregir_reparo_prefiniquito",
            //    column: "id_expediente");

            //migrationBuilder.CreateIndex(
            //    name: "ix_entregar_carpeta_expediente",
            //    table: "entregar_carpeta",
            //    column: "id_expediente");

            //migrationBuilder.CreateIndex(
            //    name: "ix_firma_banco_acreedor_cg_expediente",
            //    table: "firma_banco_acreedor_cg",
            //    column: "id_expediente");

            //migrationBuilder.CreateIndex(
            //    name: "ix_generar_carta_resguardo",
            //    table: "generar_carta_resguardo",
            //    column: "id_expediente");

            //migrationBuilder.CreateIndex(
            //    name: "ix_generar_finiquito_expediente",
            //    table: "generar_finiquito",
            //    column: "id_expediente");

            //migrationBuilder.CreateIndex(
            //    name: "ix_generar_recursos_pagos_cbr_expediente",
            //    table: "generar_recursos_pagos_cbr",
            //    column: "id_expediente");

            //migrationBuilder.CreateIndex(
            //    name: "ix_gestion_rectificatoria_expediente",
            //    table: "gestion_rectificatoria",
            //    column: "id_expediente");

            //migrationBuilder.CreateIndex(
            //    name: "ix_realizar_control_credito_expediente",
            //    table: "realizar_control_credito",
            //    column: "id_expediente");

            //migrationBuilder.CreateIndex(
            //    name: "ix_realizar_revision_previo_firma_banco",
            //    table: "realizar_revision_previo_firma_banco",
            //    column: "id_expediente");

            //migrationBuilder.CreateIndex(
            //    name: "ix_recepcionar_matriz",
            //    table: "recepcionar_matriz",
            //    column: "id_expediente");

            //migrationBuilder.CreateIndex(
            //    name: "ix_recibir_instruccion_pago_expediente",
            //    table: "recibir_instruccion_pago",
            //    column: "id_expediente");

            //migrationBuilder.CreateIndex(
            //    name: "ix_registrar_escritura_cbr_expediente",
            //    table: "registrar_escritura_cbr",
            //    column: "id_expediente");

            //migrationBuilder.CreateIndex(
            //    name: "ix_registrar_firma_apoderado_banco_expediente",
            //    table: "registrar_firma_apoderado_banco",
            //    column: "id_expediente");

            //migrationBuilder.CreateIndex(
            //    name: "ix_valorizar_cbr_expediente",
            //    table: "valorizar_cbr",
            //    column: "id_expediente");

            //migrationBuilder.CreateIndex(
            //    name: "ix_verificar_correccion_escritura_expediente",
            //    table: "verificar_correccion_escritura",
            //    column: "id_expediente");

            //migrationBuilder.CreateIndex(
            //    name: "ix_verificar_reparo_cbr_expediente",
            //    table: "verificar_reparo_cbr",
            //    column: "id_expediente");
            migrationBuilder.Sql(@"
    CREATE INDEX IF NOT EXISTS ix_aprobacion_comercial_legal_cdr ON aprobacion_comercial_legal_cdr (id_expediente);
    CREATE INDEX IF NOT EXISTS ix_corregir_carta_resguardo_expediente ON corregir_carta_resguardo (id_expediente);
    CREATE INDEX IF NOT EXISTS ix_corregir_notaria_reparo_abogados_expediente ON corregir_notaria_reparo_abogados (id_expediente);
    CREATE INDEX IF NOT EXISTS ix_corregir_reparo_cdr_expediente ON corregir_reparo_cdr (id_expediente);
    CREATE INDEX IF NOT EXISTS ix_corregir_reparo_control_credito_expediente ON corregir_reparo_control_credito (id_expediente);
    CREATE INDEX IF NOT EXISTS ix_corregir_reparo_inst_pago_expediente ON corregir_reparo_inst_pago (id_expediente);
    CREATE INDEX IF NOT EXISTS ix_corregir_reparo_prefiniquito_expediente ON corregir_reparo_prefiniquito (id_expediente);
    CREATE INDEX IF NOT EXISTS ix_entregar_carpeta_expediente ON entregar_carpeta (id_expediente);
    CREATE INDEX IF NOT EXISTS ix_firma_banco_acreedor_cg_expediente ON firma_banco_acreedor_cg (id_expediente);
    CREATE INDEX IF NOT EXISTS ix_generar_carta_resguardo ON generar_carta_resguardo (id_expediente);
    CREATE INDEX IF NOT EXISTS ix_generar_finiquito_expediente ON generar_finiquito (id_expediente);
    CREATE INDEX IF NOT EXISTS ix_generar_recursos_pagos_cbr_expediente ON generar_recursos_pagos_cbr (id_expediente);
    CREATE INDEX IF NOT EXISTS ix_gestion_rectificatoria_expediente ON gestion_rectificatoria (id_expediente);
    CREATE INDEX IF NOT EXISTS ix_realizar_control_credito_expediente ON realizar_control_credito (id_expediente);
    CREATE INDEX IF NOT EXISTS ix_realizar_revision_previo_firma_banco ON realizar_revision_previo_firma_banco (id_expediente);
    CREATE INDEX IF NOT EXISTS ix_recepcionar_matriz ON recepcionar_matriz (id_expediente);
    CREATE INDEX IF NOT EXISTS ix_recibir_instruccion_pago_expediente ON recibir_instruccion_pago (id_expediente);
    CREATE INDEX IF NOT EXISTS ix_registrar_escritura_cbr_expediente ON registrar_escritura_cbr (id_expediente);
    CREATE INDEX IF NOT EXISTS ix_registrar_firma_apoderado_banco_expediente ON registrar_firma_apoderado_banco (id_expediente);
    CREATE INDEX IF NOT EXISTS ix_valorizar_cbr_expediente ON valorizar_cbr (id_expediente);
    CREATE INDEX IF NOT EXISTS ix_verificar_correccion_escritura_expediente ON verificar_correccion_escritura (id_expediente);
    CREATE INDEX IF NOT EXISTS ix_verificar_reparo_cbr_expediente ON verificar_reparo_cbr (id_expediente);
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aprobacion_comercial_legal_cdr");

            migrationBuilder.DropTable(
                name: "cierre_copias_notaria");

            migrationBuilder.DropTable(
                name: "corregir_carta_resguardo");

            migrationBuilder.DropTable(
                name: "corregir_notaria_reparo_abogados");

            migrationBuilder.DropTable(
                name: "corregir_reparo_cdr");

            migrationBuilder.DropTable(
                name: "corregir_reparo_cierre_copias_notaria");

            migrationBuilder.DropTable(
                name: "corregir_reparo_control_credito");

            migrationBuilder.DropTable(
                name: "corregir_reparo_inst_pago");

            migrationBuilder.DropTable(
                name: "corregir_reparo_liquidacion");

            migrationBuilder.DropTable(
                name: "corregir_reparo_prefiniquito");

            migrationBuilder.DropTable(
                name: "entregar_carpeta");

            migrationBuilder.DropTable(
                name: "firma_banco_acreedor_cg");

            migrationBuilder.DropTable(
                name: "generar_carta_resguardo");

            migrationBuilder.DropTable(
                name: "generar_finiquito");

            migrationBuilder.DropTable(
                name: "generar_recursos_pagos_cbr");

            migrationBuilder.DropTable(
                name: "gestion_rectificatoria");

            migrationBuilder.DropTable(
                name: "realizar_control_credito");

            migrationBuilder.DropTable(
                name: "realizar_revision_previo_firma_banco");

            migrationBuilder.DropTable(
                name: "recepcionar_matriz");

            migrationBuilder.DropTable(
                name: "recibir_instruccion_pago");

            migrationBuilder.DropTable(
                name: "registrar_escritura_cbr");

            migrationBuilder.DropTable(
                name: "registrar_firma_apoderado_banco");

            migrationBuilder.DropTable(
                name: "reingresar_escritura_cbr");

            migrationBuilder.DropTable(
                name: "revisar_desembolso");

            migrationBuilder.DropTable(
                name: "revisar_inscripcion_cbr");

            migrationBuilder.DropTable(
                name: "revisar_liquidacion");

            migrationBuilder.DropTable(
                name: "valorizar_cbr");

            migrationBuilder.DropTable(
                name: "verificar_correccion_escritura");

            migrationBuilder.DropTable(
                name: "verificar_reparo_cbr");

            migrationBuilder.DropColumn(
                name: "id_usuario_solicitante",
                table: "verificar_reparo_datos_operacion");

            migrationBuilder.DropColumn(
                name: "is_subsanar",
                table: "verificar_reparo_datos_operacion");

            migrationBuilder.DropColumn(
                name: "id_usuario_solicitante",
                table: "corregir_reparo_visado");

            migrationBuilder.DropColumn(
                name: "is_subsanar",
                table: "corregir_reparo_visado");

            migrationBuilder.DropColumn(
                name: "id_usuario_solicitante",
                table: "corregir_reparo_generar_memo_escritura");

            migrationBuilder.DropColumn(
                name: "existe_rol_avaluo",
                table: "corregir_reparo_calculo_doc");

            migrationBuilder.DropColumn(
                name: "id_usuario_solicitante",
                table: "corregir_reparo_calculo_doc");

            migrationBuilder.DropColumn(
                name: "is_subsanar",
                table: "corregir_reparo_calculo_doc");

            migrationBuilder.DropColumn(
                name: "rol_avaluo_editado",
                table: "corregir_reparo_calculo_doc");

            migrationBuilder.DropColumn(
                name: "valor_avaluo_pesos",
                table: "corregir_reparo_calculo_doc");

            migrationBuilder.AlterColumn<string>(
                name: "observaciones",
                table: "verificar_reparo_datos_operacion",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "tipo_tasacion",
                table: "tasacion_detalle",
                type: "boolean",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.Sql(@"
    ALTER TABLE revisar_ingreso_datos_credito
    ALTER COLUMN ubicacion TYPE boolean
    USING (
        CASE
            WHEN lower(ubicacion) IN ('true', 't', '1', 'si')
            THEN true
            ELSE false
        END
    );
");

            migrationBuilder.AlterColumn<string>(
                name: "ubicacion",
                table: "datos_operacion_datos_credito",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "observaciones",
                table: "corregir_reparo_visado",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "observaciones",
                table: "corregir_reparo_generar_memo_escritura",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "observaciones",
                table: "corregir_reparo_generar_borrador_escritura",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "observaciones",
                table: "corregir_reparo_calculo_doc",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "reparo_calculo_doc_detalle",
                columns: table => new
                {
                    id_reparo_calculo_doc = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fecha_ingreso = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    observaciones = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    solicitante = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    subsanar = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reparo_calculo_doc_detalle", x => x.id_reparo_calculo_doc);
                });

            migrationBuilder.CreateTable(
                name: "rol_avaluo_calculo_doc_detalle",
                columns: table => new
                {
                    id_rol_avaluo_calculo_doc = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    comuna = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    direccion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    existe_rol_avaluo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    region = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    rol_avaluo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    tipo_direccion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    tipo_propiedad = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    valor_avaluo_pesos = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rol_avaluo_calculo_doc_detalle", x => x.id_rol_avaluo_calculo_doc);
                });

            migrationBuilder.CreateTable(
                name: "verificar_reparo_datos_operacion_detalle",
                columns: table => new
                {
                    id_verificar_reparo_datos_operacion_detalle = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fecha_ingreso = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    observaciones = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    solicitante = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    subsanar = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_verificar_reparo_datos_operacion_detalle", x => x.id_verificar_reparo_datos_operacion_detalle);
                });

            migrationBuilder.CreateIndex(
                name: "ix_reparo_calculo_doc_detalle_expediente",
                table: "reparo_calculo_doc_detalle",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "ix_rol_avaluo_calculo_doc_detalle_expediente",
                table: "rol_avaluo_calculo_doc_detalle",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "ix_verificar_reparo_datos_operacion_detalle_expediente",
                table: "verificar_reparo_datos_operacion_detalle",
                column: "id_expediente");
        }
    }
}
