using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface IVerificarReparoEstudioTituloApplication : IMultibancaGenericApplication<verificar_reparo_estudio_titulo>
    {
        Task<verificar_reparo_estudio_titulo?> GetByExpediente(long id_expediente);
        Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id);
    }
}
