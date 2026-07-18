using Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco;

namespace Data.Repository.Interfaces.Repositories.Multibanca.CargaOperacionBanco
{
    public interface ICargaOperacionBancoDatosOperacionRepository : IMultibancaGenericRepository<carga_operacion_banco_datos_operacion_entity>, IDisposable
    {
        Task<carga_operacion_banco_datos_operacion_entity> GetByExpediente(long id_expediente);
    }
}
