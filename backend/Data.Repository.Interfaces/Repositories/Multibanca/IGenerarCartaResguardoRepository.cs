using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IGenerarCartaResguardoRepository : IMultibancaGenericRepository<generar_carta_resguardo_entity>, IDisposable
    {
        Task<generar_carta_resguardo_entity?> GetByExpediente(long id_expediente);
    }
}
