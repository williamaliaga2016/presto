using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IControlEscrituraRepository : IMultibancaGenericRepository<control_escritura_entity>, IDisposable
    {
        Task<control_escritura_entity?> GetByExpediente(long id_expediente);
    }
}
