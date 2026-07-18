using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface ICorregirReparoTasacionApplication
        : IMultibancaGenericApplication<corregir_reparo_tasacion>
    {
        Task<corregir_reparo_tasacion?> GetByExpediente(long id_expediente);

        Task MarcarReparoTasacionSubsanado(long id_expediente, int usuario_id);

        Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        );
    }
}
