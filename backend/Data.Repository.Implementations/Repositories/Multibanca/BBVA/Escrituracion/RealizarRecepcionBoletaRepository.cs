using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA.Escrituracion
{
    public class RealizarRecepcionBoletaRepository : MultibancaGenericRepository<realizar_recepcion_boleta_entity>, IRealizarRecepcionBoletaRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public RealizarRecepcionBoletaRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<realizar_recepcion_boleta_entity?> GetByExpediente(long idExpediente)
        {
            return await MultibancaDBContext.realizar_recepcion_boleta
                .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
                .OrderByDescending(q => q.id)
                .FirstOrDefaultAsync();
        }
    }
}
