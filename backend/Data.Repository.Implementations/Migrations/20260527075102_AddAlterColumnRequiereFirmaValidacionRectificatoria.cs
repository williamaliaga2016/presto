using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class AddAlterColumnRequiereFirmaValidacionRectificatoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE validacion_rectificatoria_legal
                ALTER COLUMN encargado_firma TYPE boolean
                USING false;
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"
                ALTER TABLE validacion_rectificatoria_legal
                ALTER COLUMN encargado_firma TYPE boolean
                USING encargado_firma::boolean;
            ");
        }
    }
}
