using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface IAprobacionComercialLegalCdRApplication : IMultibancaGenericApplication<aprobacion_comercial_legal_cdr>
    {
        Task<aprobacion_comercial_legal_cdr?> GetByExpediente(long id_expediente);
        Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id);
    }
}
