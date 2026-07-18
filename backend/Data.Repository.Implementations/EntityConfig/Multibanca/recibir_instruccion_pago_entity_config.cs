using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class recibir_instruccion_pago_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<recibir_instruccion_pago_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("recibir_instruccion_pago");

            entityTypeBuilder.HasKey(q => q.id_recibir_instruccion_pago);

            entityTypeBuilder.Property(q => q.id_recibir_instruccion_pago).IsRequired();
            entityTypeBuilder.Property(q => q.id_expediente).IsRequired();
            entityTypeBuilder.Property(q => q.enviar_a_reparo).HasColumnName("enviar_a_reparo");
            entityTypeBuilder.Property(q => q.condicion_especial_desembolso).HasMaxLength(1000);
            entityTypeBuilder.Property(q => q.observaciones).HasMaxLength(1000);

            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");

            entityTypeBuilder.HasIndex(q => new { q.id_expediente })
                .HasDatabaseName("ix_recibir_instruccion_pago_expediente");
        }
    }
}
