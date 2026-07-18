using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Microsoft.EntityFrameworkCore;
using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA;

public class CartaAprobacionBbvaRepository : ICartaAprobacionBbvaRepository
{
    private readonly MultibancaDBContext MultibancaDBContext;

    public CartaAprobacionBbvaRepository(MultibancaDBContext multibancaDBContext)
    {
        MultibancaDBContext = multibancaDBContext;
    }

    public async Task<carta_aprobacion_bbva?> GetByExpediente(long idExpediente)
    {
        return await MultibancaDBContext.CartaAprobacionBbva
            .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
            .OrderByDescending(q => q.version)
            .ThenByDescending(q => q.id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<carta_aprobacion_bbva>> GetHistoricoByExpediente(long idExpediente)
    {
        return await MultibancaDBContext.CartaAprobacionBbva
            .Where(q => q.id_expediente == idExpediente && q.row_status)
            .OrderByDescending(q => q.version)
            .ThenByDescending(q => q.id)
            .ToListAsync();
    }

    public async Task<carta_aprobacion_bbva> Crear(carta_aprobacion_bbva entity)
    {
        MultibancaDBContext.CartaAprobacionBbva.Add(entity);
        await MultibancaDBContext.SaveChangesAsync();
        return entity;
    }

    public async Task<carta_aprobacion_bbva> Actualizar(carta_aprobacion_bbva entity)
    {
        MultibancaDBContext.CartaAprobacionBbva.Update(entity);
        await MultibancaDBContext.SaveChangesAsync();
        return entity;
    }
}
