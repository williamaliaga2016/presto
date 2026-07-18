using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface ICargarDocumentosConstructoraApplication : IMultibancaGenericApplication<cargar_documentos_constructora>
    {
        Task<cargar_documentos_constructora?> GetByExpediente(long idExpediente);
        Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int idUsuario, string idActividad);
    }
}
