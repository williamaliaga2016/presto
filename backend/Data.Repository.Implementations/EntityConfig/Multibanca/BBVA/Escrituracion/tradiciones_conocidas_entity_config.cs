using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.BBVA.Escrituracion;

public class tradiciones_conocidas_entity_config : IEntityTypeConfiguration<tradiciones_conocidas_entity>
{
    public void Configure(EntityTypeBuilder<tradiciones_conocidas_entity> builder)
    {
        builder.ToTable("tradiciones_conocidas");
        builder.HasKey(e => e.id);
    }
}
