using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ICorregirReparoTasacionRepository
        : IMultibancaGenericRepository<corregir_reparo_tasacion_entity>, IDisposable
    {
        Task<corregir_reparo_tasacion_entity?> GetByExpediente(long id_expediente);
    }
}
