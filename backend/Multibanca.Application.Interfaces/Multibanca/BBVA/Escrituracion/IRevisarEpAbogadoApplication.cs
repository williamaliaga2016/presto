using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

namespace Multibanca.Application.Interfaces.Multibanca.BBVA.Escrituracion;

public interface IRevisarEpAbogadoApplication : IMultibancaGenericApplication<revisar_ep_abogado_bbva>
{
    Task<revisar_ep_abogado_bbva?> GetByExpediente(long idExpediente);
    Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId);
    Task<object> GetControles(long idExpediente);
}
