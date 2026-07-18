using Framework.WorkFlow.Common.DTO;

namespace Framework.WorkFlow.Application.Interfaces
{
    public interface ICaseApplication
    {
        Task<List<xpdl_transition_DTO>> GetTransitions(string activityId);

        Task<business_case_DTO?> GetCase(long caseId);

        Task<business_case_DTO?> CreateCase(string workflowId, string name, string owner);

        Task<business_case_DTO?> DispatchActivity(
            long caseId,
            string activityId,
            string transitionId
        );

        Task<bool> SuspendCase(long caseId);

        Task<business_case_DTO?> ActivateCase(long caseId);

        Task<bool> CloseCase(long caseId);

        Task<bool> CancelCase(long caseId);

        Task<bool> CreateIncident(long caseId, string activityId);

        Task<business_case_DTO?> SolveIncident(long caseId, string activityId);

        Task<bool> UpdateCase(long caseId, string title, string owner);

        Task<bool> ReactivateCase(long caseId);

        Task<bool> ReprocessingActivity(
            long caseId,
            string activityIdA,
            string activityIdB
        );

        void ValidateStringParameter(string parameterName, string parameterValue);

        void ValidateNullParameter(string parameterName, object parameterValue);

        Task<bool> IsValidDateTime(string dateToValidate);
    }
}