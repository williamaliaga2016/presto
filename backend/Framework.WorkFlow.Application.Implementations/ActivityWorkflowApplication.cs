using Framework.WorkFlow.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Framework.WorkFlow.Repository.Interface;
using Npgsql;

namespace Framework.WorkFlow.Application.Implementations
{
    public class ActivityWorkflowApplication : IActivityWorkflowApplication
    {
        private readonly IActivityWorkflowRepository ActivityWorkflowRepositoryProvider;
        private readonly IXpdlWorkflowRepository XpdlWorkflowRepositoryProvider;
        private readonly IBusinessCaseWorkflowRepository BusinessCaseWorkflowRepositoryProvider;

        public ActivityWorkflowApplication(
            IActivityWorkflowRepository _activityWorkflowRepository,
            IXpdlWorkflowRepository _xpdlWorkflowRepository,
            IBusinessCaseWorkflowRepository _businessCaseWorkflowRepository
        )
        {
            ActivityWorkflowRepositoryProvider = _activityWorkflowRepository;
            XpdlWorkflowRepositoryProvider = _xpdlWorkflowRepository;
            BusinessCaseWorkflowRepositoryProvider = _businessCaseWorkflowRepository;
        }

        public async Task UpdateCaseActivityStatus(
            string status,
            long caseId,
            string activityId,
            string? toActivity
        )
        {
            await ActivityWorkflowRepositoryProvider.UpdateCaseActivityStatus(
                status,
                caseId,
                activityId,
                toActivity
            );
        }

        public async Task<long> CreateCaseActivity(business_activity_DTO activity)
        {
            return await ActivityWorkflowRepositoryProvider.InsertCaseActivity(activity);
        }

        public async Task CreateXpdlActivity(business_activity_DTO activity)
        {
            await ActivityWorkflowRepositoryProvider.InsertXpdlActivity(activity);
        }

        public async Task<List<business_activity_DTO>> Initialize(
            long caseId,
            string workflowProcessId
        )
        {
            List<business_activity_DTO> activities = new List<business_activity_DTO>();

            if (caseId <= 0)
            {
                return activities;
            }

            List<xpdl_activity_DTO> xpdlActivities =
                await XpdlWorkflowRepositoryProvider.GetXpdlActivities();

            List<xpdl_activity_DTO> startActivities = xpdlActivities
                .Where(x =>
                    x.workflow_process_id == workflowProcessId &&
                    x.name == "StartEvent"
                )
                .ToList();

            foreach (xpdl_activity_DTO xpdlInfo in startActivities)
            {
                business_activity_DTO activity = new business_activity_DTO
                {
                    case_id = caseId,
                    activity_id = xpdlInfo.activity_id,
                    secuence = "001.001",
                    display_name = xpdlInfo.display_name,
                    name = xpdlInfo.name,
                    status = "New",
                    date_processed = DateTime.Now,
                    performer = xpdlInfo.performer,
                    task_form_type = xpdlInfo.task_form_type,
                    task_form_uri = xpdlInfo.task_form_uri
                };

                await LoadTransitionsToActivity(activity);

                await ActivityWorkflowRepositoryProvider.InsertCaseActivity(activity);

                activities.Add(activity);
            }

            return activities;
        }

