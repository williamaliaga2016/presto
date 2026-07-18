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
    public class CorregirReparoPrefiniquitoApplication
        : MultibancaGenericApplication<corregir_reparo_prefiniquito, corregir_reparo_prefiniquito_entity, ICorregirReparoPrefiniquitoRepository>,
          ICorregirReparoPrefiniquitoApplication
    {
        private readonly ICorregirReparoPrefiniquitoRepository CorregirReparoPrefiniquitoRepositoryProvider;
        private readonly ICorregirReparoInstPagoApplication CorregirReparoInstPagoApplicationProvider;
        private readonly IUserApplication UserApplicationProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public CorregirReparoPrefiniquitoApplication(
            MultibancaDBContext _multibancaDBContext,
            ICorregirReparoPrefiniquitoRepository _corregirReparoPrefiniquitoRepository,
            ICorregirReparoInstPagoApplication _corregirReparoInstPagoApplication,
            IUserApplication _userApplication,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper
        ) : base(_multibancaDBContext, _corregirReparoPrefiniquitoRepository, _mapper)
        {
            CorregirReparoPrefiniquitoRepositoryProvider = _corregirReparoPrefiniquitoRepository;
            CorregirReparoInstPagoApplicationProvider = _corregirReparoInstPagoApplication;
            UserApplicationProvider = _userApplication;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<corregir_reparo_prefiniquito?> GetByExpediente(
            long id_expediente
        )
        {
            corregir_reparo_prefiniquito_entity? entity =
                await CorregirReparoPrefiniquitoRepositoryProvider.GetByExpediente(
                    id_expediente
                );

            corregir_reparo_prefiniquito domain =
                Mapper.Map<corregir_reparo_prefiniquito?>(entity)
                ?? new corregir_reparo_prefiniquito();

            domain.id_expediente = id_expediente;

            corregir_reparo_inst_pago? actividadOrigen = null;

            try
            {
                actividadOrigen =
                    await CorregirReparoInstPagoApplicationProvider.GetByExpediente(
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
            corregir_reparo_prefiniquito_entity? entity =
                await CorregirReparoPrefiniquitoRepositoryProvider.GetByExpediente(
                    expediente_id
                );

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Corregir Reparo Prefiniquito para el expediente {expediente_id}."
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
                .Where(x => x.name == "CorregirReparoPrefiniquito_RecibirInstruccionPago")
                .Select(x => x.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Corregir Reparo Prefiniquito."
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
