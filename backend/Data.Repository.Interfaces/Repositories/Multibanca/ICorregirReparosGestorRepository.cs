using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ICorregirReparosGestorRepository : IMultibancaGenericRepository<corregir_reparos_gestor_entity>, IDisposable
    {
        Task<corregir_reparos_gestor_entity?> GetByExpediente(long id_expediente);
    }
}
