using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class reports_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<reports_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("reports");

            entityTypeBuilder.HasKey(q => q.id_reporte);

            entityTypeBuilder.Property(q => q.id_reporte).IsRequired();
            entityTypeBuilder.Property(q => q.nombre).IsRequired();
            entityTypeBuilder.Property(q => q.descripcion).HasMaxLength(1000);
            entityTypeBuilder.Property(q => q.report_path).HasMaxLength(200).IsRequired();
            entityTypeBuilder.Property(q => q.template).HasMaxLength(250);
            entityTypeBuilder.Property(q => q.extension).HasMaxLength(10);
            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");
        }
    }
}
