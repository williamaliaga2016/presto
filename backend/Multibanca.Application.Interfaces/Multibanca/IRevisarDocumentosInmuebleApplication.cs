using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface IRevisarDocumentosInmuebleApplication
        : IMultibancaGenericApplication<revisar_documentos_inmueble>
    {
        Task<object> GetByExpediente(long idExpediente);
        Task<object> GetControles();
        Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int idUsuario, string idActividad);
    }
}
