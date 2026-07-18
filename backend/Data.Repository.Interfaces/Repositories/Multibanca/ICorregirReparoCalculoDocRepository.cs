using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ICorregirReparoCalculoDocRepository
        : IMultibancaGenericRepository<corregir_reparo_calculo_doc_entity>, IDisposable
    {
        Task<corregir_reparo_calculo_doc_entity?> GetByExpediente(long id_expediente);
    }
}
