using Multibanca.DTO.Common;
using Multibanca.DTO.Utilidades;

namespace Multibanca.Application.Interface.Utilidades
{
    public interface IUtilidadesApplication
    {
        Task<List<ControlBaseDTO>> GetActivities(long idExpediente);
        Task<int> Validate_RequestNumber(long idExpediente);
        Task<List<ControlBaseDTO>> GetActivitiesUsers(long idExpediente);
        Task<List<ControlBaseDTO>> GetUsersActivities(long idExpediente, long idActividad);
        Task<bool> Save_CancelActivity(long idExpediente);
        Task<bool> Save_DeleteReassign(UtilidadesDTO util, int idUsuario);
        Task<ReassignmentWithdrawalDTO> GetMessageUtility(int opcion, long idExpediente, int idUsuario, int? idUsuarioReasignado, long? idActividad);
    }
}
