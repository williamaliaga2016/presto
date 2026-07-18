using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ICorregirCartaResguardoRepository
        : IMultibancaGenericRepository<corregir_carta_resguardo_entity>, IDisposable
    {
        Task<corregir_carta_resguardo_entity?> GetByExpediente(long id_expediente);
    }
}
