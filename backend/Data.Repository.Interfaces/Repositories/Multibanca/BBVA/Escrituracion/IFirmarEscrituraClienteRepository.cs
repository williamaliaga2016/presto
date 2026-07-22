using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA;

public interface IFirmarEscrituraClienteRepository : IMultibancaGenericRepository<firmar_escritura_cliente_entity>
{
    Task<firmar_escritura_cliente_entity?> GetByExpediente(long idExpediente);
    Task<object?> GetDatosNotariaHerencia(long idExpediente);
}
