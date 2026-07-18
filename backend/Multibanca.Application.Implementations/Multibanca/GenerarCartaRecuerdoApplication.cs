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
    public class GenerarCartaRecuerdoApplication : MultibancaGenericApplication<generar_carta_resguardo, generar_carta_resguardo_entity, IGenerarCartaResguardoRepository>, IGenerarCartaResguardoApplication
    {
        private readonly IGenerarCartaResguardoRepository GenerarCartaResguardoRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public GenerarCartaRecuerdoApplication(
            MultibancaDBContext _multibancaDBContext,
            IGenerarCartaResguardoRepository _generarCartaResguardoRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _generarCartaResguardoRepository, _mapper)
        {
            GenerarCartaResguardoRepositoryProvider = _generarCartaResguardoRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<generar_carta_resguardo?> GetByExpediente(long id_expediente)
        {
            var entity = await GenerarCartaResguardoRepositoryProvider.GetByExpediente(id_expediente);
            if (entity != null)
                entity.id_expediente = id_expediente;
            return Mapper.Map<generar_carta_resguardo?>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long id_expediente, int id_usuario, string id_actividad)
        {
            var entity = await GenerarCartaResguardoRepositoryProvider.GetByExpediente(id_expediente);
            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Generar Carta Resguardo para el expediente {id_expediente}. Debe guardar la actividad antes de avanzar.");
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(id_expediente, id_actividad);
            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(id_actividad);

            string transitionsID;


            bool envioReparo = entity.enviar_a_reparo ?? false;
            if (envioReparo)
            {
                transitionsID = listTransitions
                    .Where(x => x.name == "GenerarCartaResguardo_CorregirCartaResguardo_ReparoSI")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }
            else
            {
                transitionsID = listTransitions
                    .Where(x => x.name == "GenerarCartaResguardo_RealizarAprobacionComercialLegalCdr_ReparoNO")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }

            return await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, id_usuario);
        }

    }
}
