using Data.Repository.Interfaces.Entities.Multibanca.RevisarDatosOperacion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.RevisarDatosOperacion
{
    public interface IRevisarDatosOperacionRepository : IMultibancaGenericRepository<revisar_datos_operacion_entity>, IDisposable
    {
        Task<revisar_datos_operacion_entity?> GetByExpediente(long id_expediente);
    }
}
