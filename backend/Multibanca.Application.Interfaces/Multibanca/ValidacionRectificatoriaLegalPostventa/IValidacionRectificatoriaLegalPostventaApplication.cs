using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca.ValidacionRectificatoriaLegalPostventa;

namespace Multibanca.Application.Interfaces.Multibanca.ValidacionRectificatoriaLegal
{
    public interface IValidacionRectificatoriaLegalPostventaApplication : IMultibancaGenericApplication<validacion_rectificatoria_legal_postventa>
    {
        Task<validacion_rectificatoria_legal_postventa> GetByExpediente(long id_expediente);

        Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id);
    }
}
