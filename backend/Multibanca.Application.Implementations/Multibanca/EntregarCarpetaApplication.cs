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
    public class EntregarCarpetaApplication : MultibancaGenericApplication<entregar_carpeta, entregar_carpeta_entity, IEntregarCarpetaRepository>, IEntregarCarpetaApplication
    {
        private readonly IEntregarCarpetaRepository EntregarCarpetaRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public EntregarCarpetaApplication(
            MultibancaDBContext _multibancaDBContext,
            IEntregarCarpetaRepository _entregarCarpetaRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _entregarCarpetaRepository, _mapper)
        {
            EntregarCarpetaRepositoryProvider = _entregarCarpetaRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<entregar_carpeta?> GetByExpediente(long id_expediente)
        {
            var entity = await EntregarCarpetaRepositoryProvider.GetByExpediente(id_expediente);
            if (entity != null)
                entity.id_expediente = id_expediente;
            return Mapper.Map<entregar_carpeta?>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);
            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);
            var entity = await EntregarCarpetaRepositoryProvider.GetByExpediente(expediente_id);
            if (entity == null)
            {
                throw new InvalidOperationException($"No existe registro de Entregar Carpeta para el expediente {expediente_id}.");
            }

            bool envioReparo = entity.enviar_a_reparo ?? false;

            string transitionsID;
            if (envioReparo)
            {
                transitionsID = listTransitions
                    .Where(x => x.name == "EntregarCarpeta_CorregirReparoEntregarCarpeta_ReparoSI")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;

            }
            else
            {
                transitionsID = listTransitions
                    .Where(x => x.name == "EntregarCarpeta_ControlEscritura_ReparoNo")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }
            List<AssignActivityDTO> listAssignActivityDTO = new List<AssignActivityDTO>();

            listAssignActivityDTO = await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
            return listAssignActivityDTO;
        }
    }
}
