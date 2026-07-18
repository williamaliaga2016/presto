using Framework.WorkFlow.Common.DTO;
using Npgsql;

namespace Framework.WorkFlow.Repository.Interface
{
    public interface IActivityWorkflowRepository
    {
        Task<List<business_activity_DTO>> GetActivitiesByCriteria(
            string criteria,
            List<NpgsqlParameter> parameters,
            string? orderBy
        );

        Task<List<business_activity_DTO>> GetActivitiesByCaseId(long caseId);

        Task InsertXpdlActivity(business_activity_DTO activity);

        Task<long> InsertCaseActivity(business_activity_DTO activity);

        Task UpdateCaseActivityStatus(
            string status,
            long caseId,
            string activityId,
            string? toActivity
        );
    }
}