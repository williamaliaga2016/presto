using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

namespace Multibanca.Application.Interfaces.Multibanca.BBVA.Escrituracion;

public interface IRealizarEntregaEpFirmadaApplication : IMultibancaGenericApplication<realizar_entrega_ep_firmada>
{
    Task<object?> GetByExpediente(long idExpediente);
    Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId);
}
