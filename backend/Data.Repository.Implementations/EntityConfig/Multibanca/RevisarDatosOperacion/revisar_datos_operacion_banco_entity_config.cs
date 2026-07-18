using Data.Repository.Interfaces.Entities.Multibanca.RevisarDatosOperacion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.RevisarDatosOperacion
{
    internal class revisar_datos_operacion_banco_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<revisar_datos_operacion_banco_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("revisar_datos_operacion_banco");
            entityTypeBuilder.HasKey(q => q.id_revisar_datos_operacion_banco);
            entityTypeBuilder.Property(q => q.id_revisar_datos_operacion_banco)
                .HasColumnName("id_revisar_datos_operacion_banco")
                .IsRequired();
            entityTypeBuilder.Property(q => q.id_revisar_datos_operacion)
                .HasColumnName("id_revisar_datos_operacion")
                .IsRequired();
            entityTypeBuilder.Property(q => q.id_expediente)
                .HasColumnName("id_expediente")
                .IsRequired();


            entityTypeBuilder.Property(q => q.cuenta_carta_resguardo).HasColumnName("cuenta_carta_resguardo");
            entityTypeBuilder.Property(q => q.institucion).HasColumnName("institucion").HasMaxLength(250);
            entityTypeBuilder.Property(q => q.rut_banco_acreedor).HasColumnName("rut_banco_acreedor").HasMaxLength(20);
            entityTypeBuilder.Property(q => q.enviar_a_reparo).HasColumnName("enviar_a_reparo");
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
