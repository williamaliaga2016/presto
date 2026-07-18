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
    public class CorregirNotariaReparoAbogadosApplication
        : MultibancaGenericApplication<corregir_notaria_reparo_abogados, corregir_notaria_reparo_abogados_entity, ICorregirNotariaReparoAbogadosRepository>,
          ICorregirNotariaReparoAbogadosApplication
    {
        private readonly ICorregirNotariaReparoAbogadosRepository CorregirNotariaReparoAbogadosRepositoryProvider;
        private readonly IRealizarRevisionPrevioFirmaBancoApplication RealizarRevisionPrevioFirmaBancoApplicationProvider;
        private readonly IUserApplication UserApplicationProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public CorregirNotariaReparoAbogadosApplication(
            MultibancaDBContext _multibancaDBContext,
            ICorregirNotariaReparoAbogadosRepository _corregirNotariaReparoAbogadosRepository,
            IRealizarRevisionPrevioFirmaBancoApplication _realizarRevisionPrevioFirmaBancoApplication,
            IUserApplication _userApplication,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper
        ) : base(_multibancaDBContext, _corregirNotariaReparoAbogadosRepository, _mapper)
        {
            CorregirNotariaReparoAbogadosRepositoryProvider = _corregirNotariaReparoAbogadosRepository;
            RealizarRevisionPrevioFirmaBancoApplicationProvider = _realizarRevisionPrevioFirmaBancoApplication;
            UserApplicationProvider = _userApplication;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<corregir_notaria_reparo_abogados?> GetByExpediente(
            long id_expediente
        )
        {
            var entity =
                await CorregirNotariaReparoAbogadosRepositoryProvider.GetByExpediente(
                    id_expediente
                );

            var domain = Mapper.Map<corregir_notaria_reparo_abogados?>(entity)
                ?? new corregir_notaria_reparo_abogados();

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
                await CorregirNotariaReparoAbogadosRepositoryProvider.GetByExpediente(
                    expediente_id
                );

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Corregir en Notaria Reparo de Abogados para el expediente {expediente_id}."
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
                .Where(x => x.name == "CorregirNotariaReparoAbogados_VerificarCorreccionEscritura")
                .Select(x => x.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Corregir en Notaria Reparo de Abogados."
                );
            }

            return await WorkflowApplicationProvider.AvanzarActividad(
                transitionsID,
                folio,
                usuario_id
            );
        }

        private async Task CompletarDatosHeredados(
            corregir_notaria_reparo_abogados domain,
            long id_expediente
        )
        {
            realizar_revision_previo_firma_banco? actividadOrigen =
                await RealizarRevisionPrevioFirmaBancoApplicationProvider.GetByExpediente(
                    id_expediente
                );

            if (actividadOrigen != null)
            {
                domain.observaciones_reparo = actividadOrigen.observaciones;
                domain.fecha_ingreso = actividadOrigen.modified_date ?? actividadOrigen.created_date;
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
