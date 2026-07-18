using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRecepcionarMatrizRepository : IMultibancaGenericRepository<recepcionar_matriz_entity>, IDisposable
    {
        Task<recepcionar_matriz_entity?> GetByExpediente(long id_expediente);
    }
}
