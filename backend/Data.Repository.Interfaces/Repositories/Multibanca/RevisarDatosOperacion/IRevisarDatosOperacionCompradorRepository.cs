using Data.Repository.Interfaces.Entities.Multibanca.RevisarDatosOperacion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.RevisarDatosOperacion
{
    public interface IRevisarDatosOperacionCompradorRepository : IMultibancaGenericRepository<revisar_datos_operacion_comprador_entity>, IDisposable
    {
        Task<IList<revisar_datos_operacion_comprador_entity>> GetListByExpediente(long id_expediente);
    }
}
