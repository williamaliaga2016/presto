using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA.Escrituracion
{
    public class RealizarGestionComercialRepository : MultibancaGenericRepository<realizar_gestion_comercial_entity>, IRealizarGestionComercialRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public RealizarGestionComercialRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<realizar_gestion_comercial_entity?> GetByExpediente(long idExpediente)
        {
            return await MultibancaDBContext.realizar_gestion_comercial
                .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
                .OrderByDescending(q => q.id)
                .FirstOrDefaultAsync();
        }
    }
}
