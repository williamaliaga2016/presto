using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA.Escrituracion
{
    public class FirmarRepLegalRepository : MultibancaGenericRepository<firmar_rep_legal_entity>, IFirmarRepLegalRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public FirmarRepLegalRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<firmar_rep_legal_entity?> GetByExpediente(long idExpediente)
        {
            return await MultibancaDBContext.firmar_rep_legal
                .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
                .OrderByDescending(q => q.id)
                .FirstOrDefaultAsync();
        }
    }
}
