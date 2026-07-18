using Framework.WorkFlow.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;

namespace Framework.WorkFlow.Application.Implementations
{
    public class WorkflowRuntimeApplication : IWorkflowRuntimeApplication
    {
        private readonly IBusinessCaseProcessorApplication BusinessCaseProcessorApplicationProvider;
        private readonly IActivityWorkflowApplication ActivityWorkflowApplicationProvider;
        private readonly ITransitionProcessorApplication TransitionProcessorApplicationProvider;
        private readonly ICaseActivityObserverApplication CaseActivityObserverApplicationProvider;

        public WorkflowRuntimeApplication(
            IBusinessCaseProcessorApplication _businessCaseProcessorApplication,
            IActivityWorkflowApplication _activityWorkflowApplication,
            ITransitionProcessorApplication _transitionProcessorApplication,
            ICaseActivityObserverApplication _caseActivityObserverApplication
        )
        {
            BusinessCaseProcessorApplicationProvider = _businessCaseProcessorApplication;
            ActivityWorkflowApplicationProvider = _activityWorkflowApplication;
            TransitionProcessorApplicationProvider = _transitionProcessorApplication;
            CaseActivityObserverApplicationProvider = _caseActivityObserverApplication;
        }

        public async Task<List<xpdl_transition_DTO>> GetTransitions(string activityId)
        {
            return await TransitionProcessorApplicationProvider.GetTransitions(activityId);
        }

        public async Task<business_case_DTO?> GetCase(long caseId)
        {
            business_case_DTO? workflowCase =
                await BusinessCaseProcessorApplicationProvider.GetCase(caseId);

            if (workflowCase == null)
            {
                return null;
            }

            workflowCase.current_activities =
                await ActivityWorkflowApplicationProvider.LoadNextActivities(caseId);

            return workflowCase;
        }

        public async Task<long> CreateCase(string workflowProcessId,string nameOrTitle,string owner)
        {
            bool existsWorkflow =
                await BusinessCaseProcessorApplicationProvider.ExistsWorkflowProcess(
                    workflowProcessId
                );

            if (!existsWorkflow)
            {
                throw new InvalidOperationException(
                    $"El workflowProcessId {workflowProcessId} no existe."
                );
            }

            business_case_DTO workflowCase = new business_case_DTO
            {
                workflow_process_id = workflowProcessId,
                title = nameOrTitle,
                created_by = owner,
                workflow_instance_id = Guid.NewGuid(),
                state = "New",
                created = DateTime.Now
            };

            long caseId =
                await BusinessCaseProcessorApplicationProvider.InsertCase(workflowCase);

            workflowCase.case_id = caseId;

            await ActivityWorkflowApplicationProvider.Initialize(
                caseId,
                workflowProcessId
            );

            workflowCase.current_activities =
                await ActivityWorkflowApplicationProvider.GetActivities(caseId);

            await TrackCase(workflowCase, "CreateCase");

            return caseId;
        }

        public async Task<business_case_DTO?> AssignCase(long caseId)
        {
            business_case_DTO? workflowCase =
                await BusinessCaseProcessorApplicationProvider.GetCase(caseId);

            if (workflowCase == null)
            {
                return null;
            }

            workflowCase.state = "Assigned";

            await BusinessCaseProcessorApplicationProvider.SaveChanges(workflowCase);

            await TrackCase(workflowCase, "AssignCase");

            workflowCase.current_activities =
                await ActivityWorkflowApplicationProvider.LoadNextActivities(caseId);

            return workflowCase;
        }

        public async Task<bool> ProcessCase(
            long caseId,
            string activityId,
            string? transitionId
        )
        {
            business_case_DTO? workflowCase =
                await BusinessCaseProcessorApplicationProvider.GetCase(caseId);

            if (workflowCase == null)
            {
                return false;
            }

            business_activity_DTO? activity =
                await ActivityWorkflowApplicationProvider.GetActivity(caseId, activityId);

            if (activity == null)
            {
                return false;
            }

            workflowCase.state = "InProgress";

            await BusinessCaseProcessorApplicationProvider.SaveChanges(workflowCase);

            await TrackCase(workflowCase, "ProcessCase");

            string? toActivity = string.Empty;

            List<xpdl_transition_DTO> transitions =
                await TransitionProcessorApplicationProvider.GetTransitions(activityId);

            if (!string.IsNullOrWhiteSpace(transitionId))
            {
                xpdl_transition_DTO? selectedTransition =
                    transitions.FirstOrDefault(x => x.transition_id == transitionId);

                toActivity = selectedTransition?.to_activity;
            }
            else if (transitions.Count > 0)
            {
                toActivity = transitions[0].to_activity;
            }

            await ActivityWorkflowApplicationProvider.UpdateCaseActivityStatus(
                "Completed",
                caseId,
                activityId,
                toActivity
            );

            if (!string.IsNullOrWhiteSpace(toActivity))
            {
                await ActivityWorkflowApplicationProvider.CreateActivities(
                    caseId,
                    activityId,
                    toActivity
                );
            }

            return true;
        }

