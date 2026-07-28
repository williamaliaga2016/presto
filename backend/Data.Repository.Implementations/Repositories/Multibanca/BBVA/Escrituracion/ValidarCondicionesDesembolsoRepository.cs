using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA.Escrituracion
{
    public class ValidarCondicionesDesembolsoRepository : MultibancaGenericRepository<validar_condiciones_desembolso_entity>, IValidarCondicionesDesembolsoRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public ValidarCondicionesDesembolsoRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<validar_condiciones_desembolso_entity?> GetByExpediente(long idExpediente)
        {
            return await MultibancaDBContext.validar_condiciones_desembolso
                .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
                .OrderByDescending(q => q.id)
                .FirstOrDefaultAsync();
        }
    }
}
