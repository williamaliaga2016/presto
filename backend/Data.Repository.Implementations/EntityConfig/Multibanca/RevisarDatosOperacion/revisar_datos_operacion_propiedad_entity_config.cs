using Data.Repository.Interfaces.Entities.Multibanca.RevisarDatosOperacion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.RevisarDatosOperacion
{
    internal class revisar_datos_operacion_propiedad_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<revisar_datos_operacion_propiedad_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("revisar_datos_operacion_propiedad");

            entityTypeBuilder.HasKey(q => q.id_revisar_datos_operacion_propiedad);

            entityTypeBuilder.Property(q => q.id_revisar_datos_operacion_propiedad)
                .HasColumnName("id_revisar_datos_operacion_propiedad")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_revisar_datos_operacion)
                .HasColumnName("id_revisar_datos_operacion")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_expediente)
                .HasColumnName("id_expediente")
                .IsRequired();

            entityTypeBuilder.Property(q => q.tipo_propiedad).HasColumnName("tipo_propiedad").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.estado).HasColumnName("estado").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.tipo_venta).HasColumnName("tipo_venta").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.tipo_construccion).HasColumnName("tipo_construccion").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.tipo_direccion).HasColumnName("tipo_direccion").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.direccion).HasColumnName("direccion").HasMaxLength(250);
            entityTypeBuilder.Property(q => q.villa_condominio).HasColumnName("villa_condominio").HasMaxLength(150);
            entityTypeBuilder.Property(q => q.numero).HasColumnName("numero").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.numero_casa_habitantes).HasColumnName("numero_casa_habitantes").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.conjunto).HasColumnName("conjunto").HasMaxLength(100);
            entityTypeBuilder.Property(q => q.manzana).HasColumnName("manzana").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.lote).HasColumnName("lote").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.region).HasColumnName("region").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.comuna).HasColumnName("comuna").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.existe_rol_avaluo).HasColumnName("existe_rol_avaluo").HasMaxLength(50);
            entityTypeBuilder.Property(q => q.rol_avaluo_1).HasColumnName("rol_avaluo_1").HasMaxLength(100);
            entityTypeBuilder.Property(q => q.rol_avaluo_2).HasColumnName("rol_avaluo_2").HasMaxLength(100);
            entityTypeBuilder.Property(q => q.valor_avaluo_pesos).HasColumnName("valor_avaluo_pesos").HasColumnType("numeric(18,2)");
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
