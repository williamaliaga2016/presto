using Data.Repository.Interfaces.Entities.Multibanca.RevisarDatosOperacion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.RevisarDatosOperacion
{
    public interface IRevisarDatosOperacionPropiedadRepository : IMultibancaGenericRepository<revisar_datos_operacion_propiedad_entity>, IDisposable
    {
        Task<revisar_datos_operacion_propiedad_entity> GetByExpediente(long id_expediente);

        Task<revisar_datos_operacion_propiedad_entity> GetByExpediente(long id_expediente, int id_revisar_datos_operacion);
    }
}
