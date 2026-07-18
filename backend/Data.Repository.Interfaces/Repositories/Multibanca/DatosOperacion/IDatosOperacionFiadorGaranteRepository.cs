using Data.Repository.Interfaces.Entities.Multibanca.DatosOperacion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.DatosOperacion
{
    public interface IDatosOperacionFiadorGaranteRepository : IMultibancaGenericRepository<datos_operacion_fiador_garante_entity>, IDisposable
    {
        Task<List<datos_operacion_fiador_garante_entity>> GetByExpediente(long id_expediente);
    }
}
