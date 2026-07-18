using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class firma_comprador_detalle_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<firma_comprador_detalle_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("firma_comprador_detalle");

            entityTypeBuilder.HasKey(q => q.id_firma_comprador_detalle);

            entityTypeBuilder.Property(q => q.id_firma_comprador).IsRequired();
            entityTypeBuilder.Property(q => q.id_expediente).IsRequired();
            entityTypeBuilder.Property(q => q.relacion_titular).HasMaxLength(100).IsRequired();
            entityTypeBuilder.Property(q => q.rut).HasMaxLength(50).IsRequired();
            entityTypeBuilder.Property(q => q.nombres).HasMaxLength(150).IsRequired();
            entityTypeBuilder.Property(q => q.apellido_materno).HasMaxLength(150).IsRequired();
            entityTypeBuilder.Property(q => q.apellido_paterno).HasMaxLength(150).IsRequired();
            entityTypeBuilder.Property(q => q.estado_civil).HasMaxLength(100).IsRequired();
            entityTypeBuilder.Property(q => q.fecha_firma).IsRequired().HasColumnType("timestamp without time zone"); ;

            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");

        }
    }
}
