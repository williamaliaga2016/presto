using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class aprobacion_comercial_legal_cdr_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<aprobacion_comercial_legal_cdr_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("aprobacion_comercial_legal_cdr");

            entityTypeBuilder.HasKey(q => q.id_aprobacion_comercial_legal_cdr);

            entityTypeBuilder.Property(q => q.id_aprobacion_comercial_legal_cdr).IsRequired();
            entityTypeBuilder.Property(q => q.id_expediente).IsRequired();
            entityTypeBuilder.Property(q => q.enviar_a_reparo).HasColumnName("enviar_a_reparo");
            entityTypeBuilder.Property(q => q.observaciones).HasMaxLength(2000);

            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");

            entityTypeBuilder.HasIndex(q => new { q.id_expediente })
                .HasDatabaseName("ix_aprobacion_comercial_legal_cdr");
        }
    }
}
