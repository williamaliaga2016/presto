using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class generar_memo_escritura_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<generar_memo_escritura_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("generar_memo_escritura");

            entityTypeBuilder.HasKey(q => q.id_generar_memo_escritura);

            entityTypeBuilder.Property(q => q.id_generar_memo_escritura)
                .HasColumnName("id_generar_memo_escritura")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_expediente)
                .HasColumnName("id_expediente")
                .IsRequired();

            entityTypeBuilder.Property(q => q.enviar_reparo)
                .HasColumnName("enviar_reparo")
                .IsRequired();

            entityTypeBuilder.Property(q => q.observaciones)
                .HasColumnName("observaciones")
                .HasMaxLength(2000);

            entityTypeBuilder.Property(q => q.is_active)
                .HasColumnName("is_active")
                .IsRequired();

            entityTypeBuilder.Property(q => q.row_status)
                .HasColumnName("row_status")
                .IsRequired();

            entityTypeBuilder.Property(q => q.created_by)
                .HasColumnName("created_by")
                .IsRequired();

            entityTypeBuilder.Property(q => q.created_date)
                .HasColumnName("created_date")
                .HasColumnType("timestamp without time zone")
                .IsRequired();

            entityTypeBuilder.Property(q => q.modified_by)
                .HasColumnName("modified_by");

            entityTypeBuilder.Property(q => q.modified_date)
                .HasColumnName("modified_date")
                .HasColumnType("timestamp without time zone");

            entityTypeBuilder.HasIndex(q => q.id_expediente)
                .HasDatabaseName("ix_generar_memo_escritura_expediente");
        }
    }
}
