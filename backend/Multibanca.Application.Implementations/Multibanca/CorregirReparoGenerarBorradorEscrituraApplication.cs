using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.GenerarBorradorEscritura;
using Multibanca.Application.Interfaces.Security;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.Domain.Models.Multibanca.GenerarBorradorEscritura;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class CorregirReparoGenerarBorradorEscrituraApplication
        : MultibancaGenericApplication<corregir_reparo_generar_borrador_escritura, corregir_reparo_generar_borrador_escritura_entity, ICorregirReparoGenerarBorradorEscrituraRepository>,
          ICorregirReparoGenerarBorradorEscrituraApplication
    {
        private readonly ICorregirReparoGenerarBorradorEscrituraRepository CorregirReparoGenerarBorradorEscrituraRepositoryProvider;
        private readonly IGenerarBorradorEscrituraApplication GenerarBorradorEscrituraApplicationProvider;
        private readonly IUserApplication UserApplicationProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public CorregirReparoGenerarBorradorEscrituraApplication(
            MultibancaDBContext _multibancaDBContext,
            ICorregirReparoGenerarBorradorEscrituraRepository _corregirReparoGenerarBorradorEscrituraRepository,
            IGenerarBorradorEscrituraApplication _generarBorradorEscrituraApplication,
            IUserApplication _userApplication,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper
        ) : base(_multibancaDBContext, _corregirReparoGenerarBorradorEscrituraRepository, _mapper)
        {
            CorregirReparoGenerarBorradorEscrituraRepositoryProvider = _corregirReparoGenerarBorradorEscrituraRepository;
            GenerarBorradorEscrituraApplicationProvider = _generarBorradorEscrituraApplication;
            UserApplicationProvider = _userApplication;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<corregir_reparo_generar_borrador_escritura?> GetByExpediente(
            long id_expediente
        )
        {
            var entity =
                await CorregirReparoGenerarBorradorEscrituraRepositoryProvider.GetByExpediente(
                    id_expediente
                );

            var domain = Mapper.Map<corregir_reparo_generar_borrador_escritura?>(entity)
                ?? new corregir_reparo_generar_borrador_escritura();

            domain.id_expediente = id_expediente;

            generar_borrador_escritura? actividadOrigen =
                await GenerarBorradorEscrituraApplicationProvider.GetByExpediente(id_expediente);

            if (actividadOrigen != null)
            {
                domain.observaciones_reparo = actividadOrigen.observaciones;
                domain.fecha_ingreso = actividadOrigen.modified_date ?? actividadOrigen.created_date;

                if (domain.id_usuario_solicitante <= 0)
                {
                    domain.id_usuario_solicitante =
                        actividadOrigen.modified_by ??
                        actividadOrigen.created_by;
                }
            }

            if (domain.id_usuario_solicitante > 0)
            {
                var usuario = UserApplicationProvider.FindId(domain.id_usuario_solicitante);

                if (usuario != null)
                {
                    domain.solicitante = string.Join(
                        " ",
                        new[]
                        {
                            usuario.name,
                            usuario.last_name_first,
                            usuario.last_name_second
                        }.Where(x => !string.IsNullOrWhiteSpace(x))
                    );
                }
            }

            return domain;
        }

        public async Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        )
        {
            var entity =
                await CorregirReparoGenerarBorradorEscrituraRepositoryProvider.GetByExpediente(
                    expediente_id
                );

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Corregir Reparo Generar Borrador Escritura para el expediente {expediente_id}."
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
                    "No se encontró transición configurada para Corregir Reparo Generar Borrador Escritura."
                );
            }

            return await WorkflowApplicationProvider.AvanzarActividad(
                transitionsID,
                folio,
                usuario_id
            );
        }
    }
}
