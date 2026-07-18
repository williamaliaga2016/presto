using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ICorregirReparoCdrRepository
        : IMultibancaGenericRepository<corregir_reparo_cdr_entity>, IDisposable
    {
        Task<corregir_reparo_cdr_entity?> GetByExpediente(long id_expediente);
    }
}
