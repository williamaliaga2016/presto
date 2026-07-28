using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.BBVA.Escrituracion;

internal class validar_condiciones_desembolso_entity_config
{
    public static void SetEntityBuilder(EntityTypeBuilder<validar_condiciones_desembolso_entity> entityTypeBuilder)
    {
        entityTypeBuilder.ToTable("validar_condiciones_desembolso");
        entityTypeBuilder.HasKey(q => q.id);
        entityTypeBuilder.Property(q => q.id).HasColumnName("id").ValueGeneratedOnAdd();
        entityTypeBuilder.Property(q => q.id_expediente).HasColumnName("id_expediente").IsRequired();
        entityTypeBuilder.Property(q => q.id_actividad).HasColumnName("id_actividad").HasMaxLength(100);
        entityTypeBuilder.Property(q => q.origen_tramite).HasColumnName("origen_tramite").HasMaxLength(50);
        entityTypeBuilder.Property(q => q.confirmar_plan_pagos).HasColumnName("confirmar_plan_pagos").IsRequired();
        entityTypeBuilder.Property(q => q.requiere_escalamiento_comercial).HasColumnName("requiere_escalamiento_comercial").HasMaxLength(2);
        entityTypeBuilder.Property(q => q.suspendida).HasColumnName("suspendida").IsRequired();
        entityTypeBuilder.Property(q => q.conteo_caidas).HasColumnName("conteo_caidas").IsRequired();
        entityTypeBuilder.Property(q => q.observaciones).HasColumnName("observaciones").HasMaxLength(500);
        entityTypeBuilder.Property(q => q.is_active).HasColumnName("is_active").IsRequired();
        entityTypeBuilder.Property(q => q.row_status).HasColumnName("row_status").IsRequired();
        entityTypeBuilder.Property(q => q.created_by).HasColumnName("created_by").IsRequired();
        entityTypeBuilder.Property(q => q.created_date).HasColumnName("created_date").IsRequired().HasColumnType("timestamp without time zone");
        entityTypeBuilder.Property(q => q.modified_by).HasColumnName("modified_by");
        entityTypeBuilder.Property(q => q.modified_date).HasColumnName("modified_date").HasColumnType("timestamp without time zone");

        entityTypeBuilder.HasIndex(q => q.id_expediente)
            .HasDatabaseName("idx_validar_condiciones_desembolso_expediente")
            .IsUnique()
            .HasFilter("is_active = true AND row_status = true");
    }
}
