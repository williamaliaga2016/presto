using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;

namespace Multibanca.Application.Interfaces.Multibanca.CargaOperacionBanco
{
    public interface ICargaOperacionBancoApplication : IMultibancaGenericApplication<carga_operacion_banco>
    {
        Task<carga_operacion_banco> GetByExpediente(long id_expediente);

        Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id);
    }
}
