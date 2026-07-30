using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Framework.WorkFlow.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.BBVA.Escrituracion;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Common;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

namespace Multibanca.Application.Implementations.Multibanca.BBVA.Escrituracion;

public class RealizarVBFinalAbogadoApplication
    : MultibancaGenericApplication<realizar_vb_final_abogado, realizar_vb_final_abogado_entity, IRealizarVBFinalAbogadoRepository>,
      IRealizarVBFinalAbogadoApplication
{
    private const string TransicionDevolucionEP = Constants.TransicionesBBVA.VBFinalAbogadoDevolucionEP;
    private const string TransicionValidarDesembolso = Constants.TransicionesBBVA.VBFinalAbogadoValidarDesembolso;
    private const string TransicionControlGarantias = Constants.TransicionesBBVA.VBFinalAbogadoControlGarantias;

    private static readonly string ActividadVBFinalAbogado = Constants.ActividadesBBVA.EscrituracionRealizarVBFinalAbogado;

    private readonly IMapper _mapper;
    private readonly ICommonApplication _commonApplication;
    private readonly IWorkflowApplication _workflowApplication;
    private readonly IBitacoraApplication _bitacoraApplication;
    private readonly IActividadesApplication _actividadesApplication;
    private readonly IActivityWorkflowApplication _activityWorkflowApplication;
    private readonly IRealizarEntregaEpFirmadaRepository _entregaEpRepository;
    private readonly IRealizarRecepcionBoletaRepository _recepcionBoletaRepository;
    private readonly IRealizarEPRegistradasRepository _epRegistradasRepository;
    private readonly IFirmarEscrituraClienteRepository _firmarEscrituraClienteRepository;
    private readonly IValidarInformacionRepository _validarInformacionRepository;

    public RealizarVBFinalAbogadoApplication(
        MultibancaDBContext multibancaDBContext,
        IRealizarVBFinalAbogadoRepository repository,
        IMapper mapper,
        ICommonApplication commonApplication,
        IWorkflowApplication workflowApplication,
        IBitacoraApplication bitacoraApplication,
        IActividadesApplication actividadesApplication,
        IActivityWorkflowApplication activityWorkflowApplication,
        IRealizarEntregaEpFirmadaRepository entregaEpRepository,
        IRealizarRecepcionBoletaRepository recepcionBoletaRepository,
        IRealizarEPRegistradasRepository epRegistradasRepository,
        IFirmarEscrituraClienteRepository firmarEscrituraClienteRepository,
        IValidarInformacionRepository validarInformacionRepository)
        : base(multibancaDBContext, repository, mapper)
    {
        _mapper = mapper;
        _commonApplication = commonApplication;
        _workflowApplication = workflowApplication;
        _bitacoraApplication = bitacoraApplication;
        _actividadesApplication = actividadesApplication;
        _activityWorkflowApplication = activityWorkflowApplication;
        _entregaEpRepository = entregaEpRepository;
        _recepcionBoletaRepository = recepcionBoletaRepository;
        _epRegistradasRepository = epRegistradasRepository;
        _firmarEscrituraClienteRepository = firmarEscrituraClienteRepository;
        _validarInformacionRepository = validarInformacionRepository;
    }

    public async Task<object?> GetByExpediente(long idExpediente)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente);

        var formulario = entity != null
            ? _mapper.Map<realizar_vb_final_abogado>(entity)
            : new realizar_vb_final_abogado { id_expediente = idExpediente };

        // Calcular banderas (CA06)
        bool vieneDeControlGarantias = await _actividadesApplication.IsCompleteActivity(
            idExpediente, Constants.ActividadesBBVA.EscrituracionGestionarControlGarantias);

        var entregaEP = await _entregaEpRepository.GetByExpediente(idExpediente);
        bool banderaExcepcion = entregaEP?.aplica_excepcion == "SI";

        formulario.origen_tramite = vieneDeControlGarantias ? "CONTROL_GARANTIAS" : "EP_REGISTRADAS";
        formulario.bandera_excepcion = banderaExcepcion;

        // Datos heredados (CA02) — si viene de EP Registradas
        var recepcionBoleta = await _recepcionBoletaRepository.GetByExpediente(idExpediente);
        var epRegistradas = await _epRegistradasRepository.GetByExpediente(idExpediente);
        var firmarEscritura = await _firmarEscrituraClienteRepository.GetByExpediente(idExpediente);
        var validarInfo = await _validarInformacionRepository.GetByExpediente(idExpediente);

        // Resolver tipo boleta
        string? tipoBoletaDescripcion = null;
        if (!string.IsNullOrWhiteSpace(recepcionBoleta?.tipo_boleta))
        {
            var catalogoTipoBoleta = await _commonApplication.GetCatalogoByType(Constants.Catalogo.TipoBoleta_L44);
            tipoBoletaDescripcion = catalogoTipoBoleta
                .FirstOrDefault(c => c.code == recepcionBoleta.tipo_boleta)?.description
                ?? recepcionBoleta.tipo_boleta;
        }

        // Resolver oficina de registro
        string? oficinaRegistroDescripcion = null;
        if (!string.IsNullOrWhiteSpace(recepcionBoleta?.oficina_registro))
        {
            var catalogoOficina = await _commonApplication.GetCatalogoByType(Constants.Catalogo.OficinaRegistro_L45);
            oficinaRegistroDescripcion = catalogoOficina
                .FirstOrDefault(c => c.code == recepcionBoleta.oficina_registro)?.description
                ?? recepcionBoleta.oficina_registro;
        }

        // Resolver tipo documento
        string? tipoDocumentoDescripcion = null;
        if (!string.IsNullOrWhiteSpace(validarInfo?.tipo_id_t1))
        {
            var catalogoTipoDoc = await _commonApplication.GetCatalogoByType(Constants.Catalogo.TipoDocumentoId);
            tipoDocumentoDescripcion = catalogoTipoDoc
                .FirstOrDefault(c => c.code == validarInfo.tipo_id_t1 || c.id.ToString() == validarInfo.tipo_id_t1)?.description
                ?? validarInfo.tipo_id_t1;
        }

        // Resolver notaría
        string? notariaDescripcion = null;
        if (!string.IsNullOrWhiteSpace(firmarEscritura?.notaria))
        {
            var catalogoNotaria = await _commonApplication.GetCatalogoByType(Constants.Catalogo.Notarias_L46);
            notariaDescripcion = catalogoNotaria
                .FirstOrDefault(c => c.code == firmarEscritura.notaria)?.description
                ?? firmarEscritura.notaria;
        }

        // Resolver tipo crédito
        string? tipoCreditoDescripcion = null;
        if (!string.IsNullOrWhiteSpace(validarInfo?.tipo_credito))
        {
            var catalogoTipoCredito = await _commonApplication.GetCatalogoByType(Constants.Catalogo.TipoCredito);
            tipoCreditoDescripcion = catalogoTipoCredito
                .FirstOrDefault(c => c.code == validarInfo.tipo_credito)?.description
                ?? validarInfo.tipo_credito;
        }

        return new
        {
            formulario,
            origen_tramite = formulario.origen_tramite,
            bandera_excepcion = formulario.bandera_excepcion,
            datos_heredados = new
            {
                // Datos Cliente
                tipo_documento = tipoDocumentoDescripcion,
                numero_documento = validarInfo?.numero_id_t1,
                nombre_completo = validarInfo?.nombre_completo_t1,
                tipo_credito = tipoCreditoDescripcion,
                // Datos Notaría
                notaria = notariaDescripcion,
                numero_notaria = firmarEscritura?.numero_notaria,
                ciudad_notaria = firmarEscritura?.ciudad_notaria,
                numero_escritura = firmarEscritura?.numero_escritura,
                fecha_escritura = firmarEscritura?.fecha_escritura,
                // Datos Recepción Boleta
                numero_boleta = recepcionBoleta?.numero_boleta,
                fecha_boleta = recepcionBoleta?.fecha_boleta,
                tipo_boleta = tipoBoletaDescripcion,
                oficina_registro = oficinaRegistroDescripcion,
                numero_matricula = recepcionBoleta?.numero_matricula,
                // Datos EP Registradas
                confirmacion_ep_registrada = epRegistradas?.confirmacion_ep_registrada,
                finalizacion = epRegistradas?.finalizacion,
                causal_ep = epRegistradas?.causal,
            }
        };
    }

    public async Task<object> GetControles()
    {
        var tipologia = await _commonApplication.GetCatalogoByType(Constants.Catalogo.TipologiaCorreccionEP_L39);
        var casuistica = await _commonApplication.GetCatalogoByType(Constants.Catalogo.CasuisticaCorreccionEP_L40);

        return new
        {
            tipologia,
            casuistica
        };
    }

    public async Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente)
            ?? throw new InvalidOperationException("Debe guardar la información antes de avanzar.");

        var formulario = _mapper.Map<realizar_vb_final_abogado>(entity);

        // Recalcular banderas
        bool vieneDeControlGarantias = await _actividadesApplication.IsCompleteActivity(
            idExpediente, Constants.ActividadesBBVA.EscrituracionGestionarControlGarantias);
        var entregaEP = await _entregaEpRepository.GetByExpediente(idExpediente);
        bool banderaExcepcion = entregaEP?.aplica_excepcion == "SI";

        formulario.origen_tramite = vieneDeControlGarantias ? "CONTROL_GARANTIAS" : "EP_REGISTRADAS";
        formulario.bandera_excepcion = banderaExcepcion;

        ValidarCamposObligatorios(formulario);

        var actividadesCreadas = new List<AssignActivityDTO>();
        List<xpdl_transition_DTO> transitions = await _workflowApplication.GetTransitions(ActividadVBFinalAbogado);
        FolioDTO folio = await _workflowApplication.CapturarDatosFolio(idExpediente, ActividadVBFinalAbogado);

        string transicionSeleccionada;
        string destinoActividad;

        if (formulario.requiere_devolucion == "SI")
        {
            // CA04 — Devolución EP
            transicionSeleccionada = TransicionDevolucionEP;
            destinoActividad = "Realizar Devolución EP";
        }
        else
        {
            // CA05 — Avance sin devolución
            if (formulario.bandera_excepcion)
            {
                // Escenario A: Aplica excepción → Control de Garantías
                transicionSeleccionada = TransicionControlGarantias;
                destinoActividad = "Gestionar Control de Garantías";
            }
            else if (formulario.origen_tramite == "CONTROL_GARANTIAS")
            {
                // Escenario C: Viene de Control de Garantías (bypass) → Retorna a Control de Garantías
                transicionSeleccionada = TransicionControlGarantias;
                destinoActividad = "Gestionar Control de Garantías";
            }
            else
            {
                // Escenario B: Sin excepción, origen regular → Validar Condiciones Desembolso
                // AND-JOIN: Solo crear Validar Condiciones si la ruta corta (Excepción Desembolso) ya completó o no aplica
                bool existeExcepcion = await _actividadesApplication.ExisteActividad(
                    idExpediente, Constants.ActividadesBBVA.EscrituracionRealizarExcepcionDesembolso);

                if (existeExcepcion)
                {
                    bool excepcionCompletada = await _actividadesApplication.IsCompleteActivity(
                        idExpediente, Constants.ActividadesBBVA.EscrituracionRealizarExcepcionDesembolso);

                    if (!excepcionCompletada)
                    {
                        // Excepción aún en curso → completar VB Final y esperar
                        // Cuando Excepción complete, verá VB Final completado y creará Validar Condiciones
                        var actVBFinal = await _actividadesApplication.ObtenerActividadPorExpedienteActividad(
                            idExpediente, ActividadVBFinalAbogado);
                        if (actVBFinal != null && actVBFinal.id > 0)
                            await _actividadesApplication.CompletarActividad(actVBFinal.id, (long)userId);

                        // También actualizar case_activities para que el workflow engine no la recree
                        await _activityWorkflowApplication.UpdateCaseActivityStatus(
                            "Completed", idExpediente, ActividadVBFinalAbogado, null);

                        RegistrarBitacora(idExpediente, userId, formulario, "VB Final completado. Esperando Excepción Desembolso (AND-JOIN)");
                        return actividadesCreadas;
                    }
                }

                // Excepción completada o no aplica → avanzar a Validar Condiciones
                transicionSeleccionada = TransicionValidarDesembolso;
                destinoActividad = "Validar Condiciones Desembolso";
            }
        }

        var transitionId = transitions.FirstOrDefault(x => x.name == transicionSeleccionada)?.transition_id
            ?? throw new InvalidOperationException($"No se encontró la transición '{transicionSeleccionada}' en el workflow.");

        var resultado = await _workflowApplication.AvanzarActividad(transitionId, folio, userId);
        actividadesCreadas.AddRange(resultado);

        RegistrarBitacora(idExpediente, userId, formulario, destinoActividad);

        return actividadesCreadas;
    }

    private static void ValidarCamposObligatorios(realizar_vb_final_abogado formulario)
    {
        var camposFaltantes = new List<string>();

        if (string.IsNullOrWhiteSpace(formulario.requiere_devolucion))
            camposFaltantes.Add("¿Requiere Devolución?");

        if (formulario.requiere_devolucion == "SI")
        {
            if (string.IsNullOrWhiteSpace(formulario.tipologia))
                camposFaltantes.Add("Tipología");
            if (string.IsNullOrWhiteSpace(formulario.casuistica))
                camposFaltantes.Add("Casuística");
            if (string.IsNullOrWhiteSpace(formulario.observaciones))
                camposFaltantes.Add("Observaciones");
        }

        if (camposFaltantes.Count > 0)
            throw new InvalidOperationException($"Campos obligatorios faltantes: {string.Join(", ", camposFaltantes)}");
    }

    private void RegistrarBitacora(long idExpediente, int userId, realizar_vb_final_abogado formulario, string destinoActividad)
    {
        var obs = $"Avance de Realizar VB Final Abogado. " +
            $"¿Requiere Devolución?: {formulario.requiere_devolucion}. " +
            $"Destino: [{destinoActividad}].";

        if (formulario.requiere_devolucion == "SI")
            obs += $" Tipología: {formulario.tipologia}. Casuística: {formulario.casuistica}.";

        if (!string.IsNullOrWhiteSpace(formulario.observaciones))
            obs += $" Observaciones: {formulario.observaciones}";

        _bitacoraApplication.Create(new bitacora
        {
            id_expediente = idExpediente,
            id_actividad = ActividadVBFinalAbogado,
            id_usuario = userId,
            fecha_alta = DateTime.Now,
            observaciones = obs,
            is_active = true,
            row_status = true
        }, userId);
    }
}
