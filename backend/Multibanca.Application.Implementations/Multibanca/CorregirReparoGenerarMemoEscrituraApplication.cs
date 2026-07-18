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
    public class CorregirReparoGenerarMemoEscrituraApplication
        : MultibancaGenericApplication<corregir_reparo_generar_memo_escritura, corregir_reparo_generar_memo_escritura_entity, ICorregirReparoGenerarMemoEscrituraRepository>,
          ICorregirReparoGenerarMemoEscrituraApplication
    {
        private readonly ICorregirReparoGenerarMemoEscrituraRepository CorregirReparoGenerarMemoEscrituraRepositoryProvider;
        private readonly IGenerarMemoEscrituraApplication GenerarMemoEscrituraApplicationProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IUserApplication UserApplicationProvider;
        private readonly IMapper Mapper;

        public CorregirReparoGenerarMemoEscrituraApplication(
            MultibancaDBContext _multibancaDBContext,
            ICorregirReparoGenerarMemoEscrituraRepository _repository,
            IGenerarMemoEscrituraApplication _generarMemoEscrituraApplication,
            IWorkflowApplication _workflowApplication,
            IUserApplication _userApplication,
            IMapper _mapper
        ) : base(_multibancaDBContext, _repository, _mapper)
        {
            CorregirReparoGenerarMemoEscrituraRepositoryProvider = _repository;
            GenerarMemoEscrituraApplicationProvider = _generarMemoEscrituraApplication;
            WorkflowApplicationProvider = _workflowApplication;
            UserApplicationProvider = _userApplication;
            Mapper = _mapper;
        }

        public async Task<corregir_reparo_generar_memo_escritura?> GetByExpediente(
            long id_expediente
        )
        {
            var entity =
                await CorregirReparoGenerarMemoEscrituraRepositoryProvider.GetByExpediente(
                    id_expediente
                );

            var domain = Mapper.Map<corregir_reparo_generar_memo_escritura?>(entity)
                ?? new corregir_reparo_generar_memo_escritura();

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
                await CorregirReparoGenerarMemoEscrituraRepositoryProvider.GetByExpediente(
                    expediente_id
                );

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Corregir Reparo Generar Memo Escritura para el expediente {expediente_id}."
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
                .Where(x => x.name == "CorregirReparoMemoEscritura_GenerarMemoEscritura_Subsanado")
                .Select(x => x.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Corregir Reparo Generar Memo Escritura."
                );
            }

            return await WorkflowApplicationProvider.AvanzarActividad(
                transitionsID,
                folio,
                usuario_id
            );
        }

        private async Task CompletarDatosHeredados(
            corregir_reparo_generar_memo_escritura domain,
            long id_expediente
        )
        {
            generar_memo_escritura? actividadOrigen =
                await GenerarMemoEscrituraApplicationProvider.GetByExpediente(
                    id_expediente
                );

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
                users user = UserApplicationProvider.FindId(domain.id_usuario_solicitante);

                if (user != null)
                {
                    domain.solicitante = string.Join(
                        " ",
                        new[]
                        {
                            user.name,
                            user.last_name_first,
                            user.last_name_second
                        }.Where(x => !string.IsNullOrWhiteSpace(x))
                    );
                }
            }
        }
    }
}
