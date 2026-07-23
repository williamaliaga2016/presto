using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.BBVA.Escrituracion;

internal class realizar_recepcion_boleta_entity_config
{
    public static void SetEntityBuilder(EntityTypeBuilder<realizar_recepcion_boleta_entity> entityTypeBuilder)
    {
        entityTypeBuilder.ToTable("realizar_recepcion_boleta");
        entityTypeBuilder.HasKey(q => q.id);
        entityTypeBuilder.Property(q => q.id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        entityTypeBuilder.Property(q => q.id_expediente).HasColumnName("id_expediente").IsRequired();
        entityTypeBuilder.Property(q => q.id_actividad).HasColumnName("id_actividad").HasMaxLength(100);

        // Campos VUR
        entityTypeBuilder.Property(q => q.numero_boleta).HasColumnName("numero_boleta").HasMaxLength(100);
        entityTypeBuilder.Property(q => q.fecha_boleta).HasColumnName("fecha_boleta").HasColumnType("date");
        entityTypeBuilder.Property(q => q.numero_matricula).HasColumnName("numero_matricula").HasMaxLength(200);

        // Campos transaccionales
        entityTypeBuilder.Property(q => q.tipo_boleta).HasColumnName("tipo_boleta").HasMaxLength(100);
        entityTypeBuilder.Property(q => q.boleta_en_poder_de).HasColumnName("boleta_en_poder_de").HasMaxLength(200);
        entityTypeBuilder.Property(q => q.codigo_zona).HasColumnName("codigo_zona").HasMaxLength(50);
        entityTypeBuilder.Property(q => q.oficina_registro).HasColumnName("oficina_registro").HasMaxLength(200);
        entityTypeBuilder.Property(q => q.boleta_recibida).HasColumnName("boleta_recibida").IsRequired();
        entityTypeBuilder.Property(q => q.aplica_excepcion).HasColumnName("aplica_excepcion").HasMaxLength(2);

        // Control VUR
        entityTypeBuilder.Property(q => q.vur_ejecutado).HasColumnName("vur_ejecutado").IsRequired();
        entityTypeBuilder.Property(q => q.vur_exitoso).HasColumnName("vur_exitoso").IsRequired();
        entityTypeBuilder.Property(q => q.vur_intentos).HasColumnName("vur_intentos").IsRequired();

        entityTypeBuilder.Property(q => q.observaciones).HasColumnName("observaciones").HasMaxLength(500);

        // Auditoría
        entityTypeBuilder.Property(q => q.is_active).HasColumnName("is_active").IsRequired();
        entityTypeBuilder.Property(q => q.row_status).HasColumnName("row_status").IsRequired();
        entityTypeBuilder.Property(q => q.created_by).HasColumnName("created_by").IsRequired();
        entityTypeBuilder.Property(q => q.created_date).HasColumnName("created_date").IsRequired().HasColumnType("timestamp without time zone");
        entityTypeBuilder.Property(q => q.modified_by).HasColumnName("modified_by");
        entityTypeBuilder.Property(q => q.modified_date).HasColumnName("modified_date").HasColumnType("timestamp without time zone");

        // Índice UNIQUE parcial
        entityTypeBuilder.HasIndex(q => q.id_expediente)
            .HasDatabaseName("idx_realizar_recepcion_boleta_expediente")
            .IsUnique()
            .HasFilter("is_active = true AND row_status = true");
    }
}
