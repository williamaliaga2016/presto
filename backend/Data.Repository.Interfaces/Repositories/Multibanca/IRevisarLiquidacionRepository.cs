using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRevisarLiquidacionRepository : IMultibancaGenericRepository<revisar_liquidacion_entity>, IDisposable
    {
        Task<revisar_liquidacion_entity?> GetByExpediente(long id_expediente);
    }
}
