using Data.Repository.Interfaces.Entities.Multibanca.BBVA;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.BBVA;

internal class firmar_escritura_cliente_entity_config
{
    public static void SetEntityBuilder(EntityTypeBuilder<firmar_escritura_cliente_entity> entityTypeBuilder)
    {
        entityTypeBuilder.ToTable("firmar_escritura_cliente");
        entityTypeBuilder.HasKey(q => q.id);
        entityTypeBuilder.Property(q => q.id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        entityTypeBuilder.Property(q => q.id_expediente)
            .HasColumnName("id_expediente")
            .IsRequired();

        entityTypeBuilder.Property(q => q.id_actividad)
            .HasColumnName("id_actividad")
            .HasMaxLength(100);

        // Bloque Información de Notaría
        entityTypeBuilder.Property(q => q.notaria)
            .HasColumnName("notaria")
            .HasMaxLength(150);
        
        entityTypeBuilder.Property(q => q.fecha_notaria)
            .HasColumnName("fecha_notaria")
            .HasColumnType("date");
        
        entityTypeBuilder.Property(q => q.numero_notaria)
            .HasColumnName("numero_notaria");
        
        entityTypeBuilder.Property(q => q.ciudad_notaria)
            .HasColumnName("ciudad_notaria")
            .HasMaxLength(100);

        // Bloque Formalización de Escritura
        entityTypeBuilder.Property(q => q.numero_escritura)
            .HasColumnName("numero_escritura")
            .HasMaxLength(20);
        
        entityTypeBuilder.Property(q => q.fecha_escritura)
            .HasColumnName("fecha_escritura")
            .HasColumnType("date");
        
        entityTypeBuilder.Property(q => q.representante_legal)
            .HasColumnName("representante_legal")
            .HasMaxLength(200);

        // Decisiones de Enrutamiento
        entityTypeBuilder.Property(q => q.requiere_escalamiento_comercial)
            .HasColumnName("requiere_escalamiento_comercial")
            .HasMaxLength(2);
        
        entityTypeBuilder.Property(q => q.tipologia)
            .HasColumnName("tipologia")
            .HasMaxLength(100);
        
        entityTypeBuilder.Property(q => q.requiere_causar)
            .HasColumnName("requiere_causar")
            .HasMaxLength(2);

        // Campos adicionales
        entityTypeBuilder.Property(q => q.observaciones)
            .HasColumnName("observaciones")
            .HasColumnType("text");
        
        entityTypeBuilder.Property(q => q.tipo_credito)
            .HasColumnName("tipo_credito")
            .HasMaxLength(100);

        // Auditoría
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
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        entityTypeBuilder.Property(q => q.modified_by)
            .HasColumnName("modified_by");

        entityTypeBuilder.Property(q => q.modified_date)
            .HasColumnName("modified_date")
            .HasColumnType("timestamp without time zone");

        // Índice UNIQUE parcial: un solo registro activo por expediente
        entityTypeBuilder.HasIndex(q => q.id_expediente)
            .HasDatabaseName("idx_fec_expediente")
            .IsUnique()
            .HasFilter("is_active = true AND row_status = true");
    }
}
