using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Framework.WorkFlow.Common.DTO;

using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Domain.Models.Multibanca;
using Syncfusion.DocIO.DLS;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class CierreCopiasNotariaApplication : MultibancaGenericApplication<cierre_copias_notaria, cierre_copias_notaria_entity, ICierreCopiasNotariaRepository>, ICierreCopiasNotariaApplication
    {
        private readonly ICierreCopiasNotariaRepository CierreCopiasNotariaRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public CierreCopiasNotariaApplication(
            MultibancaDBContext _multibancaDBContext,
            ICierreCopiasNotariaRepository _cierreCopiasNotariaRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _cierreCopiasNotariaRepository, _mapper)
        {
            CierreCopiasNotariaRepositoryProvider = _cierreCopiasNotariaRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<cierre_copias_notaria?> GetByExpediente(long id_expediente)
        {
            var entity = await CierreCopiasNotariaRepositoryProvider.GetByExpediente(id_expediente);
            if (entity != null)
                entity.id_expediente = id_expediente;
            return Mapper.Map<cierre_copias_notaria?>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long id_expediente, int id_usuario, string id_actividad)
        {
            var entity = await CierreCopiasNotariaRepositoryProvider.GetByExpediente(id_expediente);
            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Realizar Cierre y Copias Notaria para el expediente {id_expediente}. Debe guardar la actividad antes de avanzar.");
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(id_expediente, id_actividad);
            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(id_actividad);

            string transitionsID;
            bool envioReparo = entity.enviar_a_reparo ?? false;
            if (envioReparo)
            {
                transitionsID = listTransitions
                    .Where(x => x.name == "RealizarCierreCopiasNotaria_CorregirReparoCierreCopiasNotaria_ReparoSI")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }
            else
            {
                transitionsID = listTransitions
                    .Where(x => x.name == "RealizarCierreCopiasNotaria_RevisarCopiasEscrituras_ReparoNO")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }

            return await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, id_usuario);
        }

    }
}
