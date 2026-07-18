using Data.Repository.Interfaces.Entities.Multibanca.RevisarDatosOperacion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.RevisarDatosOperacion
{
    internal class revisar_datos_operacion_vendedor_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<revisar_datos_operacion_vendedor_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("revisar_datos_operacion_vendedor");

            entityTypeBuilder.HasKey(q => q.id_revisar_datos_operacion_vendedor);

            entityTypeBuilder.Property(q => q.id_revisar_datos_operacion_vendedor)
                .HasColumnName("id_revisar_datos_operacion_vendedor")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_revisar_datos_operacion)
                .HasColumnName("id_revisar_datos_operacion")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_expediente)
                .HasColumnName("id_expediente")
                .IsRequired();

            entityTypeBuilder.Property(q => q.rut).HasColumnName("rut").HasMaxLength(20);
            entityTypeBuilder.Property(q => q.tipo_persona).HasColumnName("tipo_persona").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.razon_social).HasColumnName("razon_social").HasMaxLength(250);
            entityTypeBuilder.Property(q => q.nombres).HasColumnName("nombres").HasMaxLength(150);
            entityTypeBuilder.Property(q => q.apellido_paterno).HasColumnName("apellido_paterno").HasMaxLength(100);
            entityTypeBuilder.Property(q => q.apellido_materno).HasColumnName("apellido_materno").HasMaxLength(100);
            entityTypeBuilder.Property(q => q.fecha_nacimiento).HasColumnName("fecha_nacimiento").HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.genero).HasColumnName("genero").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.estado_civil).HasColumnName("estado_civil").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.nacionalidad).HasColumnName("nacionalidad").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.profesion).HasColumnName("profesion").HasMaxLength(150);
            entityTypeBuilder.Property(q => q.relacion_titular).HasColumnName("relacion_titular").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.direccion).HasColumnName("direccion").HasMaxLength(250);
            entityTypeBuilder.Property(q => q.direccion_env_esc).HasColumnName("direccion_env_esc").HasMaxLength(250);
            entityTypeBuilder.Property(q => q.region).HasColumnName("region").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.region_env_esc).HasColumnName("region_env_esc").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.comuna).HasColumnName("comuna").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.comuna_env_esc).HasColumnName("comuna_env_esc").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.direccion_env_div).HasColumnName("direccion_env_div").HasMaxLength(250);
            entityTypeBuilder.Property(q => q.region_env_div).HasColumnName("region_env_div").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.comuna_env_div).HasColumnName("comuna_env_div").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.tipo_dir_dividendo).HasColumnName("tipo_dir_dividendo");
            entityTypeBuilder.Property(q => q.telefono).HasColumnName("telefono").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.telefono_comercial).HasColumnName("telefono_comercial").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.telefono_movil).HasColumnName("telefono_movil").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.email).HasColumnName("email").HasMaxLength(150);
            entityTypeBuilder.Property(q => q.email2).HasColumnName("email2").HasMaxLength(150);
            entityTypeBuilder.Property(q => q.enviar_reparo).HasColumnName("enviar_reparo").HasMaxLength(50);
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
