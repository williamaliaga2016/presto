using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class revisar_inscripcion_cbr_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<revisar_inscripcion_cbr_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("revisar_inscripcion_cbr");

            entityTypeBuilder.HasKey(q => q.id_revisar_inscripcion_cbr);

            entityTypeBuilder.Property(q => q.id_revisar_inscripcion_cbr).IsRequired();
            entityTypeBuilder.Property(q => q.id_expediente).IsRequired();
            entityTypeBuilder.Property(q => q.id_usuario_solicitante).IsRequired();
            entityTypeBuilder.Property(q => q.is_enviar_reparo).IsRequired();
            entityTypeBuilder.Property(q => q.observaciones).HasMaxLength(1000);

            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");

        }
    }
}
