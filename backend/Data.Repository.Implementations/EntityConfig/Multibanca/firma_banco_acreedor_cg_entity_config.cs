using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class firma_banco_acreedor_cg_entity_config
    {
        public static void SetEntityBuilder(
            EntityTypeBuilder<firma_banco_acreedor_cg_entity> b
        )
        {
            b.ToTable("firma_banco_acreedor_cg");
            b.HasKey(q => q.id_firma_banco_acreedor_cg);
            b.Property(q => q.observaciones).HasMaxLength(2000);
            b.Property(q => q.created_date)
                .IsRequired()
                .HasColumnType("timestamp without time zone");
            b.Property(q => q.modified_date).HasColumnType(
                "timestamp without time zone"
            );
            b.HasIndex(q => new { q.id_expediente })
                .HasDatabaseName("ix_firma_banco_acreedor_cg_expediente");
        }
    }
}
