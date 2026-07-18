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
    public class CorregirReparoTasacionApplication
        : MultibancaGenericApplication<corregir_reparo_tasacion, corregir_reparo_tasacion_entity, ICorregirReparoTasacionRepository>,
          ICorregirReparoTasacionApplication
    {
        private readonly ICorregirReparoTasacionRepository CorregirReparoTasacionRepositoryProvider;
        private readonly ITasacionApplication TasacionApplicationProvider;
        private readonly ITasacionDetalleApplication TasacionDetalleApplicationProvider;
        private readonly IUserApplication UserApplicationProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public CorregirReparoTasacionApplication(
            MultibancaDBContext _multibancaDBContext,
            ICorregirReparoTasacionRepository _corregirReparoTasacionRepository,
            ITasacionApplication _tasacionApplication,
            ITasacionDetalleApplication _tasacionDetalleApplication,
            IUserApplication _userApplication,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper
        ) : base(_multibancaDBContext, _corregirReparoTasacionRepository, _mapper)
        {
            CorregirReparoTasacionRepositoryProvider = _corregirReparoTasacionRepository;
            TasacionApplicationProvider = _tasacionApplication;
            TasacionDetalleApplicationProvider = _tasacionDetalleApplication;
            UserApplicationProvider = _userApplication;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<corregir_reparo_tasacion?> GetByExpediente(long id_expediente)
        {
            var entity =
                await CorregirReparoTasacionRepositoryProvider.GetByExpediente(
                    id_expediente
                );

            var domain = Mapper.Map<corregir_reparo_tasacion?>(entity)
                ?? new corregir_reparo_tasacion();

            domain.id_expediente = id_expediente;

            await CompletarDatosHeredados(domain, id_expediente);

            return domain;
        }

        public async Task MarcarReparoTasacionSubsanado(long id_expediente, int usuario_id)
        {
            await TasacionApplicationProvider.MarcarReparoSubsanado(
                id_expediente,
                usuario_id
            );
        }

        public async Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        )
        {
            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(
                expediente_id,
                actividad_id
            );

            List<xpdl_transition_DTO> listTransitions =
                await WorkflowApplicationProvider.GetTransitions(actividad_id);

            var entity =
                await CorregirReparoTasacionRepositoryProvider.GetByExpediente(
                    expediente_id
                );

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Corregir Reparo Tasación para el expediente {expediente_id}."
                );
            }

            if (!entity.is_subsanar)
            {
                throw new InvalidOperationException(
                    "Debe marcar el reparo como subsanado antes de avanzar."
                );
            }

            var detalles =
                await TasacionDetalleApplicationProvider.GetByExpediente(
                    expediente_id
                );

            if (detalles == null || detalles.Count == 0)
            {
                throw new InvalidOperationException(
                    "Debe registrar al menos una tasación para avanzar la actividad."
                );
            }

            string tipoTasacion = detalles.FirstOrDefault()?.tipo_tasacion ?? string.Empty;
            bool esColectiva = string.Equals(
                tipoTasacion,
                "COLECTIVA",
                StringComparison.OrdinalIgnoreCase
            );

            string transitionsID = ObtenerTransicionPorRegla(
                listTransitions,
                esColectiva
            );

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    $"No se encontró una transición válida para el tipo de tasación: {tipoTasacion}."
                );
            }

            return await WorkflowApplicationProvider.AvanzarActividad(
                transitionsID,
                folio,
                usuario_id
            );
        }

        private async Task CompletarDatosHeredados(
            corregir_reparo_tasacion domain,
            long id_expediente
        )
        {
            tasacion? actividadOrigen =
                await TasacionApplicationProvider.GetByExpediente(id_expediente);

            if (actividadOrigen != null)
            {
                domain.observaciones_reparo = actividadOrigen.observaciones;
                domain.fecha_ingreso =
                    actividadOrigen.modified_date ?? actividadOrigen.created_date;

                if (domain.id_usuario_solicitante <= 0)
                {
                    domain.id_usuario_solicitante =
                        actividadOrigen.modified_by ?? actividadOrigen.created_by;
                }
            }

            domain.solicitante =
                ObtenerNombreUsuario(domain.id_usuario_solicitante);
        }

        private string ObtenerNombreUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                return string.Empty;
            }

            users? usuario = UserApplicationProvider.FindId(idUsuario);

            if (usuario == null)
            {
                return string.Empty;
            }

            return string.Join(
                " ",
                new[]
                {
                    usuario.name,
                    usuario.last_name_first,
                    usuario.last_name_second
                }
                .Where(q => !string.IsNullOrWhiteSpace(q))
            ).Trim();
        }

        private static string ObtenerTransicionPorRegla(
            List<xpdl_transition_DTO> listTransitions,
            bool esColectiva
        )
        {
            if (listTransitions == null || listTransitions.Count == 0)
            {
                return string.Empty;
            }

            if (esColectiva)
            {
                return listTransitions
                    .Where(x =>
                        string.Equals(
                            x.name,
                            "CorregirReparoTasacion_RevisarIngresoDatosOperacion_Subsanado_Asociado",
                            StringComparison.OrdinalIgnoreCase
                        )
                    )
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }

            return listTransitions
                .Where(x =>
                    string.Equals(
                        x.name,
                        "CorregirReparoTasacion_GenerarEstudioTitulos_Subsanado_Particular",
                        StringComparison.OrdinalIgnoreCase
                    )
                )
                .Select(x => x.transition_id)
                .FirstOrDefault() ?? string.Empty;
        }
    }
}
