using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class IdempotencyKeyEntityConfig
    {
        public static void SetEntityBuilder(EntityTypeBuilder<idempotency_key_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("idempotency_keys");

            entityTypeBuilder.HasKey(q => q.key);
            entityTypeBuilder.Property(q => q.key).IsRequired().HasMaxLength(36);
            entityTypeBuilder.Property(q => q.id_expediente).IsRequired();
            entityTypeBuilder.Property(q => q.response_snapshot).IsRequired().HasColumnType("text");
            entityTypeBuilder.Property(q => q.created_at)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("NOW()");
        }
    }
}
