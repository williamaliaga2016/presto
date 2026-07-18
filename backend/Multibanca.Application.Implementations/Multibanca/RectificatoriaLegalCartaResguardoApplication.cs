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
    public class RectificatoriaLegalCartaResguardoApplication
        : MultibancaGenericApplication<rectificatoria_legal_carta_resguardo, rectificatoria_legal_carta_resguardo_entity, IRectificatoriaLegalCartaResguardoRepository>,
          IRectificatoriaLegalCartaResguardoApplication
    {
        private readonly IRectificatoriaLegalCartaResguardoRepository RectificatoriaLegalCartaResguardoRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public RectificatoriaLegalCartaResguardoApplication(
            MultibancaDBContext _multibancaDBContext,
            IRectificatoriaLegalCartaResguardoRepository _rectificatoriaLegalCartaResguardoRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _rectificatoriaLegalCartaResguardoRepository, _mapper)
        {
            RectificatoriaLegalCartaResguardoRepositoryProvider = _rectificatoriaLegalCartaResguardoRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<rectificatoria_legal_carta_resguardo?> GetByExpediente(long id_expediente)
        {
            var entity = await RectificatoriaLegalCartaResguardoRepositoryProvider.GetByExpediente(id_expediente);
            if (entity != null)
                entity.id_expediente = id_expediente;
            return Mapper.Map<rectificatoria_legal_carta_resguardo?>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);
            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);
            string transitionsID = listTransitions.Select(q => q.transition_id).FirstOrDefault() ?? "";
            List<AssignActivityDTO> listAssignActivityDTO = new List<AssignActivityDTO>();

            listAssignActivityDTO = await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
            return listAssignActivityDTO;
        }
    }
}
