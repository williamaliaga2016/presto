using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA.Escrituracion
{
    public class RealizarEntregaEpFirmadaRepository : MultibancaGenericRepository<realizar_entrega_ep_firmada_entity>, IRealizarEntregaEpFirmadaRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public RealizarEntregaEpFirmadaRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<realizar_entrega_ep_firmada_entity?> GetByExpediente(long idExpediente)
        {
            return await MultibancaDBContext.realizar_entrega_ep_firmada
                .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
                .OrderByDescending(q => q.id)
                .FirstOrDefaultAsync();
        }
    }
}
