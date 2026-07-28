using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
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

public class ValidarCondicionesDesembolsoApplication
    : MultibancaGenericApplication<validar_condiciones_desembolso, validar_condiciones_desembolso_entity, IValidarCondicionesDesembolsoRepository>,
      IValidarCondicionesDesembolsoApplication
{
    private const string TransicionGestionComercial = Constants.TransicionesBBVA.ValidarDesembolsoGestionComercial;
    private const string TransicionGestionarEscalamientos = Constants.TransicionesBBVA.ValidarDesembolsoGestionarEscalamientos;
    private static readonly string ActividadValidarDesembolso = Constants.ActividadesBBVA.EscrituracionValidarCondicionesDesembolso;

    private readonly IMapper _mapper;
    private readonly ICommonApplication _commonApplication;
    private readonly IWorkflowApplication _workflowApplication;
    private readonly IBitacoraApplication _bitacoraApplication;
    private readonly IActividadesApplication _actividadesApplication;
    private readonly IRealizarVBFinalAbogadoRepository _vbFinalAbogadoRepository;
    private readonly IValidarInformacionRepository _validarInformacionRepository;
    private readonly IFirmarEscrituraClienteRepository _firmarEscrituraClienteRepository;
    private readonly IRealizarRecepcionBoletaRepository _recepcionBoletaRepository;

    public ValidarCondicionesDesembolsoApplication(
        MultibancaDBContext multibancaDBContext,
        IValidarCondicionesDesembolsoRepository repository,
        IMapper mapper,
        ICommonApplication commonApplication,
        IWorkflowApplication workflowApplication,
        IBitacoraApplication bitacoraApplication,
        IActividadesApplication actividadesApplication,
        IRealizarVBFinalAbogadoRepository vbFinalAbogadoRepository,
        IValidarInformacionRepository validarInformacionRepository,
        IFirmarEscrituraClienteRepository firmarEscrituraClienteRepository,
        IRealizarRecepcionBoletaRepository recepcionBoletaRepository)
        : base(multibancaDBContext, repository, mapper)
    {
        _mapper = mapper;
        _commonApplication = commonApplication;
        _workflowApplication = workflowApplication;
        _bitacoraApplication = bitacoraApplication;
        _actividadesApplication = actividadesApplication;
        _vbFinalAbogadoRepository = vbFinalAbogadoRepository;
        _validarInformacionRepository = validarInformacionRepository;
        _firmarEscrituraClienteRepository = firmarEscrituraClienteRepository;
        _recepcionBoletaRepository = recepcionBoletaRepository;
    }

    public async Task<object?> GetByExpediente(long idExpediente)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente);

        var formulario = entity != null
            ? _mapper.Map<validar_condiciones_desembolso>(entity)
            : new validar_condiciones_desembolso { id_expediente = idExpediente };

        // Calcular origen (CA02/CA08)
        bool vieneDeExcepcion = await _actividadesApplication.IsCompleteActivity(
            idExpediente, "BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO");
        bool vieneDeAbogado = await _actividadesApplication.IsCompleteActivity(
            idExpediente, Constants.ActividadesBBVA.EscrituracionRealizarVBFinalAbogado);
        bool vieneDeGestionComercial = await _actividadesApplication.IsCompleteActivity(
            idExpediente, "BBVA_ESCRITURACION_REALIZAR_GESTION_COMERCIAL");

        string origenTramite = vieneDeExcepcion ? "EXCEPCION"
            : vieneDeGestionComercial ? "COMERCIAL"
            : vieneDeAbogado ? "ABOGADO"
            : "PREFORMALIZAR";

        formulario.origen_tramite = origenTramite;

        // Puede suspender (CA03)
        bool hayRecepcionBoletaActiva = await _actividadesApplication.ExisteActividadActiva(
            idExpediente, Constants.ActividadesBBVA.EscrituracionRealizarRecepcionBoleta);
        bool hayEPRegistradasActiva = await _actividadesApplication.ExisteActividadActiva(
            idExpediente, Constants.ActividadesBBVA.EscrituracionRealizarEPRegistradas);
        bool puedeSuspender = hayRecepcionBoletaActiva || hayEPRegistradasActiva;

        // Conteo de caídas (CA06): se almacena en el registro y se incrementa al avanzar
        // Por ahora se retorna lo que está en BD
        int conteoCaidas = formulario.conteo_caidas;

        // Datos heredados (CA02) — condicional por origen
        var vbFinalAbogado = await _vbFinalAbogadoRepository.GetByExpediente(idExpediente);

        // Datos comunes de contexto (siempre se muestran)
        var validarInfo = await _validarInformacionRepository.GetByExpediente(idExpediente);
        var firmarEscritura = await _firmarEscrituraClienteRepository.GetByExpediente(idExpediente);
        var recepcionBoleta = await _recepcionBoletaRepository.GetByExpediente(idExpediente);

        // Resolver descripciones
        string? tipoDocDesc = null;
        if (!string.IsNullOrWhiteSpace(validarInfo?.tipo_id_t1))
        {
            var cat = await _commonApplication.GetCatalogoByType(Constants.Catalogo.TipoDocumentoId);
            tipoDocDesc = cat.FirstOrDefault(c => c.code == validarInfo.tipo_id_t1 || c.id.ToString() == validarInfo.tipo_id_t1)?.description ?? validarInfo.tipo_id_t1;
        }
        string? tipoCreditoDesc = null;
        if (!string.IsNullOrWhiteSpace(validarInfo?.tipo_credito))
        {
            var cat = await _commonApplication.GetCatalogoByType(Constants.Catalogo.TipoCredito);
            tipoCreditoDesc = cat.FirstOrDefault(c => c.code == validarInfo.tipo_credito)?.description ?? validarInfo.tipo_credito;
        }
        string? notariaDesc = null;
        if (!string.IsNullOrWhiteSpace(firmarEscritura?.notaria))
        {
            var cat = await _commonApplication.GetCatalogoByType(Constants.Catalogo.Notarias_L46);
            notariaDesc = cat.FirstOrDefault(c => c.code == firmarEscritura.notaria)?.description ?? firmarEscritura.notaria;
        }
        string? tipoBoletaDesc = null;
        if (!string.IsNullOrWhiteSpace(recepcionBoleta?.tipo_boleta))
        {
            var cat = await _commonApplication.GetCatalogoByType(Constants.Catalogo.TipoBoleta_L44);
            tipoBoletaDesc = cat.FirstOrDefault(c => c.code == recepcionBoleta.tipo_boleta)?.description ?? recepcionBoleta.tipo_boleta;
        }
        string? oficinaDesc = null;
        if (!string.IsNullOrWhiteSpace(recepcionBoleta?.oficina_registro))
        {
            var cat = await _commonApplication.GetCatalogoByType(Constants.Catalogo.OficinaRegistro_L45);
            oficinaDesc = cat.FirstOrDefault(c => c.code == recepcionBoleta.oficina_registro)?.description ?? recepcionBoleta.oficina_registro;
        }

        // Bloque condicional por origen
        string origenLabel = origenTramite == "ABOGADO" ? "Desde VB Final Abogado — Concepto jurídico positivo"
            : origenTramite == "EXCEPCION" ? "Desde Realizar Excepción Desembolso"
            : origenTramite == "COMERCIAL" ? "Desde Realizar Gestión Comercial — Cliente no desiste"
            : "Desde Preformalizar";

        return new
        {
            formulario,
            puede_suspender = puedeSuspender,
            origen_tramite = origenTramite,
            conteo_caidas = conteoCaidas,
            datos_heredados = new
            {
                origen_label = origenLabel,
                concepto_juridico = vbFinalAbogado?.requiere_devolucion == "NO" ? "Favorable (Sin devolución)" : (string?)null,
                observaciones_abogado = vbFinalAbogado?.observaciones,
                // Datos Cliente
                tipo_documento = tipoDocDesc,
                numero_documento = validarInfo?.numero_id_t1,
                nombre_completo = validarInfo?.nombre_completo_t1,
                tipo_credito = tipoCreditoDesc,
                // Datos Notaría
                notaria = notariaDesc,
                numero_notaria = firmarEscritura?.numero_notaria,
                ciudad_notaria = firmarEscritura?.ciudad_notaria,
                numero_escritura = firmarEscritura?.numero_escritura,
                // Datos Boleta
                numero_boleta = recepcionBoleta?.numero_boleta,
                fecha_boleta = recepcionBoleta?.fecha_boleta,
                tipo_boleta = tipoBoletaDesc,
                oficina_registro = oficinaDesc,
                numero_matricula = recepcionBoleta?.numero_matricula,
            }
        };
    }

    public async Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente)
            ?? throw new InvalidOperationException("Debe guardar la información antes de avanzar.");

        var formulario = _mapper.Map<validar_condiciones_desembolso>(entity);

        ValidarCamposObligatorios(formulario);

        var actividadesCreadas = new List<AssignActivityDTO>();
        List<xpdl_transition_DTO> transitions = await _workflowApplication.GetTransitions(ActividadValidarDesembolso);
        FolioDTO folio = await _workflowApplication.CapturarDatosFolio(idExpediente, ActividadValidarDesembolso);

        string transicionSeleccionada;
        string destinoActividad;

        if (formulario.requiere_escalamiento_comercial == "SI")
        {
            transicionSeleccionada = TransicionGestionComercial;
            destinoActividad = "Realizar Gestión Comercial";
        }
        else
        {
            transicionSeleccionada = TransicionGestionarEscalamientos;
            destinoActividad = "Gestionar Escalamientos";
        }

        var transitionId = transitions.FirstOrDefault(x => x.name == transicionSeleccionada)?.transition_id
            ?? throw new InvalidOperationException($"No se encontró la transición '{transicionSeleccionada}' en el workflow.");

        var resultado = await _workflowApplication.AvanzarActividad(transitionId, folio, userId);
        actividadesCreadas.AddRange(resultado);

        // Bitácora
        var obs = $"Avance de Validar Condiciones Desembolso. " +
            $"Plan de Pagos confirmado: Sí. " +
            $"¿Requiere Escalamiento Comercial?: {formulario.requiere_escalamiento_comercial}. " +
            $"Destino: [{destinoActividad}].";

        if (!string.IsNullOrWhiteSpace(formulario.observaciones))
            obs += $" Observaciones: {formulario.observaciones}";

        _bitacoraApplication.Create(new bitacora
        {
            id_expediente = idExpediente,
            id_actividad = ActividadValidarDesembolso,
            id_usuario = userId,
            fecha_alta = DateTime.Now,
            observaciones = obs,
            is_active = true,
            row_status = true
        }, userId);

        return actividadesCreadas;
    }

    public async Task<bool> Suspender(long idExpediente, int userId)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente)
            ?? throw new InvalidOperationException("No existe registro para suspender.");

        var formulario = _mapper.Map<validar_condiciones_desembolso>(entity);
        formulario.suspendida = true;
        Update(formulario, userId);

        _bitacoraApplication.Create(new bitacora
        {
            id_expediente = idExpediente,
            id_actividad = ActividadValidarDesembolso,
            id_usuario = userId,
            fecha_alta = DateTime.Now,
            observaciones = "Actividad suspendida. Hay actividades registrales en curso paralelo.",
            is_active = true,
            row_status = true
        }, userId);

        return true;
    }

    private static void ValidarCamposObligatorios(validar_condiciones_desembolso formulario)
    {
        var camposFaltantes = new List<string>();

        if (!formulario.confirmar_plan_pagos)
            camposFaltantes.Add("Confirmar verificación del Plan de Pagos");

        if (string.IsNullOrWhiteSpace(formulario.requiere_escalamiento_comercial))
            camposFaltantes.Add("¿Requiere Escalamiento Comercial?");

        if (camposFaltantes.Count > 0)
            throw new InvalidOperationException($"Campos obligatorios faltantes: {string.Join(", ", camposFaltantes)}");
    }
}
