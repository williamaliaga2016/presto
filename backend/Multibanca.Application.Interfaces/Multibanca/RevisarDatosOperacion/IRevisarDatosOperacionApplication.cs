using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca.RevisarDatosOperacion;

namespace Multibanca.Application.Interfaces.Multibanca.RevisarDatosOperacion
{
    public interface IRevisarDatosOperacionApplication : IMultibancaGenericApplication<revisar_datos_operacion>
    {
        Task<revisar_datos_operacion?> GetByExpediente(long id_expediente);

        Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id);
    }
}
