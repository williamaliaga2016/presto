using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Multibanca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "actividades",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    id_actividad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    id_rol = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    id_usuario = table.Column<int>(type: "integer", nullable: false),
                    descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false),
                    fecha_alta = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    fecha_asignacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    fecha_inicio = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    fecha_termino = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    fecha_cancelacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    fecha_actualizacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    fecha_reingreso = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    fecha_suspencion = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    fecha_reactivacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_actividades", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bitacora",
                columns: table => new
                {
                    id_bitacora = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    id_actividad = table.Column<string>(type: "text", nullable: false),
                    id_usuario = table.Column<int>(type: "integer", nullable: false),
                    fecha_alta = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    observaciones = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bitacora", x => x.id_bitacora);
                });

            migrationBuilder.CreateTable(
                name: "expediente_digital",
                columns: table => new
                {
                    id_archivo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    id_documento = table.Column<int>(type: "integer", nullable: false),
                    id_usuario = table.Column<int>(type: "integer", nullable: false),
                    nombre_archivo = table.Column<string>(type: "text", nullable: false),
                    nombre_archivo_original = table.Column<string>(type: "text", nullable: false),
                    extension = table.Column<string>(type: "text", nullable: false),
                    version_archivo = table.Column<int>(type: "integer", nullable: false),
                    fecha_alta = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    comentarios = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expediente_digital", x => x.id_archivo);
                });

            migrationBuilder.CreateTable(
                name: "menus",
                columns: table => new
                {
                    menu_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    menu_padre_id = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    icon_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description_alt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    menu_url = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    is_show_navbar = table.Column<bool>(type: "boolean", nullable: false),
                    is_show_home_menu = table.Column<bool>(type: "boolean", nullable: false),
                    orden = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menus", x => x.menu_id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "role_menu",
                columns: table => new
                {
                    role_menu_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    menu_id = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_menu", x => x.role_menu_id);
                    table.ForeignKey(
                        name: "FK_role_menu_menus_menu_id",
                        column: x => x.menu_id,
                        principalTable: "menus",
                        principalColumn: "menu_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_role_menu_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name_first = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name_second = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    id_document_type = table.Column<int>(type: "integer", nullable: false),
                    nro_document = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    user_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    is_first_access = table.Column<bool>(type: "boolean", nullable: true),
                    remaining_attempts = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_users_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_role_menu_menu_id",
                table: "role_menu",
                column: "menu_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_menu_role_id",
                table: "role_menu",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                table: "users",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "actividades");

            migrationBuilder.DropTable(
                name: "bitacora");

            migrationBuilder.DropTable(
                name: "expediente_digital");

            migrationBuilder.DropTable(
                name: "role_menu");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "menus");

            migrationBuilder.DropTable(
                name: "role");
        }
    }
}
