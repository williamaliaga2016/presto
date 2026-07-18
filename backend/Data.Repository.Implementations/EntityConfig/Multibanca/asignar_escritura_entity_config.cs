using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class asignar_escritura_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<asignar_escritura_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("asignar_escritura");

            entityTypeBuilder.HasKey(q => q.id_asignar_escritura);

            entityTypeBuilder.Property(q => q.id_asignar_escritura).IsRequired();
            entityTypeBuilder.Property(q => q.id_expediente).IsRequired();
            entityTypeBuilder.Property(q => q.id_actividad).HasMaxLength(50).IsRequired();
            entityTypeBuilder.Property(q => q.abogado).HasMaxLength(200).IsRequired();
            entityTypeBuilder.Property(q => q.observaciones).HasMaxLength(1000).IsRequired();

            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");

            entityTypeBuilder.HasIndex(q => new { q.id_expediente, q.id_actividad })
                .HasDatabaseName("ix_asignar_escritura_expediente_actividad")
                .IsUnique();
        }
    }
}
