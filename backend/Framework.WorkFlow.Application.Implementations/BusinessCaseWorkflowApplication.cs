using Framework.WorkFlow.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Framework.WorkFlow.Repository.Interface;

namespace Framework.WorkFlow.Application.Implementations
{
    //public class BusinessCaseWorkflowApplication : IBusinessCaseWorkflowApplication
    //{
    //    private readonly IBusinessCaseWorkflowRepository BusinessCaseWorkflowRepositoryProvider;
    //    private readonly IActivityWorkflowRepository ActivityWorkflowRepositoryProvider;

    //    public BusinessCaseWorkflowApplication(
    //        IBusinessCaseWorkflowRepository _businessCaseWorkflowRepository,
    //        IActivityWorkflowRepository _activityWorkflowRepository
    //    )
    //    {
    //        BusinessCaseWorkflowRepositoryProvider = _businessCaseWorkflowRepository;
    //        ActivityWorkflowRepositoryProvider = _activityWorkflowRepository;
    //    }

    //    public async Task<long> CreateCase(string workflowProcessId, string nameOrTitle, string owner)
    //    {
    //        if (string.IsNullOrWhiteSpace(workflowProcessId))
    //        {
    //            throw new Exception("El workflowProcessId es obligatorio.");
    //        }

    //        if (string.IsNullOrWhiteSpace(nameOrTitle))
    //        {
    //            throw new Exception("El nombre o título del caso es obligatorio.");
    //        }

    //        if (string.IsNullOrWhiteSpace(owner))
    //        {
    //            throw new Exception("El owner es obligatorio.");
    //        }

    //        business_case_DTO workflowCase = new business_case_DTO
    //        {
    //            workflow_process_id = workflowProcessId,
    //            title = nameOrTitle,
    //            created_by = owner,
    //            workflow_instance_id = Guid.NewGuid(),
    //            state = "New",
    //            created = DateTime.Now
    //        };

    //        long caseId = await BusinessCaseWorkflowRepositoryProvider.CreateCase(workflowCase);

    //        if (caseId <= 0)
    //        {
    //            throw new Exception("No se pudo crear el caso workflow.");
    //        }

    //        return caseId;
    //    }

    //    public async Task<bool> CompleteCase(long caseId)
    //    {
    //        business_case_DTO? workflowCase =
    //            await BusinessCaseWorkflowRepositoryProvider.GetCaseById(caseId);

    //        if (workflowCase == null)
    //        {
    //            return false;
    //        }

    //        workflowCase.state = "Completed";

    //        await BusinessCaseWorkflowRepositoryProvider.UpdateCase(workflowCase);

    //        return true;
    //    }

    //    public async Task<business_case_DTO?> GetCaseById(long caseId)
    //    {
    //        return await BusinessCaseWorkflowRepositoryProvider.GetCaseById(caseId);
    //    }

    //    public async Task<List<business_activity_DTO>> GetActivitiesByCaseId(long caseId)
    //    {
    //        return await ActivityWorkflowRepositoryProvider.GetActivitiesByCaseId(caseId);
    //    }

    //    public async Task<long> InsertCase(business_case_DTO workflowCase)
    //    {
    //        if (workflowCase == null)
    //        {
    //            throw new Exception("El caso workflow es obligatorio.");
    //        }

    //        return await BusinessCaseWorkflowRepositoryProvider.InsertCase(workflowCase);
    //    }

    //    public async Task UpdateCase(business_case_DTO workflowCase)
    //    {
    //        if (workflowCase == null)
    //        {
    //            throw new Exception("El caso workflow es obligatorio.");
    //        }

    //        await BusinessCaseWorkflowRepositoryProvider.UpdateCase(workflowCase);
    //    }

    //    public async Task UpdateCaseByTitleAndOwner(long caseId, string title, string owner)
    //    {
    //        await BusinessCaseWorkflowRepositoryProvider.UpdateCaseByTitleAndOwner(caseId,title,owner);
    //    }

    //    public async Task<long> InsertCaseActivity(business_activity_DTO activity)
    //    {
    //        if (activity == null)
    //        {
    //            throw new Exception("La actividad workflow es obligatoria.");
    //        }

    //        return await ActivityWorkflowRepositoryProvider.InsertCaseActivity(activity);
    //    }

    //    public async Task UpdateCaseActivityStatus(string status, long caseId, string activityId, string? toActivity)
    //    {
    //        if (string.IsNullOrWhiteSpace(status))
    //        {
    //            throw new Exception("El estado es obligatorio.");
    //        }

    //        if (string.IsNullOrWhiteSpace(activityId))
    //        {
    //            throw new Exception("El activityId es obligatorio.");
    //        }

    //        await ActivityWorkflowRepositoryProvider.UpdateCaseActivityStatus(status,caseId,activityId,toActivity);
    //    }
    //}
}