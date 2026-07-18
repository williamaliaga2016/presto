using Framework.WorkFlow.Common.DTO;
using Npgsql;

namespace Framework.WorkFlow.Application.Interfaces
{
    public interface IActivityWorkflowApplication
    {
        Task UpdateCaseActivityStatus(
            string status,
            long caseId,
            string activityId,
            string? toActivity
        );

        Task<long> CreateCaseActivity(business_activity_DTO activity);

        Task CreateXpdlActivity(business_activity_DTO activity);

        Task<List<business_activity_DTO>> Initialize(
            long caseId,
            string workflowProcessId
        );

        Task<List<business_activity_DTO>> CreateActivities(
            long caseId,
            string fromActivity,
            string activityId
        );

        Task<business_activity_DTO?> GetActivity(string activityId);

        Task<business_activity_DTO?> GetActivity(long caseId, string activityId);

        Task<List<business_activity_DTO>> GetActivities(
            string criteria,
            List<NpgsqlParameter> parameters,
            string? orderBy
        );

        Task<List<business_activity_DTO>> GetActivities(long caseId);

        Task<List<business_activity_DTO>> LoadNextActivities(long caseId);

        Task<List<xpdl_activity_DTO>> LoadAllXpdlActivities();

        Task ReprocessingActivities(long caseId);
    }
}