using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.ValidacionRectificatoriaLegal;
using Data.Repository.Interfaces.Repositories.Multibanca.ValidacionRectificatoriaLegal;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.CargaOperacionBanco;
using Multibanca.Application.Interfaces.Multibanca.ValidacionRectificatoriaLegal;
using Multibanca.Application.Interfaces.Security;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;
using Multibanca.Domain.Models.Multibanca.ValidacionRectificatoriaLegal;
using Multibanca.Domain.Models.Security;

namespace Multibanca.Application.Implementations.Multibanca.ValidacionRectificatoriaLegal
{
    public class ValidacionRectificatoriaLegalApplication
        : MultibancaGenericApplication<validacion_rectificatoria_legal, validacion_rectificatoria_legal_entity, IValidacionRectificatoriaLegalRepository>,
          IValidacionRectificatoriaLegalApplication
    {
        private readonly IValidacionRectificatoriaLegalRepository ValidacionRectificatoriaLegalRepositoryProvider;
        private readonly IVerificarReparoCbrApplication VerificarReparoCbrApplicationProvider;
        private readonly IValidacionRectificatoriaLegalDatosPersonalesApplication ValidacionRectificatoriaLegalDatosPersonalesApplicationProvider;

        private readonly ICargaOperacionBancoAntecedenteVendedorApplication CargaOperacionBancoAntecedenteVendedorApplicationProvider;
        private readonly ICargaOperacionBancoAntecedenteCompradorApplication CargaOperacionBancoAntecedenteCompradorApplicationProvider;

        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IUserApplication UserApplicationProvider;
        private readonly IMapper Mapper;

        public ValidacionRectificatoriaLegalApplication(
            MultibancaDBContext _multibancaDBContext,
            IValidacionRectificatoriaLegalRepository _repository,
            IVerificarReparoCbrApplication _verificarReparoCbrApplication,
            IValidacionRectificatoriaLegalDatosPersonalesApplication _validacionRectificatoriaLegalDatosPersonalesApplication,
            ICargaOperacionBancoAntecedenteVendedorApplication _cargaOperacionBancoAntecedenteVendedorApplication,
            ICargaOperacionBancoAntecedenteCompradorApplication _cargaOperacionBancoAntecedenteCompradorApplication,
            IWorkflowApplication _workflowApplication,
            IUserApplication _userApplication,
            IMapper _mapper
        ) : base(_multibancaDBContext, _repository, _mapper)
        {
            ValidacionRectificatoriaLegalRepositoryProvider = _repository;
            VerificarReparoCbrApplicationProvider = _verificarReparoCbrApplication;
            ValidacionRectificatoriaLegalDatosPersonalesApplicationProvider = _validacionRectificatoriaLegalDatosPersonalesApplication;
            CargaOperacionBancoAntecedenteVendedorApplicationProvider = _cargaOperacionBancoAntecedenteVendedorApplication;
            CargaOperacionBancoAntecedenteCompradorApplicationProvider = _cargaOperacionBancoAntecedenteCompradorApplication;

            WorkflowApplicationProvider = _workflowApplication;
            UserApplicationProvider = _userApplication;
            Mapper = _mapper;
        }

        public async Task<validacion_rectificatoria_legal?> GetByExpediente(
            long id_expediente
        )
        {
            var entity =
                await ValidacionRectificatoriaLegalRepositoryProvider.GetByExpediente(
                    id_expediente
                );

            var domain = Mapper.Map<validacion_rectificatoria_legal?>(entity)
                ?? new validacion_rectificatoria_legal();

            domain.id_expediente = id_expediente;

            await CompletarDatosHeredados(domain, id_expediente);

            return domain;
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id,int usuario_id,string actividad_id)
        {
            var entity = await ValidacionRectificatoriaLegalRepositoryProvider.GetByExpediente(expediente_id);

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Validacion Rectificatoria Legal para el expediente {expediente_id}."
                );
            }

            if (!entity.is_subsanar)
            {
                throw new InvalidOperationException(
                    "Debe marcar el reparo como subsanado antes de avanzar."
                );
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id,actividad_id);

            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);

            string transitionsID = "";
            if (entity.encargado_firma) {
                transitionsID = listTransitions
                    .Where(x => x.name == "ValidaciónRectificatoriaLegal_RectificatoriaFirmaCompradorVendedor_FirmaOtrasPartes")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }
            else {
                transitionsID = listTransitions
                    .Where(x => x.name == "ValidaciónRectificatoriaLegal_GestiónRectificatoriaEscrituraFirmada_FirmaSoloBanco")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Validacion Rectificatoria Legal."
                );
            }

            return await WorkflowApplicationProvider.AvanzarActividad(transitionsID,folio,usuario_id);
        }

        private async Task CompletarDatosHeredados(
            validacion_rectificatoria_legal domain,
            long id_expediente
        )
        {
            verificar_reparo_cbr? actividadOrigen =
                await VerificarReparoCbrApplicationProvider.GetByExpediente(
                    id_expediente
                );
            List<validacion_rectificatoria_legal_datos_personales>? validacionRectificatoriaLegalDatosPersonales = await ValidacionRectificatoriaLegalDatosPersonalesApplicationProvider.GetByExpediente(
                id_expediente);
            List<carga_operacion_banco_antecedente_vendedor>? vendedorOrigen = await CargaOperacionBancoAntecedenteVendedorApplicationProvider.GetByExpediente(
                id_expediente);
            List<carga_operacion_banco_antecedente_comprador>? compradorOrigen = await CargaOperacionBancoAntecedenteCompradorApplicationProvider.GetByExpediente(
                id_expediente);
            if (validacionRectificatoriaLegalDatosPersonales != null)
            {
                domain.validacion_rectificatoria_legal_datos_personales = validacionRectificatoriaLegalDatosPersonales;
            }
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

            if (vendedorOrigen != null)
            {
                domain.antecedentes_vendedor = vendedorOrigen;
            }
            if (compradorOrigen != null)
            {
                domain.antecedentes_comprador = compradorOrigen;
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
