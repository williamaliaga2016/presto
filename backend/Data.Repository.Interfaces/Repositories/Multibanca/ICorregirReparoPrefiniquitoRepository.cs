using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ICorregirReparoPrefiniquitoRepository
        : IMultibancaGenericRepository<corregir_reparo_prefiniquito_entity>, IDisposable
    {
        Task<corregir_reparo_prefiniquito_entity?> GetByExpediente(long id_expediente);
    }
}
