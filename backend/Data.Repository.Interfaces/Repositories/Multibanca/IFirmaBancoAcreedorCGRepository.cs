using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IFirmaBancoAcreedorCGRepository
        : IMultibancaGenericRepository<firma_banco_acreedor_cg_entity>,
          IDisposable
    {
        Task<firma_banco_acreedor_cg_entity?> GetByExpediente(long id_expediente);
    }
}
