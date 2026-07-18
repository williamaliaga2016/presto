using Framework.WorkFlow.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using System.Globalization;

namespace Framework.WorkFlow.Application.Implementations
{
    public class CaseApplication : ICaseApplication
    {
        private readonly IWorkflowRuntimeApplication WorkflowRuntimeApplicationProvider;

        public CaseApplication(
            IWorkflowRuntimeApplication _workflowRuntimeApplication
        )
        {
            WorkflowRuntimeApplicationProvider = _workflowRuntimeApplication;
        }

        public async Task<List<xpdl_transition_DTO>> GetTransitions(string activityId)
        {
            try
            {
                return await WorkflowRuntimeApplicationProvider.GetTransitions(activityId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    string.Format("Get transitions - Error {0}", activityId),
                    ex
                );
            }
        }

        public async Task<business_case_DTO?> GetCase(long caseId)
        {
            try
            {
                return await WorkflowRuntimeApplicationProvider.GetCase(caseId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    string.Format("Get Case - Error {0}", caseId),
                    ex
                );
            }
        }

        public async Task<business_case_DTO?> CreateCase(
            string workflowId,
            string name,
            string owner
        )
        {
            ValidateStringParameter(nameof(workflowId), workflowId);
            ValidateStringParameter(nameof(name), name);
            ValidateStringParameter(nameof(owner), owner);

            try
            {
                long caseId = await WorkflowRuntimeApplicationProvider.CreateCase(
                    workflowId,
                    name,
                    owner
                );

                if (caseId == 0)
                {
                    return null;
                }

                business_case_DTO? workflowCase =
                    await WorkflowRuntimeApplicationProvider.AssignCase(caseId);

                if (
                    workflowCase?.current_activities != null &&
                    workflowCase.current_activities.Count > 0 &&
                    workflowCase.current_activities[0].name == "StartEvent"
                )
                {
                    if (
                        await WorkflowRuntimeApplicationProvider.ProcessCase(
                            caseId,
                            workflowCase.current_activities[0].activity_id!,
                            string.Empty
                        )
                    )
                    {
                        workflowCase =
                            await WorkflowRuntimeApplicationProvider.AssignCase(caseId);
                    }

                    if (
                        workflowCase?.current_activities != null &&
                        workflowCase.current_activities.Count > 0 &&
                        workflowCase.current_activities[0].name == "StartEvent" &&
                        await WorkflowRuntimeApplicationProvider.ProcessCase(
                            caseId,
                            workflowCase.current_activities[0].activity_id!,
                            string.Empty
                        )
                    )
                    {
                        workflowCase =
                            await WorkflowRuntimeApplicationProvider.AssignCase(caseId);
                    }
                }

                return workflowCase;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    string.Format("Create Case - Error {0} {1} {2}", workflowId, name, owner),
                    ex
                );
            }
        }

