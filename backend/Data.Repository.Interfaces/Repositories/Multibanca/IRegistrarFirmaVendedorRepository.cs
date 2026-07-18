using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRegistrarFirmaVendedorRepository : IMultibancaGenericRepository<firma_vendedor_entity>, IDisposable
    {
        Task<firma_vendedor_entity?> GetByExpedienteActividad(long id_expediente);
    }
}
