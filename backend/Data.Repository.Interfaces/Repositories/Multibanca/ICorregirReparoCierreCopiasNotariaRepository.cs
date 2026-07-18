using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ICorregirReparoCierreCopiasNotariaRepository
        : IMultibancaGenericRepository<corregir_reparo_cierre_copias_notaria_entity>, IDisposable
    {
        Task<corregir_reparo_cierre_copias_notaria_entity?> GetByExpediente(long id_expediente);
    }
}
