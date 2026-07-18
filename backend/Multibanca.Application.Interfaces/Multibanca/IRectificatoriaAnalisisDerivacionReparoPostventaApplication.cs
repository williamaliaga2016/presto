using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface IRectificatoriaAnalisisDerivacionReparoPostventaApplication : IMultibancaGenericApplication<rectificatoria_analisis_derivacion_reparo_postventa>
    {
        Task<rectificatoria_analisis_derivacion_reparo_postventa?> GetByExpediente(long id_expediente);
        Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id);
    }
}
