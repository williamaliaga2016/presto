using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRectificatoriaPostventaSolucionReparoRepository
        : IMultibancaGenericRepository<rectificatoria_postventa_solucion_reparo_entity>, IDisposable
    {
        Task<rectificatoria_postventa_solucion_reparo_entity?> GetByExpediente(long id_expediente);
    }
}
