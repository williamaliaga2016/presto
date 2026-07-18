using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class rectificatoria_firma_detalle_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<rectificatoria_firma_detalle_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("rectificatoria_firma_detalle");

            entityTypeBuilder.HasKey(q => q.id_rectificatoria_firma_detalle);

            entityTypeBuilder.Property(q => q.id_rectificatoria_firma_detalle).IsRequired();
            entityTypeBuilder.Property(q => q.id_rectificatoria_firma).IsRequired();
            entityTypeBuilder.Property(q => q.id_expediente).IsRequired();
            entityTypeBuilder.Property(q => q.rol_compadecencia).HasMaxLength(50).IsRequired();
            entityTypeBuilder.Property(q => q.rut).HasMaxLength(50).IsRequired();
            entityTypeBuilder.Property(q => q.fecha_firma).IsRequired().HasColumnType("timestamp without time zone");

            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");

        }
    }
}
