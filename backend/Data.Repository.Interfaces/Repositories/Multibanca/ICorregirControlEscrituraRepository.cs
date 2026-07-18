using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ICorregirControlEscrituraRepository
        : IMultibancaGenericRepository<corregir_control_escritura_entity>, IDisposable
    {
        Task<corregir_control_escritura_entity?> GetByExpediente(long id_expediente);
    }
}
