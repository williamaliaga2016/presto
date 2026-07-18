using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class rectificatoria_legal_cierre_copias_postventa_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<rectificatoria_legal_cierre_copias_postventa_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("rectificatoria_legal_cierre_copias_postventa");

            entityTypeBuilder.HasKey(q => q.id_rectificatoria_legal_cierre_copias_postventa);

            entityTypeBuilder.Property(q => q.id_rectificatoria_legal_cierre_copias_postventa).IsRequired();
            entityTypeBuilder.Property(q => q.id_expediente).IsRequired();
            entityTypeBuilder.Property(q => q.id_usuario_solicitante).IsRequired();
            entityTypeBuilder.Property(q => q.fecha_firma).HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.observaciones).HasMaxLength(2000);

            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");

            entityTypeBuilder.HasIndex(q => new { q.id_expediente })
                .HasDatabaseName("ix_rectificatoria_legal_cierre_copias_postventa_expediente");
        }
    }
}
