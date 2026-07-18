using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Implementations.Repositories.Multibanca;
using Data.Repository.Interfaces;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Framework.WorkFlow.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class RealizarControlCreditoApplication : MultibancaGenericApplication<realizar_control_credito, realizar_control_credito_entity, IRealizarControlCreditoRepository>, IRealizarControlCreditoApplication
    {
        private readonly IRealizarControlCreditoRepository RealizarControlCreditoRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public RealizarControlCreditoApplication(MultibancaDBContext _multibancaDBContext, IRealizarControlCreditoRepository _realizarControlCreditoRepository, IWorkflowApplication _workflowApplicationProvider, IMapper _mapper) 
            : base(_multibancaDBContext, _realizarControlCreditoRepository, _mapper)
        {
            RealizarControlCreditoRepositoryProvider = _realizarControlCreditoRepository;
            WorkflowApplicationProvider = _workflowApplicationProvider;
            Mapper = _mapper;
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long id_expediente, int id_usuario, string id_actividad)
        {
            var entity = await RealizarControlCreditoRepositoryProvider.GetByExpediente(id_expediente);
            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Realizar Control de Crédito para el expediente {id_expediente}. Debe guardar la actividad antes de avanzar.");
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(id_expediente, id_actividad);
            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(id_actividad);

            string transitionsID;
            if (entity.enviar_reparo)
            {
                transitionsID = listTransitions
                    .Where(x => x.name == "RealizarControlCredito_CorregirReparoControlCredito_ReparoSI")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }
            else
            {
                transitionsID = listTransitions
                    .Where(x => x.name == "RealizarControlCredito_RecepcionarMatriz_ReparoNO")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }

            return await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, id_usuario);
        }

        public async Task<realizar_control_credito?> GetByExpediente(long id_expediente)
        {
            var entity = await RealizarControlCreditoRepositoryProvider.GetByExpediente(id_expediente);
            if (entity != null)
                entity.id_expediente = id_expediente;
            return Mapper.Map<realizar_control_credito?>(entity);
        }
    }
}
