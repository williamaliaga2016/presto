using Data.Repository.Implementations;
using Data.Repository.Implementations.Repositories.Common;
using Data.Repository.Implementations.Repositories.Multibanca;
using Data.Repository.Implementations.Repositories.Multibanca.BBVA;
using Data.Repository.Implementations.Repositories.Multibanca.CargaOperacionBanco;
using Data.Repository.Implementations.Repositories.Multibanca.DatosOperacion;
using Data.Repository.Implementations.Repositories.Multibanca.GenerarBorradorEscritura;
using Data.Repository.Implementations.Repositories.Multibanca.RevisarDatosOperacion;
using Data.Repository.Implementations.Repositories.Multibanca.ValidacionRectificatoriaLegal;
using Data.Repository.Implementations.Repositories.Multibanca.ValidacionRectificatoriaLegalPostventa;
using Data.Repository.Implementations.Repositories.Utilidades;
using Data.Repository.Interfaces;
using Data.Repository.Interfaces.Repositories.Common;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Data.Repository.Interfaces.Repositories.Multibanca.CargaOperacionBanco;
using Data.Repository.Interfaces.Repositories.Multibanca.DatosOperacion;
using Data.Repository.Interfaces.Repositories.Multibanca.GenerarBorradorEscritura;
using Data.Repository.Interfaces.Repositories.Multibanca.RevisarDatosOperacion;
using Data.Repository.Interfaces.Repositories.Multibanca.ValidacionRectificatoriaLegal;
using Data.Repository.Interfaces.Repositories.Multibanca.ValidacionRectificatoriaLegalPostventa;
using Data.Repository.Interfaces.Repositories.Utilidades;
using Framework.WorkFlow.Application.Implementations;
using Framework.WorkFlow.Application.Interfaces;
using Framework.WorkFlow.Repository.Implementation;
using Framework.WorkFlow.Repository.Interface;
using Microsoft.Extensions.DependencyInjection;
using Multibanca.Application.Implementation.Utilidades;
using Multibanca.Application.Implementations.Common;
using Multibanca.Application.Implementations.Multibanca;
using Multibanca.Application.Implementations.Multibanca.BBVA;
using Multibanca.Application.Interfaces.Multibanca.BBVA;
using Multibanca.Application.Implementations.Multibanca.CargaOperacionBanco;
using Multibanca.Application.Implementations.Multibanca.DatosOperacion;
using Multibanca.Application.Implementations.Multibanca.GenerarBorradorEscritura;
using Multibanca.Application.Implementations.Multibanca.RevisarDatosOperacion;
using Multibanca.Application.Implementations.Multibanca.ValidacionRectificatoriaLegal;
using Multibanca.Application.Implementations.Multibanca.ValidacionRectificatoriaLegalPostventa;
using Multibanca.Application.Implementations.Workflow;
using Multibanca.Application.Interface.Utilidades;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.CargaOperacionBanco;
using Multibanca.Application.Interfaces.Multibanca.DatosOperacion;
using Multibanca.Application.Interfaces.Multibanca.GenerarBorradorEscritura;
using Multibanca.Application.Interfaces.Multibanca.RevisarDatosOperacion;
using Multibanca.Application.Interfaces.Multibanca.ValidacionRectificatoriaLegal;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Application.Interfaces.Multibanca.BBVA.Escrituracion;
using Multibanca.Application.Implementations.Multibanca.BBVA.Escrituracion;
using Data.Repository.Implementations.Repositories.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;

namespace Multibanca.Register.IoC.Multibanca
{
    public static class IoCRegisterMultibanca
    {
        public static IServiceCollection AddRegistration(this IServiceCollection services)
        {
            AddRegisterWF(services);
            AddRegisterApplications(services);
            AddRegisterRespositories(services);
            return services;
        }

        public static IServiceCollection AddRegisterWF(IServiceCollection services)
        {
            services.AddScoped<ICaseApplication, CaseApplication>();
            services.AddScoped<IWorkflowApplication, WorkflowApplication>();
            services.AddScoped<IActivityWorkflowApplication, ActivityWorkflowApplication>();
            services.AddScoped<IBusinessCaseProcessorApplication, BusinessCaseProcessorApplication>();
            services.AddScoped<ICaseActivityObserverApplication, CaseActivityObserverApplication>();
            services.AddScoped<ITransitionProcessorApplication, TransitionProcessorApplication>();
            services.AddScoped<IWorkflowRuntimeApplication, WorkflowRuntimeApplication>();

            services.AddScoped<IBusinessCaseWorkflowRepository, BusinessCaseWorkflowRepository>();
            services.AddScoped<IActivityWorkflowRepository, ActivityWorkflowRepository>();
            services.AddScoped<IXpdlWorkflowRepository, XpdlWorkflowRepository>();
            services.AddScoped<ICaseActivityTrackingWorkflowRepository, CaseActivityTrackingWorkflowRepository>();

            return services;
        }

