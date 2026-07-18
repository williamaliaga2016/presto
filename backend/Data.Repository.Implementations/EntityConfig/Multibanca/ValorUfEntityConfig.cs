using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class ValorUfEntityConfig
    {
        public static void SetEntityBuilder(EntityTypeBuilder<valor_uf_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("valor_uf");
            entityTypeBuilder.HasKey(q => q.id_valor_uf);
            entityTypeBuilder.Property(q => q.fecha).IsRequired().HasColumnType("date");
            entityTypeBuilder.Property(q => q.valor).IsRequired().HasColumnType("numeric(12,2)");
            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");

            entityTypeBuilder.HasIndex(q => q.fecha).IsUnique().HasDatabaseName("IX_valor_uf_fecha");
        }
    }
}
