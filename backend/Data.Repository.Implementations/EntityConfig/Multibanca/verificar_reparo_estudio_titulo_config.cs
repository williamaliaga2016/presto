using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class verificar_reparo_estudio_titulo_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<verificar_reparo_estudio_titulo_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("verificar_reparo_estudio_titulo");

            entityTypeBuilder.HasKey(q => q.id_verificar_reparo_estudio_titulo);

            entityTypeBuilder.Property(q => q.id_verificar_reparo_estudio_titulo).IsRequired();
            entityTypeBuilder.Property(q => q.id_expediente).IsRequired();
            entityTypeBuilder.Property(q => q.enviar_a_reparo).HasColumnName("enviar_a_reparo");
            entityTypeBuilder.Property(q => q.observaciones).HasMaxLength(1000);

            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");

            entityTypeBuilder.HasIndex(q => new { q.id_expediente })
                .HasDatabaseName("ix_verificar_reparo_estudio_titulo_expediente");
        }
    }
}
