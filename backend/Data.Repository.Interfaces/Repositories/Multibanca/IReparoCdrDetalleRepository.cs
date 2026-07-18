using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IReparoCdrDetalleRepository : IMultibancaGenericRepository<reparo_cdr_detalle_entity>, IDisposable
    {
        Task<reparo_cdr_detalle_entity> GetByExpediente(long id_expediente);
    }
}
