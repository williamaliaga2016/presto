using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.BBVA.Escrituracion;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Common;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

namespace Multibanca.Application.Implementations.Multibanca.BBVA.Escrituracion;

public class FirmarRepLegalApplication
    : MultibancaGenericApplication<firmar_rep_legal, firmar_rep_legal_entity, IFirmarRepLegalRepository>,
      IFirmarRepLegalApplication
{
    // Constantes de transición
    private const string TransicionDevolucion = Constants.TransicionesBBVA.FirmarRepLegalDevolucion;
    private const string TransicionEntregaEP = Constants.TransicionesBBVA.FirmarRepLegalEntregaEP;
    private const string TransicionPreformalizar = Constants.TransicionesBBVA.FirmarRepLegalPreformalizar;
    private const string TransicionExcepcionDesembolso = Constants.TransicionesBBVA.FirmarRepLegalExcepcionDesembolso;

    // ID de la actividad actual en el workflow
    private static readonly string ActividadFirmarRepLegal = Constants.ActividadesBBVA.EscrituracionFirmarRepLegal;

    // Códigos de concepto de firma
    private const string ConceptoFirmadaConforme = "CRL-1";
    private const string ConceptoNoFirmada = "CRL-2";

    // Constante para tipo desembolso
    private const string TipoDesembolsoEscritura = "ESCRITURA";

    // Tipos de crédito que aplican excepción desembolso
    private static readonly string[] TiposExcepcionDesembolso = new[]
    {
        "CONSTRUCTOR_INDIVIDUAL",
        "HIPOTECARIO_CXI",
        "HIPOTECARIO_USADO",
        "LEASING_NUEVO",
        "LEASING_USADO",
        "LEASING_CXI",
        "REMODELACION_AMPLIAR_HIPOTECAR"
    };

    // Dependencias
    private readonly IMapper _mapper;
    private readonly ICommonApplication _commonApplication;
    private readonly IWorkflowApplication _workflowApplication;
    private readonly IBitacoraApplication _bitacoraApplication;
    private readonly IValidarInformacionRepository _validarInformacionRepository;
    private readonly ITradicionesConocidasRepository _tradicionesRepository;
    private readonly IActividadesApplication _actividadesApplication;

    public FirmarRepLegalApplication(
        MultibancaDBContext multibancaDBContext,
        IFirmarRepLegalRepository firmarRepLegalRepository,
        IMapper mapper,
        ICommonApplication commonApplication,
        IWorkflowApplication workflowApplication,
        IBitacoraApplication bitacoraApplication,
        IValidarInformacionRepository validarInformacionRepository,
        ITradicionesConocidasRepository tradicionesRepository,
        IActividadesApplication actividadesApplication)
        : base(multibancaDBContext, firmarRepLegalRepository, mapper)
    {
        _mapper = mapper;
        _commonApplication = commonApplication;
        _workflowApplication = workflowApplication;
        _bitacoraApplication = bitacoraApplication;
        _validarInformacionRepository = validarInformacionRepository;
        _tradicionesRepository = tradicionesRepository;
        _actividadesApplication = actividadesApplication;
    }

    public async Task<firmar_rep_legal?> GetByExpediente(long idExpediente)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente);

        if (entity == null)
        {
            return new firmar_rep_legal
            {
                id_expediente = idExpediente,
                concepto_firma = null,
                tipologia = null,
                casuistica = null,
                observaciones = null
            };
        }

        return _mapper.Map<firmar_rep_legal>(entity);
    }

    public async Task<object> GetControles()
    {
        var conceptoFirma = await _commonApplication.GetCatalogoByType(Constants.Catalogo.ConceptoFirmaRepLegal_L41);
        var tipologia = await _commonApplication.GetCatalogoByType(Constants.Catalogo.TipologiaRepLegal_L42);
        var casuistica = await _commonApplication.GetCatalogoByTypeWithParentCode(Constants.Catalogo.CasuisticaRepLegal_L43);

        return new
        {
            concepto_firma = conceptoFirma,
            tipologia = tipologia,
            casuistica = casuistica
        };
    }

    public async Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente)
            ?? throw new InvalidOperationException("Debe guardar la información antes de avanzar.");

        var formulario = _mapper.Map<firmar_rep_legal>(entity);

        // Validar campos obligatorios
        ValidarCamposObligatorios(formulario);

        var actividadesCreadas = new List<AssignActivityDTO>();
        List<xpdl_transition_DTO> transitions = await _workflowApplication.GetTransitions(ActividadFirmarRepLegal);
        FolioDTO folio = await _workflowApplication.CapturarDatosFolio(idExpediente, ActividadFirmarRepLegal);

        if (formulario.concepto_firma == ConceptoNoFirmada)
        {
            // CRL-2: Escritura NO firmada → Realizar Devolución EP (lineal)
            var transitionId = transitions.FirstOrDefault(x => x.name == TransicionDevolucion)?.transition_id
                ?? throw new InvalidOperationException($"No se encontró la transición '{TransicionDevolucion}' en el workflow.");

            var resultado = await _workflowApplication.AvanzarActividad(transitionId, folio, userId);
            actividadesCreadas.AddRange(resultado);

            RegistrarBitacora(idExpediente, userId, formulario, "Realizar Devolución EP");
        }
        else
        {
            // CRL-1: Escritura firmada Conforme
            // El workflow pasa por un nodo Parallel que crea: Entrega EP + Preformalizar
            var transEntrega = transitions.FirstOrDefault(x => x.name == TransicionEntregaEP)?.transition_id
                ?? throw new InvalidOperationException($"No se encontró la transición '{TransicionEntregaEP}' en el workflow.");

            var resultadoParalelo = await _workflowApplication.AvanzarActividad(transEntrega, folio, userId);
            actividadesCreadas.AddRange(resultadoParalelo);

            // Evaluar condición de Excepción Desembolso (se crea manualmente, no por workflow)
            if (await AplicaExcepcionDesembolso(idExpediente))
            {
                try
                {
                    var excepcionActividad = new actividades
                    {
                        id_expediente = idExpediente,
                        id_actividad = Constants.ActividadesBBVA.EscrituracionRealizarExcepcionDesembolso,
                        id_rol = 0, // Se asignará por el CommonApplication
                        id_usuario = 0,
                        descripcion = "Realizar Excepción Desembolso",
                        status = "Nueva",
                        activo = true,
                        fecha_asignacion = DateTime.Now,
                        fecha_alta = DateTime.Now
                    };

                    // Asignar rol y usuario según cat_actividades_ws
                    var asignacion = await _commonApplication.AsignarActividad(idExpediente, "COMERCIAL");
                    excepcionActividad.id_rol = (int)asignacion.id_rol;
                    excepcionActividad.id_usuario = (int)asignacion.id_usuario;

                    var created = _actividadesApplication.Create(excepcionActividad, userId);
                    actividadesCreadas.Add(new AssignActivityDTO
                    {
                        id_actividad = created.id_actividad,
                        display_name = "Realizar Excepción Desembolso",
                        id_rol = (int)created.id_rol,
                        id_usuario = (int)created.id_usuario
                    });
                }
                catch (Exception ex)
                {
                    // No bloquear el flujo principal si falla la creación de Excepción
                    _bitacoraApplication.Create(new bitacora
                    {
                        id_expediente = idExpediente,
                        id_actividad = ActividadFirmarRepLegal,
                        id_usuario = userId,
                        fecha_alta = DateTime.Now,
                        observaciones = $"ADVERTENCIA: Aplica Excepción Desembolso pero no se pudo crear: {ex.Message}",
                        is_active = true,
                        row_status = true
                    }, userId);
                }
            }

            var destinos = string.Join(" + ", actividadesCreadas.Select(a => a.display_name ?? "N/A"));
            RegistrarBitacora(idExpediente, userId, formulario, destinos);
        }

        return actividadesCreadas;
    }

    // ======================== Métodos privados ========================

    /// <summary>
    /// Evalúa si aplica Excepción de Desembolso:
    /// 1. Buscar código_proyecto del expediente en tabla tradiciones_conocidas
    /// 2. Si existe y tipo_desembolso = "ESCRITURA"
    /// 3. Verificar tipo_credito en la lista de tipos que aplican
    /// </summary>
    private async Task<bool> AplicaExcepcionDesembolso(long idExpediente)
    {
        // Obtener datos del expediente (código_proyecto y tipo_credito)
        var validarInfo = await _validarInformacionRepository.GetByExpediente(idExpediente);
        string? codigoProyecto = validarInfo?.codigo_proyecto;
        if (string.IsNullOrEmpty(codigoProyecto)) return false;

        // Buscar en tradiciones_conocidas por código_proyecto
        var tradicion = await _tradicionesRepository.GetByCodigoProyecto(codigoProyecto);
        if (tradicion == null) return false;

        // Verificar tipo_desembolso = "ESCRITURA"
        if (!string.Equals(tradicion.tipo_desembolso, TipoDesembolsoEscritura, StringComparison.OrdinalIgnoreCase))
            return false;

        // Verificar tipo_credito del expediente contra la lista
        string? tipoCredito = validarInfo?.tipo_credito;
        return !string.IsNullOrEmpty(tipoCredito)
            && TiposExcepcionDesembolso.Contains(tipoCredito, StringComparer.OrdinalIgnoreCase);
    }

    private static void ValidarCamposObligatorios(firmar_rep_legal formulario)
    {
        var camposFaltantes = new List<string>();

        if (string.IsNullOrWhiteSpace(formulario.concepto_firma))
            camposFaltantes.Add("Concepto de Firma");

        if (formulario.concepto_firma == ConceptoNoFirmada)
        {
            if (string.IsNullOrWhiteSpace(formulario.tipologia))
                camposFaltantes.Add("Tipología");

            if (string.IsNullOrWhiteSpace(formulario.casuistica))
                camposFaltantes.Add("Casuística");

            if (string.IsNullOrWhiteSpace(formulario.observaciones))
                camposFaltantes.Add("Observaciones");
        }

        if (camposFaltantes.Count > 0)
        {
            throw new InvalidOperationException(
                $"Campos obligatorios faltantes: {string.Join(", ", camposFaltantes)}");
        }
    }

    private void RegistrarBitacora(
        long idExpediente,
        int userId,
        firmar_rep_legal formulario,
        string destinoActividad)
    {
        var observacionesBitacora = $"Avance de Firmar Rep. Legal. " +
            $"Concepto: {formulario.concepto_firma}. " +
            $"Destino: [{destinoActividad}].";

        if (formulario.concepto_firma == ConceptoNoFirmada)
        {
            observacionesBitacora += $" Tipología: {formulario.tipologia}. Casuística: {formulario.casuistica}.";
        }

        if (!string.IsNullOrWhiteSpace(formulario.observaciones))
            observacionesBitacora += $" Observaciones: {formulario.observaciones}";

        _bitacoraApplication.Create(new bitacora
        {
            id_expediente = idExpediente,
            id_actividad = ActividadFirmarRepLegal,
            id_usuario = userId,
            fecha_alta = DateTime.Now,
            observaciones = observacionesBitacora,
            is_active = true,
            row_status = true
        }, userId);
    }
}
