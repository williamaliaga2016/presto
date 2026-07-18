using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class AsignarEscrituraApplication : MultibancaGenericApplication<asignar_escritura, asignar_escritura_entity, IAsignarEscrituraRepository>, IAsignarEscrituraApplication
    {
        private readonly IAsignarEscrituraRepository AsignarEscrituraRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public AsignarEscrituraApplication(
            MultibancaDBContext _multibancaDBContext,
            IAsignarEscrituraRepository _asignarEscrituraRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _asignarEscrituraRepository, _mapper)
        {
            AsignarEscrituraRepositoryProvider = _asignarEscrituraRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<asignar_escritura?> GetByExpediente(long id_expediente)
        {
            var entity = await AsignarEscrituraRepositoryProvider.GetByExpedienteActividad(
                id_expediente,
                Constants.Actividades.AsignarEscritura);

            return Mapper.Map<asignar_escritura?>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);

            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);

            string transitionsID = listTransitions
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;

            List<AssignActivityDTO> listAssignActivityDTO =
                await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);

            return listAssignActivityDTO;
        }

    }
}
