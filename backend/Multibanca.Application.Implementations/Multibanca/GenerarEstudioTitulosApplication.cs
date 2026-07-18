using AutoMapper;
using Common.Application.Implementations;
using Common.Application.Interfaces;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Multibanca.Application.Interfaces.Workflow;
using Framework.WorkFlow.Common.DTO;

using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class GenerarEstudioTitulosApplication:MultibancaGenericApplication<generar_estudio_titulos, generar_estudio_titulos_entity, IGenerarEstudioTitulosRepository>,IGenerarEstudioTitulosApplication
    {
        private readonly IGenerarEstudioTitulosRepository GenerarEstudioTitulosRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;
        public GenerarEstudioTitulosApplication(
            MultibancaDBContext multibancaDBContext,
            IGenerarEstudioTitulosRepository generarEstudioTitulosRepositoryProvider,
            IWorkflowApplication workflowApplicationProvider,
            IMapper mapper) : base(multibancaDBContext, generarEstudioTitulosRepositoryProvider, mapper)
        {
            GenerarEstudioTitulosRepositoryProvider = generarEstudioTitulosRepositoryProvider;
            WorkflowApplicationProvider = workflowApplicationProvider;
            Mapper = mapper;
        }
        public async Task<generar_estudio_titulos?> GetByExpediente(long id_expediente)
        {
            var entity = await GenerarEstudioTitulosRepositoryProvider.GetByExpediente(id_expediente);
            return Mapper.Map<generar_estudio_titulos?>(entity);
        }
        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);
            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);
            var entity = await GenerarEstudioTitulosRepositoryProvider.GetByExpediente(expediente_id);

            bool envioReparo = entity.enviar_reparo;

            string transitionsID;

            if (envioReparo)
            {
                transitionsID = listTransitions
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }
            else
            {
                transitionsID = listTransitions
                    .Where(x =>
                        x.name == "GenerarEstudioTitulos_VerificarReparoEstudioTitulos_ReparoNO")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }

            List<AssignActivityDTO> listAssignActivityDTO = new List<AssignActivityDTO>();

            listAssignActivityDTO = await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
            return listAssignActivityDTO;
        }
    }
}
