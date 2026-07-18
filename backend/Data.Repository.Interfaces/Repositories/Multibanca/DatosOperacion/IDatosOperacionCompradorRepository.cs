using Data.Repository.Interfaces.Entities.Multibanca.DatosOperacion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.DatosOperacion
{
    public interface IDatosOperacionCompradorRepository : IMultibancaGenericRepository<datos_operacion_comprador_entity>, IDisposable
    {
        Task<List<datos_operacion_comprador_entity>> GetByExpediente(long id_expediente);
    }
}
