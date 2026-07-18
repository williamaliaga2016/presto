using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class reparo_cdr_detalle_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<reparo_cdr_detalle_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("reparo_cdr_detalle");

            entityTypeBuilder.HasKey(q => q.id_reparo_cdr);

            entityTypeBuilder.Property(q => q.id_reparo_cdr).IsRequired();
            entityTypeBuilder.Property(q => q.id_expediente).IsRequired();
            entityTypeBuilder.Property(q => q.solicitante).HasMaxLength(500);
            entityTypeBuilder.Property(q => q.observaciones).HasMaxLength(2000);
            entityTypeBuilder.Property(q => q.fecha_ingreso).HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.subsanar).IsRequired().HasDefaultValue(false);

            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");

            entityTypeBuilder.HasIndex(q => new { q.id_expediente })
                .HasDatabaseName("ix_reparo_cdr_detalle_expediente");
        }
    }
}
