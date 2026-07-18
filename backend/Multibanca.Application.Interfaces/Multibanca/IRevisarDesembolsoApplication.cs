using Common.Application.Interfaces;
using Data.Repository.Interfaces.Entities.Multibanca;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface IRevisarDesembolsoApplication : IMultibancaGenericApplication<revisar_desembolso>
    {
        Task<revisar_desembolso?> GetByExpediente(long id_expediente);
        Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id);
    }
}
