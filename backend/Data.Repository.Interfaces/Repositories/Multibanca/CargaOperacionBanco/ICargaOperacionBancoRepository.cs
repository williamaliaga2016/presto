using Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco;

namespace Data.Repository.Interfaces.Repositories.Multibanca.CargaOperacionBanco
{
    public interface ICargaOperacionBancoRepository : IMultibancaGenericRepository<carga_operacion_banco_entity>, IDisposable
    {
        Task<carga_operacion_banco_entity> GetByExpediente(long id_expediente);
    }
}
