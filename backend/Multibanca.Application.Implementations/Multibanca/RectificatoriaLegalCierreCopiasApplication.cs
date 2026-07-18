using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca.ValidacionRectificatoriaLegal;
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
    public class RectificatoriaLegalCierreCopiasApplication
        : MultibancaGenericApplication<rectificatoria_legal_cierre_copias, rectificatoria_legal_cierre_copias_entity, IRectificatoriaLegalCierreCopiasRepository>,
          IRectificatoriaLegalCierreCopiasApplication
    {
        private readonly IRectificatoriaLegalCierreCopiasRepository RectificatoriaLegalCierreCopiasRepositoryProvider;
        private readonly ICargaOperacionBancoDatosOperacionApplication CargaOperacionBancoDatosOperacionApplicationProvider;
        private readonly ICommonApplication CommonApplicationProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IValidacionRectificatoriaLegalRepository ValidacionRectificatoriaLegalRepositoryProvider;
        private readonly IMapper Mapper;

        public RectificatoriaLegalCierreCopiasApplication(
            MultibancaDBContext _multibancaDBContext,
            IRectificatoriaLegalCierreCopiasRepository _rectificatoriaLegalCierreCopiasRepository,
            ICargaOperacionBancoDatosOperacionApplication _cargaOperacionBancoDatosOperacionApplication,
            ICommonApplication _commonApplication,
            IWorkflowApplication _workflowApplication,
            IValidacionRectificatoriaLegalRepository _validacionRectificatoriaLegalRepository,
            IMapper _mapper) : base(_multibancaDBContext, _rectificatoriaLegalCierreCopiasRepository, _mapper)
        {
            RectificatoriaLegalCierreCopiasRepositoryProvider = _rectificatoriaLegalCierreCopiasRepository;
            CargaOperacionBancoDatosOperacionApplicationProvider = _cargaOperacionBancoDatosOperacionApplication;
            CommonApplicationProvider = _commonApplication;
            WorkflowApplicationProvider = _workflowApplication;
            ValidacionRectificatoriaLegalRepositoryProvider = _validacionRectificatoriaLegalRepository;
            Mapper = _mapper;
        }

        public async Task<rectificatoria_legal_cierre_copias?> GetByExpediente(long id_expediente)
        {
            var entity = await RectificatoriaLegalCierreCopiasRepositoryProvider.GetByExpediente(id_expediente);

            var domain = Mapper.Map<rectificatoria_legal_cierre_copias?>(entity)
                ?? new rectificatoria_legal_cierre_copias();

            domain.id_expediente = id_expediente;

            await CompletarDatosHeredados(domain, id_expediente);

            return domain;
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);

            List<xpdl_transition_DTO> listTransitions =
                await WorkflowApplicationProvider.GetTransitions(actividad_id);

            var entity = await ValidacionRectificatoriaLegalRepositoryProvider.GetByExpediente(expediente_id);

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Rectificatoria Legal Cierre de Copias para el expediente {expediente_id}."
                );
            }

            bool requiereCbr = entity.requiere_inscripcion_cbr;

            string targetTransitionName = requiereCbr
                ? "RectificatoriaLegalCierreCopias_VerificarReparoCBR_SI"
                : "RectificatoriaLegalCierreCopias_EntregarCarpetaCBR_NO";

            string transitionsID = listTransitions
                .Where(x => x.name == targetTransitionName)
                .Select(x => x.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Cierre de Copias."
                );
            }

            return await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
        }

        private async Task CompletarDatosHeredados(
            rectificatoria_legal_cierre_copias domain,
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
