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
    public class VerificarReparoCbrApplication : MultibancaGenericApplication<verificar_reparo_cbr, verificar_reparo_cbr_entity, IVerificarReparoCbrRepository>, IVerificarReparoCbrApplication
    {
        private readonly IVerificarReparoCbrRepository VerificarReparoCbrRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public VerificarReparoCbrApplication(
            MultibancaDBContext _multibancaDBContext,
            IVerificarReparoCbrRepository _verificarReparoCbrRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _verificarReparoCbrRepository, _mapper)
        {
            VerificarReparoCbrRepositoryProvider = _verificarReparoCbrRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<verificar_reparo_cbr?> GetByExpediente(long id_expediente)
        {
            var entity = await VerificarReparoCbrRepositoryProvider.GetByExpediente(id_expediente);
            if (entity != null)
                entity.id_expediente = id_expediente;
            return Mapper.Map<verificar_reparo_cbr?>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            var entity = await VerificarReparoCbrRepositoryProvider.GetByExpediente(expediente_id);

            if (entity == null)
            {
                throw new InvalidOperationException($"No existe registro de Verificar Reparo en CBR para el expediente {expediente_id}.");
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);

            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);

            bool envioReparo = entity.enviar_a_reparo ?? false;

            string transitionName;
            if (!envioReparo)
            {
                transitionName = "VerificarReparoCBR_RegistrarFechaRegistroCBR";
            }
            else
            {
                transitionName = (entity.enviar_reparo_a ?? string.Empty) switch
                {
                    "01" => "VerificarReparoCBR_CorregirReparoGestor_COMERCIAL",
                    "02" => "VerificarReparoCBR_ReparoFormulario2890_COMERCIAL",
                    "03" => "VerificarReparoCBR_ValidacionRectificatoriaLegal_LEGAL",
                    _ => string.Empty
                };
            }

            string transitionsID = listTransitions
                .Where(x => x.name == transitionName)
                .Select(x => x.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException($"No se encontró transición configurada para Verificar Reparo en CBR (tipo de reparo: '{entity.enviar_reparo_a}').");
            }

            return await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
        }
    }
}
