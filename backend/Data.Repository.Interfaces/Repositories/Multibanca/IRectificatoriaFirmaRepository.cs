using Data.Repository.Interfaces.Entities.Multibanca;
using Multibanca.Domain.Models.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRectificatoriaFirmaRepository
        : IMultibancaGenericRepository<rectificatoria_firma_entity>, IDisposable
    {
        Task<rectificatoria_firma_entity?> GetByExpediente(long id_expediente);
        Task<List<rectificatoria_firma_detalle_entity>> GetRectificatoriaDetByExpediente(
            long id_expediente
        );
    }
}