        public async Task<List<business_activity_DTO>> CreateActivities(
            long caseId,
            string fromActivity,
            string activityId
        )
        {
            List<business_activity_DTO> activities = new List<business_activity_DTO>();

            if (caseId <= 0 || string.IsNullOrWhiteSpace(activityId))
            {
                return activities;
            }

            List<xpdl_activity_DTO> xpdlActivities =
                await XpdlWorkflowRepositoryProvider.GetXpdlActivities();

            List<xpdl_activity_DTO> targetActivities = xpdlActivities
                .Where(x => x.activity_id == activityId)
                .ToList();

            foreach (xpdl_activity_DTO xpdlActivity in targetActivities)
            {
                bool canCreate = await CheckActivity(caseId, fromActivity, xpdlActivity.activity_id);

                if (!canCreate)
                {
                    continue;
                }

                business_activity_DTO activity = new business_activity_DTO
                {
                    case_id = caseId,
                    activity_id = xpdlActivity.activity_id,
                    secuence = "001.001",
                    display_name = xpdlActivity.display_name,
                    name = xpdlActivity.name,
                    date_processed = DateTime.Now,
                    performer = xpdlActivity.performer,
                    from_activity = fromActivity,
                    task_form_type = xpdlActivity.task_form_type,
                    task_form_uri = xpdlActivity.task_form_uri
                };

                await LoadTransitionsToActivity(activity);

                if (xpdlActivity.task_type == "SubFlow")
                {
                    activity.status = "SubFlow InProgress";
                    await ActivityWorkflowRepositoryProvider.InsertCaseActivity(activity);

                    await LoadSubActivities(
                        caseId,
                        fromActivity,
                        activities,
                        xpdlActivities,
                        xpdlActivity
                    );
                }
                else if (xpdlActivity.task_type == "Parallel")
                {
                    activity.status = "Completed";
                    await ActivityWorkflowRepositoryProvider.InsertCaseActivity(activity);

                    List<xpdl_transition_DTO> transitions =
                        await XpdlWorkflowRepositoryProvider.GetXpdlTransitionsByCriteria(
                            activityId,
                            1
                        );

                    await LoadActivityTransitions(
                        caseId,
                        activityId,
                        activities,
                        xpdlActivities,
                        transitions
                    );
                }
                else if (xpdlActivity.task_type == "Parallel Conditional")
                {
                    activity.status = "Completed";
                    await ActivityWorkflowRepositoryProvider.InsertCaseActivity(activity);

                    business_case_DTO? workflowCase =
                        await BusinessCaseWorkflowRepositoryProvider.GetCaseById(caseId);

                    if (workflowCase == null)
                    {
                        continue;
                    }

                    List<xpdl_transition_DTO> transitions =
                        await XpdlWorkflowRepositoryProvider.GetXpdlTransitionsByCriteria(
                            activityId,
                            1
                        );

                    foreach (xpdl_transition_DTO transition in transitions)
                    {
                        if (transition.name != workflowCase.workflow_process_id)
                        {
                            continue;
                        }

                        List<xpdl_activity_DTO> subActivities = xpdlActivities
                            .Where(a => a.activity_id == transition.to_activity)
                            .ToList();

                        foreach (xpdl_activity_DTO xpdlSubActivity in subActivities)
                        {
                            if (xpdlSubActivity.task_type == "SubFlow")
                            {
                                business_activity_DTO subFlowActivity = new business_activity_DTO
                                {
                                    case_id = caseId,
                                    activity_id = xpdlSubActivity.activity_id,
                                    secuence = "002.001",
                                    display_name = xpdlSubActivity.display_name,
                                    name = xpdlSubActivity.name,
                                    date_processed = DateTime.Now,
                                    performer = xpdlSubActivity.performer,
                                    from_activity = transition.to_activity,
                                    task_form_type = xpdlSubActivity.task_form_type,
                                    task_form_uri = xpdlSubActivity.task_form_uri,
                                    status = "SubFlow InProgress"
                                };

                                await ActivityWorkflowRepositoryProvider.InsertCaseActivity(subFlowActivity);

                                List<xpdl_activity_DTO> startSubActivities = xpdlActivities
                                    .Where(a =>
                                        a.workflow_process_id == xpdlSubActivity.sub_flow_id &&
                                        a.name == "StartEvent"
                                    )
                                    .ToList();

                                foreach (xpdl_activity_DTO xpdlInfo in startSubActivities)
                                {
                                    business_activity_DTO startSubActivity = new business_activity_DTO
                                    {
                                        case_id = caseId,
                                        activity_id = xpdlInfo.activity_id,
                                        secuence = "003.001",
                                        display_name = xpdlInfo.display_name,
                                        name = xpdlInfo.name,
                                        status = "New",
                                        date_processed = DateTime.Now,
                                        performer = xpdlInfo.performer,
                                        from_activity = subFlowActivity.activity_id,
                                        task_form_type = xpdlInfo.task_form_type,
                                        task_form_uri = xpdlInfo.task_form_uri
                                    };

                                    await ActivityWorkflowRepositoryProvider.InsertCaseActivity(startSubActivity);
                                    activities.Add(startSubActivity);
                                }
                            }
                            else
                            {
                                business_activity_DTO normalActivity = new business_activity_DTO
                                {
                                    case_id = caseId,
                                    activity_id = xpdlSubActivity.activity_id,
                                    secuence = "002.001",
                                    display_name = xpdlSubActivity.display_name,
                                    name = xpdlSubActivity.name,
                                    status = "New",
                                    date_processed = DateTime.Now,
                                    performer = xpdlSubActivity.performer,
                                    from_activity = activityId,
                                    task_form_type = xpdlSubActivity.task_form_type,
                                    task_form_uri = xpdlSubActivity.task_form_uri
                                };

                                await LoadTransitionsToActivity(normalActivity);
                                await ActivityWorkflowRepositoryProvider.InsertCaseActivity(normalActivity);

                                activities.Add(normalActivity);
                            }
                        }
                    }
                }
                else if (xpdlActivity.name == "EndEvent")
                {
                    activity.status = "Completed";
                    await ActivityWorkflowRepositoryProvider.InsertCaseActivity(activity);

                    List<xpdl_activity_DTO> subFlowParents = xpdlActivities
                        .Where(a =>
                            a.sub_flow_id == xpdlActivity.workflow_process_id &&
                            a.task_type == "SubFlow"
                        )
                        .ToList();

                    if (subFlowParents.Count > 0)
                    {
                        List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
                        {
                            new NpgsqlParameter("@p_case_id", caseId),
                            new NpgsqlParameter("@p_activity_id", subFlowParents[0].activity_id)
                        };

                        List<business_activity_DTO> parentActivities =
                            await GetActivities(
                                "case_id = @p_case_id AND activity_id = @p_activity_id",
                                parameters,
                                string.Empty
                            );

                        if (parentActivities.Count > 0)
                        {
                            business_activity_DTO parentActivity = parentActivities[0];

                            string? nextActivity =
                                parentActivity.transitions != null &&
                                parentActivity.transitions.Count > 0
                                    ? parentActivity.transitions[0].to_activity
                                    : string.Empty;

                            await CompleteActivity(parentActivity, nextActivity);

                            await CreateActivities(
                                caseId,
                                parentActivity.activity_id!,
                                nextActivity!
                            );
                        }
                    }
                }
                else
                {
                    activity.status = "New";
                    await ActivityWorkflowRepositoryProvider.InsertCaseActivity(activity);

                    activities.Add(activity);
                }
            }

            return activities;
        }

