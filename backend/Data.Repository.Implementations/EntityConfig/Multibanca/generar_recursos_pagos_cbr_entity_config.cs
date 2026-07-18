using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class generar_recursos_pagos_cbr_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<generar_recursos_pagos_cbr_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("generar_recursos_pagos_cbr");

            entityTypeBuilder.HasKey(q => q.id_generar_recursos_pagos_cbr);

            entityTypeBuilder.Property(q => q.id_generar_recursos_pagos_cbr).IsRequired();
            entityTypeBuilder.Property(q => q.id_expediente).IsRequired();
            entityTypeBuilder.Property(q => q.observaciones).HasMaxLength(1000);

            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");

            entityTypeBuilder.HasIndex(q => new { q.id_expediente })
                .HasDatabaseName("ix_generar_recursos_pagos_cbr_expediente");
        }
    }
}
