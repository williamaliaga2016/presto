using Data.Repository.Interfaces.Entities.Multibanca.RevisarDatosOperacion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.RevisarDatosOperacion
{
    internal class revisar_datos_operacion_credito_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<revisar_datos_operacion_credito_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("revisar_datos_operacion_credito");

            entityTypeBuilder.HasKey(q => q.id_revisar_datos_operacion_credito);

            entityTypeBuilder.Property(q => q.id_revisar_datos_operacion_credito)
                .HasColumnName("id_revisar_datos_operacion_credito")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_revisar_datos_operacion)
                .HasColumnName("id_revisar_datos_operacion")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_expediente)
                .HasColumnName("id_expediente")
                .IsRequired();

            entityTypeBuilder.Property(q => q.santiago).HasColumnName("santiago");
            entityTypeBuilder.Property(q => q.regiones).HasColumnName("regiones");
            entityTypeBuilder.Property(q => q.tipo_operacion).HasColumnName("tipo_operacion").HasMaxLength(250);
            entityTypeBuilder.Property(q => q.fines_generales).HasColumnName("fines_generales");
            entityTypeBuilder.Property(q => q.nombre_proyecto).HasColumnName("nombre_proyecto").HasMaxLength(250);
            entityTypeBuilder.Property(q => q.credito_segunda_vivienda).HasColumnName("credito_segunda_vivienda");
            entityTypeBuilder.Property(q => q.inmobiliaria).HasColumnName("inmobiliaria").HasMaxLength(250);
            entityTypeBuilder.Property(q => q.vivienda_social).HasColumnName("vivienda_social");
            entityTypeBuilder.Property(q => q.dfl2).HasColumnName("dfl2");
            entityTypeBuilder.Property(q => q.propietario_0_1_dfl2).HasColumnName("propietario_0_1_dfl2");
            entityTypeBuilder.Property(q => q.recepcion_final_mayor_2).HasColumnName("recepcion_final_mayor_2");
            entityTypeBuilder.Property(q => q.porcentaje_impuesto).HasColumnName("porcentaje_impuesto").HasColumnType("numeric(5,2)");
            entityTypeBuilder.Property(q => q.monto_credito_afecto).HasColumnName("monto_credito_afecto").HasColumnType("numeric(18,2)");
            entityTypeBuilder.Property(q => q.impuesto_a_pagar).HasColumnName("impuesto_a_pagar").HasColumnType("numeric(18,2)");
            entityTypeBuilder.Property(q => q.observaciones).HasColumnName("observaciones").HasMaxLength(2000);

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
