using Data.Repository.Interfaces.Entities.Multibanca.RevisarDatosOperacion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.RevisarDatosOperacion
{
    public interface IRevisarDatosOperacionVendedorRepository : IMultibancaGenericRepository<revisar_datos_operacion_vendedor_entity>, IDisposable
    {
        Task<IList<revisar_datos_operacion_vendedor_entity>> GetListByExpediente(long id_expediente);
    }
}
