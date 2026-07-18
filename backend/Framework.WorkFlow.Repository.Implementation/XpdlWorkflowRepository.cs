using Data.Extensions.Repository;
using Framework.WorkFlow.Common.DTO;
using Framework.WorkFlow.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Framework.WorkFlow.Repository.Implementation
{
    public class XpdlWorkflowRepository : IXpdlWorkflowRepository
    {
        private readonly WorkFlowDBContext WorkFlowDBContext;

        public XpdlWorkflowRepository(WorkFlowDBContext _workFlowDBContext)
        {
            WorkFlowDBContext = _workFlowDBContext;
        }

        public async Task<int> ExistsXpdlWorkflowProcess(string workflowProcessId)
        {
            object data = new
            {
                p_workflow_process_id = workflowProcessId
            };

            int exists = 0;

            using (DbCommand command = WorkFlowDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await WorkFlowDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                        SELECT public.sp_wf_exists_workflow_process_id(
                            @p_workflow_process_id
                        );
                    ";

                    command.CommandType = CommandType.Text;
                    command.Parameters.ToArray<object>(data);

                    object? result = await command.ExecuteScalarAsync();

                    if (result != null && result != DBNull.Value)
                    {
                        exists = Convert.ToInt32(result);
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

            return exists;
        }

        public async Task<List<xpdl_transition_DTO>> GetXpdlTransitionsByCriteria(
            string activityId,
            int filter
        )
        {
            object data = new
            {
                p_activity_id = activityId,
                p_filter_id = filter
            };

            List<xpdl_transition_DTO> list = new List<xpdl_transition_DTO>();

            using (DbCommand command = WorkFlowDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await WorkFlowDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                        SELECT
                            transition_id,
                            name,
                            from_activity,
                            to_activity,
                            condition,
                            workflow_process_id
                        FROM public.sp_wf_get_xpdl_transitions_by_criteria(
                            @p_activity_id,
                            @p_filter_id
                        );
                    ";

                    command.CommandType = CommandType.Text;
                    command.Parameters.ToArray<object>(data);

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    list = reader.MapToListDomain<xpdl_transition_DTO>();
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

        public async Task<List<xpdl_activity_DTO>> GetXpdlActivities()
        {
            List<xpdl_activity_DTO> list = new List<xpdl_activity_DTO>();

            using (DbCommand command = WorkFlowDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await WorkFlowDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                        SELECT
                            activity_id,
                            workflow_process_id,
                            display_name,
                            name,
                            task_type,
                            task_form_type,
                            task_form_uri,
                            performer,
                            sub_flow_id
                        FROM public.sp_wf_get_xpdl_activities();
                    ";

                    command.CommandType = CommandType.Text;

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    list = reader.MapToListDomain<xpdl_activity_DTO>();
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

        public async Task<xpdl_activity_DTO?> GetActivityById(string activityId)
        {
            object data = new
            {
                p_activity_id = activityId
            };

            List<xpdl_activity_DTO> list = new List<xpdl_activity_DTO>();

            using (DbCommand command = WorkFlowDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await WorkFlowDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                        SELECT
                            activity_id,
                            workflow_process_id,
                            display_name,
                            name,
                            task_type,
                            task_form_type,
                            task_form_uri,
                            performer,
                            sub_flow_id
                        FROM public.xpdl_activities
                        WHERE activity_id = @p_activity_id
                        LIMIT 1;
                    ";

                    command.CommandType = CommandType.Text;
                    command.Parameters.ToArray<object>(data);

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    list = reader.MapToListDomain<xpdl_activity_DTO>();
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

        public async Task InsertXpdlWorkflowProcess(xpdl_workflow_process_DTO flow)
        {
            object data = new
            {
                p_workflow_process_id = flow.workflow_process_id,
                p_display_name = flow.display_name,
                p_name = flow.name,
                p_description = flow.description
            };

            using (DbCommand command = WorkFlowDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await WorkFlowDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                        SELECT public.sp_wf_insert_xpdl_workflow_processes(
                            @p_workflow_process_id,
                            @p_display_name,
                            @p_name,
                            @p_description
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

        public async Task InsertXpdlParticipant(xpdl_participant_DTO participant)
        {
            object data = new
            {
                p_participant_id = participant.participant_id,
                p_display_name = participant.display_name,
                p_name = participant.name,
                p_participant_type = participant.participant_type
            };

            using (DbCommand command = WorkFlowDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await WorkFlowDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                        SELECT public.sp_wf_insert_xpdl_participant(
                            @p_participant_id,
                            @p_display_name,
                            @p_name,
                            @p_participant_type
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

        public async Task InsertXpdlTransition(xpdl_transition_DTO transition)
        {
            object data = new
            {
                p_transition_id = transition.transition_id,
                p_name = transition.name,
                p_from_activity = transition.from_activity,
                p_to_activity = transition.to_activity,
                p_condition = transition.condition
            };

            using (DbCommand command = WorkFlowDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await WorkFlowDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                        SELECT public.sp_wf_insert_xpdl_transition(
                            @p_transition_id,
                            @p_name,
                            @p_from_activity,
                            @p_to_activity,
                            @p_condition
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