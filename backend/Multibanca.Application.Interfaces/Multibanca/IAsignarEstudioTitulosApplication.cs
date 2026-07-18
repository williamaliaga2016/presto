using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface IAsignarEstudioTitulosApplication : IMultibancaGenericApplication<asignar_estudio_titulos>
    {
        Task<asignar_estudio_titulos?> GetByExpediente(long id_expediente);
        Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id);
    }
}

