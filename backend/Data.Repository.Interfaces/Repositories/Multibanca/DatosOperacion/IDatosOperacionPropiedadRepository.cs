using Data.Repository.Interfaces.Entities.Multibanca.DatosOperacion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.DatosOperacion
{
    public interface IDatosOperacionPropiedadRepository : IMultibancaGenericRepository<datos_operacion_propiedad_entity>, IDisposable
    {
        Task<datos_operacion_propiedad_entity> GetByExpediente(long id_expediente);
        Task<List<datos_operacion_propiedad_entity>> GetAllByExpediente(long id_expediente);
    }
}
