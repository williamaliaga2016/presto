using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface ICorregirReparoInstPagoApplication
        : IMultibancaGenericApplication<corregir_reparo_inst_pago>
    {
        Task<corregir_reparo_inst_pago?> GetByExpediente(long id_expediente);

        Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        );
    }
}
