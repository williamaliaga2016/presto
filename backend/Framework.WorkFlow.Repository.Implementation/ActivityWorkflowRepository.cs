using Data.Extensions.Repository;
using Framework.WorkFlow.Common.DTO;
using Framework.WorkFlow.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;
using System.Data.Common;

namespace Framework.WorkFlow.Repository.Implementation
{
    public class ActivityWorkflowRepository : IActivityWorkflowRepository
    {
        private readonly WorkFlowDBContext WorkFlowDBContext;

        public ActivityWorkflowRepository(WorkFlowDBContext _workFlowDBContext)
        {
            WorkFlowDBContext = _workFlowDBContext;
        }

        public async Task<List<business_activity_DTO>> GetActivitiesByCriteria(string criteria,List<NpgsqlParameter> parameters,string? orderBy)
        {
            List<business_activity_DTO> list = new List<business_activity_DTO>();

            if (string.IsNullOrWhiteSpace(criteria))
            {
                return list;
            }

            using (DbCommand command = WorkFlowDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await WorkFlowDBContext.Database.OpenConnectionAsync();

                    string sql = $@"
                        SELECT
                            case_id,
                            activity_id,
                            secuence,
                            display_name,
                            name,
                            performer,
                            task_form_type,
                            task_form_uri,
                            status,
                            date_processed,
                            date_suspended,
                            date_incidented,
                            date_completed,
                            from_activity,
                            to_activity,
                            condition_activity
                        FROM public.vw_wf_case_activities_xpdl_participants
                        WHERE {criteria}
                    ";

                    if (!string.IsNullOrWhiteSpace(orderBy))
                    {
                        sql += $" ORDER BY {orderBy}";
                    }

                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;

                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (NpgsqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    list = reader.MapToListDomain<business_activity_DTO>()
                           ?? new List<business_activity_DTO>();
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

        public async Task<List<business_activity_DTO>> GetActivitiesByCaseId(long caseId)
        {
            object data = new
            {
                p_case_id = caseId
            };

            List<business_activity_DTO> list = new List<business_activity_DTO>();

            using (DbCommand command = WorkFlowDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await WorkFlowDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                        SELECT
                            case_id,
                            activity_id,
                            secuence,
                            display_name,
                            name,
                            performer,
                            task_form_type,
                            task_form_uri,
                            status,
                            date_processed,
                            date_suspended,
                            date_incidented,
                            date_completed,
                            from_activity,
                            to_activity,
                            condition_activity
                        FROM public.vw_wf_case_activities_xpdl_participants
                        WHERE case_id = @p_case_id
                        ORDER BY secuence;
                    ";

                    command.CommandType = CommandType.Text;
                    command.Parameters.ToArray<object>(data);

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    list = reader.MapToListDomain<business_activity_DTO>();
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

        public async Task InsertXpdlActivity(business_activity_DTO activity)
        {
            object data = new
            {
                p_activity_id = activity.activity_id,
                p_workflow_process_id = activity.workflow_process_id,
                p_display_name = activity.display_name,
                p_name = activity.name,
                p_task_type = activity.task_type,
                p_task_form_type = activity.task_form_type,
                p_task_form_uri = activity.task_form_uri,
                p_performer = activity.performer,
                p_sub_flow_id = activity.sub_flow_id
            };

            using (DbCommand command = WorkFlowDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await WorkFlowDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                        SELECT public.sp_wf_create_xpdl_activity(
                            @p_activity_id,
                            @p_workflow_process_id,
                            @p_display_name,
                            @p_name,
                            @p_task_type,
                            @p_task_form_type,
                            @p_task_form_uri,
                            @p_performer,
                            @p_sub_flow_id
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

        public async Task<long> InsertCaseActivity(business_activity_DTO activity)
        {
            object data = new
            {
                p_case_id = activity.case_id,
                p_activity_id = activity.activity_id,
                p_secuence = activity.secuence,
                p_display_name = activity.display_name,
                p_name = activity.name,
                p_performer = activity.performer,
                p_task_form_type = activity.task_form_type,
                p_task_form_uri = activity.task_form_uri,
                p_status = activity.status,
                p_from_activity = activity.from_activity,
                p_to_activity = activity.to_activity,
                p_condition_activity = activity.condition_activity
            };

            long idCaseActivities = -1;

            using (DbCommand command = WorkFlowDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await WorkFlowDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                        SELECT public.sp_wf_insert_case_activities(
                            @p_case_id,
                            @p_activity_id,
                            @p_secuence,
                            @p_display_name,
                            @p_name,
                            @p_performer,
                            @p_task_form_type,
                            @p_task_form_uri,
                            @p_status,
                            @p_from_activity,
                            @p_to_activity,
                            @p_condition_activity
                        );
                    ";

                    command.CommandType = CommandType.Text;
                    command.Parameters.ToArray<object>(data);

                    object? result = await command.ExecuteScalarAsync();

                    if (result != null && result != DBNull.Value)
                    {
                        idCaseActivities = Convert.ToInt64(result);
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

            return idCaseActivities;
        }

        public async Task UpdateCaseActivityStatus(
            string status,
            long caseId,
            string activityId,
            string? toActivity
        )
        {
            object data = new
            {
                p_status = status,
                p_case_id = caseId,
                p_activity_id = activityId,
                p_to_activity = toActivity
            };

            using (DbCommand command = WorkFlowDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await WorkFlowDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                        SELECT public.sp_wf_update_case_activity_status(
                            @p_status,
                            @p_case_id,
                            @p_activity_id,
                            @p_to_activity
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