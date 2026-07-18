using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca.DatosOperacion;

namespace Multibanca.Application.Interfaces.Multibanca.DatosOperacion
{
    public interface IDatosOperacionApplication : IMultibancaGenericApplication<datos_operacion>
    {
        Task<datos_operacion> GetByExpediente(long id_expediente);

        Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id);
    }
}
