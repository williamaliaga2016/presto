using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IGenerarPreFiniquitoRepository:IMultibancaGenericRepository<generar_prefiniquito_entity>,IDisposable
    {
        Task<generar_prefiniquito_entity?> GetByExpediente(long id_expediente);
    }
}
