using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ICorregirReparoCopiasEscriturasRepository
        : IMultibancaGenericRepository<corregir_reparo_copias_escrituras_entity>, IDisposable
    {
        Task<corregir_reparo_copias_escrituras_entity?> GetByExpediente(long id_expediente);
    }
}
