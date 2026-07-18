using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ICierreCopiasNotariaRepository : IMultibancaGenericRepository<cierre_copias_notaria_entity>, IDisposable
    {
        Task<cierre_copias_notaria_entity?> GetByExpediente(long id_expediente);
    }
}
