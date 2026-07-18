using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface IRegistrarFirmaBancoAcreedorCGApplication
        : IMultibancaGenericApplication<firma_banco_acreedor_cg>
    {
        Task<firma_banco_acreedor_cg?> GetByExpediente(long id_expediente);
        Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        );
    }
}
