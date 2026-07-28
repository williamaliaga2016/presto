using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA.Escrituracion
{
    public class RealizarDevolucionEPRepository : MultibancaGenericRepository<realizar_devolucion_ep_entity>, IRealizarDevolucionEPRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public RealizarDevolucionEPRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<realizar_devolucion_ep_entity?> GetByExpediente(long idExpediente)
        {
            return await MultibancaDBContext.realizar_devolucion_ep
                .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
                .OrderByDescending(q => q.id)
                .FirstOrDefaultAsync();
        }
    }
}
