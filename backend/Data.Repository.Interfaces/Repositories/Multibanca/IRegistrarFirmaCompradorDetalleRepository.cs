using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRegistrarFirmaCompradorDetalleRepository : IMultibancaGenericRepository<firma_comprador_detalle_entity>, IDisposable
    {
        Task<List<firma_comprador_detalle_entity>> GetByIdFirmaComprador(int id_firma_comprador);
        Task<List<firma_comprador_detalle_entity>> GetDefaultFromExpediente(long id_expediente);
    }
}
