using Data.Repository.Interfaces.Entities.Multibanca.DatosOperacion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.DatosOperacion
{
    public interface IDatosOperacionRepository : IMultibancaGenericRepository<datos_operacion_entity>, IDisposable
    {
        Task<datos_operacion_entity> GetByExpediente(long id_expediente);
    }
}
