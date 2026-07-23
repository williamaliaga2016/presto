using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

namespace Multibanca.Application.Interfaces.Multibanca.BBVA.Escrituracion;

public interface IRealizarEPRegistradasApplication : IMultibancaGenericApplication<realizar_ep_registradas>
{
    Task<object?> GetByExpediente(long idExpediente);
    Task<object> GetControles();
    Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId);
}
