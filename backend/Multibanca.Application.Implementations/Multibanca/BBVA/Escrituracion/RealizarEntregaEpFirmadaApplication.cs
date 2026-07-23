using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca.BBVA.Escrituracion;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Common;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

namespace Multibanca.Application.Implementations.Multibanca.BBVA.Escrituracion;

public class RealizarEntregaEpFirmadaApplication
    : MultibancaGenericApplication<realizar_entrega_ep_firmada, realizar_entrega_ep_firmada_entity, IRealizarEntregaEpFirmadaRepository>,
      IRealizarEntregaEpFirmadaApplication
{
    // Constantes de transición
    private const string TransicionRecepcionBoleta = Constants.TransicionesBBVA.EntregaEpFirmadaRecepcionBoleta;
    private const string TransicionExcepcionDesembolso = Constants.TransicionesBBVA.EntregaEpFirmadaExcepcionDesembolso;

    // ID de la actividad actual
    private static readonly string ActividadEntregaEpFirmada = Constants.ActividadesBBVA.EscrituracionRealizarEntregaEpFirmada;

    // Tipos de crédito que aplican para excepción de desembolso (CA02)
    // TODO: Mover a catálogo en BD cuando se implemente la paramétrica L5 de constructoras
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
    private readonly IFirmarRepLegalRepository _firmarRepLegalRepository;
    private readonly IFirmarEscrituraClienteRepository _firmarEscrituraClienteRepository;

    public RealizarEntregaEpFirmadaApplication(
        MultibancaDBContext multibancaDBContext,
        IRealizarEntregaEpFirmadaRepository repository,
        IMapper mapper,
        ICommonApplication commonApplication,
        IWorkflowApplication workflowApplication,
        IBitacoraApplication bitacoraApplication,
        IValidarInformacionRepository validarInformacionRepository,
        IFirmarRepLegalRepository firmarRepLegalRepository,
        IFirmarEscrituraClienteRepository firmarEscrituraClienteRepository)
        : base(multibancaDBContext, repository, mapper)
    {
        _mapper = mapper;
        _commonApplication = commonApplication;
        _workflowApplication = workflowApplication;
        _bitacoraApplication = bitacoraApplication;
        _validarInformacionRepository = validarInformacionRepository;
        _firmarRepLegalRepository = firmarRepLegalRepository;
        _firmarEscrituraClienteRepository = firmarEscrituraClienteRepository;
    }

    public async Task<object?> GetByExpediente(long idExpediente)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente);

        var formulario = entity != null
            ? _mapper.Map<realizar_entrega_ep_firmada>(entity)
            : new realizar_entrega_ep_firmada { id_expediente = idExpediente };

        // Calcular aplica_excepcion desde tipo_credito (validar_informacion_bbva)
        var validarInfo = await _validarInformacionRepository.GetByExpediente(idExpediente);
        string? tipoCredito = validarInfo?.tipo_credito;
        formulario.aplica_excepcion = CalcularAplicaExcepcion(tipoCredito);

        // Datos heredados de actividades previas
        var firmarRepLegal = await _firmarRepLegalRepository.GetByExpediente(idExpediente);
        var firmarEscritura = await _firmarEscrituraClienteRepository.GetByExpediente(idExpediente);

        // Obtener descripción del concepto de firma desde catálogo
        string? conceptoFirmaDescripcion = null;
        if (!string.IsNullOrWhiteSpace(firmarRepLegal?.concepto_firma))
        {
            var catalogoConcepto = await _commonApplication.GetCatalogoByType(Constants.Catalogo.ConceptoFirmaRepLegal_L41);
            conceptoFirmaDescripcion = catalogoConcepto
                .FirstOrDefault(c => c.code == firmarRepLegal.concepto_firma)?.description;
        }

        return new
        {
            formulario,
            datos_heredados = new
            {
                // De Firmar Rep. Legal (BBV-91)
                concepto_firma = firmarRepLegal?.concepto_firma,
                concepto_firma_descripcion = conceptoFirmaDescripcion,
                // De Firmar Escritura Cliente (BBV-86)
                notaria = firmarEscritura?.notaria,
                numero_notaria = firmarEscritura?.numero_notaria,
                ciudad_notaria = firmarEscritura?.ciudad_notaria,
                fecha_notaria = firmarEscritura?.fecha_notaria,
                numero_escritura = firmarEscritura?.numero_escritura,
                fecha_escritura = firmarEscritura?.fecha_escritura,
                representante_legal = firmarEscritura?.representante_legal,
            }
        };
    }

    public async Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente)
            ?? throw new InvalidOperationException("Debe guardar la información antes de avanzar.");

        var formulario = _mapper.Map<realizar_entrega_ep_firmada>(entity);

        // Calcular aplica_excepcion
        var validarInfo = await _validarInformacionRepository.GetByExpediente(idExpediente);
        formulario.aplica_excepcion = CalcularAplicaExcepcion(validarInfo?.tipo_credito);

        // Validar campos obligatorios
        ValidarCamposObligatorios(formulario);

        var actividadesCreadas = new List<AssignActivityDTO>();
        List<xpdl_transition_DTO> transitions = await _workflowApplication.GetTransitions(ActividadEntregaEpFirmada);
        FolioDTO folio = await _workflowApplication.CapturarDatosFolio(idExpediente, ActividadEntregaEpFirmada);

        // CA06 — Siempre avanza a "Realizar Recepción Boleta"
        var transitionIdBoleta = transitions.FirstOrDefault(x => x.name == TransicionRecepcionBoleta)?.transition_id
            ?? throw new InvalidOperationException($"No se encontró la transición '{TransicionRecepcionBoleta}' en el workflow.");

        var resultadoBoleta = await _workflowApplication.AvanzarActividad(transitionIdBoleta, folio, userId);
        actividadesCreadas.AddRange(resultadoBoleta);

        // CA02/CA05 — Si aplica excepción, también crear actividad paralela
        if (formulario.aplica_excepcion == "SI")
        {
            var transitionIdExcepcion = transitions.FirstOrDefault(x => x.name == TransicionExcepcionDesembolso)?.transition_id;

            if (transitionIdExcepcion != null)
            {
                try
                {
                    var resultadoExcepcion = await _workflowApplication.AvanzarActividad(transitionIdExcepcion, folio, userId);
                    actividadesCreadas.AddRange(resultadoExcepcion);
                }
                catch (Exception ex)
                {
                    // No bloquear el flujo principal si falla la excepción paralela
                    Console.WriteLine($"[WARN] No se pudo crear actividad paralela de excepción: {ex.Message}");
                }
            }
        }

        // Registrar bitácora
        RegistrarBitacora(idExpediente, userId, formulario, actividadesCreadas);

        return actividadesCreadas;
    }

    // ======================== Métodos privados ========================

    private static string CalcularAplicaExcepcion(string? tipoCredito)
    {
        if (string.IsNullOrWhiteSpace(tipoCredito))
            return "NO";

        return TiposExcepcionDesembolso.Contains(tipoCredito, StringComparer.OrdinalIgnoreCase)
            ? "SI"
            : "NO";
    }

    private static void ValidarCamposObligatorios(realizar_entrega_ep_firmada formulario)
    {
        var camposFaltantes = new List<string>();

        if (string.IsNullOrWhiteSpace(formulario.entregado_a))
            camposFaltantes.Add("Entregado a");

        if (camposFaltantes.Count > 0)
        {
            throw new InvalidOperationException(
                $"Campos obligatorios faltantes: {string.Join(", ", camposFaltantes)}");
        }
    }

    private void RegistrarBitacora(
        long idExpediente,
        int userId,
        realizar_entrega_ep_firmada formulario,
        List<AssignActivityDTO> actividadesCreadas)
    {
        var observacionesBitacora = $"Avance de Realizar Entrega EP Firmada. " +
            $"Entregado a: {formulario.entregado_a}. " +
            $"¿Aplica Excepción?: {formulario.aplica_excepcion}. " +
            $"Destino principal: [Realizar Recepción Boleta].";

        if (formulario.aplica_excepcion == "SI")
            observacionesBitacora += " Destino paralelo: [Realizar Excepción Desembolso].";

        if (!string.IsNullOrWhiteSpace(formulario.observaciones))
            observacionesBitacora += $" Observaciones: {formulario.observaciones}";

        _bitacoraApplication.Create(new bitacora
        {
            id_expediente = idExpediente,
            id_actividad = ActividadEntregaEpFirmada,
            id_usuario = userId,
            fecha_alta = DateTime.Now,
            observaciones = observacionesBitacora,
            is_active = true,
            row_status = true
        }, userId);
    }
}
