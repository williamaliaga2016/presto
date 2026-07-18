using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.DTO.Multibanca;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface IGenerarMemoEscrituraApplication
        : IMultibancaGenericApplication<generar_memo_escritura>
    {
        Task<generar_memo_escritura?> GetByExpediente(long id_expediente);
        Task<MemoEscrituraDataDTO> GetDatosMemo(long id_expediente);
        Task<expediente_digital> GenerarPdf(MemoEscrituraDTO request, int id_usuario);
        Task<List<expediente_digital>> ListarVersiones(long id_expediente);
        Task<List<AssignActivityDTO>> Avanzar(long id_expediente, int id_usuario, string id_actividad);
    }
}
