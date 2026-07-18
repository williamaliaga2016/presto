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
    public class CorregirReparoFabricaApplication
        : MultibancaGenericApplication<corregir_reparo_fabrica, corregir_reparo_fabrica_entity, ICorregirReparoFabricaRepository>,
          ICorregirReparoFabricaApplication
    {
        private readonly ICorregirReparoFabricaRepository CorregirReparoFabricaRepositoryProvider;
        private readonly IRecepcionCargaFabricaApplication RecepcionCargaFabricaApplicationProvider;
        private readonly IUserApplication UserApplicationProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public CorregirReparoFabricaApplication(
            MultibancaDBContext _multibancaDBContext,
            ICorregirReparoFabricaRepository _corregirReparoFabricaRepository,
            IRecepcionCargaFabricaApplication _recepcionCargaFabricaApplication,
            IUserApplication _userApplication,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper
        ) : base(_multibancaDBContext, _corregirReparoFabricaRepository, _mapper)
        {
            CorregirReparoFabricaRepositoryProvider = _corregirReparoFabricaRepository;
            RecepcionCargaFabricaApplicationProvider = _recepcionCargaFabricaApplication;
            UserApplicationProvider = _userApplication;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<corregir_reparo_fabrica?> GetByExpediente(long id_expediente)
        {
            var entity =
                await CorregirReparoFabricaRepositoryProvider.GetByExpediente(
                    id_expediente
                );

            var domain = Mapper.Map<corregir_reparo_fabrica?>(entity)
                ?? new corregir_reparo_fabrica();

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
                await CorregirReparoFabricaRepositoryProvider.GetByExpediente(
                    expediente_id
                );

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Corregir Reparo Fábrica para el expediente {expediente_id}."
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
                    "No se encontró transición configurada para Corregir Reparo Fábrica."
                );
            }

            return await WorkflowApplicationProvider.AvanzarActividad(
                transitionsID,
                folio,
                usuario_id
            );
        }

        private async Task CompletarDatosHeredados(
            corregir_reparo_fabrica domain,
            long id_expediente
        )
        {
            recepcion_carga_fabrica? actividadOrigen =
                await RecepcionCargaFabricaApplicationProvider.GetByExpediente(
                    id_expediente
                );

            if (actividadOrigen == null)
            {
                return;
            }

            domain.observaciones_reparo = actividadOrigen.observaciones;
            domain.fecha_ingreso = actividadOrigen.modified_date ?? actividadOrigen.created_date;

            if (domain.id_usuario_solicitante <= 0)
            {
                domain.id_usuario_solicitante = actividadOrigen.id_usuario_solicitante;
            }

            if (domain.id_usuario_solicitante > 0)
            {
                users usuario = UserApplicationProvider.FindId(domain.id_usuario_solicitante);

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
        }
    }
}