        public async Task<business_case_DTO?> DispatchActivity(
            long caseId,
            string activityId,
            string transitionId
        )
        {
            ValidateStringParameter(nameof(activityId), activityId);

            try
            {
                business_case_DTO? workflowCase =
                    await WorkflowRuntimeApplicationProvider.GetCase(caseId);

                if (
                    workflowCase != null &&
                    workflowCase.case_id > 0 &&
                    await WorkflowRuntimeApplicationProvider.ProcessCase(
                        caseId,
                        activityId,
                        transitionId
                    )
                )
                {
                    workflowCase =
                        await WorkflowRuntimeApplicationProvider.AssignCase(caseId);

                    if (workflowCase?.current_activities != null)
                    {
                        foreach (
                            business_activity_DTO businessActivity in
                            workflowCase.current_activities.Where(a =>
                                a.name == "StartEvent" ||
                                a.name == "EndEvent"
                            )
                        )
                        {
                            if (
                                await WorkflowRuntimeApplicationProvider.ProcessCase(
                                    caseId,
                                    businessActivity.activity_id!,
                                    string.Empty
                                )
                            )
                            {
                                workflowCase =
                                    await WorkflowRuntimeApplicationProvider.AssignCase(caseId);
                            }
                        }
                    }
                }

                return workflowCase;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "DispatchActivity - Error {0} {1} {2}",
                        caseId,
                        activityId,
                        transitionId
                    ),
                    ex
                );
            }
        }

        public async Task<bool> SuspendCase(long caseId)
        {
            return await WorkflowRuntimeApplicationProvider.SuspendCase(caseId);
        }

        public async Task<business_case_DTO?> ActivateCase(long caseId)
        {
            business_case_DTO? workflowCase =
                await WorkflowRuntimeApplicationProvider.GetCase(caseId);

            if (
                workflowCase != null &&
                workflowCase.case_id > 0 &&
                await WorkflowRuntimeApplicationProvider.ReassignCase(caseId)
            )
            {
                workflowCase =
                    await WorkflowRuntimeApplicationProvider.GetCase(caseId);
            }

            return workflowCase;
        }

        public async Task<bool> CloseCase(long caseId)
        {
            return await WorkflowRuntimeApplicationProvider.CompleteCase(caseId);
        }

        public async Task<bool> CancelCase(long caseId)
        {
            return await WorkflowRuntimeApplicationProvider.CancelCase(caseId);
        }

        public async Task<bool> CreateIncident(long caseId, string activityId)
        {
            return await WorkflowRuntimeApplicationProvider.IncidentCase(
                caseId,
                activityId
            );
        }

        public async Task<business_case_DTO?> SolveIncident(
            long caseId,
            string activityId
        )
        {
            business_case_DTO? workflowCase =
                await WorkflowRuntimeApplicationProvider.GetCase(caseId);

            if (workflowCase != null && workflowCase.case_id > 0)
            {
                workflowCase =
                    await WorkflowRuntimeApplicationProvider.ReprocessingCase(
                        caseId,
                        activityId
                    );
            }

            return workflowCase;
        }

        public async Task<bool> UpdateCase(long caseId, string title, string owner)
        {
            business_case_DTO? workflowCase =
                await WorkflowRuntimeApplicationProvider.GetCase(caseId);

            if (workflowCase == null || workflowCase.case_id <= 0)
            {
                return false;
            }

            return await WorkflowRuntimeApplicationProvider.UpdateCase(
                caseId,
                title,
                owner
            );
        }

        public async Task<bool> ReactivateCase(long caseId)
        {
            bool result = false;

            business_case_DTO? workflowCase =
                await WorkflowRuntimeApplicationProvider.GetCase(caseId);

            if (workflowCase != null && workflowCase.case_id > 0)
            {
                result = await WorkflowRuntimeApplicationProvider.ReassignCase(caseId);

                if (result)
                {
                    business_case_DTO? assignedCase =
                        await WorkflowRuntimeApplicationProvider.AssignCase(caseId);

                    result = assignedCase != null && assignedCase.case_id > 0;
                }
            }

            return result;
        }

        public async Task<bool> ReprocessingActivity(
            long caseId,
            string activityIdA,
            string activityIdB
        )
        {
            bool result =
                await WorkflowRuntimeApplicationProvider.ReprocessingActivities(
                    caseId,
                    activityIdA,
                    activityIdB
                );

            if (result)
            {
                result = await WorkflowRuntimeApplicationProvider.ReassignCase(caseId);

                if (result)
                {
                    business_case_DTO? assignedCase =
                        await WorkflowRuntimeApplicationProvider.AssignCase(caseId);

                    result = assignedCase != null && assignedCase.case_id > 0;
                }
            }

            return result;
        }

        public void ValidateStringParameter(string parameterName, string parameterValue)
        {
            if (string.IsNullOrEmpty(parameterValue))
            {
                throw new ArgumentException("Parameter invalid", parameterName);
            }

            if (parameterValue.Trim().Length == 0)
            {
                throw new ArgumentException("Parameter invalid", parameterName);
            }
        }

        public void ValidateNullParameter(string parameterName, object parameterValue)
        {
            if (parameterValue == null)
            {
                throw new ArgumentException("Parameter invalid", parameterName);
            }
        }

        public async Task<bool> IsValidDateTime(string dateToValidate)
        {
            return await Task.FromResult(
                DateTime.TryParse(
                    dateToValidate,
                    CultureInfo.CurrentCulture,
                    DateTimeStyles.None,
                    out _
                )
            );
        }
    }
}