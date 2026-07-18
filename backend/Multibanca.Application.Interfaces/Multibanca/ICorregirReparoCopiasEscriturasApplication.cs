using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface ICorregirReparoCopiasEscriturasApplication
        : IMultibancaGenericApplication<corregir_reparo_copias_escrituras>
    {
        Task<corregir_reparo_copias_escrituras?> GetByExpediente(long id_expediente);

        Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        );
    }
}
