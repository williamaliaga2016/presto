using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.BBVA;

internal class cargar_documentos_cliente_entity_config
{
    /// <summary>
    /// Configura el mapeo EF Core para la tabla que audita la confirmacion de carga documental del cliente.
    /// </summary>
    /// <param name="entityTypeBuilder">Builder EF Core de la entidad `cargar_documentos_cliente`.</param>
    public static void SetEntityBuilder(
        EntityTypeBuilder<cargar_documentos_cliente_entity> entityTypeBuilder)
    {
        entityTypeBuilder.ToTable("cargar_documentos_cliente");
        entityTypeBuilder.HasKey(q => q.id);
        entityTypeBuilder.Property(q => q.id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        entityTypeBuilder.Property(q => q.id_expediente).HasColumnName("id_expediente");
        entityTypeBuilder.Property(q => q.id_actividad).HasColumnName("id_actividad").HasMaxLength(50);
        entityTypeBuilder.Property(q => q.documentos_adjuntos).HasColumnName("documentos_adjuntos");
        entityTypeBuilder.Property(q => q.observaciones).HasColumnName("observaciones").HasMaxLength(500);
        entityTypeBuilder.Property(q => q.is_active).HasColumnName("is_active");
        entityTypeBuilder.Property(q => q.row_status).HasColumnName("row_status");
        entityTypeBuilder.Property(q => q.created_by).HasColumnName("created_by");
        entityTypeBuilder.Property(q => q.created_date).HasColumnName("created_date").HasColumnType("timestamp without time zone");
        entityTypeBuilder.Property(q => q.modified_by).HasColumnName("modified_by");
        entityTypeBuilder.Property(q => q.modified_date).HasColumnName("modified_date").HasColumnType("timestamp without time zone");

        entityTypeBuilder.HasIndex(q => q.id_expediente)
            .HasDatabaseName("IX_cargar_docs_cliente_expediente");
    }
}
