using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Implementations.Multibanca.CargaOperacionBanco;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.CargaOperacionBanco;
using Multibanca.Application.Interfaces.Multibanca.DatosOperacion;
using Multibanca.Application.Interfaces.Multibanca.RevisarDatosOperacion;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class RecepcionMatrizApplication : MultibancaGenericApplication<recepcionar_matriz, recepcionar_matriz_entity, IRecepcionarMatrizRepository>, IRecepcionarMatrizApplication
    {
        private readonly IRecepcionarMatrizRepository RecepcionarMatrizRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;

        private readonly ICommonApplication CommonApplicationProvider;

        private readonly IDatosOperacionBancoAcreedorApplication DatosOperacionBancoAcreedorApplicationProvider;
        private readonly IRevisarDatosOperacionBancoApplication RevisarDatosOperacionBancoApplicationProvider;

        private readonly ICargaOperacionBancoDatosOperacionApplication CargaOperacionBancoDatosOperacionApplicationProvider;
        private readonly ICargaOperacionBancoAntecedenteCreditoApplication CargaOperacionBancoAntecedenteCreditoApplicationProvider;
        private readonly IMapper Mapper;

        public RecepcionMatrizApplication(
            MultibancaDBContext _multibancaDBContext,
            IRecepcionarMatrizRepository _recepcionarMatrizRepository,
            IWorkflowApplication _workflowApplication,
             ICommonApplication _commonApplication,
            IDatosOperacionBancoAcreedorApplication _datosOperacionBancoAcreedorApplication,
            IRevisarDatosOperacionBancoApplication _revisarDatosOperacionBancoApplication,
            ICargaOperacionBancoDatosOperacionApplication _cargaOperacionBancoDatosOperacionApplication,
            ICargaOperacionBancoAntecedenteCreditoApplication _cargaOperacionBancoAntecedenteCreditoApplication,
            IMapper _mapper) : base(_multibancaDBContext, _recepcionarMatrizRepository, _mapper)
        {
            RecepcionarMatrizRepositoryProvider = _recepcionarMatrizRepository;
            WorkflowApplicationProvider = _workflowApplication;

            CommonApplicationProvider = _commonApplication;
            DatosOperacionBancoAcreedorApplicationProvider = _datosOperacionBancoAcreedorApplication;
            RevisarDatosOperacionBancoApplicationProvider = _revisarDatosOperacionBancoApplication;
            CargaOperacionBancoDatosOperacionApplicationProvider = _cargaOperacionBancoDatosOperacionApplication;
            CargaOperacionBancoAntecedenteCreditoApplicationProvider = _cargaOperacionBancoAntecedenteCreditoApplication;
            Mapper = _mapper;
        }

        public async Task<recepcionar_matriz?> GetByExpediente(long id_expediente)
        {
            var entity = await RecepcionarMatrizRepositoryProvider.GetByExpediente(id_expediente);
            if (entity != null)
                entity.id_expediente = id_expediente;
            return Mapper.Map<recepcionar_matriz?>(entity);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long id_expediente, int id_usuario, string id_actividad)
        {
            var entity = await RecepcionarMatrizRepositoryProvider.GetByExpediente(id_expediente);
            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Recepcionar Matriz para el expediente {id_expediente}. Debe guardar la actividad antes de avanzar.");
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(id_expediente, id_actividad);
            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(id_actividad);
            var glosaProductos = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.GlosaProducto);

            var tiposPrestamo = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.TipoPrestamo);

            var cargaOperacion = await CargaOperacionBancoDatosOperacionApplicationProvider.GetByExpediente(id_expediente);
            var cargaOperacionCredito = await CargaOperacionBancoAntecedenteCreditoApplicationProvider.GetByExpediente(id_expediente);
            var datosBancoAcreedor = await DatosOperacionBancoAcreedorApplicationProvider.GetByExpediente(id_expediente);
            var datosOperacionBanco = await RevisarDatosOperacionBancoApplicationProvider.GetByExpediente(id_expediente);
            
            var glosaProducto = glosaProductos
                .FirstOrDefault(x => x.code == cargaOperacion.glosa_producto);

            var tipoPrestamo = tiposPrestamo
                .FirstOrDefault(x => x.code == cargaOperacionCredito.tipo_prestamo);

            bool esProntoPago =
                    glosaProducto?.description?.Contains(
                        "PRONTO PAGO",
                        StringComparison.OrdinalIgnoreCase) == true
                    ||
                    tipoPrestamo?.description?.Contains(
                        "PRONTO PAGO",
                        StringComparison.OrdinalIgnoreCase) == true;

            bool tieneCartaResguardo = (datosBancoAcreedor?.cuenta_carta_resguardo ?? false)||
                (datosOperacionBanco?.cuenta_carta_resguardo ?? false);

            bool tieneBancoAlzante =
                (!string.IsNullOrWhiteSpace(cargaOperacion?.banco_alzante))
                ||
                (!string.IsNullOrWhiteSpace(datosBancoAcreedor?.institucion));
            string transitionsID;
            if (esProntoPago)
            {
                if (tieneCartaResguardo)
                {
                    transitionsID = listTransitions
                        .Where(x => x.name == "RecepcionarMatriz_Paral1")
                        .Select(x => x.transition_id)
                        .FirstOrDefault() ?? string.Empty;
                } else
                {
                    transitionsID = listTransitions
                        .Where(x => x.name == "RecepcionarMatriz_Paral2")
                        .Select(x => x.transition_id)
                        .FirstOrDefault() ?? string.Empty;
                }
            }
            else
            {
                if (tieneBancoAlzante)
                {
                    transitionsID = listTransitions
                        .Where(x => x.name == "RecepcionarMatriz_GenerarCartaResguardo_ProntoPago_NO")
                        .Select(x => x.transition_id)
                        .FirstOrDefault() ?? string.Empty;
                }
                else
                {
                    transitionsID = listTransitions
                        .Where(x => x.name == "RecepcionarMatriz_RealizarRevisionPrevioFirmaBanco_ProntoPago_NO")
                        .Select(x => x.transition_id)
                        .FirstOrDefault() ?? string.Empty;
                }
            }

            return await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, id_usuario);
        }

    }
}
