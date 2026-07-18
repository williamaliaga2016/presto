using Data.Repository.Interfaces.Entities.Multibanca.RevisarDatosOperacion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.RevisarDatosOperacion
{
    internal class revisar_datos_operacion_fiador_garante_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<revisar_datos_operacion_fiador_garante_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("revisar_datos_operacion_fiador_garante");
            entityTypeBuilder.HasKey(q => q.id_revisar_datos_operacion_fiador_garante);
            entityTypeBuilder.Property(q => q.id_revisar_datos_operacion_fiador_garante)
                .HasColumnName("id_revisar_datos_operacion_fiador_garante")
                .IsRequired();
            entityTypeBuilder.Property(q => q.id_revisar_datos_operacion)
                .HasColumnName("id_revisar_datos_operacion")
                .IsRequired();
            entityTypeBuilder.Property(q => q.id_expediente)
                .HasColumnName("id_expediente")
                .IsRequired();

            entityTypeBuilder.Property(q => q.rut).HasColumnName("rut").HasMaxLength(20);
            entityTypeBuilder.Property(q => q.nombres).HasColumnName("nombres").HasMaxLength(250);
            entityTypeBuilder.Property(q => q.apellido_paterno).HasColumnName("apellido_paterno").HasMaxLength(250);
            entityTypeBuilder.Property(q => q.apellido_materno).HasColumnName("apellido_materno").HasMaxLength(250);
            entityTypeBuilder.Property(q => q.fecha_nacimiento).HasColumnName("fecha_nacimiento").HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.genero).HasColumnName("genero").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.estado_civil).HasColumnName("estado_civil").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.nacionalidad).HasColumnName("nacionalidad").HasMaxLength(100);
            entityTypeBuilder.Property(q => q.profesion).HasColumnName("profesion").HasMaxLength(250);
            entityTypeBuilder.Property(q => q.direccion).HasColumnName("direccion").HasMaxLength(500);
            entityTypeBuilder.Property(q => q.region).HasColumnName("region").HasMaxLength(100);
            entityTypeBuilder.Property(q => q.comuna).HasColumnName("comuna").HasMaxLength(100);
            entityTypeBuilder.Property(q => q.telefono_fijo).HasColumnName("telefono_fijo").HasMaxLength(20);
            entityTypeBuilder.Property(q => q.telefono_movil).HasColumnName("telefono_movil").HasMaxLength(20);
            entityTypeBuilder.Property(q => q.email).HasColumnName("email").HasMaxLength(250);
            entityTypeBuilder.Property(q => q.relacion_titular).HasColumnName("relacion_titular").HasMaxLength(100);
            entityTypeBuilder.Property(q => q.observaciones).HasColumnName("observaciones").HasMaxLength(1000);

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

            entityTypeBuilder.HasIndex(q => q.id_expediente);

            entityTypeBuilder.HasIndex(q => q.id_revisar_datos_operacion);
        }
    }
}
