using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ICorregirReparoFabricaRepository : IMultibancaGenericRepository<corregir_reparo_fabrica_entity>, IDisposable
    {
        Task<corregir_reparo_fabrica_entity?> GetByExpediente(long id_expediente);
    }
}
