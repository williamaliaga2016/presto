using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRevisarDesembolsoRepository : IMultibancaGenericRepository<revisar_desembolso_entity>,IDisposable
    {
        Task<revisar_desembolso_entity?> GetByExpediente(long id_expediente);
    }
}
