using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class ControlEscrituraApplication : MultibancaGenericApplication<control_escritura, control_escritura_entity, IControlEscrituraRepository>, IControlEscrituraApplication
    {
        private readonly IControlEscrituraRepository ControlEscrituraRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public ControlEscrituraApplication(
            MultibancaDBContext _multibancaDBContext,
            IControlEscrituraRepository _controlEscrituraRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _controlEscrituraRepository, _mapper)
        {
            ControlEscrituraRepositoryProvider = _controlEscrituraRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<control_escritura?> GetByExpediente(long id_expediente)
        {
            var entity = await ControlEscrituraRepositoryProvider.GetByExpediente(id_expediente);
            if (entity != null)
                entity.id_expediente = id_expediente;
            return Mapper.Map<control_escritura?>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);
            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);
            var entity = await ControlEscrituraRepositoryProvider.GetByExpediente(expediente_id);

            if (entity == null)
                throw new InvalidOperationException($"No existe registro de Control de Escritura para el expediente {expediente_id}.");

            string transitionsID;
            if (entity.is_enviar_reparo)
            {
           
                transitionsID = listTransitions
                    .Where(x => x.name == "ControlEscritura_CorregirControlEscritura_ReparoSI")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }
            else
            {
          
                transitionsID = listTransitions
                    .Where(x => x.name == "ControlEscritura_EndEvent_ReparoNO")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }

            List<AssignActivityDTO> listAssignActivityDTO = await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
            return listAssignActivityDTO;
        }
    }
}
