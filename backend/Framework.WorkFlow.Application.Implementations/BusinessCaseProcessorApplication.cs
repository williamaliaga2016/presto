using Framework.WorkFlow.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Framework.WorkFlow.Repository.Interface;

namespace Framework.WorkFlow.Application.Implementations
{
    public class BusinessCaseProcessorApplication : IBusinessCaseProcessorApplication
    {
        private readonly IBusinessCaseWorkflowRepository BusinessCaseWorkflowRepositoryProvider;
        private readonly IXpdlWorkflowRepository XpdlWorkflowRepositoryProvider;

        public BusinessCaseProcessorApplication(
            IBusinessCaseWorkflowRepository _businessCaseWorkflowRepository,
            IXpdlWorkflowRepository _xpdlWorkflowRepository
        )
        {
            BusinessCaseWorkflowRepositoryProvider = _businessCaseWorkflowRepository;
            XpdlWorkflowRepositoryProvider = _xpdlWorkflowRepository;
        }

        public async Task<bool> ExistsWorkflowProcess(string workflowProcessId)
        {
            if (string.IsNullOrWhiteSpace(workflowProcessId))
            {
                return false;
            }

            int total = await XpdlWorkflowRepositoryProvider.ExistsXpdlWorkflowProcess(
                workflowProcessId
            );

            return total > 0;
        }

        public async Task<business_case_DTO?> GetCase(long caseId)
        {
            if (caseId <= 0)
            {
                return null;
            }

            return await BusinessCaseWorkflowRepositoryProvider.GetCaseById(caseId);
        }

        public async Task<business_case_DTO?> GetCase(Guid workflowInstanceId)
        {
            if (workflowInstanceId == Guid.Empty)
            {
                return null;
            }

            List<business_case_DTO> cases =
                await BusinessCaseWorkflowRepositoryProvider.GetCasesByWorkflowInstanceId(
                    workflowInstanceId
                );

            return cases.FirstOrDefault();
        }

        public async Task<long> GetIdCase(Guid workflowInstanceId)
        {
            business_case_DTO? workflowCase = await GetCase(workflowInstanceId);

            return workflowCase != null ? workflowCase.case_id : 0;
        }

        public async Task<List<business_case_DTO>> GetAllCases()
        {
            List<business_case_DTO> cases =
                await BusinessCaseWorkflowRepositoryProvider.GetAllCases();

            return cases ?? new List<business_case_DTO>();
        }

        public async Task<long> InsertCase(business_case_DTO newCase)
        {
            if (newCase == null)
            {
                throw new Exception("El caso workflow es obligatorio.");
            }

            if (newCase.case_id > 0)
            {
                throw new Exception("El case_id ya se encuentra asignado.");
            }

            long caseId = await BusinessCaseWorkflowRepositoryProvider.InsertCase(newCase);

            if (caseId <= 0)
            {
                throw new Exception("No se pudo recuperar el caso workflow creado.");
            }

            newCase.case_id = caseId;

            return caseId;
        }

        public async Task SaveChanges(business_case_DTO caseItem)
        {
            if (caseItem == null)
            {
                throw new Exception("El caso workflow es obligatorio.");
            }

            bool update = false;

            if (caseItem.case_id > 0)
            {
                update = true;
            }
            else
            {
                business_case_DTO? existingCase = await GetCase(
                    caseItem.workflow_instance_id
                );

                if (existingCase != null)
                {
                    caseItem.case_id = existingCase.case_id;
                    caseItem.title = existingCase.title;
                    caseItem.state = existingCase.state;
                    caseItem.workflow_instance_id = existingCase.workflow_instance_id;
                    caseItem.workflow_process_id = existingCase.workflow_process_id;
                    caseItem.created_by = existingCase.created_by;
                    caseItem.created = existingCase.created;

                    update = true;
                }
            }

            if (update)
            {
                await BusinessCaseWorkflowRepositoryProvider.UpdateCase(caseItem);
            }
            else
            {
                long caseId = await BusinessCaseWorkflowRepositoryProvider.InsertCase(caseItem);
                caseItem.case_id = caseId;
            }
        }
    }
}