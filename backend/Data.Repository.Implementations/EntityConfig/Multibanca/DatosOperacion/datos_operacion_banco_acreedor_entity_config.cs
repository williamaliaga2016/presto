using Data.Repository.Interfaces.Entities.Multibanca.DatosOperacion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.DatosOperacion
{
    internal class datos_operacion_banco_acreedor_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<datos_operacion_banco_acreedor_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("datos_operacion_banco_acreedor");

            entityTypeBuilder.HasKey(q => q.id_datos_operacion_banco_acreedor);

            entityTypeBuilder.Property(q => q.id_datos_operacion_banco_acreedor)
                .HasColumnName("id_datos_operacion_banco_acreedor")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_datos_operacion)
                .HasColumnName("id_datos_operacion")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_expediente)
                .HasColumnName("id_expediente")
                .IsRequired();


            entityTypeBuilder.Property(q => q.cuenta_carta_resguardo).HasColumnName("cuenta_carta_resguardo");
            entityTypeBuilder.Property(q => q.institucion).HasColumnName("institucion").HasMaxLength(250);
            entityTypeBuilder.Property(q => q.rut_banco_acreedor).HasColumnName("rut_banco_acreedor").HasMaxLength(20);


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

            entityTypeBuilder.HasIndex(q => q.id_datos_operacion);
        }
    }
}
