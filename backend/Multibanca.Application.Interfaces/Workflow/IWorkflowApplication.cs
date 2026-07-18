using Framework.WorkFlow.Common.DTO;

namespace Multibanca.Application.Interfaces.Workflow
{
    public interface IWorkflowApplication
    {
        Task<List<AssignActivityDTO>> AvanzarActividad(string actionId, FolioDTO oFolio, int id_usuario);
        Task<AssignActivityDTO> AvanzarActividad_UsuarioDefault(string actionId, FolioDTO oFolio, int id_usuario, int rolIdDefault, int usuarioIdDefault);
        Task<FolioDTO> CreateCase(string title, string workFlowId, string performerId, int userId);
        Task<bool> CancelCase(long caseId);
        Task<FolioDTO> CapturarDatosFolio(long id_expediente, string ActivityID);
        Task<List<xpdl_transition_DTO>> GetTransitions(string ActivityId);
        Task<bool> ExisteActividadFolio(long idExpediente, string idActividad);
    }
}
