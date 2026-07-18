using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface IVerificarReparoDatosOperacionApplication : IMultibancaGenericApplication<verificar_reparo_datos_operacion>
    {
        Task<verificar_reparo_datos_operacion?> GetByExpediente(long id_expediente);
        Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id);
    }
}
