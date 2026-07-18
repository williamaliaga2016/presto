using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class recepcion_carga_fabrica_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<recepcion_carga_fabrica_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("recepcion_carga_fabrica");

            entityTypeBuilder.HasKey(q => q.id_recepcion_carga_fabrica);

            entityTypeBuilder.Property(q => q.id_recepcion_carga_fabrica).IsRequired();
            entityTypeBuilder.Property(q => q.id_expediente).IsRequired();
            entityTypeBuilder.Property(q => q.id_usuario_solicitante).IsRequired();
            entityTypeBuilder.Property(q => q.is_enviar_reparo).IsRequired();
            entityTypeBuilder.Property(q => q.observaciones).HasMaxLength(1000);

            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");

            entityTypeBuilder.HasIndex(q => new { q.id_expediente })
                .HasDatabaseName("ix_recepcion_carga_fabrica_expediente");
        }
    }
}
