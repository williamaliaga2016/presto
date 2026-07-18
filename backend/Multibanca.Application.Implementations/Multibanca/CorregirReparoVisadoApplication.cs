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
    public class CorregirReparoVisadoApplication
        : MultibancaGenericApplication<corregir_reparo_visado, corregir_reparo_visado_entity, ICorregirReparoVisadoRepository>,
          ICorregirReparoVisadoApplication
    {
        private readonly ICorregirReparoVisadoRepository CorregirReparoVisadoRepositoryProvider;
        private readonly IVisarOperacionApplication VisarOperacionApplicationProvider;
        private readonly IUserApplication UserApplicationProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public CorregirReparoVisadoApplication(
            MultibancaDBContext _multibancaDBContext,
            ICorregirReparoVisadoRepository _corregirReparoVisadoRepository,
            IVisarOperacionApplication _visarOperacionApplication,
            IUserApplication _userApplication,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper
        ) : base(_multibancaDBContext, _corregirReparoVisadoRepository, _mapper)
        {
            CorregirReparoVisadoRepositoryProvider = _corregirReparoVisadoRepository;
            VisarOperacionApplicationProvider = _visarOperacionApplication;
            UserApplicationProvider = _userApplication;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<corregir_reparo_visado?> GetByExpediente(long id_expediente)
        {
            var entity = await CorregirReparoVisadoRepositoryProvider.GetByExpediente(id_expediente);

            if (entity != null)
            {
                entity.id_expediente = id_expediente;
            }

            var domain = Mapper.Map<corregir_reparo_visado?>(entity)
                ?? new corregir_reparo_visado();

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
            var entity = await CorregirReparoVisadoRepositoryProvider.GetByExpediente(expediente_id);

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Corregir Reparo Visado para el expediente {expediente_id}."
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
                    "No se encontró transición configurada para Corregir Reparo Visado."
                );
            }

            return await WorkflowApplicationProvider.AvanzarActividad(
                transitionsID,
                folio,
                usuario_id
            );
        }

        private async Task CompletarDatosHeredados(corregir_reparo_visado domain)
        {
            if (domain == null || domain.id_expediente <= 0)
            {
                return;
            }

            visar_operacion? actividadOrigen =
                await VisarOperacionApplicationProvider.GetByExpediente(domain.id_expediente);

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
