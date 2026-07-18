using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRecepcionCargaFabricaRepository : IMultibancaGenericRepository<recepcion_carga_fabrica_entity>, IDisposable
    {
        Task<recepcion_carga_fabrica_entity?> GetByExpediente(long id_expediente);
    }
}
