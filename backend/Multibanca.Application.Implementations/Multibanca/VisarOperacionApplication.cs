using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Multibanca.Application.Interfaces.Workflow;
using Framework.WorkFlow.Common.DTO;

using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class VisarOperacionApplication : MultibancaGenericApplication<visar_operacion, visar_operacion_entity, IVisarOperacionRepository>, IVisarOperacionApplication
    {
        private readonly IVisarOperacionRepository VisarOperacionRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public VisarOperacionApplication(
            MultibancaDBContext _multibancaDBContext,
            IVisarOperacionRepository _visarOperacionRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _visarOperacionRepository, _mapper)
        {
            VisarOperacionRepositoryProvider = _visarOperacionRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<visar_operacion?> GetByExpediente(long id_expediente)
        {
            var entity = await VisarOperacionRepositoryProvider.GetByExpediente(id_expediente);
            if (entity != null)
                entity.id_expediente = id_expediente;
            return Mapper.Map<visar_operacion?>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);
            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);
            var entity = await VisarOperacionRepositoryProvider.GetByExpediente(expediente_id);
            if (entity == null)
            {
                throw new InvalidOperationException($"No existe registro de Generar Borrador Escritura para el expediente {expediente_id}.");
            }

            bool envioReparo = entity.enviar_a_reparo ?? false;

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
                    .Where(x => x.name == "VisarOperacion_AsignarEscritura_ReparoNO")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }
            List<AssignActivityDTO> listAssignActivityDTO = new List<AssignActivityDTO>();

            listAssignActivityDTO = await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
            return listAssignActivityDTO;
        }
    }
}
