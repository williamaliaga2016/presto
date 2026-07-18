using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class ActividadesEntityConfig
    {
        public static void SetEntityBuilder(EntityTypeBuilder<actividades_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("actividades");

            entityTypeBuilder.HasKey(q => q.id);
            entityTypeBuilder.Property(q => q.id).IsRequired();
            entityTypeBuilder.Property(q => q.id_expediente).IsRequired();
            entityTypeBuilder.Property(q => q.id_actividad).IsRequired().HasMaxLength(50);
            entityTypeBuilder.Property(q => q.id_rol).IsRequired();
            entityTypeBuilder.Property(q => q.status).IsRequired().HasMaxLength(50);
            entityTypeBuilder.Property(q => q.id_usuario).IsRequired();
            entityTypeBuilder.Property(q => q.descripcion).HasMaxLength(500);
            entityTypeBuilder.Property(q => q.fecha_asignacion).HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.fecha_inicio).HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.fecha_termino).HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.fecha_cancelacion).HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.fecha_actualizacion).HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.fecha_reingreso).HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.fecha_alta).HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.fecha_suspencion).HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.fecha_reactivacion).HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.activo).IsRequired();
        }
    }
}
