using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IReparoEstudioTitulosDetalleRepository : IMultibancaGenericRepository<reparo_estudio_titulos_detalle_entity>, IDisposable
    {
        Task<reparo_estudio_titulos_detalle_entity> GetByExpediente(long id_expediente);
    }
}
