using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA.Escrituracion;

public class TradicionesConocidasRepository : ITradicionesConocidasRepository
{
    private readonly MultibancaDBContext _dbContext;

    public TradicionesConocidasRepository(MultibancaDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<tradiciones_conocidas_entity?> GetByCodigoProyecto(string codigoProyecto)
    {
        return await _dbContext.Set<tradiciones_conocidas_entity>()
            .AsNoTracking()
            .Where(x => x.codigo_proyecto == codigoProyecto && x.is_active && x.row_status)
            .OrderByDescending(x => x.id)
            .FirstOrDefaultAsync();
    }
}
