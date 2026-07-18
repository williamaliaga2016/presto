using Data.Repository.Interfaces.Entities.Multibanca.DatosOperacion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.DatosOperacion
{
    public interface IDatosOperacionBancoAcreedorRepository : IMultibancaGenericRepository<datos_operacion_banco_acreedor_entity>, IDisposable
    {
        Task<datos_operacion_banco_acreedor_entity> GetByExpediente(long id_expediente);
    }
}
