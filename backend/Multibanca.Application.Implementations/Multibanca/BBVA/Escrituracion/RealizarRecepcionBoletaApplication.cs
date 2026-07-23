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

public class RealizarRecepcionBoletaApplication
    : MultibancaGenericApplication<realizar_recepcion_boleta, realizar_recepcion_boleta_entity, IRealizarRecepcionBoletaRepository>,
      IRealizarRecepcionBoletaApplication
{
    // Constantes de transición
    private const string TransicionEPRegistradas = Constants.TransicionesBBVA.RecepcionBoletaEPRegistradas;
    private const string TransicionExcepcionDesembolso = Constants.TransicionesBBVA.RecepcionBoletaExcepcionDesembolso;

    // ID de la actividad actual
    private static readonly string ActividadRecepcionBoleta = Constants.ActividadesBBVA.EscrituracionRealizarRecepcionBoleta;

    // Tipos de crédito que aplican excepción cuando tipo_desembolso = BOLETA (CA06)
    private static readonly string[] TiposExcepcionBoleta = new[]
    {
        "LEASING_USADO",
        "HIPOTECARIO_USADO",
        "REMODELACION_AMPLIAR_HIPOTECAR"
    };

    // Dependencias
    private readonly IMapper _mapper;
    private readonly ICommonApplication _commonApplication;
    private readonly IWorkflowApplication _workflowApplication;
    private readonly IBitacoraApplication _bitacoraApplication;
    private readonly IValidarInformacionRepository _validarInformacionRepository;
    private readonly IFirmarEscrituraClienteRepository _firmarEscrituraClienteRepository;

    public RealizarRecepcionBoletaApplication(
        MultibancaDBContext multibancaDBContext,
        IRealizarRecepcionBoletaRepository repository,
        IMapper mapper,
        ICommonApplication commonApplication,
        IWorkflowApplication workflowApplication,
        IBitacoraApplication bitacoraApplication,
        IValidarInformacionRepository validarInformacionRepository,
        IFirmarEscrituraClienteRepository firmarEscrituraClienteRepository)
        : base(multibancaDBContext, repository, mapper)
    {
        _mapper = mapper;
        _commonApplication = commonApplication;
        _workflowApplication = workflowApplication;
        _bitacoraApplication = bitacoraApplication;
        _validarInformacionRepository = validarInformacionRepository;
        _firmarEscrituraClienteRepository = firmarEscrituraClienteRepository;
    }

    public async Task<object?> GetByExpediente(long idExpediente)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente);

        var formulario = entity != null
            ? _mapper.Map<realizar_recepcion_boleta>(entity)
            : new realizar_recepcion_boleta { id_expediente = idExpediente };

        // Calcular aplica_excepcion
        var validarInfo = await _validarInformacionRepository.GetByExpediente(idExpediente);
        string? tipoCredito = validarInfo?.tipo_credito;
        formulario.aplica_excepcion = CalcularAplicaExcepcion(tipoCredito);

        // Datos heredados
        var firmarEscritura = await _firmarEscrituraClienteRepository.GetByExpediente(idExpediente);

        // Resolver descripción del tipo de documento
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
                // Datos Cliente (de validar_informacion_bbva)
                tipo_documento = tipoDocumentoDescripcion,
                numero_documento = validarInfo?.numero_id_t1,
                nombre_completo = validarInfo?.nombre_completo_t1,
                tipo_credito = validarInfo?.tipo_credito,
                // Datos Notaría (de firmar_escritura_cliente)
                ciudad_notaria = firmarEscritura?.ciudad_notaria,
                numero_notaria = firmarEscritura?.numero_notaria,
                numero_escritura = firmarEscritura?.numero_escritura,
            }
        };
    }

    public async Task<object> GetControles()
    {
        var tipoBoleta = await _commonApplication.GetCatalogoByType(Constants.Catalogo.TipoBoleta);
        var oficinaRegistro = await _commonApplication.GetCatalogoByType(Constants.Catalogo.OficinaRegistro);

        return new
        {
            tipo_boleta = tipoBoleta,
            oficina_registro = oficinaRegistro
        };
    }

    public async Task<realizar_recepcion_boleta> EjecutarVUR(long idExpediente, int userId)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente);

        var formulario = entity != null
            ? _mapper.Map<realizar_recepcion_boleta>(entity)
            : new realizar_recepcion_boleta
            {
                id_expediente = idExpediente,
                id_actividad = ActividadRecepcionBoleta
            };

        const int MAX_INTENTOS = 3;
        bool exitoso = false;

        for (int intento = 1; intento <= MAX_INTENTOS && !exitoso; intento++)
        {
            formulario.vur_intentos = intento;

            try
            {
                // TODO: Integrar con el servicio real del Bot VUR
                // var resultado = await _vurService.ConsultarRegistro(idExpediente);
                // formulario.numero_boleta = resultado.NumeroRadicado;
                // formulario.fecha_boleta = resultado.FechaRadicacion;
                // formulario.numero_matricula = resultado.NumeroMatricula;
                // formulario.oficina_registro = resultado.OficinaRegistro;
                // exitoso = true;

                // PLACEHOLDER: Simular fallo hasta integración real
                exitoso = false;
            }
            catch
            {
                exitoso = false;
            }
        }

        formulario.vur_ejecutado = true;
        formulario.vur_exitoso = exitoso;

        // Guardar resultado
        if (entity != null)
        {
            var updated = _mapper.Map<realizar_recepcion_boleta>(entity);
            updated.vur_ejecutado = formulario.vur_ejecutado;
            updated.vur_exitoso = formulario.vur_exitoso;
            updated.vur_intentos = formulario.vur_intentos;
            updated.numero_boleta = formulario.numero_boleta;
            updated.fecha_boleta = formulario.fecha_boleta;
            updated.numero_matricula = formulario.numero_matricula;
            updated.oficina_registro = formulario.oficina_registro;
            Update(updated, userId);
            formulario.id = updated.id;
        }
        else
        {
            formulario.is_active = true;
            formulario.row_status = true;
            var saved = Create(formulario, userId);
            formulario.id = saved.id;
        }

        return formulario;
    }

    public async Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente)
            ?? throw new InvalidOperationException("Debe guardar la información antes de avanzar.");

        var formulario = _mapper.Map<realizar_recepcion_boleta>(entity);

        // Calcular aplica_excepcion
        var validarInfo = await _validarInformacionRepository.GetByExpediente(idExpediente);
        formulario.aplica_excepcion = CalcularAplicaExcepcion(validarInfo?.tipo_credito);

        // Validar campos obligatorios
        ValidarCamposObligatorios(formulario);

        var actividadesCreadas = new List<AssignActivityDTO>();
        List<xpdl_transition_DTO> transitions = await _workflowApplication.GetTransitions(ActividadRecepcionBoleta);
        FolioDTO folio = await _workflowApplication.CapturarDatosFolio(idExpediente, ActividadRecepcionBoleta);

        // Siempre avanza a "Realizar EP Registradas"
        var transitionIdEP = transitions.FirstOrDefault(x => x.name == TransicionEPRegistradas)?.transition_id
            ?? throw new InvalidOperationException($"No se encontró la transición '{TransicionEPRegistradas}' en el workflow.");

        var resultadoEP = await _workflowApplication.AvanzarActividad(transitionIdEP, folio, userId);
        actividadesCreadas.AddRange(resultadoEP);

        // Si aplica excepción, también crear actividad paralela
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

        // TODO: Agregar validación de tipo_desembolso = BOLETA cuando exista la paramétrica
        return TiposExcepcionBoleta.Contains(tipoCredito, StringComparer.OrdinalIgnoreCase)
            ? "SI"
            : "NO";
    }

    private static void ValidarCamposObligatorios(realizar_recepcion_boleta formulario)
    {
        var camposFaltantes = new List<string>();

        if (string.IsNullOrWhiteSpace(formulario.numero_boleta))
            camposFaltantes.Add("Número de Boleta");

        if (!formulario.fecha_boleta.HasValue)
            camposFaltantes.Add("Fecha de Boleta");
        else if (formulario.fecha_boleta.Value > DateTime.Today)
            camposFaltantes.Add("Fecha de Boleta (no puede ser futura)");

        if (string.IsNullOrWhiteSpace(formulario.numero_matricula))
            camposFaltantes.Add("Número de Matrícula");

        if (string.IsNullOrWhiteSpace(formulario.tipo_boleta))
            camposFaltantes.Add("Tipo de Boleta");

        if (string.IsNullOrWhiteSpace(formulario.codigo_zona))
            camposFaltantes.Add("Código Zona");

        if (string.IsNullOrWhiteSpace(formulario.oficina_registro))
            camposFaltantes.Add("Oficina de Registro");

        if (!formulario.boleta_recibida)
            camposFaltantes.Add("Boleta Recibida (debe estar marcada)");

        // CA11 — Si tipo boleta es Física Original o Física Copia
        if (formulario.tipo_boleta == "TBOL-2" || formulario.tipo_boleta == "TBOL-3")
        {
            if (string.IsNullOrWhiteSpace(formulario.boleta_en_poder_de))
                camposFaltantes.Add("Boleta En Poder De");
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
        realizar_recepcion_boleta formulario,
        List<AssignActivityDTO> actividadesCreadas)
    {
        var observacionesBitacora = $"Avance de Realizar Recepción Boleta. " +
            $"Número Boleta: {formulario.numero_boleta}. " +
            $"Fecha Boleta: {formulario.fecha_boleta:yyyy-MM-dd}. " +
            $"Matrícula: {formulario.numero_matricula}. " +
            $"VUR Exitoso: {(formulario.vur_exitoso ? "Sí" : "No")}. " +
            $"¿Aplica Excepción?: {formulario.aplica_excepcion}. " +
            $"Destino principal: [Realizar EP Registradas].";

        if (formulario.aplica_excepcion == "SI")
            observacionesBitacora += " Destino paralelo: [Realizar Excepción Desembolso].";

        if (!string.IsNullOrWhiteSpace(formulario.observaciones))
            observacionesBitacora += $" Observaciones: {formulario.observaciones}";

        _bitacoraApplication.Create(new bitacora
        {
            id_expediente = idExpediente,
            id_actividad = ActividadRecepcionBoleta,
            id_usuario = userId,
            fecha_alta = DateTime.Now,
            observaciones = observacionesBitacora,
            is_active = true,
            row_status = true
        }, userId);
    }
}
