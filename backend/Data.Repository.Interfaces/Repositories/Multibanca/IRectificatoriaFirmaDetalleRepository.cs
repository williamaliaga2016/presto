using Data.Repository.Interfaces.Entities.Multibanca;
using Multibanca.Domain.Models.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRectificatoriaFirmaDetalleRepository
        : IMultibancaGenericRepository<rectificatoria_firma_detalle_entity>, IDisposable
    {

    }
}