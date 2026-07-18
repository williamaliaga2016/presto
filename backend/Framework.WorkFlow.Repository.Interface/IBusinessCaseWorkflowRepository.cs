using Framework.WorkFlow.Common.DTO;

namespace Framework.WorkFlow.Repository.Interface
{
    public interface IBusinessCaseWorkflowRepository
    {
        Task<List<business_case_DTO>> GetAllCases();

        Task<List<business_case_DTO>> GetCasesByWorkflowInstanceId(Guid workflowInstanceId);

        Task<business_case_DTO?> GetCaseById(long caseId);

        Task<long> InsertCase(business_case_DTO workflowCase);

        Task UpdateCase(business_case_DTO workflowCase);

        Task UpdateCaseByTitleAndOwner(long caseId, string title, string owner);
    }
}