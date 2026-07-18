using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ICorregirReparoLiquidacionRepository
        : IMultibancaGenericRepository<corregir_reparo_liquidacion_entity>, IDisposable
    {
        Task<corregir_reparo_liquidacion_entity?> GetByExpediente(long id_expediente);
    }
}
