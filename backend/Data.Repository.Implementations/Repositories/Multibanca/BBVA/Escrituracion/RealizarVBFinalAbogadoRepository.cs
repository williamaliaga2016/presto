using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository.Implementations.Repositories.Multibanca.BBVA.Escrituracion
{
    public class RealizarVBFinalAbogadoRepository : MultibancaGenericRepository<realizar_vb_final_abogado_entity>, IRealizarVBFinalAbogadoRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public RealizarVBFinalAbogadoRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<realizar_vb_final_abogado_entity?> GetByExpediente(long idExpediente)
        {
            return await MultibancaDBContext.realizar_vb_final_abogado
                .Where(q => q.id_expediente == idExpediente && q.is_active && q.row_status)
                .OrderByDescending(q => q.id)
                .FirstOrDefaultAsync();
        }
    }
}
