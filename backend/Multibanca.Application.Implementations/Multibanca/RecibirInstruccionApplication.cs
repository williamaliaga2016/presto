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
    public class RecibirInstruccionApplication : MultibancaGenericApplication<recibir_instruccion_pago, recibir_instruccion_pago_entity, IRecibirInstruccionPagoRepository>, IRecibirInstruccionPagoApplication
    {
        private readonly IRecibirInstruccionPagoRepository RecibirInstruccionPagoRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public RecibirInstruccionApplication(
            MultibancaDBContext _multibancaDBContext,
            IRecibirInstruccionPagoRepository _recibirInstruccionPagoRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _recibirInstruccionPagoRepository, _mapper)
        {
            RecibirInstruccionPagoRepositoryProvider = _recibirInstruccionPagoRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<recibir_instruccion_pago?> GetByExpediente(long id_expediente)
        {
            var entity = await RecibirInstruccionPagoRepositoryProvider.GetByExpediente(id_expediente);
            if (entity != null)
                entity.id_expediente = id_expediente;
            return Mapper.Map<recibir_instruccion_pago?>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            var entity = await RecibirInstruccionPagoRepositoryProvider.GetByExpediente(expediente_id);

            if (entity == null)
            {
                throw new InvalidOperationException($"No existe registro de Recibir Instrucción de Pago para el expediente {expediente_id}.");
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);

            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);

            bool envioReparo = entity.enviar_a_reparo ?? false;

            string transitionsID;
            if (envioReparo)
            {
                transitionsID = listTransitions
                    .Where(x => x.name == "RecibirInstruccionPago_CorregirReparoInstPago_SI")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }
            else
            {
                transitionsID = listTransitions
                    .Where(x => x.name == "RecibirInstruccionPago_RevisarLiquidacion_NO")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException("No se encontró transición configurada para Recibir Instrucción de Pago.");
            }

            return await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
        }
    }
}
