using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.BBVA;

internal class titular_bbva_entity_config
{
    public static void SetEntityBuilder(EntityTypeBuilder<titular_bbva> entityTypeBuilder)
    {
        entityTypeBuilder.ToTable("titular_bbva");
        entityTypeBuilder.HasKey(q => q.id);
        entityTypeBuilder.Property(q => q.id).HasColumnName("id").ValueGeneratedOnAdd();
        entityTypeBuilder.Property(q => q.id_expediente).HasColumnName("id_expediente");
        entityTypeBuilder.Property(q => q.id_actividad).HasColumnName("id_actividad").HasMaxLength(50);
        entityTypeBuilder.Property(q => q.numero_titular).HasColumnName("numero_titular");
        entityTypeBuilder.Property(q => q.tipo_identificacion).HasColumnName("tipo_identificacion").HasMaxLength(20);
        entityTypeBuilder.Property(q => q.numero_identificacion).HasColumnName("numero_identificacion").HasMaxLength(30);
        entityTypeBuilder.Property(q => q.nombre_completo).HasColumnName("nombre_completo").HasMaxLength(200);
        entityTypeBuilder.Property(q => q.celular_cliente).HasColumnName("celular_cliente").HasMaxLength(20);
        entityTypeBuilder.Property(q => q.telefono_residente).HasColumnName("telefono_residente").HasMaxLength(20);
        entityTypeBuilder.Property(q => q.email).HasColumnName("email").HasMaxLength(150);
        entityTypeBuilder.Property(q => q.direccion_residencia).HasColumnName("direccion_residencia").HasMaxLength(300);
        entityTypeBuilder.Property(q => q.telefono_declarativo).HasColumnName("telefono_declarativo").HasMaxLength(20);
        entityTypeBuilder.Property(q => q.correo_declarativo).HasColumnName("correo_declarativo").HasMaxLength(150);
        entityTypeBuilder.Property(q => q.is_active).HasColumnName("is_active");
        entityTypeBuilder.Property(q => q.row_status).HasColumnName("row_status");
        entityTypeBuilder.Property(q => q.created_by).HasColumnName("created_by");
        entityTypeBuilder.Property(q => q.created_date).HasColumnName("created_date").HasColumnType("timestamp without time zone");
        entityTypeBuilder.Property(q => q.modified_by).HasColumnName("modified_by");
        entityTypeBuilder.Property(q => q.modified_date).HasColumnName("modified_date").HasColumnType("timestamp without time zone");

        entityTypeBuilder.HasIndex(q => q.id_expediente)
            .HasDatabaseName("IX_titular_bbva_expediente");
    }
}
