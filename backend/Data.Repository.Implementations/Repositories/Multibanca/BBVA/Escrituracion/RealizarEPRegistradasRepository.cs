using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA.Escrituracion
{
    public class RealizarEPRegistradasRepository : MultibancaGenericRepository<realizar_ep_registradas_entity>, IRealizarEPRegistradasRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public RealizarEPRegistradasRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<realizar_ep_registradas_entity?> GetByExpediente(long idExpediente)
        {
            return await MultibancaDBContext.realizar_ep_registradas
                .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
                .OrderByDescending(q => q.id)
                .FirstOrDefaultAsync();
        }
    }
}
