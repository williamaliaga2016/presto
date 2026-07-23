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
using Multibanca.DTO.Common;

namespace Multibanca.Application.Implementations.Multibanca.BBVA.Escrituracion;

public class RealizarEPRegistradasApplication
    : MultibancaGenericApplication<realizar_ep_registradas, realizar_ep_registradas_entity, IRealizarEPRegistradasRepository>,
      IRealizarEPRegistradasApplication
{
    private const string TransicionVBFinalAbogado = Constants.TransicionesBBVA.EPRegistradasVBFinalAbogado;
    private static readonly string ActividadEPRegistradas = Constants.ActividadesBBVA.EscrituracionRealizarEPRegistradas;

    private readonly IMapper _mapper;
    private readonly ICommonApplication _commonApplication;
    private readonly IWorkflowApplication _workflowApplication;
    private readonly IBitacoraApplication _bitacoraApplication;
    private readonly IValidarInformacionRepository _validarInformacionRepository;
    private readonly IFirmarEscrituraClienteRepository _firmarEscrituraClienteRepository;
    private readonly IRealizarRecepcionBoletaRepository _recepcionBoletaRepository;

    public RealizarEPRegistradasApplication(
        MultibancaDBContext multibancaDBContext,
        IRealizarEPRegistradasRepository repository,
        IMapper mapper,
        ICommonApplication commonApplication,
        IWorkflowApplication workflowApplication,
        IBitacoraApplication bitacoraApplication,
        IValidarInformacionRepository validarInformacionRepository,
        IFirmarEscrituraClienteRepository firmarEscrituraClienteRepository,
        IRealizarRecepcionBoletaRepository recepcionBoletaRepository)
        : base(multibancaDBContext, repository, mapper)
    {
        _mapper = mapper;
        _commonApplication = commonApplication;
        _workflowApplication = workflowApplication;
        _bitacoraApplication = bitacoraApplication;
        _validarInformacionRepository = validarInformacionRepository;
        _firmarEscrituraClienteRepository = firmarEscrituraClienteRepository;
        _recepcionBoletaRepository = recepcionBoletaRepository;
    }

    public async Task<object?> GetByExpediente(long idExpediente)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente);

        var formulario = entity != null
            ? _mapper.Map<realizar_ep_registradas>(entity)
            : new realizar_ep_registradas { id_expediente = idExpediente };

        // Datos heredados
        var validarInfo = await _validarInformacionRepository.GetByExpediente(idExpediente);
        var firmarEscritura = await _firmarEscrituraClienteRepository.GetByExpediente(idExpediente);
        var recepcionBoleta = await _recepcionBoletaRepository.GetByExpediente(idExpediente);

        // Resolver tipo documento
        string? tipoDocumentoDescripcion = null;
        if (!string.IsNullOrWhiteSpace(validarInfo?.tipo_id_t1))
        {
            var catalogoTipoDoc = await _commonApplication.GetCatalogoByType(Constants.Catalogo.TipoDocumentoId);
            tipoDocumentoDescripcion = catalogoTipoDoc
                .FirstOrDefault(c => c.code == validarInfo.tipo_id_t1 || c.id.ToString() == validarInfo.tipo_id_t1)?.description
                ?? validarInfo.tipo_id_t1;
        }

        return new
        {
            formulario,
            datos_heredados = new
            {
                // Datos Cliente
                tipo_documento = tipoDocumentoDescripcion,
                numero_documento = validarInfo?.numero_id_t1,
                nombre_completo = validarInfo?.nombre_completo_t1,
                tipo_credito = validarInfo?.tipo_credito,
                // Datos Notaría
                ciudad_notaria = firmarEscritura?.ciudad_notaria,
                numero_notaria = firmarEscritura?.numero_notaria,
                numero_escritura = firmarEscritura?.numero_escritura,
                // Datos Recepción Boleta
                numero_boleta = recepcionBoleta?.numero_boleta,
                fecha_boleta = recepcionBoleta?.fecha_boleta,
                tipo_boleta = recepcionBoleta?.tipo_boleta,
                oficina_registro = recepcionBoleta?.oficina_registro,
                numero_matricula = recepcionBoleta?.numero_matricula,
            }
        };
    }

    public async Task<object> GetControles()
    {
        return new { };
    }

    public async Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente)
            ?? throw new InvalidOperationException("Debe guardar la información antes de avanzar.");

        var formulario = _mapper.Map<realizar_ep_registradas>(entity);

        ValidarCamposObligatorios(formulario);

        var actividadesCreadas = new List<AssignActivityDTO>();
        List<xpdl_transition_DTO> transitions = await _workflowApplication.GetTransitions(ActividadEPRegistradas);
        FolioDTO folio = await _workflowApplication.CapturarDatosFolio(idExpediente, ActividadEPRegistradas);

        // Flujo lineal — único destino: VB Final Abogado
        var transitionId = transitions.FirstOrDefault(x => x.name == TransicionVBFinalAbogado)?.transition_id
            ?? throw new InvalidOperationException($"No se encontró la transición '{TransicionVBFinalAbogado}' en el workflow.");

        var resultado = await _workflowApplication.AvanzarActividad(transitionId, folio, userId);
        actividadesCreadas.AddRange(resultado);

        RegistrarBitacora(idExpediente, userId, formulario);

        return actividadesCreadas;
    }

    // ======================== Métodos privados ========================

    private static void ValidarCamposObligatorios(realizar_ep_registradas formulario)
    {
        var camposFaltantes = new List<string>();

        if (!formulario.finalizacion.HasValue)
            camposFaltantes.Add("Finalización");

        if (string.IsNullOrWhiteSpace(formulario.causal))
            camposFaltantes.Add("Causal");

        if (!formulario.fecha_finalizacion.HasValue)
            camposFaltantes.Add("Fecha Finalización");

        if (!formulario.confirmacion_ep_registrada)
            camposFaltantes.Add("Confirmación de EP Registrada (debe estar marcada)");

        if (camposFaltantes.Count > 0)
        {
            throw new InvalidOperationException(
                $"Campos obligatorios faltantes: {string.Join(", ", camposFaltantes)}");
        }
    }

    private void RegistrarBitacora(long idExpediente, int userId, realizar_ep_registradas formulario)
    {
        var observacionesBitacora = $"Avance de Realizar EP Registradas. " +
            $"Finalización: {formulario.finalizacion:yyyy-MM-dd}. " +
            $"Causal: {formulario.causal}. " +
            $"Fecha Finalización: {formulario.fecha_finalizacion:yyyy-MM-dd}. " +
            $"Confirmación EP: Sí. " +
            $"Destino: [Realizar VB Final Abogado].";

        if (!string.IsNullOrWhiteSpace(formulario.observaciones))
            observacionesBitacora += $" Observaciones: {formulario.observaciones}";

        _bitacoraApplication.Create(new bitacora
        {
            id_expediente = idExpediente,
            id_actividad = ActividadEPRegistradas,
            id_usuario = userId,
            fecha_alta = DateTime.Now,
            observaciones = observacionesBitacora,
            is_active = true,
            row_status = true
        }, userId);
    }
}
