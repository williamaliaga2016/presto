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
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

namespace Multibanca.Application.Implementations.Multibanca.BBVA.Escrituracion;

public class RealizarDevolucionEPApplication
    : MultibancaGenericApplication<realizar_devolucion_ep, realizar_devolucion_ep_entity, IRealizarDevolucionEPRepository>,
      IRealizarDevolucionEPApplication
{
    // Transiciones
    private const string TransicionGestionComercial = Constants.TransicionesBBVA.DevolucionEPGestionComercial;
    private const string TransicionFirmarEscritura = Constants.TransicionesBBVA.DevolucionEPFirmarEscritura;
    private const string TransicionFirmarRepLegal = Constants.TransicionesBBVA.DevolucionEPFirmarRepLegal;
    private const string TransicionEPRegistradas = Constants.TransicionesBBVA.DevolucionEPEPRegistradas;

    private static readonly string ActividadDevolucionEP = Constants.ActividadesBBVA.EscrituracionRealizarDevolucionEP;

    private readonly IMapper _mapper;
    private readonly ICommonApplication _commonApplication;
    private readonly IWorkflowApplication _workflowApplication;
    private readonly IBitacoraApplication _bitacoraApplication;

    // Repositorios de orígenes (para grilla de dictámenes)
    private readonly IRealizarVBFinalAbogadoRepository _vbFinalAbogadoRepository;
    private readonly IFirmarRepLegalRepository _firmarRepLegalRepository;
    private readonly IRevisarEpAbogadoRepository _revisarEpAbogadoRepository;
    private readonly IRealizarGestionComercialRepository _gestionComercialRepository;
    private readonly IActividadesApplication _actividadesApplication;

    public RealizarDevolucionEPApplication(
        MultibancaDBContext multibancaDBContext,
        IRealizarDevolucionEPRepository repository,
        IMapper mapper,
        ICommonApplication commonApplication,
        IWorkflowApplication workflowApplication,
        IBitacoraApplication bitacoraApplication,
        IRealizarVBFinalAbogadoRepository vbFinalAbogadoRepository,
        IFirmarRepLegalRepository firmarRepLegalRepository,
        IRevisarEpAbogadoRepository revisarEpAbogadoRepository,
        IRealizarGestionComercialRepository gestionComercialRepository,
        IActividadesApplication actividadesApplication)
        : base(multibancaDBContext, repository, mapper)
    {
        _mapper = mapper;
        _commonApplication = commonApplication;
        _workflowApplication = workflowApplication;
        _bitacoraApplication = bitacoraApplication;
        _vbFinalAbogadoRepository = vbFinalAbogadoRepository;
        _firmarRepLegalRepository = firmarRepLegalRepository;
        _revisarEpAbogadoRepository = revisarEpAbogadoRepository;
        _gestionComercialRepository = gestionComercialRepository;
        _actividadesApplication = actividadesApplication;
    }

    public async Task<object?> GetByExpediente(long idExpediente)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente);
        var formulario = entity != null
            ? _mapper.Map<realizar_devolucion_ep>(entity)
            : new realizar_devolucion_ep { id_expediente = idExpediente };

        // Grilla consolidada de Conceptos / Dictámenes Previos (CA02/CA05)
        // Consulta los 4 orígenes que pueden llegar a Devolución EP
        var dictamenes = await BuildDictamenesPrevios(idExpediente);

        return new
        {
            formulario,
            dictamenes_previos = dictamenes
        };
    }

    /// <summary>
    /// Construye la grilla de Conceptos/Dictámenes Previos.
    /// Muestra una fila por cada origen que haya sido completado para este expediente.
    /// Resuelve códigos de paramétrica a su descripción legible.
    /// </summary>
    private async Task<List<object>> BuildDictamenesPrevios(long idExpediente)
    {
        var dictamenes = new List<object>();

        // 1. Firmar Rep. Legal (BBV-91) — paramétricas L42/L43
        bool pasoRepLegal = await _actividadesApplication.IsCompleteActivity(idExpediente, Constants.ActividadesBBVA.EscrituracionFirmarRepLegal);
        if (pasoRepLegal)
        {
            var repLegal = await _firmarRepLegalRepository.GetByExpediente(idExpediente);
            dictamenes.Add(new
            {
                area = "Rep. Legal",
                tipologia = await ResolverDescripcion(Constants.Catalogo.TipologiaRepLegal_L42, repLegal?.tipologia),
                casuistica = await ResolverDescripcion(Constants.Catalogo.CasuisticaRepLegal_L43, repLegal?.casuistica),
                observaciones = repLegal?.observaciones
            });
        }

        // 2. Realizar VB Final Abogado (BBV-96) — paramétricas L39/L40
        bool pasoVBFinal = await _actividadesApplication.IsCompleteActivity(idExpediente, Constants.ActividadesBBVA.EscrituracionRealizarVBFinalAbogado);
        if (pasoVBFinal)
        {
            var vbFinal = await _vbFinalAbogadoRepository.GetByExpediente(idExpediente);
            dictamenes.Add(new
            {
                area = "Abogado",
                tipologia = await ResolverDescripcion(Constants.Catalogo.TipologiaCorreccionEP_L39, vbFinal?.tipologia),
                casuistica = await ResolverDescripcion(Constants.Catalogo.CasuisticaCorreccionEP_L40, vbFinal?.casuistica),
                observaciones = vbFinal?.observaciones
            });
        }

        // 3. Revisar EP Abogado (BBV-130) — paramétricas L39/L40
        bool pasoRevisarEP = await _actividadesApplication.IsCompleteActivity(idExpediente, Constants.ActividadesBBVA.EscrituracionRevisarEPAbogado);
        if (pasoRevisarEP)
        {
            var revisarEP = await _revisarEpAbogadoRepository.GetByExpediente(idExpediente);
            dictamenes.Add(new
            {
                area = "Revisar EP",
                tipologia = await ResolverDescripcion(Constants.Catalogo.TipologiaCorreccionEP_L39, revisarEP?.tipologia),
                casuistica = await ResolverDescripcion(Constants.Catalogo.CasuisticaCorreccionEP_L40, revisarEP?.casuistica),
                observaciones = revisarEP?.observaciones_legales
            });
        }

        // 4. Realizar Gestión Comercial — sin paramétricas de tipología/casuística
        bool pasoGestionComercial = await _actividadesApplication.IsCompleteActivity(idExpediente, Constants.ActividadesBBVA.EscrituracionRealizarGestionComercial);
        if (pasoGestionComercial)
        {
            var gestionComercial = await _gestionComercialRepository.GetByExpediente(idExpediente);
            dictamenes.Add(new
            {
                area = "Gestión Comercial",
                tipologia = (string?)null,
                casuistica = (string?)null,
                observaciones = gestionComercial?.observaciones
            });
        }

        return dictamenes;
    }

    /// <summary>
    /// Resuelve un código de catálogo a su descripción. Si no encuentra, retorna el código original.
    /// </summary>
    private async Task<string?> ResolverDescripcion(string tipoCatalogo, string? codigo)
    {
        if (string.IsNullOrEmpty(codigo)) return null;
        var catalogo = await _commonApplication.GetCatalogoByTypeAndCode(tipoCatalogo, codigo);
        return catalogo?.description ?? codigo;
    }

    public async Task<object> GetControles()
    {
        var tipologia = await _commonApplication.GetCatalogoByType(Constants.Catalogo.TipologiaEscalamiento);
        return new { tipologia };
    }

    public async Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente)
            ?? throw new InvalidOperationException("Debe guardar la información antes de avanzar.");

        var formulario = _mapper.Map<realizar_devolucion_ep>(entity);
        ValidarCamposObligatorios(formulario);

        var actividadesCreadas = new List<AssignActivityDTO>();
        List<xpdl_transition_DTO> transitions = await _workflowApplication.GetTransitions(ActividadDevolucionEP);
        FolioDTO folio = await _workflowApplication.CapturarDatosFolio(idExpediente, ActividadDevolucionEP);

        string transicionSeleccionada;
        string destinoActividad;

        if (formulario.requiere_escalamiento_comercial == "SI")
        {
            transicionSeleccionada = TransicionGestionComercial;
            destinoActividad = "Realizar Gestión Comercial";
        }
        else
        {
            // Acción a seguir determina destino
            transicionSeleccionada = formulario.accion_a_seguir switch
            {
                "FIRMAR_ESCRITURA" => TransicionFirmarEscritura,
                "FIRMAR_REP_LEGAL" => TransicionFirmarRepLegal,
                "EP_REGISTRADAS" => TransicionEPRegistradas,
                _ => throw new InvalidOperationException("Acción a seguir no válida.")
            };
            destinoActividad = formulario.accion_a_seguir switch
            {
                "FIRMAR_ESCRITURA" => "Firmar Escritura Cliente",
                "FIRMAR_REP_LEGAL" => "Firmar Rep. Legal",
                "EP_REGISTRADAS" => "Realizar EP Registradas",
                _ => "Desconocido"
            };
        }

        var transitionId = transitions.FirstOrDefault(x => x.name == transicionSeleccionada)?.transition_id
            ?? throw new InvalidOperationException($"No se encontró la transición '{transicionSeleccionada}' en el workflow.");

        var resultado = await _workflowApplication.AvanzarActividad(transitionId, folio, userId);
        actividadesCreadas.AddRange(resultado);

        _bitacoraApplication.Create(new bitacora
        {
            id_expediente = idExpediente,
            id_actividad = ActividadDevolucionEP,
            id_usuario = userId,
            fecha_alta = DateTime.Now,
            observaciones = $"Avance Devolución EP. Escalamiento: {formulario.requiere_escalamiento_comercial}. Destino: [{destinoActividad}]. {(formulario.observaciones != null ? $"Obs: {formulario.observaciones}" : "")}",
            is_active = true,
            row_status = true
        }, userId);

        return actividadesCreadas;
    }

    private static void ValidarCamposObligatorios(realizar_devolucion_ep formulario)
    {
        var camposFaltantes = new List<string>();

        if (string.IsNullOrWhiteSpace(formulario.requiere_escalamiento_comercial))
            camposFaltantes.Add("¿Requiere Escalamiento Comercial?");

        if (formulario.requiere_escalamiento_comercial == "SI")
        {
            if (string.IsNullOrWhiteSpace(formulario.tipologia))
                camposFaltantes.Add("Tipología");
        }

        if (formulario.requiere_escalamiento_comercial == "NO")
        {
            if (string.IsNullOrWhiteSpace(formulario.accion_a_seguir))
                camposFaltantes.Add("Acción a Seguir");
        }

        if (camposFaltantes.Count > 0)
            throw new InvalidOperationException($"Campos obligatorios faltantes: {string.Join(", ", camposFaltantes)}");
    }
}
