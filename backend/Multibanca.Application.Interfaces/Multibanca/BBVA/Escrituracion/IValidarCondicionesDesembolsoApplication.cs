using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

namespace Multibanca.Application.Interfaces.Multibanca.BBVA.Escrituracion;

public interface IValidarCondicionesDesembolsoApplication : IMultibancaGenericApplication<validar_condiciones_desembolso>
{
    Task<object?> GetByExpediente(long idExpediente);
    Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId);
    Task<bool> Suspender(long idExpediente, int userId);
}
