using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRectificatoriaAnalisisDerivacionReparoPostventaRepository : IMultibancaGenericRepository<rectificatoria_analisis_derivacion_reparo_postventa_entity>, IDisposable
    {
        Task<rectificatoria_analisis_derivacion_reparo_postventa_entity?> GetByExpediente(long id_expediente);
    }
}
