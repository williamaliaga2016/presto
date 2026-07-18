using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface IRectificatoriaLegalCierreCopiasApplication
        : IMultibancaGenericApplication<rectificatoria_legal_cierre_copias>
    {
        Task<rectificatoria_legal_cierre_copias?> GetByExpediente(long id_expediente);
        Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        );
    }
}
