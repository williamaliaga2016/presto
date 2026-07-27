using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

namespace Multibanca.Application.Interfaces.Multibanca.BBVA.Escrituracion;

public interface IRevisionMarcacionCoberturaApplication : IMultibancaGenericApplication<revision_marcacion_cobertura_bbva>
{
    Task<RevisionMarcacionCoberturaResponse> GetByExpediente(long idExpediente);
    Task<object> GetControles();
    Task<revision_marcacion_cobertura_bbva> Guardar(revision_marcacion_cobertura_bbva request, int userId);
    Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId);
}
