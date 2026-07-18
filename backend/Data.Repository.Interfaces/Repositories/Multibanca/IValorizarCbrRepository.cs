using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IValorizarCbrRepository : IMultibancaGenericRepository<valorizar_cbr_entity>, IDisposable
    {
        Task<valorizar_cbr_entity?> GetByExpediente(long id_expediente);
    }
}
