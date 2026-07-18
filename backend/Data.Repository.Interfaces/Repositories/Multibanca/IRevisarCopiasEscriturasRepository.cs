using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRevisarCopiasEscriturasRepository : IMultibancaGenericRepository<revisar_copias_escrituras_entity>, IDisposable
    {
        Task<revisar_copias_escrituras_entity?> GetByExpediente(long id_expediente);
    }
}
