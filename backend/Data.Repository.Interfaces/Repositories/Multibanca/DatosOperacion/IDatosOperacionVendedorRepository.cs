using Data.Repository.Interfaces.Entities.Multibanca.DatosOperacion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.DatosOperacion
{
    public interface IDatosOperacionVendedorRepository : IMultibancaGenericRepository<datos_operacion_vendedor_entity>, IDisposable
    {
        Task<List<datos_operacion_vendedor_entity>> GetByExpediente(long id_expediente);
    }
}