        public static IServiceCollection AddRegisterApplications(IServiceCollection services)
        {
            services.AddScoped<ICommonApplication, CommonApplication>();
            services.AddScoped<IBandejaActividadesApplication, BandejaActividadesApplication>();
            services.AddScoped<IConsultActivityApplication, ConsultActivityApplication>();
            services.AddScoped<IEncabezadoApplication, EncabezadoApplication>();
            services.AddScoped<IReportsApplication, ReportsApplication>();

            services.AddScoped<IActividadesApplication, ActividadesApplication>();
            services.AddScoped<ICargaOperacionBancoApplication, CargaOperacionBancoApplication>();
            services.AddScoped<ICargaOperacionBancoDatosOperacionApplication, CargaOperacionBancoDatosOperacionApplication>();
            services.AddScoped<IRecepcionCargaFabricaApplication, RecepcionCargaFabricaApplication>();
            services.AddScoped<ICorregirReparoFabricaApplication, CorregirReparoFabricaApplication>();
            services.AddScoped<ICorregirReparoTasacionApplication, CorregirReparoTasacionApplication>();
            services.AddScoped<IAsignarEscrituraApplication, AsignarEscrituraApplication>();
            services.AddScoped<IAsignarEstudioTitulosApplication, AsignarEstudioTitulosApplication>();
            services.AddScoped<IGenerarPreFiniquitoApplication, GenerarPreFiniquitoApplication>();
            services.AddScoped<ICorregirReparoPrefiniquitoApplication, CorregirReparoPrefiniquitoApplication>();
            services.AddScoped<ICorregirReparoCalculoDocApplication, CorregirReparoCalculoDocApplication>();
            services.AddScoped<ICorregirReparoGenerarMemoEscrituraApplication, CorregirReparoGenerarMemoEscrituraApplication>();
            services.AddScoped<ICorregirReparoGenerarBorradorEscrituraApplication, CorregirReparoGenerarBorradorEscrituraApplication>();
            services.AddScoped<ICorregirReparoDatosOperacionApplication, CorregirReparoDatosOperacionApplication>();
            services.AddScoped<ICorregirReparoCdrApplication, CorregirReparoCdrApplication>();
            services.AddScoped<ICargaOperacionBancoAntecedenteCompradorApplication, CargaOperacionBancoAntecedenteCompradorApplication>();
            services.AddScoped<ICargaOperacionBancoAntecedenteVendedorApplication, CargaOperacionBancoAntecedenteVendedorApplication>();
            services.AddScoped<ICargaOperacionBancoAntecedenteCreditoApplication, CargaOperacionBancoAntecedenteCreditoApplication>();
            services.AddScoped<IDatosOperacionApplication, DatosOperacionApplication>();
            services.AddScoped<IDatosOperacionDatosCreditoApplication, DatosOperacionDatosCreditoApplication>();
            services.AddScoped<IDatosOperacionCompradorApplication, DatosOperacionCompradorApplication>();
            services.AddScoped<IDatosOperacionVendedorApplication, DatosOperacionVendedorApplication>();
            services.AddScoped<IDatosOperacionFiadorGaranteApplication, DatosOperacionFiadorGaranteApplication>();
            services.AddScoped<IDatosOperacionBancoAcreedorApplication, DatosOperacionBancoAcreedorApplication>();
            services.AddScoped<IDatosOperacionPropiedadApplication, DatosOperacionPropiedadApplication>();
            services.AddScoped<IRevisarDatosOperacionApplication, RevisarDatosOperacionApplication>();
            services.AddScoped<IRevisarDatosOperacionPropiedadApplication, RevisarDatosOperacionPropiedadApplication>();
            services.AddScoped<IRevisarDatosOperacionCreditoApplication, RevisarDatosOperacionCreditoApplication>();
            services.AddScoped<IRevisarDatosOperacionVendedorApplication, RevisarDatosOperacionVendedorApplication>();
            services.AddScoped<IRevisarDatosOperacionCompradorApplication, RevisarDatosOperacionCompradorApplication>();
            services.AddScoped<IUtilidadesApplication, UtilidadesApplication>();
            services.AddScoped<IGenerarMemoEscrituraApplication, GenerarMemoEscrituraApplication>();
            services.AddScoped<ICalculoGeneracionDocumentoApplication, CalculoGeneracionDocumentoApplication>();
            services.AddScoped<ICorregirReparoEstudioTitulosApplication, CorregirReparoEstudioTitulosApplication>();
            services.AddScoped<ITasacionApplication, TasacionApplication>();
            services.AddScoped<ITasacionDetalleApplication, TasacionDetalleApplication>();            
            services.AddScoped<IGenerarEstudioTitulosApplication, GenerarEstudioTitulosApplication>();
            services.AddScoped<ICorregirReparoVisadoApplication, CorregirReparoVisadoApplication>();
            services.AddScoped<IReparoVisadoDetalleApplication, ReparoVisadoDetalleApplication>();
            services.AddScoped<ICorregirCartaResguardoApplication, CorregirCartaResguardoApplication>();
            services.AddScoped<ICorregirReparoDatosOperacionApplication, CorregirReparoDatosOperacionApplication>();
            services.AddScoped<IVerificarReparoEstudioTituloApplication, VerificarReparoEstudioTituloApplication>();
            services.AddScoped<IVerificarReparoDatosOperacionApplication, VerificarReparoDatosOperacionApplication>();
            services.AddScoped<IRevisarIngresoDatosCreditoApplication, RevisarIngresoDatosCreditoApplication>();
            services.AddScoped<IRevisarDatosOperacionBancoApplication, RevisarDatosOperacionBancoApplication>();
            services.AddScoped<IRevisarDatosOperacionFiadorGaranteApplication, RevisarDatosOperacionFiadorGaranteApplication>();
            services.AddScoped<IGenerarBorradorEscrituraApplication, GenerarBorradorEscrituraApplication>();
            services.AddScoped<IGenerarBorradorEscrituraDetalleApplication, GenerarBorradorEscrituraDetalleApplication>();
            services.AddScoped<IVisarOperacionApplication, VisarOperacionApplication>();
            services.AddScoped<IRegistrarFirmaCompradorApplication, RegistrarFirmaCompradorApplication>();
            services.AddScoped<IRegistrarFirmaVendedorApplication, RegistrarFirmaVendedorApplication>();
            services.AddScoped<IRegistrarFirmaCompradorDetalleApplication, RegistrarFirmaCompradorDetalleApplication>();
            services.AddScoped<IRegistrarFirmaVendedorDetalleApplication, RegistrarFirmaVendedorDetalleApplication>();
            services.AddScoped<IRegistrarFirmaBancoAcreedorCGApplication, RegistrarFirmaBancoAcreedorCGApplication>();
            services.AddScoped<ICierreCopiasNotariaApplication, CierreCopiasNotariaApplication>();
            services.AddScoped<ICorregirReparoCierreCopiasNotariaApplication, CorregirReparoCierreCopiasNotariaApplication>();
            services.AddScoped<IAprobacionComercialLegalCdRApplication, AprobacionComercialLegalCdRApplication>();
            services.AddScoped<IRevisarInscripcionCbrApplication, RevisarInscripcionCbrApplication>();
            services.AddScoped<IReingresarEscrituraCbrApplication, ReingresarEscrituraCbrApplication>();
            services.AddScoped<ICorregirReparoLiquidacionApplication, CorregirReparoLiquidacionApplication>();
            services.AddScoped<IRevisarLiquidacionApplication, RevisarLiquidacionApplication>();
            services.AddScoped<IControlEscrituraApplication, ControlEscrituraApplication>();
            services.AddScoped<ICorregirControlEscrituraApplication, CorregirControlEscrituraApplication>();
            services.AddScoped<IRevisarDesembolsoApplication, RevisarDesembolsoApplication>();
            services.AddScoped<ICorregirNotariaReparoAbogadosApplication, CorregirNotariaReparoAbogadosApplication>();
            services.AddScoped<ICorregirReparoEntregarCarpetaApplication, CorregirReparoEntregarCarpetaApplication>();
            services.AddScoped<ICorregirReparoInstPagoApplication, CorregirReparoInstPagoApplication>();
            services.AddScoped<ICorregirReparosGestorApplication, CorregirReparosGestorApplication>();
            services.AddScoped<IEntregarCarpetaApplication, EntregarCarpetaApplication>();
            services.AddScoped<IGenerarCartaResguardoApplication, GenerarCartaRecuerdoApplication>();
            services.AddScoped<IGenerarRecursosPagosCbrApplication, GenerarRecursosPagosCbrApplication>();
            services.AddScoped<IRecepcionarMatrizApplication, RecepcionMatrizApplication>();
            services.AddScoped<IVerificarReparoCbrApplication, VerificarReparoCbrApplication>();
            services.AddScoped<IRecibirInstruccionPagoApplication, RecibirInstruccionApplication>();
            services.AddScoped<IRegistrarEscrituraCbrApplication, RegistrarEscrituraCbrApplication>();
            services.AddScoped<IRealizarRevisionPrevioFirmaBancoApplication, RealizarRevisionPrevioFirmaBancoApplication>();
            services.AddScoped<IValorizarCbrApplication, ValorizarCbrApplication>();
            services.AddScoped<IRegistrarFechaRegistroCbrApplication, RegistrarFechaRegistroCbrApplication>();
            services.AddScoped<IGenerarFiniquitoApplication, GenerarFiniquitoApplication>();
            services.AddScoped<IVerificarCorreccionEscrituraApplication, VerificarCorreccionEscrituraApplication>();
            services.AddScoped<IRegistrarFirmaApoderadoBancoApplication, RegistrarFirmaApoderadoBancoApplication>();
            services.AddScoped<IRealizarControlCreditoApplication, RealizarControlCreditoApplication>();
            services.AddScoped<ICorregirReparoControlCreditoApplication, CorregirReparoControlCreditoApplication>();
            services.AddScoped<IValidacionRectificatoriaLegalApplication, ValidacionRectificatoriaLegalApplication>();
            services.AddScoped<IValidacionRectificatoriaLegalDatosPersonalesApplication, ValidacionRectificatoriaLegalDatosPersonalesApplication>();
            services.AddScoped<IGestionRectificatoriaApplication, GestionRectificatoriaApplication>();
            services.AddScoped<IGestionRectificatoriaSolucionReparoApplication, GestionRectificatoriaSolucionReparoApplication>();
            services.AddScoped<IGestionReparoApplication, GestionReparoApplication>();
            services.AddScoped<IReparoFormularioApplication, ReparoFormularioApplication>();
            services.AddScoped<IRectificatoriaFirmaApplication, RectificatoriaFirmaApplication>();
            services.AddScoped<IRectificatoriaFirmaDetalleApplication, RectificatoriaFirmaDetalleApplication>();
            services.AddScoped<IRectificatoriaLegalCartaResguardoApplication, RectificatoriaLegalCartaResguardoApplication>();
            services.AddScoped<IRectificatoriaLegalFirmaAlzanteApplication, RectificatoriaLegalFirmaAlzanteApplication>();
            services.AddScoped<IRectificatoriaLegalCierreCopiasApplication, RectificatoriaLegalCierreCopiasApplication>();
            services.AddScoped<IRectificatoriaLegalCierreCopiasPostventaApplication, RectificatoriaLegalCierreCopiasPostventaApplication>();
            services.AddScoped<IGestionRectificatoriaEscrituraFirmadaApplication, GestionRectificatoriaEscrituraFirmadaApplication>();
            services.AddScoped<IRectificatoriaAnalisisDerivacionReparoPostventaApplication, RectificatoriaAnalisisDerivacionReparoPostventaApplication>();
            services.AddScoped<IRectificatoriaPostventaSolucionReparoApplication, RectificatoriaPostventaSolucionReparoApplication>();
            services.AddScoped<IValidacionRectificatoriaLegalPostventaApplication, ValidacionRectificatoriaLegalPostventaApplication>();
            services.AddScoped<IValidacionRectificatoriaLegalPostventaDatosPersonalesApplication, ValidacionRectificatoriaLegalPostventaDatosPersonalesApplication>();
            services.AddScoped<IRectificatoriaFirmaPostVentaApplication, RectificatoriaFirmaPostVentaApplication>();
            services.AddScoped<IRectificatoriaFirmaPostVentaDetalleApplication, RectificatoriaFirmaPostVentaDetalleApplication>();
            services.AddScoped<IGestionRectificatoriaEscrituraFirmadaPostventaApplication, GestionRectificatoriaEscrituraFirmadaPostventaApplication>();
            services.AddScoped<IRevisarCopiasEscriturasApplication, RevisarCopiasEscriturasApplication>();
            services.AddScoped<ICorregirReparoCopiasEscriturasApplication, CorregirReparoCopiasEscriturasApplication>();
            services.AddScoped<IAccesoTemporalApplication, AccesoTemporalApplication>();

            // BBVA Colombia — Presto Legalización
            services.AddScoped<IValidarInformacionApplication, ValidarInformacionApplication>();
            services.AddScoped<IRegistroContactoApplication, RegistroContactoApplication>();
            services.AddScoped<IDefinirInmuebleApplication, DefinirInmuebleApplication>();
            services.AddScoped<IValidarIntegracionApplication, ValidarIntegracionApplication>();
            services.AddScoped<IDevolucionVbComercialApplication, DevolucionVbComercialApplication>();
            services.AddScoped<ICargarDocumentosConstructoraApplication, CargarDocumentosConstructoraApplication>();
            services.AddScoped<IRevisarDocumentosInmuebleApplication, RevisarDocumentosInmuebleApplication>();
            services.AddScoped<ICartaAprobacionBbvaApplication, CartaAprobacionBbvaApplication>();
            services.AddScoped<IAsignarFirmasApplication, AsignarFirmasApplication>();
            services.AddScoped<IAsignadorFirmasService, MockAsignadorFirmasService>();
            services.AddScoped<ICargarDocumentosClienteApplication, CargarDocumentosClienteApplication>();
            services.AddScoped<ICargarSoportesPagoApplication, CargarSoportesPagoApplication>();
            services.AddScoped<IGestionarFirmaApplication, GestionarFirmaApplication>();
            services.AddScoped<IGestionarFirmaFisicaApplication, GestionarFirmaFisicaApplication>();
            services.AddScoped<IFirmarEscrituraClienteApplication, FirmarEscrituraClienteApplication>();
            services.AddScoped<IFirmarRepLegalApplication, FirmarRepLegalApplication>();
            services.AddScoped<IRevisarEpAbogadoApplication, RevisarEpAbogadoApplication>();

            return services;
        }

