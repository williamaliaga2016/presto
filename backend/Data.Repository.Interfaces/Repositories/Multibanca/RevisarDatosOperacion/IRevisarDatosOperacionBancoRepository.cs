using Data.Repository.Interfaces.Entities.Multibanca.RevisarDatosOperacion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.RevisarDatosOperacion
{
    public interface IRevisarDatosOperacionBancoRepository: IMultibancaGenericRepository<revisar_datos_operacion_banco_entity>, IDisposable
    {
        Task<revisar_datos_operacion_banco_entity> GetByExpediente(long id_expediente);
    }
}
