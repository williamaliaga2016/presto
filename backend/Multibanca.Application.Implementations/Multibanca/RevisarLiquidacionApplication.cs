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
    public class RevisarLiquidacionApplication : MultibancaGenericApplication<revisar_liquidacion, revisar_liquidacion_entity, IRevisarLiquidacionRepository>, IRevisarLiquidacionApplication
    {
        private readonly IRevisarLiquidacionRepository RevisarLiquidacionRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public RevisarLiquidacionApplication(
            MultibancaDBContext _multibancaDBContext,
            IRevisarLiquidacionRepository _revisarLiquidacionRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _revisarLiquidacionRepository, _mapper)
        {
            RevisarLiquidacionRepositoryProvider = _revisarLiquidacionRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<revisar_liquidacion?> GetByExpediente(long id_expediente)
        {
            var entity = await RevisarLiquidacionRepositoryProvider.GetByExpediente(id_expediente);
            if (entity != null)
                entity.id_expediente = id_expediente;
            return Mapper.Map<revisar_liquidacion?>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            var entity = await RevisarLiquidacionRepositoryProvider.GetByExpediente(expediente_id);

            if (entity == null)
                throw new InvalidOperationException($"No existe registro de Revisar Liquidacion para el expediente {expediente_id}.");

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);

            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);

            string transitionsID;
            if (entity.is_enviar_reparo)
            {
                transitionsID = listTransitions
                    .Where(x => x.name == "RevisarLiquidacion_CorregirReparoLiquidacion_SI")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }
            else
            {
                transitionsID = listTransitions
                    .Where(x => x.name == "RevisarLiquidacion_RevisarDesembolso_NO")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException("No se encontró transición configurada para Revisar Liquidación.");
            }

            return await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
        }
    }
}
