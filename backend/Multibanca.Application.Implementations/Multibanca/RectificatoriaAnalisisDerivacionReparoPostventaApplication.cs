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
    public class RectificatoriaAnalisisDerivacionReparoPostventaApplication : MultibancaGenericApplication<rectificatoria_analisis_derivacion_reparo_postventa, rectificatoria_analisis_derivacion_reparo_postventa_entity, IRectificatoriaAnalisisDerivacionReparoPostventaRepository>, IRectificatoriaAnalisisDerivacionReparoPostventaApplication
    {
        private readonly IRectificatoriaAnalisisDerivacionReparoPostventaRepository RectificatoriaAnalisisDerivacionReparoPostventaRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public RectificatoriaAnalisisDerivacionReparoPostventaApplication(
            MultibancaDBContext _multibancaDBContext,
            IRectificatoriaAnalisisDerivacionReparoPostventaRepository _rectificatoriaAnalisisDerivacionReparoPostventaRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _rectificatoriaAnalisisDerivacionReparoPostventaRepository, _mapper)
        {
            RectificatoriaAnalisisDerivacionReparoPostventaRepositoryProvider = _rectificatoriaAnalisisDerivacionReparoPostventaRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<rectificatoria_analisis_derivacion_reparo_postventa?> GetByExpediente(long id_expediente)
        {
            var entity = await RectificatoriaAnalisisDerivacionReparoPostventaRepositoryProvider.GetByExpediente(id_expediente);
            if (entity != null)
                entity.id_expediente = id_expediente;
            return Mapper.Map<rectificatoria_analisis_derivacion_reparo_postventa?>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);
            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);
            var entity = await RectificatoriaAnalisisDerivacionReparoPostventaRepositoryProvider.GetByExpediente(expediente_id);
            if (entity == null)
            {
                throw new InvalidOperationException($"No existe registro de Rectificacion Analisis Derivacion Reparo PostVenta para el expediente {expediente_id}.");
            }

            string transitionsID;
            bool enviarReparo = entity.enviar_reparo_a != null
                    && entity.enviar_reparo_a == "01";

            string targetTransitionName = enviarReparo
                    ? "RectificatoriaAnalisisDerivaciónReparoPostventa_RectificatoriaAnalisisDerivacionReparoPostventaSolucionReparo_COMERCIAL"
                    : "RectificatoriaAnalisisDerivaciónReparoPostventa_ValidacionRectificatoriaLegalPostventa_LEGAL";

            transitionsID = listTransitions
                .Where(x => x.name == targetTransitionName)
                .Select(x => x.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Rectificacion Analisis Derivacion Reparo PostVenta."
                );
            }

            List<AssignActivityDTO> listAssignActivityDTO = new List<AssignActivityDTO>();

            listAssignActivityDTO = await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
            return listAssignActivityDTO;
        }
    }
}
