using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.BBVA.Escrituracion;

internal class realizar_ep_registradas_entity_config
{
    public static void SetEntityBuilder(EntityTypeBuilder<realizar_ep_registradas_entity> entityTypeBuilder)
    {
        entityTypeBuilder.ToTable("realizar_ep_registradas");
        entityTypeBuilder.HasKey(q => q.id);
        entityTypeBuilder.Property(q => q.id).HasColumnName("id").ValueGeneratedOnAdd();
        entityTypeBuilder.Property(q => q.id_expediente).HasColumnName("id_expediente").IsRequired();
        entityTypeBuilder.Property(q => q.id_actividad).HasColumnName("id_actividad").HasMaxLength(100);

        entityTypeBuilder.Property(q => q.finalizacion).HasColumnName("finalizacion").HasColumnType("date");
        entityTypeBuilder.Property(q => q.causal).HasColumnName("causal").HasMaxLength(200);
        entityTypeBuilder.Property(q => q.fecha_finalizacion).HasColumnName("fecha_finalizacion").HasColumnType("date");
        entityTypeBuilder.Property(q => q.confirmacion_ep_registrada).HasColumnName("confirmacion_ep_registrada").IsRequired();
        entityTypeBuilder.Property(q => q.observaciones).HasColumnName("observaciones").HasMaxLength(500);

        // Auditoría
        entityTypeBuilder.Property(q => q.is_active).HasColumnName("is_active").IsRequired();
        entityTypeBuilder.Property(q => q.row_status).HasColumnName("row_status").IsRequired();
        entityTypeBuilder.Property(q => q.created_by).HasColumnName("created_by").IsRequired();
        entityTypeBuilder.Property(q => q.created_date).HasColumnName("created_date").IsRequired().HasColumnType("timestamp without time zone");
        entityTypeBuilder.Property(q => q.modified_by).HasColumnName("modified_by");
        entityTypeBuilder.Property(q => q.modified_date).HasColumnName("modified_date").HasColumnType("timestamp without time zone");

        entityTypeBuilder.HasIndex(q => q.id_expediente)
            .HasDatabaseName("idx_realizar_ep_registradas_expediente")
            .IsUnique()
            .HasFilter("is_active = true AND row_status = true");
    }
}
