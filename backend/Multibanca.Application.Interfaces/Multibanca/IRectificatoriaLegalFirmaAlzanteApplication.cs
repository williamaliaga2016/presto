using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface IRectificatoriaLegalFirmaAlzanteApplication
        : IMultibancaGenericApplication<rectificatoria_legal_firma_alzante>
    {
        Task<rectificatoria_legal_firma_alzante?> GetByExpediente(long id_expediente);
        Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        );
    }
}