        public static IServiceCollection AddRegisterRespositories(IServiceCollection services)
        {
            services.AddScoped<IMultibancaUnitOfWork, MultibancaUnitOfWork>();
            services.AddScoped<IIdempotencyStore, IdempotencyStore>();           

            services.AddScoped<ICommonRepository, CommonRepository>();
            services.AddScoped<IBandejaActividadesRepository, BandejaActividadesRepository>();
            services.AddScoped<IConsultActivityRepository, ConsultActivityRepository>();
            services.AddScoped<IEncabezadoRepository, EncabezadoRepository>();
            services.AddScoped<IReportsRepository, ReportsRepository>();
            services.AddScoped<IActividadesRepository, ActividadesRepository>();
            services.AddScoped<ICargaOperacionBancoRepository, CargaOperacionBancoRepository>();
            services.AddScoped<ICargaOperacionBancoDatosOperacionRepository, CargaOperacionBancoDatosOperacionRepository>();
            services.AddScoped<IRecepcionCargaFabricaRepository, RecepcionCargaFabricaRepository>();
            services.AddScoped<IAsignarEscrituraRepository, AsignarEscrituraRepository>();
            services.AddScoped<IAsignarEstudioTitulosRepository, AsignarEstudioTitulosRepository>();
            services.AddScoped<ICorregirReparoFabricaRepository, CorregirReparoFabricaRepository>();
            services.AddScoped<ICorregirReparoTasacionRepository, CorregirReparoTasacionRepository>();
            services.AddScoped<IGenerarPreFiniquitoRepository, GenerarPreFiniquitoRepository>();
            services.AddScoped<ICorregirReparoPrefiniquitoRepository, CorregirReparoPrefiniquitoRepository>();
            services.AddScoped<ICorregirReparoCalculoDocRepository, CorregirReparoCalculoDocRepository>();
            services.AddScoped<ICorregirReparoGenerarMemoEscrituraRepository, CorregirReparoGenerarMemoEscrituraRepository>();
            services.AddScoped<ICorregirReparoGenerarBorradorEscrituraRepository, CorregirReparoGenerarBorradorEscrituraRepository>();
            services.AddScoped<ICorregirReparoDatosOperacionRepository, CorregirReparoDatosOperacionRepository>();
            services.AddScoped<ICorregirReparoCdrRepository, CorregirReparoCdrRepository>();
            services.AddScoped<ICargaOperacionBancoAntecedenteCompradorRepository, CargaOperacionBancoAntecedenteCompradorRepository>();
            services.AddScoped<ICargaOperacionBancoAntecedenteVendedorRepository, CargaOperacionBancoAntecedenteVendedorRepository>();
            services.AddScoped<ICargaOperacionBancoAntecedenteCreditoRepository, CargaOperacionBancoAntecedenteCreditoRepository>();
            services.AddScoped<ICargaOperacionBancoDatosComercialRepository, CargaOperacionBancoDatosComercialRepository>();
            services.AddScoped<ICargaOperacionBancoDatosComercialApplication, CargaOperacionBancoDatosComercialApplication>();
            services.AddScoped<IDatosOperacionRepository, DatosOperacionRepository>();
            services.AddScoped<IDatosOperacionDatosCreditoRepository, DatosOperacionDatosCreditoRepository>();
            services.AddScoped<IDatosOperacionCompradorRepository, DatosOperacionCompradorRepository>();
            services.AddScoped<IDatosOperacionVendedorRepository, DatosOperacionVendedorRepository>();
            services.AddScoped<IDatosOperacionFiadorGaranteRepository, DatosOperacionFiadorGaranteRepository>();
            services.AddScoped<IDatosOperacionBancoAcreedorRepository, DatosOperacionBancoAcreedorRepository>();
            services.AddScoped<IDatosOperacionPropiedadRepository, DatosOperacionPropiedadRepository>();
            services.AddScoped<IRevisarDatosOperacionRepository, RevisarDatosOperacionRepository>();
            services.AddScoped<IRevisarDatosOperacionPropiedadRepository, RevisarDatosOperacionPropiedadRepository>();
            services.AddScoped<IRevisarDatosOperacionCreditoRepository, RevisarDatosOperacionCreditoRepository>();
            services.AddScoped<IRevisarDatosOperacionVendedorRepository, RevisarDatosOperacionVendedorRepository>();
            services.AddScoped<IRevisarDatosOperacionCompradorRepository, RevisarDatosOperacionCompradorRepository>();
            services.AddScoped<IUtilidadesRepository, UtilidadesRepository>();
            services.AddScoped<IRevisarIngresoDatosCreditoRepository, RevisarIngresoDatosCreditoRepository>();
            services.AddScoped<IVerificarReparoEstudioTituloRepository, VerificarReparoEstudioTituloRepository>();
            services.AddScoped<IVerificarReparoDatosOperacionRepository, VerificarReparoDatosOperacionRepository>();
            services.AddScoped<ICalculoGeneracionDocumentoRepository, CalculoGeneracionDocumentoRepository>();
            services.AddScoped<IValorUfRepository, ValorUfRepository>();
            services.AddScoped<ICorregirReparoEstudioTitulosRepository, CorregirReparoEstudioTitulosRepository>();
            services.AddScoped<ITasacionRepository, TasacionRepository>();
            services.AddScoped<ITasacionDetalleRepository, TasacionDetalleRepository>();            
            services.AddScoped<IGenerarEstudioTitulosRepository, GenerarEstudioTitulosRepository>();
            services.AddScoped<ICorregirReparoVisadoRepository, CorregirReparoVisadoRepository>();
            services.AddScoped<IReparoVisadoDetalleRepository, ReparoVisadoDetalleRepository>();
            services.AddScoped<ICorregirCartaResguardoRepository, CorregirCartaResguardoRepository>();
            services.AddScoped<IVisarOperacionRepository, VisarOperacionRepository>();

            services.AddScoped<IRevisarDatosOperacionBancoRepository, RevisarDatosOperacionBancoRepository>();
            services.AddScoped<IRevisarDatosOperacionFiadorGaranteRepository, RevisarDatosOperacionFiadorGaranteRepository>();
            services.AddScoped<IGenerarBorradorEscrituraRepository, GenerarBorradorEscrituraRepository>();
            services.AddScoped<IGenerarBorradorEscrituraDetalleRepository, GenerarBorradorEscrituraDetalleRepository>();
            services.AddScoped<IGenerarMemoEscrituraRepository, GenerarMemoEscrituraRepository>();
            services.AddScoped<IRegistrarFirmaCompradorRepository, RegistrarFirmaCompradorRepository>();
            services.AddScoped<IRegistrarFirmaCompradorDetalleRepository, RegistrarFirmaCompradorDetalleRepository>();
            services.AddScoped<IRegistrarFirmaVendedorRepository, RegistrarFirmaVendedorRepository>();
            services.AddScoped<IRegistrarFirmaVendedorDetalleRepository, RegistrarFirmaVendedorDetalleRepository>();
            services.AddScoped<IFirmaBancoAcreedorCGRepository, FirmaBancoAcreedorCGRepository>();
            services.AddScoped<ICierreCopiasNotariaRepository, CierreCopiasNotariaRepository>();
            services.AddScoped<ICorregirReparoCierreCopiasNotariaRepository, CorregirReparoCierreCopiasNotariaRepository>();
            services.AddScoped<IAprobacionComercialLegalCdRRepository, AprobacionComercialLegalCdRRepository>();
            services.AddScoped<IRevisarInscripcionCbrRepository, RevisarInscripcionCbrRepository>();
            services.AddScoped<IReingresarEscrituraCbrRepository, ReingresarEscrituraCbrRepository>();
            services.AddScoped<ICorregirReparoLiquidacionRepository, CorregirReparoLiquidacionRepository>();
            services.AddScoped<IRevisarLiquidacionRepository, RevisarLiquidacionRepository>();
            services.AddScoped<IControlEscrituraRepository, ControlEscrituraRepository>();
            services.AddScoped<ICorregirControlEscrituraRepository, CorregirControlEscrituraRepository>();
            services.AddScoped<IRevisarDesembolsoRepository, RevisarDesembolsoRepository>();
            services.AddScoped<ICorregirNotariaReparoAbogadosRepository, CorregirNotariaReparoAbogadosRepository>();

            services.AddScoped<ICorregirReparoEntregarCarpetaRepository, CorregirReparoEntregarCarpetaRepository>();

            services.AddScoped<ICorregirReparoInstPagoRepository, CorregirReparoInstPagoRepository>();
            services.AddScoped<ICorregirReparosGestorRepository, CorregirReparosGestorRepository>();

            services.AddScoped<IEntregarCarpetaRepository, EntregarCarpetaRepository>();

            services.AddScoped<IGenerarCartaResguardoRepository, GenerarCartaResguardoRepository>();

            services.AddScoped<IGenerarRecursosPagosCbrRepository, GenerarRecursosPagosCbrRepository>();

            services.AddScoped<IRecepcionarMatrizRepository, RecepcionarMatrizRepository>();

            services.AddScoped<IVerificarReparoCbrRepository, VerificarReparoCbrRepository>();
            services.AddScoped<IRecibirInstruccionPagoRepository, RecibirInstruccionPagoRepository>();

            services.AddScoped<IRegistrarEscrituraCbrRepository, RegistrarEscrituraCbrRepository>();

            services.AddScoped<IRealizarRevisionPrevioFirmaBancoRepository, RealizarRevisionPrevioFirmaBancoRepository>();

            services.AddScoped<IValorizarCbrRepository, ValorizarCbrRepository>();
            services.AddScoped<IRegistrarFechaRegistroCbrRepository, RegistrarFechaRegistroCbrRepository>();

            services.AddScoped<IGenerarFiniquitoRepository, GenerarFiniquitoRepository>();
            services.AddScoped<IVerificarCorreccionEscrituraRepository, VerificarCorreccionEscrituraRepository>();
            services.AddScoped<IRegistrarFirmaApoderadoBancoRepository,RegistrarFirmaApoderadoBancoRepository>();
            services.AddScoped<IRealizarControlCreditoRepository, RealizarControlCreditoRepository>();
            services.AddScoped<ICorregirReparoControlCreditoRepository, CorregirReparoControlCreditoRepository>();
            services.AddScoped<IValidacionRectificatoriaLegalRepository, ValidacionRectificatoriaLegalRepository>();
            services.AddScoped<IValidacionRectificatoriaLegalDatosPersonalesRepository, ValidacionRectificatoriaLegalDatosPersonalesRepository>();

            services.AddScoped<IGestionRectificatoriaRepository, GestionRectificatoriaRepository>();
            services.AddScoped<IGestionRectificatoriaSolucionReparoRepository,GestionRectificatoriaSolucionReparoRepository>();
            services.AddScoped<IGestionReparoRepository, GestionReparoRepository>();
            services.AddScoped<IReparoFormularioRepository, ReparoFormularioRepository>();
            services.AddScoped<IRectificatoriaFirmaRepository, RectificatoriaFirmaRepository>();
            services.AddScoped<IRectificatoriaFirmaDetalleRepository, RectificatoriaFirmaDetalleRepository>();
            services.AddScoped<IRectificatoriaLegalCartaResguardoRepository, RectificatoriaLegalCartaResguardoRepository>();
            services.AddScoped<IRectificatoriaLegalFirmaAlzanteRepository, RectificatoriaLegalFirmaAlzanteRepository>();
            services.AddScoped<IRectificatoriaLegalCierreCopiasRepository, RectificatoriaLegalCierreCopiasRepository>();
            services.AddScoped<IRectificatoriaLegalCierreCopiasPostventaRepository, RectificatoriaLegalCierreCopiasPostventaRepository>();
            services.AddScoped<IGestionRectificatoriaEscrituraFirmadaRepository, GestionRectificatoriaEscrituraFirmadaRepository>();
            services.AddScoped<IRectificatoriaAnalisisDerivacionReparoPostventaRepository, RectificatoriaAnalisisDerivacionReparoPostventaRepository>();
            services.AddScoped<IRectificatoriaPostventaSolucionReparoRepository, RectificatoriaPostventaSolucionReparoRepository>();
            services.AddScoped<IValidacionRectificatoriaLegalPostventaRepository, ValidacionRectificatoriaLegalPostventaRepository>();
            services.AddScoped<IValidacionRectificatoriaLegalPostventaDatosPersonalesRepository, ValidacionRectificatoriaLegalPostventaDatosPersonalesRepository>();
            services.AddScoped<IRectificatoriaFirmaPostVentaRepository, RectificatoriaFirmaPostVentaRepository>();
            services.AddScoped<IRectificatoriaFirmaPostVentaDetalleRepository, RectificatoriaFirmaPostVentaDetalleRepository>();
            services.AddScoped<IGestionRectificatoriaEscrituraFirmadaPostventaRepository, GestionRectificatoriaEscrituraFirmadaPostventaRepository>();
            services.AddScoped<IRevisarCopiasEscriturasRepository, RevisarCopiasEscriturasRepository>();
            services.AddScoped<ICorregirReparoCopiasEscriturasRepository, CorregirReparoCopiasEscriturasRepository>();
            services.AddScoped<IAccesoTemporalRepository, AccesoTemporalRepository>();

            // BBVA Colombia — Presto Legalización
            services.AddScoped<IValidarInformacionRepository, ValidarInformacionRepository>();
            services.AddScoped<IRegistroContactoRepository, RegistroContactoRepository>();
            services.AddScoped<IDefinirInmuebleRepository, DefinirInmuebleRepository>();
            services.AddScoped<ICargarDocumentosConstructoraRepository, CargarDocumentosConstructoraRepository>();
            services.AddScoped<IRevisarDocumentosInmuebleRepository, RevisarDocumentosInmuebleRepository>();
            services.AddScoped<ICartaAprobacionBbvaRepository, CartaAprobacionBbvaRepository>();
            services.AddScoped<IAsignarFirmasRepository, AsignarFirmasRepository>();
            services.AddScoped<ICargarDocumentosClienteRepository, CargarDocumentosClienteRepository>();
            services.AddScoped<ICargarSoportesPagoRepository, CargarSoportesPagoRepository>();
            services.AddScoped<IValidarIntegracionRepository, ValidarIntegracionRepository>();
            services.AddScoped<IDevolucionVbComercialRepository, DevolucionVbComercialRepository>();
            services.AddScoped<IGestionarFirmaRepository, GestionarFirmaRepository>();
            services.AddScoped<IGestionarFirmaFisicaRepository, GestionarFirmaFisicaRepository>();
            services.AddScoped<IFirmarEscrituraClienteRepository, FirmarEscrituraClienteRepository>();
            services.AddScoped<IFirmarRepLegalRepository, FirmarRepLegalRepository>();
            services.AddScoped<IRevisarEpAbogadoRepository, RevisarEpAbogadoRepository>();

            return services;
        }
    }
}
