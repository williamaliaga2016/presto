using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ICorregirReparoVisadoRepository
        : IMultibancaGenericRepository<corregir_reparo_visado_entity>, IDisposable
    {
        Task<corregir_reparo_visado_entity?> GetByExpediente(long id_expediente);
    }
}