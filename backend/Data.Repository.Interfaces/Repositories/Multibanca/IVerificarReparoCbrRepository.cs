using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IVerificarReparoCbrRepository : IMultibancaGenericRepository<verificar_reparo_cbr_entity>, IDisposable
    {
        Task<verificar_reparo_cbr_entity?> GetByExpediente(long id_expediente);
    }
}