        public async Task<bool> SuspendCase(long caseId)
        {
            business_case_DTO? workflowCase =
                await BusinessCaseProcessorApplicationProvider.GetCase(caseId);

            if (workflowCase == null)
            {
                return false;
            }

            workflowCase.state = "Suspended";

            await BusinessCaseProcessorApplicationProvider.SaveChanges(workflowCase);

            await TrackCase(workflowCase, "SuspendCase");

            List<business_activity_DTO> activities =
                await ActivityWorkflowApplicationProvider.GetActivities(caseId);

            foreach (business_activity_DTO activity in activities.Where(x =>
                x.status == "New" || x.status == "InProgress"
            ))
            {
                await ActivityWorkflowApplicationProvider.UpdateCaseActivityStatus(
                    "Suspended",
                    caseId,
                    activity.activity_id!,
                    activity.to_activity
                );
            }

            return true;
        }

        public async Task<bool> ReassignCase(long caseId)
        {
            business_case_DTO? workflowCase =
                await BusinessCaseProcessorApplicationProvider.GetCase(caseId);

            if (workflowCase == null)
            {
                return false;
            }

            workflowCase.state = "InProgress";

            await BusinessCaseProcessorApplicationProvider.SaveChanges(workflowCase);

            await TrackCase(workflowCase, "ReassignCase");

            await ActivityWorkflowApplicationProvider.ReprocessingActivities(caseId);

            return true;
        }

        public async Task<bool> CompleteCase(long caseId)
        {
            business_case_DTO? workflowCase =
                await BusinessCaseProcessorApplicationProvider.GetCase(caseId);

            if (workflowCase == null)
            {
                return false;
            }

            workflowCase.state = "Completed";

            await BusinessCaseProcessorApplicationProvider.SaveChanges(workflowCase);

            await TrackCase(workflowCase, "CompleteCase");

            return true;
        }

        public async Task<bool> CancelCase(long caseId)
        {
            business_case_DTO? workflowCase =
                await BusinessCaseProcessorApplicationProvider.GetCase(caseId);

            if (workflowCase == null)
            {
                return false;
            }

            workflowCase.state = "Completed";

            await BusinessCaseProcessorApplicationProvider.SaveChanges(workflowCase);

            await TrackCase(workflowCase, "CancelCase");

            List<business_activity_DTO> activities =
                await ActivityWorkflowApplicationProvider.GetActivities(caseId);

            foreach (business_activity_DTO activity in activities.Where(x =>
                x.status != "Completed"
            ))
            {
                await ActivityWorkflowApplicationProvider.UpdateCaseActivityStatus(
                    "Canceled",
                    caseId,
                    activity.activity_id!,
                    activity.to_activity
                );
            }

            return true;
        }

        public async Task<bool> IncidentCase(long caseId, string activityId)
        {
            business_case_DTO? workflowCase =
                await BusinessCaseProcessorApplicationProvider.GetCase(caseId);

            if (workflowCase == null)
            {
                return false;
            }

            business_activity_DTO? activity =
                await ActivityWorkflowApplicationProvider.GetActivity(caseId, activityId);

            if (activity == null)
            {
                return false;
            }

            workflowCase.state = "Incidented";

            await BusinessCaseProcessorApplicationProvider.SaveChanges(workflowCase);

            await TrackCase(workflowCase, "IncidentCase");

            await ActivityWorkflowApplicationProvider.UpdateCaseActivityStatus(
                "Incidented",
                caseId,
                activityId,
                activity.to_activity
            );

            return true;
        }

        public async Task<business_case_DTO?> ReprocessingCase(
            long caseId,
            string activityId
        )
        {
            business_case_DTO? workflowCase =
                await BusinessCaseProcessorApplicationProvider.GetCase(caseId);

            if (workflowCase == null)
            {
                return null;
            }

            workflowCase.state = "InProgress";

            await BusinessCaseProcessorApplicationProvider.SaveChanges(workflowCase);

            await TrackCase(workflowCase, "ReprocessingCase");

            await ActivityWorkflowApplicationProvider.UpdateCaseActivityStatus(
                "Reprocess",
                caseId,
                activityId,
                null
            );

            return await GetCase(caseId);
        }

        public async Task<bool> ReprocessingActivities(
            long caseId,
            string activityIdA,
            string activityIdB
        )
        {
            business_activity_DTO? activityA =
                await ActivityWorkflowApplicationProvider.GetActivity(caseId, activityIdA);

            business_activity_DTO? activityB =
                await ActivityWorkflowApplicationProvider.GetActivity(caseId, activityIdB);

            if (activityA == null || activityB == null)
            {
                return false;
            }

            await ActivityWorkflowApplicationProvider.UpdateCaseActivityStatus(
                "Reprocess",
                caseId,
                activityIdA,
                activityA.to_activity
            );

            await ActivityWorkflowApplicationProvider.UpdateCaseActivityStatus(
                "Reprocess",
                caseId,
                activityIdB,
                activityB.to_activity
            );

            return true;
        }

        public async Task<bool> UpdateCase(long caseId, string title, string owner)
        {
            business_case_DTO? workflowCase =
                await BusinessCaseProcessorApplicationProvider.GetCase(caseId);

            if (workflowCase == null)
            {
                return false;
            }

            workflowCase.title = title;
            workflowCase.created_by = owner;

            await BusinessCaseProcessorApplicationProvider.SaveChanges(workflowCase);

            return true;
        }

        private async Task TrackCase(
            business_case_DTO workflowCase,
            string trackingSource
        )
        {
            await CaseActivityObserverApplicationProvider.Notify(
                new notify_item_DTO
                {
                    case_item = workflowCase,
                    tracking_source = trackingSource
                }
            );
        }
    }
}