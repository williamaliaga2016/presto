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
    public class CorregirReparoCdrApplication
        : MultibancaGenericApplication<corregir_reparo_cdr, corregir_reparo_cdr_entity, ICorregirReparoCdrRepository>,
          ICorregirReparoCdrApplication
    {
        private readonly ICorregirReparoCdrRepository CorregirReparoCdrRepositoryProvider;
        private readonly IAprobacionComercialLegalCdRApplication AprobacionComercialLegalCdRApplicationProvider;
        private readonly IUserApplication UserApplicationProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public CorregirReparoCdrApplication(
            MultibancaDBContext _multibancaDBContext,
            ICorregirReparoCdrRepository _corregirReparoCdrRepository,
            IAprobacionComercialLegalCdRApplication _aprobacionComercialLegalCdRApplication,
            IUserApplication _userApplication,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper
        ) : base(_multibancaDBContext, _corregirReparoCdrRepository, _mapper)
        {
            CorregirReparoCdrRepositoryProvider = _corregirReparoCdrRepository;
            AprobacionComercialLegalCdRApplicationProvider = _aprobacionComercialLegalCdRApplication;
            UserApplicationProvider = _userApplication;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<corregir_reparo_cdr?> GetByExpediente(long id_expediente)
        {
            corregir_reparo_cdr_entity? entity =
                await CorregirReparoCdrRepositoryProvider.GetByExpediente(id_expediente);

            corregir_reparo_cdr domain =
                Mapper.Map<corregir_reparo_cdr?>(entity)
                ?? new corregir_reparo_cdr();

            domain.id_expediente = id_expediente;

            await CompletarDatosHeredados(domain, id_expediente);

            return domain;
        }

        public async Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        )
        {
            var entity =
                await CorregirReparoCdrRepositoryProvider.GetByExpediente(
                    expediente_id
                );

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Corregir Reparo CdR para el expediente {expediente_id}."
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
                .Where(x => x.name == "CorregirReparoCdr_RealizarAprobacionComercialLegalCdr")
                .Select(x => x.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Corregir Reparo CdR."
                );
            }

            return await WorkflowApplicationProvider.AvanzarActividad(
                transitionsID,
                folio,
                usuario_id
            );
        }

        private async Task CompletarDatosHeredados(
            corregir_reparo_cdr domain,
            long id_expediente
        )
        {
            aprobacion_comercial_legal_cdr? actividadOrigen =
                await AprobacionComercialLegalCdRApplicationProvider.GetByExpediente(
                    id_expediente
                );

            if (actividadOrigen != null)
            {
                domain.observaciones_reparo = actividadOrigen.observaciones;
                domain.fecha_ingreso =
                    actividadOrigen.modified_date ?? actividadOrigen.created_date;

                if (domain.id_usuario_solicitante == 0)
                {
                    domain.id_usuario_solicitante =
                        actividadOrigen.modified_by ?? actividadOrigen.created_by;
                }
            }

            if (domain.id_usuario_solicitante > 0)
            {
                users? usuario = UserApplicationProvider.FindId(
                    domain.id_usuario_solicitante
                );

                domain.solicitante = usuario != null
                    ? string.Join(
                        " ",
                        new[]
                        {
                            usuario.name,
                            usuario.last_name_first,
                            usuario.last_name_second
                        }.Where(x => !string.IsNullOrWhiteSpace(x))
                    )
                    : string.Empty;
            }
        }

    }
}
