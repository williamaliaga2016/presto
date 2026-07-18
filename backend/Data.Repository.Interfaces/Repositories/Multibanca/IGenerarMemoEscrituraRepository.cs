using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IGenerarMemoEscrituraRepository
        : IMultibancaGenericRepository<generar_memo_escritura_entity>, IDisposable
    {
        Task<generar_memo_escritura_entity?> GetByExpediente(long id_expediente);
    }
}
