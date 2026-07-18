using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRegistrarFirmaCompradorRepository : IMultibancaGenericRepository<firma_comprador_entity>, IDisposable
    {
        Task<firma_comprador_entity?> GetByExpedienteActividad(long id_expediente);
    }
}
