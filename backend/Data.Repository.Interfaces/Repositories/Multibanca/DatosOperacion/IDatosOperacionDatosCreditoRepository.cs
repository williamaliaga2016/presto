using Data.Repository.Interfaces.Entities.Multibanca.DatosOperacion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.DatosOperacion
{
    public interface IDatosOperacionDatosCreditoRepository : IMultibancaGenericRepository<datos_operacion_datos_credito_entity>, IDisposable
    {
        Task<datos_operacion_datos_credito_entity> GetByExpediente(long id_expediente);
    }
}
