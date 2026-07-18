using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Multibanca.Application.Interfaces.Workflow;
using Framework.WorkFlow.Common.DTO;

using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Domain.Models.Multibanca;
using Syncfusion.DocIO.DLS;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class VerificarReparoEstudioTituloApplication : MultibancaGenericApplication<verificar_reparo_estudio_titulo, verificar_reparo_estudio_titulo_entity, IVerificarReparoEstudioTituloRepository>, IVerificarReparoEstudioTituloApplication
    {
        private readonly IVerificarReparoEstudioTituloRepository VerificarReparoEstudioTituloRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public VerificarReparoEstudioTituloApplication(
            MultibancaDBContext _multibancaDBContext,
            IVerificarReparoEstudioTituloRepository _verificarReparoEstudioTituloRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _verificarReparoEstudioTituloRepository, _mapper)
        {
            VerificarReparoEstudioTituloRepositoryProvider = _verificarReparoEstudioTituloRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<verificar_reparo_estudio_titulo?> GetByExpediente(long id_expediente)
        {
            var entity = await VerificarReparoEstudioTituloRepositoryProvider.GetByExpediente(id_expediente);
            if (entity != null)
                entity.id_expediente = id_expediente;
            return Mapper.Map<verificar_reparo_estudio_titulo?>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);
            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);

            var entity = await VerificarReparoEstudioTituloRepositoryProvider.GetByExpediente(expediente_id);

            if (entity == null)
            {
                throw new InvalidOperationException($"No existe registro de Tasación para el expediente {expediente_id}.");
            }

            bool envioReparo = entity.enviar_a_reparo ?? false;

            string transitionsID;

            if (envioReparo)
            {
                transitionsID = listTransitions
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }
            else
            {
                    transitionsID = listTransitions
                        .Where(x =>
                            x.name == "VerificarReparoEstudioTitulos_RevisarIngresoDatosOperacion_ReparoNO")
                        .Select(x => x.transition_id)
                        .FirstOrDefault() ?? string.Empty;
            }
            List<AssignActivityDTO> listAssignActivityDTO = new List<AssignActivityDTO>();

            listAssignActivityDTO = await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
            return listAssignActivityDTO;
        }
    }
}
