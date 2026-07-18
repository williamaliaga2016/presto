using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRealizarRevisionPrevioFirmaBancoRepository : IMultibancaGenericRepository<realizar_revision_previo_firma_banco_entity>, IDisposable
    {
        Task<realizar_revision_previo_firma_banco_entity?> GetByExpediente(long id_expediente);
    }
}
