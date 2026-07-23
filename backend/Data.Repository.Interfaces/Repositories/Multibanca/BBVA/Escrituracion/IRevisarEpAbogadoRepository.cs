using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA;

public interface IRevisarEpAbogadoRepository : IMultibancaGenericRepository<revisar_ep_abogado_entity>
{
    Task<revisar_ep_abogado_entity?> GetByExpediente(long idExpediente);
    Task<firmar_escritura_cliente_entity?> GetDatosHerencia(long idExpediente);
    Task<string?> GetEstadoCartaAprobacion(long idExpediente);
}
