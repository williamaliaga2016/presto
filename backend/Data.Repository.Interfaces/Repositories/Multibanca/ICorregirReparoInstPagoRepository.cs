using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ICorregirReparoInstPagoRepository
        : IMultibancaGenericRepository<corregir_reparo_inst_pago_entity>, IDisposable
    {
        Task<corregir_reparo_inst_pago_entity?> GetByExpediente(long id_expediente);
    }
}
