using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository.Implementations.Repositories.Multibanca
{
    public class CargarDocumentosConstructoraRepository : MultibancaGenericRepository<cargar_documentos_constructora_entity>, ICargarDocumentosConstructoraRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public CargarDocumentosConstructoraRepository(MultibancaDBContext multibancaDBContext) : base(multibancaDBContext)
        {
            MultibancaDBContext = multibancaDBContext;
        }

        public async Task<cargar_documentos_constructora_entity?> GetByExpediente(long idExpediente)
        {
            return await MultibancaDBContext.CargarDocumentosConstructora
                .Where(x => x.id_expediente == idExpediente && x.row_status)
                .OrderByDescending(x => x.id)
                .FirstOrDefaultAsync();
        }
    }
}
