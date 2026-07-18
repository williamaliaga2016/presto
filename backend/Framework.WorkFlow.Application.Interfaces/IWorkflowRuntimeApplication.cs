using Framework.WorkFlow.Common.DTO;

namespace Framework.WorkFlow.Application.Interfaces
{
    public interface IWorkflowRuntimeApplication
    {
        Task<List<xpdl_transition_DTO>> GetTransitions(string activityId);

        Task<business_case_DTO?> GetCase(long caseId);

        Task<long> CreateCase(string workflowProcessId, string nameOrTitle, string owner);

        Task<business_case_DTO?> AssignCase(long caseId);

        Task<bool> ProcessCase(long caseId, string activityId, string? transitionId);

        Task<bool> SuspendCase(long caseId);

        Task<bool> ReassignCase(long caseId);

        Task<bool> CompleteCase(long caseId);

        Task<bool> CancelCase(long caseId);

        Task<bool> IncidentCase(long caseId, string activityId);

        Task<business_case_DTO?> ReprocessingCase(long caseId, string activityId);

        Task<bool> ReprocessingActivities(long caseId, string activityIdA, string activityIdB);

        Task<bool> UpdateCase(long caseId, string title, string owner);
    }
}