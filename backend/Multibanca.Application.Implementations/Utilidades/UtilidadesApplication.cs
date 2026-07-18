using AutoMapper;
using Data.Repository.Interfaces.Repositories.Utilidades;
using Multibanca.Application.Interface.Utilidades;
using Multibanca.DTO.Common;
using Multibanca.DTO.Utilidades;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Domain.Models.Multibanca;
using Data.Repository.Interfaces.Entities.Multibanca;

namespace Multibanca.Application.Implementation.Utilidades
{
    public class UtilidadesApplication : IUtilidadesApplication
    {
        private readonly IUtilidadesRepository UtilidadesRepositoryProvider;
        private readonly IActividadesRepository ActividadesRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public UtilidadesApplication(
            IUtilidadesRepository utilidadesRepository,
            IActividadesRepository actividadesRepository,
            IWorkflowApplication workflowApplication,
            IMapper mapper)
        {
            UtilidadesRepositoryProvider = utilidadesRepository;
            ActividadesRepositoryProvider = actividadesRepository;
            WorkflowApplicationProvider = workflowApplication;
            Mapper = mapper;
        }

        public async Task<List<ControlBaseDTO>> GetActivities(long idExpediente)
        {
            List<ControlBaseDTO> listControls = new List<ControlBaseDTO>();

            List<actividades> listActivities = Mapper.Map<List<actividades>>(
                await ActividadesRepositoryProvider.GetCurrentActivitiesByIdExpediente(idExpediente)
            );

            foreach (var activity in listActivities)
            {
                ControlBaseDTO controlBase = new ControlBaseDTO();
                controlBase.idBig = activity.id;
                controlBase.description = activity.descripcion;
                listControls.Add(controlBase);
            }

            return listControls;
        }

        public async Task<int> Validate_RequestNumber(long idExpediente)
        {
            return await UtilidadesRepositoryProvider.Validate_RequestNumber(idExpediente);
        }

        public async Task<List<ControlBaseDTO>> GetActivitiesUsers(long idExpediente)
        {
            return await UtilidadesRepositoryProvider.GetActivitiesUsers(idExpediente);
        }

        public async Task<List<ControlBaseDTO>> GetUsersActivities(long idExpediente, long idActividad)
        {
            return await UtilidadesRepositoryProvider.GetUsersActivities(idExpediente, idActividad);
        }

        public async Task<bool> Save_CancelActivity(long idExpediente)
        {
            bool result = await WorkflowApplicationProvider.CancelCase(idExpediente);
            bool band = false;

            if (result)
            {
                List<actividades> listActivities = Mapper.Map<List<actividades>>(
                    await ActividadesRepositoryProvider.GetCurrentActivitiesByIdExpediente(idExpediente)
                );

                foreach (var activity in listActivities)
                {
                    if (activity.status == "Nueva" || activity.status == "En progreso")
                    {
                        activity.status = "Desistido";
                        activity.fecha_cancelacion = DateTime.Now;

                        await ActividadesRepositoryProvider.InsertActividad(
                            Mapper.Map<actividades_entity>(activity)
                        );

                        band = true;
                    }
                }
            }

            return band;
        }

        public async Task<bool> Save_DeleteReassign(UtilidadesDTO util, int idUsuario)
        {
            return await UtilidadesRepositoryProvider.Save_DeleteReassign(util, idUsuario);
        }

        public async Task<ReassignmentWithdrawalDTO> GetMessageUtility(int opcion, long idExpediente, int idUsuario, int? idUsuarioReasignado, long? idActividad)
        {
            return await UtilidadesRepositoryProvider.GetMessageUtility(opcion, idExpediente, idUsuario, idUsuarioReasignado, idActividad);
        }
    }
}
