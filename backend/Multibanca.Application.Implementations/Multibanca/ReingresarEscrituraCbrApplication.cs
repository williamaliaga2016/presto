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
    public class ReingresarEscrituraCbrApplication
        : MultibancaGenericApplication<reingresar_escritura_cbr, reingresar_escritura_cbr_entity, IReingresarEscrituraCbrRepository>,
          IReingresarEscrituraCbrApplication
    {
        private readonly IReingresarEscrituraCbrRepository ReingresarEscrituraCbrRepositoryProvider;
        private readonly IRevisarInscripcionCbrApplication RevisarInscripcionCbrApplicationProvider;
        private readonly IUserApplication UserApplicationProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public ReingresarEscrituraCbrApplication(
            MultibancaDBContext _multibancaDBContext,
            IReingresarEscrituraCbrRepository _reingresarEscrituraCbrRepository,
            IRevisarInscripcionCbrApplication _revisarInscripcionCbrApplication,
            IUserApplication _userApplication,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper
        ) : base(_multibancaDBContext, _reingresarEscrituraCbrRepository, _mapper)
        {
            ReingresarEscrituraCbrRepositoryProvider = _reingresarEscrituraCbrRepository;
            RevisarInscripcionCbrApplicationProvider = _revisarInscripcionCbrApplication;
            UserApplicationProvider = _userApplication;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<reingresar_escritura_cbr?> GetByExpediente(
            long id_expediente
        )
        {
            reingresar_escritura_cbr_entity? entity =
                await ReingresarEscrituraCbrRepositoryProvider.GetByExpediente(
                    id_expediente
                );

            reingresar_escritura_cbr domain =
                Mapper.Map<reingresar_escritura_cbr?>(entity)
                ?? new reingresar_escritura_cbr();

            domain.id_expediente = id_expediente;

            revisar_inscripcion_cbr? actividadOrigen = null;

            try
            {
                actividadOrigen =
                    await RevisarInscripcionCbrApplicationProvider.GetByExpediente(
                        id_expediente
                    );
            }
            catch
            {
                actividadOrigen = null;
            }

            if (actividadOrigen != null)
            {
                domain.observaciones_reparo = actividadOrigen.observaciones;
                domain.fecha_ingreso =
                    actividadOrigen.modified_date ??
                    actividadOrigen.created_date;

                if (domain.id_usuario_solicitante == 0)
                {
                    domain.id_usuario_solicitante =
                        actividadOrigen.modified_by ??
                        actividadOrigen.created_by;
                }
            }

            domain.solicitante = GetNombreSolicitante(domain.id_usuario_solicitante);

            return domain;
        }

        public async Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        )
        {
            reingresar_escritura_cbr_entity? entity =
                await ReingresarEscrituraCbrRepositoryProvider.GetByExpediente(
                    expediente_id
                );

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Reingresar Escritura en CBR para el expediente {expediente_id}."
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
                .Where(x => x.name == "ReingresarEscrituraCBR_GenerarFiniquito")
                .Select(x => x.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Reingresar Escritura en CBR."
                );
            }

            return await WorkflowApplicationProvider.AvanzarActividad(
                transitionsID,
                folio,
                usuario_id
            );
        }

        private string GetNombreSolicitante(int id_usuario_solicitante)
        {
            if (id_usuario_solicitante <= 0)
            {
                return string.Empty;
            }

            try
            {
                users user = UserApplicationProvider.FindId(id_usuario_solicitante);

                if (user == null)
                {
                    return string.Empty;
                }

                return string.Join(
                    " ",
                    new[]
                    {
                        user.name,
                        user.last_name_first,
                        user.last_name_second
                    }.Where(x => !string.IsNullOrWhiteSpace(x))
                ).Trim();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
