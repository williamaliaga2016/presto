using Data.Repository.Interfaces.Entities.Multibanca.ValidacionRectificatoriaLegal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.ValidacionRectificatoriaLegal
{
    internal class validacion_rectificatoria_legal_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<validacion_rectificatoria_legal_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("validacion_rectificatoria_legal");

            entityTypeBuilder.HasKey(q => q.id_validacion_rectificatoria_legal);

            entityTypeBuilder.Property(q => q.id_validacion_rectificatoria_legal)
                .HasColumnName("id_validacion_rectificatoria_legal")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_expediente)
                .HasColumnName("id_expediente")
                .IsRequired();
            entityTypeBuilder.Property(q => q.id_usuario_solicitante).IsRequired();
            entityTypeBuilder.Property(q => q.is_subsanar).IsRequired();
            entityTypeBuilder.Property(q => q.observaciones).HasMaxLength(1000);
            entityTypeBuilder.Property(q => q.require_documentacion).HasColumnName("require_documentacion");
            entityTypeBuilder.Property(q => q.realiza_pago).HasColumnName("realiza_pago");
            entityTypeBuilder.Property(q => q.encargado_firma).HasColumnName("encargado_firma");
            entityTypeBuilder.Property(q => q.requiere_inscripcion_cbr).IsRequired();

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
                .HasDatabaseName("ix_validacion_rectificatoria_legal_expediente");
        }
    }
}
