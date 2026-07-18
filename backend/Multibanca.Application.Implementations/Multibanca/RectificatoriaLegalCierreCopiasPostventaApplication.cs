using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca.ValidacionRectificatoriaLegalPostventa;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.CargaOperacionBanco;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;
using Syncfusion.DocIO.DLS;
using static Amazon.S3.Util.S3EventNotification;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class RectificatoriaLegalCierreCopiasPostventaApplication
        : MultibancaGenericApplication<rectificatoria_legal_cierre_copias_postventa, rectificatoria_legal_cierre_copias_postventa_entity, IRectificatoriaLegalCierreCopiasPostventaRepository>,
          IRectificatoriaLegalCierreCopiasPostventaApplication
    {
        private readonly IRectificatoriaLegalCierreCopiasPostventaRepository RectificatoriaLegalCierreCopiasPostventaRepositoryProvider;
        private readonly ICargaOperacionBancoDatosOperacionApplication CargaOperacionBancoDatosOperacionApplicationProvider;
        private readonly ICommonApplication CommonApplicationProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IValidacionRectificatoriaLegalPostventaRepository ValidacionRectificatoriaLegalPostventaRepositoryProvider;
        private readonly IMapper Mapper;

        public RectificatoriaLegalCierreCopiasPostventaApplication(
            MultibancaDBContext _multibancaDBContext,
            IRectificatoriaLegalCierreCopiasPostventaRepository _rectificatoriaLegalCierreCopiasPostventaRepository,
            ICargaOperacionBancoDatosOperacionApplication _cargaOperacionBancoDatosOperacionApplication,
            ICommonApplication _commonApplication,
            IWorkflowApplication _workflowApplication,
            IValidacionRectificatoriaLegalPostventaRepository _validacionRectificatoriaLegalPostventaRepository,
            IMapper _mapper) : base(_multibancaDBContext, _rectificatoriaLegalCierreCopiasPostventaRepository, _mapper)
        {
            RectificatoriaLegalCierreCopiasPostventaRepositoryProvider = _rectificatoriaLegalCierreCopiasPostventaRepository;
            CargaOperacionBancoDatosOperacionApplicationProvider = _cargaOperacionBancoDatosOperacionApplication;
            CommonApplicationProvider = _commonApplication;
            WorkflowApplicationProvider = _workflowApplication;
            ValidacionRectificatoriaLegalPostventaRepositoryProvider = _validacionRectificatoriaLegalPostventaRepository;
            Mapper = _mapper;
        }

        public async Task<rectificatoria_legal_cierre_copias_postventa?> GetByExpediente(long id_expediente)
        {
            var entity = await RectificatoriaLegalCierreCopiasPostventaRepositoryProvider.GetByExpediente(id_expediente);

            var domain = Mapper.Map<rectificatoria_legal_cierre_copias_postventa?>(entity)
                ?? new rectificatoria_legal_cierre_copias_postventa();

            domain.id_expediente = id_expediente;

            await CompletarDatosHeredados(domain, id_expediente);

            return domain;
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            var entity = await ValidacionRectificatoriaLegalPostventaRepositoryProvider.GetByExpediente(expediente_id);

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No se encontró la validación de rectificatoria legal postventa del expediente {expediente_id}."
                );
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);

            List<xpdl_transition_DTO> listTransitions =
                await WorkflowApplicationProvider.GetTransitions(actividad_id);

            bool requiereCbr = entity.requiere_inscripcion_cbr;

            string targetTransitionName = requiereCbr
                ? "RectificatoriaLegalCierreCopiasPostventa_VerificarReparoCBR_SI"
                : "RectificatoriaLegalCierreCopiasPostventa_EntregarCarpetaCBR_NO";

            string transitionsID = listTransitions
                .Where(x => x.name == targetTransitionName)
                .Select(x => x.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Cierre de Copias Postventa."
                );
            }

            return await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
        }

        private async Task CompletarDatosHeredados(
            rectificatoria_legal_cierre_copias_postventa domain,
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
