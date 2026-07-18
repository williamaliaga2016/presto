using Framework.WorkFlow.Common.DTO;

namespace Framework.WorkFlow.Repository.Interface
{
    public interface ICaseActivityTrackingWorkflowRepository
    {
        Task<Guid> TrackActivity(
            business_case_DTO caseInstance,
            string? xmlCaseInstance,
            string? trackSource
        );
    }
}