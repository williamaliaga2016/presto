using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface IRectificatoriaLegalCartaResguardoApplication
        : IMultibancaGenericApplication<rectificatoria_legal_carta_resguardo>
    {
        Task<rectificatoria_legal_carta_resguardo?> GetByExpediente(long id_expediente);
        Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        );
    }
}
