using Data.Extensions.Repository;
using Framework.WorkFlow.Common.DTO;
using Framework.WorkFlow.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Framework.WorkFlow.Repository.Implementation
{
    public class CaseActivityTrackingWorkflowRepository : ICaseActivityTrackingWorkflowRepository
    {
        private readonly WorkFlowDBContext WorkFlowDBContext;

        public CaseActivityTrackingWorkflowRepository(WorkFlowDBContext _workFlowDBContext)
        {
            WorkFlowDBContext = _workFlowDBContext;
        }

        public async Task<Guid> TrackActivity(
    business_case_DTO caseInstance,
    string? xmlCaseInstance,
    string? trackSource
)
        {
            if (caseInstance == null)
            {
                throw new Exception("El caso workflow es obligatorio para registrar tracking.");
            }

            object data = new
            {
                p_workflow_case_id = caseInstance.case_id,
                p_status = caseInstance.state,
                p_workflow_instance_id = caseInstance.workflow_instance_id,
                p_workflow_process_id = caseInstance.workflow_process_id,
                p_workflow_case_instance = xmlCaseInstance,
                p_track_source = trackSource
            };

            Guid trackId = Guid.Empty;

            using (DbCommand command = WorkFlowDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await WorkFlowDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                SELECT public.sp_wf_tracking_business_case(
                    CAST(@p_workflow_case_id AS bigint),
                    CAST(@p_status AS varchar),
                    CAST(@p_workflow_instance_id AS uuid),
                    CAST(@p_workflow_process_id AS varchar),
                    CAST(@p_workflow_case_instance AS xml),
                    CAST(@p_track_source AS varchar)
                );
            ";

                    command.CommandType = CommandType.Text;
                    command.Parameters.ToArray<object>(data);

                    object? result = await command.ExecuteScalarAsync();

                    if (result != null && result != DBNull.Value)
                    {
                        trackId = Guid.Parse(result.ToString()!);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    await WorkFlowDBContext.Database.CloseConnectionAsync();
                }
            }

            return trackId;
        }
    }
}