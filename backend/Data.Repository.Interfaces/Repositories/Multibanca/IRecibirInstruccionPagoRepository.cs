using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRecibirInstruccionPagoRepository : IMultibancaGenericRepository<recibir_instruccion_pago_entity>, IDisposable
    {
        Task<recibir_instruccion_pago_entity?> GetByExpediente(long id_expediente);
    }
}
