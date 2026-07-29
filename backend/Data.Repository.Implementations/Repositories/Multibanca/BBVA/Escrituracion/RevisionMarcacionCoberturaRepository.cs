using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA.Escrituracion
{
    public class RevisionMarcacionCoberturaRepository
        : MultibancaGenericRepository<revision_marcacion_cobertura_entity>, IRevisionMarcacionCoberturaRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public RevisionMarcacionCoberturaRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<revision_marcacion_cobertura_entity?> GetByExpediente(long idExpediente)
        {
            return await MultibancaDBContext.revision_marcacion_cobertura
                .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
                .OrderByDescending(q => q.id)
                .FirstOrDefaultAsync();
        }
    }
}
