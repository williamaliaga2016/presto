using Data.Repository.Interfaces.Entities.Multibanca.RevisarDatosOperacion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.RevisarDatosOperacion
{
    public interface IRevisarDatosOperacionFiadorGaranteRepository : IMultibancaGenericRepository<revisar_datos_operacion_fiador_garante_entity>, IDisposable
    {
        Task<List<revisar_datos_operacion_fiador_garante_entity>> GetByExpediente(long id_expediente);
    }
}
