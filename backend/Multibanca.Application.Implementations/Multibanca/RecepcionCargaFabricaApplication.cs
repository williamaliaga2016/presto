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
    public class RecepcionCargaFabricaApplication : MultibancaGenericApplication<recepcion_carga_fabrica, recepcion_carga_fabrica_entity, IRecepcionCargaFabricaRepository>, IRecepcionCargaFabricaApplication
    {
        private readonly IRecepcionCargaFabricaRepository RecepcionCargaFabricaRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public RecepcionCargaFabricaApplication(
            MultibancaDBContext _multibancaDBContext,
            IRecepcionCargaFabricaRepository _recepcionCargaFabricaRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _recepcionCargaFabricaRepository, _mapper)
        {
            RecepcionCargaFabricaRepositoryProvider = _recepcionCargaFabricaRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<recepcion_carga_fabrica?> GetByExpediente(long id_expediente)
        {
            var entity = await RecepcionCargaFabricaRepositoryProvider.GetByExpediente(id_expediente);
            if (entity != null)
                entity.id_expediente = id_expediente;
            return Mapper.Map<recepcion_carga_fabrica?>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);
            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);
            var entity = await RecepcionCargaFabricaRepositoryProvider.GetByExpediente(expediente_id);
            string transitionsID = string.Empty;
            if (!entity.is_enviar_reparo)
            {
                transitionsID = listTransitions.Where(q => q.name == "RecepcionCargaFabrica_DatosOperacion_NO_enviarReparo").Select(q => q.transition_id).FirstOrDefault() ?? "";
            }
            else
            {
                transitionsID = listTransitions.Where(q => q.name == "RecepcionCargaFabrica_CorregirReparoFabrica_enviarReparo").Select(q => q.transition_id).FirstOrDefault() ?? "";
            }
            List<AssignActivityDTO> listAssignActivityDTO = new List<AssignActivityDTO>();

            listAssignActivityDTO = await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
            return listAssignActivityDTO;
        }
    }
}
