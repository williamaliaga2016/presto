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
    public class RevisarInscripcionCbrApplication : MultibancaGenericApplication<revisar_inscripcion_cbr, revisar_inscripcion_cbr_entity, IRevisarInscripcionCbrRepository>, IRevisarInscripcionCbrApplication
    {
        private readonly IRevisarInscripcionCbrRepository RevisarInscripcionCbrRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public RevisarInscripcionCbrApplication(
            MultibancaDBContext _multibancaDBContext,
            IRevisarInscripcionCbrRepository _revisarInscripcionCbrRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _revisarInscripcionCbrRepository, _mapper)
        {
            RevisarInscripcionCbrRepositoryProvider = _revisarInscripcionCbrRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<revisar_inscripcion_cbr?> GetByExpediente(long id_expediente)
        {
            var entity = await RevisarInscripcionCbrRepositoryProvider.GetByExpediente(id_expediente);
            if (entity != null)
                entity.id_expediente = id_expediente;
            return Mapper.Map<revisar_inscripcion_cbr?>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            var entity = await RevisarInscripcionCbrRepositoryProvider.GetByExpediente(expediente_id);

            if (entity == null)
            {
                throw new InvalidOperationException($"No existe registro de Revisar Inscripción CBR para el expediente {expediente_id}.");
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);

            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);

            string transitionsID;
            if (entity.is_enviar_reparo)
            {
                transitionsID = listTransitions
                    .Where(x => x.name == "RevisarInscripcionCBR_ReingresarEscrituraCBR_SI")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }
            else
            {
                transitionsID = listTransitions
                    .Where(x => x.name == "RevisarInscripcionCBR_GenerarFiniquito_NO")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException("No se encontró transición configurada para Revisar Inscripción CBR.");
            }

            return await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
        }
    }
}
