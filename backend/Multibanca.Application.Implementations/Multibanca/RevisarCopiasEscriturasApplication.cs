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
    public class RevisarCopiasEscriturasApplication : MultibancaGenericApplication<revisar_copias_escrituras, revisar_copias_escrituras_entity, IRevisarCopiasEscriturasRepository>, IRevisarCopiasEscriturasApplication
    {
        private readonly IRevisarCopiasEscriturasRepository RevisarCopiasEscriturasRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public RevisarCopiasEscriturasApplication(
            MultibancaDBContext _multibancaDBContext,
            IRevisarCopiasEscriturasRepository _revisarCopiasEscriturasRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _revisarCopiasEscriturasRepository, _mapper)
        {
            RevisarCopiasEscriturasRepositoryProvider = _revisarCopiasEscriturasRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<revisar_copias_escrituras?> GetByExpediente(long id_expediente)
        {
            var entity = await RevisarCopiasEscriturasRepositoryProvider.GetByExpediente(id_expediente);
            if (entity != null)
                entity.id_expediente = id_expediente;
            return Mapper.Map<revisar_copias_escrituras?>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long id_expediente, int id_usuario, string id_actividad)
        {
            var entity = await RevisarCopiasEscriturasRepositoryProvider.GetByExpediente(id_expediente);
            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Revisar Copias de Escrituras para el expediente {id_expediente}. Debe guardar la actividad antes de avanzar.");
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(id_expediente, id_actividad);
            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(id_actividad);

            string transitionsID;


            bool envioReparo = entity.enviar_a_reparo ?? false;
            if (envioReparo)
            {
                transitionsID = listTransitions
                    .Where(x => x.name == "RevisarCopiasEscrituras_CorregirReparoCopiasEscrituras_ReparoSI")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }
            else
            {
                transitionsID = listTransitions
                    .Where(x => x.name == "RevisarCopiasEscrituras_ValorizarCBR_ReparoNo")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }

            return await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, id_usuario);
        }

    }
}
