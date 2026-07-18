using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.RevisarDatosOperacion;
using Data.Repository.Interfaces.Repositories.Multibanca.RevisarDatosOperacion;
using Multibanca.Application.Interfaces.Workflow;
using Framework.WorkFlow.Common.DTO;

using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.RevisarDatosOperacion;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.Domain.Models.Multibanca.RevisarDatosOperacion;

namespace Multibanca.Application.Implementations.Multibanca.RevisarDatosOperacion
{
    public class RevisarDatosOperacionApplication :
        MultibancaGenericApplication<
            revisar_datos_operacion,
            revisar_datos_operacion_entity,
            IRevisarDatosOperacionRepository>,
        IRevisarDatosOperacionApplication
    {
        private readonly IRevisarDatosOperacionRepository RepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public RevisarDatosOperacionApplication(
            MultibancaDBContext _multibancaDBContext,
            IRevisarDatosOperacionRepository _repository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper)
            : base(_multibancaDBContext, _repository, _mapper)
        {
            RepositoryProvider = _repository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<revisar_datos_operacion?> GetByExpediente(long id_expediente)
        {
            var entity = await RepositoryProvider.GetByExpediente(id_expediente);
            return Mapper.Map<revisar_datos_operacion>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            var entity = await RepositoryProvider.GetByExpediente(expediente_id);

            if (entity == null)
            {
                throw new InvalidOperationException($"No existe registro de Revisar Datos Operación para el expediente {expediente_id}.");
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);

            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);

            bool envioReparo = entity.enviar_reparo;

            string transitionName = envioReparo
                ? "RevisarIngresoDatosOperacion_VerificarReparoIngresoDatosOperacion_ReparoSI"
                : "RevisarIngresoDatosOperacion_CalculoGeneracionDocumento_ReparoNO";

            string transitionsID = listTransitions
                .Where(x => x.name == transitionName)
                .Select(x => x.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException($"No se encontró transición configurada para Revisar Datos Operación (reparo: {(envioReparo ? "SÍ" : "NO")}).");
            }

            return await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
        }
    }
}
