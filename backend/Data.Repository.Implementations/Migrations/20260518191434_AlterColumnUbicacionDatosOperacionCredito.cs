using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class AlterColumnUbicacionDatosOperacionCredito : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE revisar_ingreso_datos_credito
                ALTER COLUMN ubicacion
                TYPE boolean
                USING (
                    CASE
                        WHEN ubicacion IN ('1', 'true', 'TRUE', 'True', 'Santiago')
                        THEN true
                        ELSE false
                    END
                );
            ");

             migrationBuilder.Sql(@"
                ALTER TABLE datos_operacion_datos_credito
                ALTER COLUMN ubicacion
                TYPE boolean
                USING (
                    CASE
                        WHEN ubicacion IN ('1', 'true', 'TRUE', 'True', 'Santiago')
                        THEN true
                        ELSE false
                    END
                );
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ubicacion",
                table: "revisar_ingreso_datos_credito",
                type: "text",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ubicacion",
                table: "datos_operacion_datos_credito",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);
        }
    }
}
