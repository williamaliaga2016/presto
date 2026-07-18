using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface IRectificatoriaPostventaSolucionReparoApplication
        : IMultibancaGenericApplication<rectificatoria_postventa_solucion_reparo>
    {
        Task<rectificatoria_postventa_solucion_reparo?> GetByExpediente(long id_expediente);

        Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        );
    }
}