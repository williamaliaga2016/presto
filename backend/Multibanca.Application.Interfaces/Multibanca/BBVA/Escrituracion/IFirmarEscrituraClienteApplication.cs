using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

namespace Multibanca.Application.Interfaces.Multibanca.BBVA.Escrituracion;

public interface IFirmarEscrituraClienteApplication : IMultibancaGenericApplication<firmar_escritura_cliente_bbva>
{
    Task<firmar_escritura_cliente_bbva?> GetByExpediente(long idExpediente);
    Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId);
    Task<object> GetControles(long idExpediente);
}
