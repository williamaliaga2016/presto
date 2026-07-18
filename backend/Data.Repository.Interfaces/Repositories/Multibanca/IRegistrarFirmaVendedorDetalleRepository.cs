using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRegistrarFirmaVendedorDetalleRepository : IMultibancaGenericRepository<firma_vendedor_detalle_entity>, IDisposable
    {
        Task<List<firma_vendedor_detalle_entity>> GetByIdFirmaVendedor(int id_firma_vendedor);
        Task<List<firma_vendedor_detalle_entity>> GetDefaultFromExpediente(long id_expediente);
    }
}