        public async Task<business_activity_DTO?> GetActivity(string activityId)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("@p_activity_id", activityId)
            };

            List<business_activity_DTO> activities =
                await GetActivities(
                    "activity_id = @p_activity_id",
                    parameters,
                    string.Empty
                );

            return activities.FirstOrDefault();
        }

        public async Task<business_activity_DTO?> GetActivity(
            long caseId,
            string activityId
        )
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("@p_case_id", caseId),
                new NpgsqlParameter("@p_activity_id", activityId)
            };

            List<business_activity_DTO> activities =
                await GetActivities(
                    "case_id = @p_case_id AND activity_id = @p_activity_id",
                    parameters,
                    string.Empty
                );

            return activities.FirstOrDefault();
        }

        public async Task ReprocessingActivities(long caseId)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("@p_case_id", caseId)
            };

            List<business_activity_DTO> activities =
                await GetActivities(
                    "case_id = @p_case_id AND status IN('Suspended', 'SubFlow InProgress')",
                    parameters,
                    string.Empty
                );

            foreach (business_activity_DTO activity in activities)
            {
                await UpdateCaseActivityStatus(
                    "Reprocess",
                    activity.case_id,
                    activity.activity_id!,
                    activity.to_activity
                );
            }
        }

        public async Task<List<business_activity_DTO>> GetActivities(string criteria, List<NpgsqlParameter> parameters, string? orderBy)
        {
            List<business_activity_DTO> activities = await ActivityWorkflowRepositoryProvider.GetActivitiesByCriteria(criteria, parameters, orderBy) ?? new List<business_activity_DTO>();

            foreach (business_activity_DTO activity in activities)
            {
                await LoadTransitionsToActivity(activity);
            }

            return activities;
        }

        public async Task<List<business_activity_DTO>> GetActivities(long caseId)
        {
            List<business_activity_DTO> activities =
                await ActivityWorkflowRepositoryProvider.GetActivitiesByCaseId(caseId);

            foreach (business_activity_DTO activity in activities)
            {
                await LoadTransitionsToActivity(activity);
            }

            return activities;
        }

        public async Task<List<xpdl_activity_DTO>> LoadAllXpdlActivities()
        {
            return await XpdlWorkflowRepositoryProvider.GetXpdlActivities();
        }

        private async Task<bool> CheckActivity(long caseId, string? fromActivity, string? activityId)
        {
            if (string.IsNullOrWhiteSpace(activityId))
            {
                return false;
            }

            /*
             * Regla Workflow:
             * - Si la actividad destino ya está activa, no se vuelve a crear.
             * - Si la actividad destino viene por la transición seleccionada desde la actividad actual,
             *   se debe permitir crearla aunque existan otras transiciones entrantes desde ramas paralelas.
             * - Solo se conserva la validación histórica de "todas las entradas completadas"
             *   como fallback cuando no se puede confirmar que el destino corresponde a la rama actual.
             *
             * Esto evita que una rama paralela quede solo como Completed sin crear su siguiente actividad.
             * Ejemplo:
             * Datos Operación -> Parallel -> Registrar Tasación / Asignar Estudio Títulos
             * Registrar Tasación -> Generar Estudio Títulos
             */
            List<NpgsqlParameter> targetParameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("@p_case_id", caseId),
                new NpgsqlParameter("@p_activity_id", activityId)
            };

            List<business_activity_DTO> targetActivities =
                await GetActivities(
                    "case_id = @p_case_id AND activity_id = @p_activity_id",
                    targetParameters,
                    string.Empty
                );

            bool existsActiveTarget = targetActivities.Any(activity =>
                string.Equals(activity.status, "New", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(activity.status, "InProgress", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(activity.status, "SubFlow InProgress", StringComparison.OrdinalIgnoreCase)
            );

            if (existsActiveTarget)
            {
                return false;
            }

            List<xpdl_transition_DTO> fromTransitions =
                await XpdlWorkflowRepositoryProvider.GetXpdlTransitionsByCriteria(
                    activityId,
                    2
                );

            bool isDirectTransitionFromCurrentActivity = fromTransitions.Any(transition =>
                string.Equals(transition.from_activity, fromActivity, StringComparison.OrdinalIgnoreCase)
            );

            if (isDirectTransitionFromCurrentActivity)
            {
                return true;
            }

            bool canCreate = true;

            foreach (xpdl_transition_DTO fromTransition in fromTransitions)
            {
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter("@p_case_id", caseId),
                    new NpgsqlParameter("@p_activity_id", fromTransition.from_activity)
                };

                List<business_activity_DTO> activities =
                    await GetActivities(
                        "case_id = @p_case_id AND activity_id = @p_activity_id",
                        parameters,
                        string.Empty
                    );

                foreach (business_activity_DTO activity in activities)
                {
                    if (!string.Equals(activity.status, "Completed", StringComparison.OrdinalIgnoreCase))
                    {
                        canCreate = false;
                    }
                }
            }

            return canCreate;
        }

        private async Task LoadSubActivities(
            long caseId,
            string fromActivity,
            List<business_activity_DTO> activities,
            List<xpdl_activity_DTO> xpdlActivities,
            xpdl_activity_DTO xpdlActivity
        )
        {
            List<xpdl_activity_DTO> startActivities = xpdlActivities
                .Where(a =>
                    a.workflow_process_id == xpdlActivity.sub_flow_id &&
                    a.name == "StartEvent"
                )
                .ToList();

            foreach (xpdl_activity_DTO xpdlInfo in startActivities)
            {
                business_activity_DTO activity = new business_activity_DTO
                {
                    case_id = caseId,
                    activity_id = xpdlInfo.activity_id,
                    secuence = "002.001",
                    display_name = xpdlInfo.display_name,
                    name = xpdlInfo.name,
                    status = "New",
                    date_processed = DateTime.Now,
                    performer = xpdlInfo.performer,
                    from_activity = fromActivity,
                    task_form_type = xpdlInfo.task_form_type,
                    task_form_uri = xpdlInfo.task_form_uri
                };

                await LoadTransitionsToActivity(activity);
                await ActivityWorkflowRepositoryProvider.InsertCaseActivity(activity);

                activities.Add(activity);
            }
        }

        private async Task LoadActivityTransitions(
            long caseId,
            string activityId,
            List<business_activity_DTO> activities,
            List<xpdl_activity_DTO> xpdlActivities,
            List<xpdl_transition_DTO> transitions
        )
        {
            foreach (xpdl_transition_DTO transition in transitions)
            {
                List<xpdl_activity_DTO> subActivities = xpdlActivities
                    .Where(a => a.activity_id == transition.to_activity)
                    .ToList();

                foreach (xpdl_activity_DTO xpdlSubActivity in subActivities)
                {
                    if (xpdlSubActivity.task_type == "SubFlow")
                    {
                        business_activity_DTO subActivity = new business_activity_DTO
                        {
                            case_id = caseId,
                            activity_id = xpdlSubActivity.activity_id,
                            secuence = "002.001",
                            display_name = xpdlSubActivity.display_name,
                            name = xpdlSubActivity.name,
                            date_processed = DateTime.Now,
                            performer = xpdlSubActivity.performer,
                            from_activity = transition.to_activity,
                            task_form_type = xpdlSubActivity.task_form_type,
                            task_form_uri = xpdlSubActivity.task_form_uri,
                            status = "SubFlow InProgress"
                        };

                        await ActivityWorkflowRepositoryProvider.InsertCaseActivity(subActivity);

                        await LoadSubParallelActivities(
                            caseId,
                            activities,
                            xpdlActivities,
                            xpdlSubActivity,
                            subActivity
                        );
                    }
                    else
                    {
                        business_activity_DTO activity = new business_activity_DTO
                        {
                            case_id = caseId,
                            activity_id = xpdlSubActivity.activity_id,
                            secuence = "002.001",
                            display_name = xpdlSubActivity.display_name,
                            name = xpdlSubActivity.name,
                            status = "New",
                            date_processed = DateTime.Now,
                            performer = xpdlSubActivity.performer,
                            from_activity = activityId,
                            task_form_type = xpdlSubActivity.task_form_type,
                            task_form_uri = xpdlSubActivity.task_form_uri
                        };

                        await LoadTransitionsToActivity(activity);
                        await ActivityWorkflowRepositoryProvider.InsertCaseActivity(activity);

                        activities.Add(activity);
                    }
                }
            }
        }

        private async Task LoadSubParallelActivities(
            long caseId,
            List<business_activity_DTO> activities,
            List<xpdl_activity_DTO> xpdlActivities,
            xpdl_activity_DTO xpdlSubActivity,
            business_activity_DTO subActivity
        )
        {
            List<xpdl_activity_DTO> startActivities = xpdlActivities
                .Where(a =>
                    a.workflow_process_id == xpdlSubActivity.sub_flow_id &&
                    a.name == "StartEvent"
                )
                .ToList();

            foreach (xpdl_activity_DTO xpdlInfo in startActivities)
            {
                business_activity_DTO activity = new business_activity_DTO
                {
                    case_id = caseId,
                    activity_id = xpdlInfo.activity_id,
                    secuence = "003.001",
                    display_name = xpdlInfo.display_name,
                    name = xpdlInfo.name,
                    status = "New",
                    date_processed = DateTime.Now,
                    performer = xpdlInfo.performer,
                    from_activity = subActivity.activity_id,
                    task_form_type = xpdlInfo.task_form_type,
                    task_form_uri = xpdlInfo.task_form_uri
                };

                await ActivityWorkflowRepositoryProvider.InsertCaseActivity(activity);

                activities.Add(activity);
            }
        }

        private async Task CompleteActivity(
            business_activity_DTO activity,
            string? toActivity
        )
        {
            await ActivityWorkflowRepositoryProvider.UpdateCaseActivityStatus(
                "Completed",
                activity.case_id,
                activity.activity_id!,
                toActivity
            );
        }

        public async Task<List<business_activity_DTO>> LoadNextActivities(long caseId)
        {
            List<business_activity_DTO> activities =
                await ActivityWorkflowRepositoryProvider.GetActivitiesByCaseId(caseId);

            List<business_activity_DTO> nextActivities = activities
                .Where(activity =>
                    string.Equals(activity.status, "New", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(activity.status, "SubFlow InProgress", StringComparison.OrdinalIgnoreCase)
                )
                .ToList();

            foreach (business_activity_DTO activity in nextActivities)
            {
                await LoadTransitionsToActivity(activity);
            }

            return nextActivities;
        }

        private async Task LoadTransitionsToActivity(business_activity_DTO activity)
        {
            if (activity == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(activity.activity_id))
            {
                activity.transitions = new List<xpdl_transition_DTO>();
                return;
            }

            activity.transitions = await XpdlWorkflowRepositoryProvider.GetXpdlTransitionsByCriteria(activity.activity_id, 1) ?? new List<xpdl_transition_DTO>();
        }
    }
}