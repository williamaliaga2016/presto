using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.CargaOperacionBanco;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class RectificatoriaLegalFirmaAlzanteApplication
        : MultibancaGenericApplication<rectificatoria_legal_firma_alzante, rectificatoria_legal_firma_alzante_entity, IRectificatoriaLegalFirmaAlzanteRepository>,
          IRectificatoriaLegalFirmaAlzanteApplication
    {
        private readonly IRectificatoriaLegalFirmaAlzanteRepository RectificatoriaLegalFirmaAlzanteRepositoryProvider;
        private readonly ICargaOperacionBancoDatosOperacionApplication CargaOperacionBancoDatosOperacionApplicationProvider;
        private readonly ICommonApplication CommonApplicationProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public RectificatoriaLegalFirmaAlzanteApplication(
            MultibancaDBContext _multibancaDBContext,
            IRectificatoriaLegalFirmaAlzanteRepository _rectificatoriaLegalFirmaAlzanteRepository,
            ICargaOperacionBancoDatosOperacionApplication _cargaOperacionBancoDatosOperacionApplication,
            ICommonApplication _commonApplication,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _rectificatoriaLegalFirmaAlzanteRepository, _mapper)
        {
            RectificatoriaLegalFirmaAlzanteRepositoryProvider = _rectificatoriaLegalFirmaAlzanteRepository;
            CargaOperacionBancoDatosOperacionApplicationProvider = _cargaOperacionBancoDatosOperacionApplication;
            CommonApplicationProvider = _commonApplication;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<rectificatoria_legal_firma_alzante?> GetByExpediente(long id_expediente)
        {
            var entity = await RectificatoriaLegalFirmaAlzanteRepositoryProvider.GetByExpediente(id_expediente);

            var domain = Mapper.Map<rectificatoria_legal_firma_alzante?>(entity)
                ?? new rectificatoria_legal_firma_alzante();

            domain.id_expediente = id_expediente;

            await CompletarDatosHeredados(domain, id_expediente);

            return domain;
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);

            List<xpdl_transition_DTO> listTransitions =
                await WorkflowApplicationProvider.GetTransitions(actividad_id);

            string transitionsID = listTransitions
                .Select(q => q.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Rectificatoria Legal Firma Alzante."
                );
            }

            return await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
        }

        private async Task CompletarDatosHeredados(
            rectificatoria_legal_firma_alzante domain,
            long id_expediente
        )
        {
            carga_operacion_banco_datos_operacion? datosOperacion =
                await CargaOperacionBancoDatosOperacionApplicationProvider.GetByExpediente(id_expediente);

            if (datosOperacion == null || string.IsNullOrWhiteSpace(datosOperacion.banco_alzante))
            {
                return;
            }

            var catalogoBancoAlzante =
                await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.BancoAlzante);

            var bancoAlzante = catalogoBancoAlzante
                .FirstOrDefault(c => c.code == datosOperacion.banco_alzante);

            domain.nombre_banco_alzante = bancoAlzante?.description ?? datosOperacion.banco_alzante;
        }
    }
}
