using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

namespace Multibanca.Application.Interfaces.Multibanca.BBVA.Escrituracion;

public interface IExcepcionDesembolsoApplication : IMultibancaGenericApplication<excepcion_desembolso_bbva>
{
    Task<ExcepcionDesembolsoResponse> GetByExpediente(long idExpediente);
    Task<excepcion_desembolso_bbva> Guardar(excepcion_desembolso_bbva request, int userId);
    Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId);
}
