using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

namespace Multibanca.Application.Interfaces.Multibanca.BBVA.Escrituracion;

public interface IVoboGerenciaCohApplication : IMultibancaGenericApplication<vobo_gerencia_coh_bbva>
{
    Task<VoboGerenciaCohResponse> GetByExpediente(long idExpediente);
    Task<vobo_gerencia_coh_bbva> Guardar(vobo_gerencia_coh_bbva request, int userId);
    Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId);
}
