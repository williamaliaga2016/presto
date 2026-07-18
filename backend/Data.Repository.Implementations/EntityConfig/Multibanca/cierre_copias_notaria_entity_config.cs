using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class cierre_copias_notaria_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<cierre_copias_notaria_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("cierre_copias_notaria");

            entityTypeBuilder.HasKey(q => q.id_cierre_copias_notaria);

            entityTypeBuilder.Property(q => q.id_cierre_copias_notaria).IsRequired();
            entityTypeBuilder.Property(q => q.id_expediente).IsRequired();
            entityTypeBuilder.Property(q => q.enviar_a_reparo).HasColumnName("enviar_a_reparo");
            entityTypeBuilder.Property(q => q.observaciones).HasMaxLength(1000);

            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");

        }
    }
}
