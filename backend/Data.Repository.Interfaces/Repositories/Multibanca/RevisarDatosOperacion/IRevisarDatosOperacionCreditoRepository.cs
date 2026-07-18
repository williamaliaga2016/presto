using Data.Repository.Interfaces.Entities.Multibanca.RevisarDatosOperacion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.RevisarDatosOperacion
{
    public interface IRevisarDatosOperacionCreditoRepository : IMultibancaGenericRepository<revisar_datos_operacion_credito_entity>, IDisposable
    {
        Task<revisar_datos_operacion_credito_entity> GetByExpediente(long id_expediente);

        Task<revisar_datos_operacion_credito_entity> GetByExpediente(long id_expediente, int id_revisar_datos_operacion);
    }
}
