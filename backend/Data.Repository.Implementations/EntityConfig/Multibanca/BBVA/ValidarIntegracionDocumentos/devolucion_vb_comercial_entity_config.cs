using Data.Repository.Interfaces.Entities.Multibanca.BBVA;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.BBVA.ValidarIntegracionDocumentos
{
    internal class devolucion_vb_comercial_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<devolucion_vb_comercial_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("devolucion_vb_comercial", "public");
            entityTypeBuilder.HasKey(q => q.id);

            entityTypeBuilder.Property(q => q.id).HasColumnName("id").IsRequired();
            entityTypeBuilder.Property(q => q.id_expediente).HasColumnName("id_expediente").IsRequired();
            entityTypeBuilder.Property(q => q.id_actividad).HasColumnName("id_actividad").HasMaxLength(100).IsRequired();
            entityTypeBuilder.Property(q => q.cliente_desiste).HasColumnName("cliente_desiste");
            entityTypeBuilder.Property(q => q.motivo_cierre).HasColumnName("motivo_cierre").HasMaxLength(100);
            entityTypeBuilder.Property(q => q.observaciones).HasColumnName("observaciones");

            entityTypeBuilder.Property(q => q.created_date).HasColumnName("created_date").HasColumnType("timestamp without time zone").IsRequired();
            entityTypeBuilder.Property(q => q.modified_date).HasColumnName("modified_date").HasColumnType("timestamp without time zone");

            entityTypeBuilder.HasIndex(q => q.id_expediente).HasDatabaseName("idx_dvc_expediente");
        }
    }
}
