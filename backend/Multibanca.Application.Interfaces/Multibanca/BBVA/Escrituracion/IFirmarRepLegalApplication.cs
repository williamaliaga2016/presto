using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

namespace Multibanca.Application.Interfaces.Multibanca.BBVA.Escrituracion;

public interface IFirmarRepLegalApplication : IMultibancaGenericApplication<firmar_rep_legal>
{
    Task<firmar_rep_legal?> GetByExpediente(long idExpediente);
    Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId);
    Task<object> GetControles();
}
