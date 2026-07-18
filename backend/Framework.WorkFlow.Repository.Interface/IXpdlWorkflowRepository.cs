using Framework.WorkFlow.Common.DTO;

namespace Framework.WorkFlow.Repository.Interface
{
    public interface IXpdlWorkflowRepository
    {
        Task<int> ExistsXpdlWorkflowProcess(string workflowProcessId);

        Task<List<xpdl_transition_DTO>> GetXpdlTransitionsByCriteria(
            string activityId,
            int filter
        );

        Task<List<xpdl_activity_DTO>> GetXpdlActivities();

        Task<xpdl_activity_DTO?> GetActivityById(string activityId);

        Task InsertXpdlWorkflowProcess(xpdl_workflow_process_DTO flow);

        Task InsertXpdlParticipant(xpdl_participant_DTO participant);

        Task InsertXpdlTransition(xpdl_transition_DTO transition);
    }
}