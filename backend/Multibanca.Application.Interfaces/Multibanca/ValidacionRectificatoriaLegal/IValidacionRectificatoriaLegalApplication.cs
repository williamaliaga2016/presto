using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca.ValidacionRectificatoriaLegal;

namespace Multibanca.Application.Interfaces.Multibanca.ValidacionRectificatoriaLegal
{
    public interface IValidacionRectificatoriaLegalApplication : IMultibancaGenericApplication<validacion_rectificatoria_legal>
    {
        Task<validacion_rectificatoria_legal> GetByExpediente(long id_expediente);

        Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id);
    }
}
