using Data.Repository.Interfaces.Entities.Multibanca;
using Multibanca.Domain.Models.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRectificatoriaFirmaPostVentaRepository
        : IMultibancaGenericRepository<rectificatoria_firma_post_venta_entity>, IDisposable
    {
        Task<rectificatoria_firma_post_venta_entity?> GetByExpediente(long id_expediente);
        Task<List<rectificatoria_firma_post_venta_detalle_entity>> GetRectificatoriaDetByExpediente(
            long id_expediente
        );
    }
}