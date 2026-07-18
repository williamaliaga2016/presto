using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Security;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.Domain.Models.Security;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class ReparoFormularioApplication
        : MultibancaGenericApplication<reparo_formulario, reparo_formulario_entity, IReparoFormularioRepository>,
          IReparoFormularioApplication
    {
        private readonly IReparoFormularioRepository ReparoFormularioRepositoryProvider;
        private readonly IVerificarReparoCbrApplication VerificarReparoCbrApplicationProvider;
        private readonly IUserApplication UserApplicationProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public ReparoFormularioApplication(
            MultibancaDBContext _multibancaDBContext,
            IReparoFormularioRepository _reparoFormularioRepository,
            IVerificarReparoCbrApplication _verificarReparoCbrApplication,
            IUserApplication _userApplication,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper
        ) : base(_multibancaDBContext, _reparoFormularioRepository, _mapper)
        {
            ReparoFormularioRepositoryProvider = _reparoFormularioRepository;
            VerificarReparoCbrApplicationProvider = _verificarReparoCbrApplication;
            UserApplicationProvider = _userApplication;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<reparo_formulario?> GetByExpediente(long id_expediente)
        {
            var entity = await ReparoFormularioRepositoryProvider.GetByExpediente(id_expediente);

            if (entity != null)
            {
                entity.id_expediente = id_expediente;
            }

            var domain = Mapper.Map<reparo_formulario?>(entity)
                ?? new reparo_formulario();

            domain.id_expediente = id_expediente;

            await CompletarDatosHeredados(domain);

            return domain;
        }

        public async Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        )
        {

            var entity = await ReparoFormularioRepositoryProvider.GetByExpediente(expediente_id);

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Reparo Formulario para el expediente {expediente_id}."
                );
            }

            if (!entity.is_subsanar)
            {
                throw new InvalidOperationException(
                    "Debe marcar el reparo como subsanado antes de avanzar."
                );
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);

            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);


            string transitionsID = listTransitions.Select(q => q.transition_id).FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Reparo Formulario."
                );
            }

            List<AssignActivityDTO> listAssignActivityDTO = new List<AssignActivityDTO>();

            listAssignActivityDTO = await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
            return listAssignActivityDTO;
        }

        private async Task CompletarDatosHeredados(reparo_formulario domain)
        {
            if (domain == null || domain.id_expediente <= 0)
            {
                return;
            }

            verificar_reparo_cbr? actividadOrigen =
                await VerificarReparoCbrApplicationProvider.GetByExpediente(domain.id_expediente);

            if (actividadOrigen != null)
            {
                domain.observaciones_reparo = actividadOrigen.observaciones;
                domain.fecha_ingreso = actividadOrigen.modified_date ?? actividadOrigen.created_date;

                if (domain.id_usuario_solicitante <= 0)
                {
                    domain.id_usuario_solicitante =
                        actividadOrigen.modified_by ?? actividadOrigen.created_by;
                }
            }

            if (domain.id_usuario_solicitante > 0)
            {
                users usuario = UserApplicationProvider.FindId(domain.id_usuario_solicitante);

                if (usuario != null)
                {
                    domain.solicitante =
                        $"{usuario.name} {usuario.last_name_first} {usuario.last_name_second}"
                        .Replace("  ", " ")
                        .Trim();
                }
            }
        }
    }
}
