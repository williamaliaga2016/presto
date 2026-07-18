using Data.Extensions.Repository;
using Framework.WorkFlow.Common.DTO;
using Framework.WorkFlow.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using System.Globalization;

namespace Framework.WorkFlow.Repository.Implementation
{
    public class BusinessCaseWorkflowRepository : IBusinessCaseWorkflowRepository
    {
        private readonly WorkFlowDBContext WorkFlowDBContext;

        public BusinessCaseWorkflowRepository(WorkFlowDBContext _workFlowDBContext)
        {
            WorkFlowDBContext = _workFlowDBContext;
        }

        public async Task<List<business_case_DTO>> GetAllCases()
        {
            List<business_case_DTO> list = new List<business_case_DTO>();

            using (DbCommand command = WorkFlowDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await WorkFlowDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                        SELECT
                            case_id,
                            title,
                            state,
                            workflow_instance_id,
                            workflow_process_id,
                            created_by,
                            created
                        FROM public.sp_wf_get_all_cases();
                    ";

                    command.CommandType = CommandType.Text;

                    DbDataReader reader = await command.ExecuteReaderAsync();
                    list = reader.MapToListDomain<business_case_DTO>();
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

            return list;
        }

        public async Task<List<business_case_DTO>> GetCasesByWorkflowInstanceId(Guid workflowInstanceId)
        {
            object data = new
            {
                p_workflow_id = workflowInstanceId
            };

            List<business_case_DTO> list = new List<business_case_DTO>();

            using (DbCommand command = WorkFlowDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await WorkFlowDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                SELECT
                    case_id,
                    title,
                    state,
                    workflow_instance_id,
                    workflow_process_id,
                    created_by,
                    created
                FROM public.sp_wf_get_cases_by_workflow_instance_id(
                    CAST(@p_workflow_id AS uuid)
                );
            ";

                    command.CommandType = CommandType.Text;
                    command.Parameters.ToArray<object>(data);

                    DbDataReader reader = await command.ExecuteReaderAsync();
                    list = reader.MapToListDomain<business_case_DTO>();
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

            return list;
        }

        public async Task<business_case_DTO?> GetCaseById(long caseId)
        {
            object data = new
            {
                p_case_id = caseId
            };

            List<business_case_DTO> list = new List<business_case_DTO>();

            using (DbCommand command = WorkFlowDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await WorkFlowDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                        SELECT
                            case_id,
                            title,
                            state,
                            workflow_instance_id,
                            workflow_process_id,
                            created_by,
                            created
                        FROM public.sp_wf_get_case_by_id(
                            @p_case_id
                        );
                    ";

                    command.CommandType = CommandType.Text;
                    command.Parameters.ToArray<object>(data);

                    DbDataReader reader = await command.ExecuteReaderAsync();
                    list = reader.MapToListDomain<business_case_DTO>();
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

            return list.FirstOrDefault();
        }

        public async Task<long> InsertCase(business_case_DTO workflowCase)
        {
            object data = new
            {
                p_title = workflowCase.title,
                p_state = workflowCase.state,
                p_workflow_instance_id = workflowCase.workflow_instance_id,
                p_workflow_process_id = workflowCase.workflow_process_id,
                p_created_by = workflowCase.created_by
            };

            using (DbCommand command = WorkFlowDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await WorkFlowDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                        SELECT public.sp_wf_insert_case(
                            CAST(@p_title AS varchar),
                            CAST(@p_state AS varchar),
                            CAST(@p_workflow_instance_id AS uuid),
                            CAST(@p_workflow_process_id AS varchar),
                            CAST(@p_created_by AS varchar)
                        );
                    ";

                    command.CommandType = CommandType.Text;
                    command.Parameters.ToArray<object>(data);

                    await command.ExecuteNonQueryAsync();
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

            business_case_DTO? createdCase =
                (await GetCasesByWorkflowInstanceId(workflowCase.workflow_instance_id))
                .FirstOrDefault();

            return createdCase?.case_id ?? 0;
        }

        public async Task UpdateCase(business_case_DTO workflowCase)
        {
            object data = new
            {
                p_case_id = workflowCase.case_id,
                p_title = workflowCase.title,
                p_state = workflowCase.state,
                p_workflow_instance_id = workflowCase.workflow_instance_id,
                p_workflow_process_id = workflowCase.workflow_process_id,
                p_created_by = workflowCase.created_by
            };

            using (DbCommand command = WorkFlowDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await WorkFlowDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                        SELECT public.sp_wf_update_case(
                            CAST(@p_case_id AS bigint),
                            CAST(@p_title AS varchar),
                            CAST(@p_state AS varchar),
                            CAST(@p_workflow_instance_id AS uuid),
                            CAST(@p_workflow_process_id AS varchar),
                            CAST(@p_created_by AS varchar)
                        );
                    ";

                    command.CommandType = CommandType.Text;
                    command.Parameters.ToArray<object>(data);

                    await command.ExecuteNonQueryAsync();
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
        }

        public async Task UpdateCaseByTitleAndOwner(long caseId, string title, string owner)
        {
            object data = new
            {
                p_case_id = caseId,
                p_title = title,
                p_created_by = owner
            };

            using (DbCommand command = WorkFlowDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await WorkFlowDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                        SELECT public.sp_wf_update_case_by_title_and_owner(
                            @p_case_id,
                            @p_title,
                            @p_created_by
                        );
                    ";

                    command.CommandType = CommandType.Text;
                    command.Parameters.ToArray<object>(data);

                    await command.ExecuteNonQueryAsync();
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
        }
    }
}