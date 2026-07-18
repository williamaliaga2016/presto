using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface ICorregirNotariaReparoAbogadosApplication
        : IMultibancaGenericApplication<corregir_notaria_reparo_abogados>
    {
        Task<corregir_notaria_reparo_abogados?> GetByExpediente(long id_expediente);

        Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        );
    }
}
