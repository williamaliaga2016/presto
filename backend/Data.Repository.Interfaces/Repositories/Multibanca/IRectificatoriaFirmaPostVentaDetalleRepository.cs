using Data.Repository.Interfaces.Entities.Multibanca;
using Multibanca.Domain.Models.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRectificatoriaFirmaPostVentaDetalleRepository
        : IMultibancaGenericRepository<rectificatoria_firma_post_venta_detalle_entity>, IDisposable
    {

    }
}