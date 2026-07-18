using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.RevisarDatosOperacion;
using Multibanca.Application.Interfaces.Security;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class VerificarReparoDatosOperacionApplication
        : MultibancaGenericApplication<verificar_reparo_datos_operacion, verificar_reparo_datos_operacion_entity, IVerificarReparoDatosOperacionRepository>,
          IVerificarReparoDatosOperacionApplication
    {
        private readonly IVerificarReparoDatosOperacionRepository VerificarReparoDatosOperacionRepositoryProvider;
        private readonly IRevisarDatosOperacionApplication RevisarDatosOperacionApplicationProvider;
        private readonly IUserApplication UserApplicationProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public VerificarReparoDatosOperacionApplication(
            MultibancaDBContext _multibancaDBContext,
            IVerificarReparoDatosOperacionRepository _verificarReparoDatosOperacionRepository,
            IRevisarDatosOperacionApplication _revisarDatosOperacionApplication,
            IUserApplication _userApplication,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper
        ) : base(_multibancaDBContext, _verificarReparoDatosOperacionRepository, _mapper)
        {
            VerificarReparoDatosOperacionRepositoryProvider = _verificarReparoDatosOperacionRepository;
            RevisarDatosOperacionApplicationProvider = _revisarDatosOperacionApplication;
            UserApplicationProvider = _userApplication;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<verificar_reparo_datos_operacion?> GetByExpediente(long id_expediente)
        {
            var entity = await VerificarReparoDatosOperacionRepositoryProvider.GetByExpediente(id_expediente);

            var domain = Mapper.Map<verificar_reparo_datos_operacion?>(entity)
                ?? new verificar_reparo_datos_operacion();

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
            var entity = await VerificarReparoDatosOperacionRepositoryProvider.GetByExpediente(expediente_id);

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Verificar Reparo Ingresar Datos Operación para el expediente {expediente_id}."
                );
            }

            if (!entity.is_subsanar)
            {
                throw new InvalidOperationException(
                    "Debe marcar el reparo como subsanado antes de avanzar."
                );
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(
                expediente_id,
                actividad_id
            );

            List<xpdl_transition_DTO> listTransitions =
                await WorkflowApplicationProvider.GetTransitions(actividad_id);

            string transitionsID = listTransitions
                .Select(q => q.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Verificar Reparo Ingresar Datos Operación."
                );
            }

            return await WorkflowApplicationProvider.AvanzarActividad(
                transitionsID,
                folio,
                usuario_id
            );
        }

        private async Task CompletarDatosHeredados(verificar_reparo_datos_operacion domain)
        {
            var actividadOrigen =
                await RevisarDatosOperacionApplicationProvider.GetByExpediente(domain.id_expediente);

            if (actividadOrigen != null)
            {
                domain.observaciones_reparo = actividadOrigen.observaciones;
                domain.fecha_ingreso = actividadOrigen.modified_date ?? actividadOrigen.created_date;

                if (domain.id_usuario_solicitante == 0)
                {
                    domain.id_usuario_solicitante =
                        actividadOrigen.modified_by ?? actividadOrigen.created_by;
                }
            }

            if (domain.id_usuario_solicitante > 0)
            {
                var usuario = UserApplicationProvider.FindId(domain.id_usuario_solicitante);

                domain.solicitante = usuario != null
                    ? $"{usuario.name} {usuario.last_name_first} {usuario.last_name_second}".Trim()
                    : string.Empty;
            }
        }
    }
}
