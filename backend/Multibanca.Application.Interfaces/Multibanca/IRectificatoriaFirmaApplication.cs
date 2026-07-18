using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface IRectificatoriaFirmaApplication
        : IMultibancaGenericApplication<rectificatoria_firma>
    {
        Task<rectificatoria_firma?> GetByExpediente(long id_expediente);
        Task<List<rectificatoria_firma_detalle>>GetRectificatoriaDetByExpediente(long id_expediente,string rol_comparecencia);

        Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        );
    }
}