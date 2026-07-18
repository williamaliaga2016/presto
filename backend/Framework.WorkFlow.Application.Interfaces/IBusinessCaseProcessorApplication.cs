using Framework.WorkFlow.Common.DTO;

namespace Framework.WorkFlow.Application.Interfaces
{
    public interface IBusinessCaseProcessorApplication
    {
        Task<bool> ExistsWorkflowProcess(string workflowProcessId);

        Task<business_case_DTO?> GetCase(long caseId);

        Task<business_case_DTO?> GetCase(Guid workflowInstanceId);

        Task<long> GetIdCase(Guid workflowInstanceId);

        Task<List<business_case_DTO>> GetAllCases();

        Task<long> InsertCase(business_case_DTO newCase);

        Task SaveChanges(business_case_DTO caseItem);
    }
}