using Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco;

namespace Data.Repository.Interfaces.Repositories.Multibanca.CargaOperacionBanco
{
    public interface ICargaOperacionBancoDatosComercialRepository : IMultibancaGenericRepository<carga_operacion_banco_datos_comercial_entity>, IDisposable
    {
        Task<carga_operacion_banco_datos_comercial_entity> GetByExpediente(long id_expediente);
    }
}
