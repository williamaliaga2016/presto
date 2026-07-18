using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Multibanca.Domain.Models.Multibanca;

namespace Data.Repository.Implementations.EntityConfig.Multibanca;

internal class token_acceso_temporal_entity_config
{
    /// <summary>
    /// Configura la tabla, columnas e indices del token de acceso temporal.
    /// </summary>
    /// <param name="entityTypeBuilder">Constructor de metadata EF Core para la entidad.</param>
    public static void SetEntityBuilder(
        EntityTypeBuilder<token_acceso_temporal> entityTypeBuilder)
    {
        entityTypeBuilder.ToTable("token_acceso_temporal");
        entityTypeBuilder.HasKey(q => q.id);
        entityTypeBuilder.Property(q => q.id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        entityTypeBuilder.Property(q => q.id_expediente).HasColumnName("id_expediente");
        entityTypeBuilder.Property(q => q.id_actividad).HasColumnName("id_actividad").HasMaxLength(100);
        entityTypeBuilder.Property(q => q.id_usuario).HasColumnName("id_usuario");
        entityTypeBuilder.Property(q => q.token)
            .HasColumnName("token")
            .HasDefaultValueSql("gen_random_uuid()");
        entityTypeBuilder.Property(q => q.fecha_expiracion)
            .HasColumnName("fecha_expiracion")
            .HasColumnType("timestamp without time zone");
        entityTypeBuilder.Property(q => q.usado)
            .HasColumnName("usado")
            .HasDefaultValue(false);
        entityTypeBuilder.Property(q => q.fecha_uso)
            .HasColumnName("fecha_uso")
            .HasColumnType("timestamp without time zone");
        entityTypeBuilder.Property(q => q.is_active)
            .HasColumnName("is_active")
            .HasDefaultValue(true);
        entityTypeBuilder.Property(q => q.created_by).HasColumnName("created_by");
        entityTypeBuilder.Property(q => q.created_date)
            .HasColumnName("created_date")
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("NOW()");

        entityTypeBuilder.HasIndex(q => q.token)
            .IsUnique()
            .HasDatabaseName("idx_token_acceso_unico");

        entityTypeBuilder.HasIndex(q => new { q.id_expediente, q.id_actividad })
            .HasDatabaseName("idx_token_expediente");
    }
}
