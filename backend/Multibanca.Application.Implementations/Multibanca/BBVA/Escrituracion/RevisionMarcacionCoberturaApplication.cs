using System.Text.RegularExpressions;
using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA.Escrituracion;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.BBVA.Escrituracion;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Common;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

namespace Multibanca.Application.Implementations.Multibanca.BBVA.Escrituracion;

public class RevisionMarcacionCoberturaApplication
    : MultibancaGenericApplication<revision_marcacion_cobertura_bbva, revision_marcacion_cobertura_entity,
        IRevisionMarcacionCoberturaRepository>,
      IRevisionMarcacionCoberturaApplication
{
    // Constante de transición (workflow XPDL)
    private const string TransicionValidarCondiciones = Constants.TransicionesBBVA.MarcacionCoberturaValidarCondiciones;

    // ID de la actividad actual en el workflow
    private static readonly string ActividadRevisarMarcacionCobertura = Constants.ActividadesBBVA.EscrituracionRevisarMarcacionCobertura;

    private static readonly Regex EmailRegex = new(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", RegexOptions.Compiled);

    private readonly IMapper _mapper;
    private readonly IWorkflowApplication _workflowApplication;
    private readonly IBitacoraApplication _bitacoraApplication;
    private readonly IEncabezadoApplication _encabezadoApplication;
    private readonly IEmailNotificationApplication _emailNotificationApplication;

    public RevisionMarcacionCoberturaApplication(
        MultibancaDBContext multibancaDBContext,
        IRevisionMarcacionCoberturaRepository repository,
        IMapper mapper,
        IWorkflowApplication workflowApplication,
        IBitacoraApplication bitacoraApplication,
        IEncabezadoApplication encabezadoApplication,
        IEmailNotificationApplication emailNotificationApplication)
        : base(multibancaDBContext, repository, mapper)
    {
        _mapper                        = mapper;
        _workflowApplication           = workflowApplication;
        _bitacoraApplication           = bitacoraApplication;
        _encabezadoApplication         = encabezadoApplication;
        _emailNotificationApplication  = emailNotificationApplication;
    }

    public async Task<RevisionMarcacionCoberturaResponse> GetByExpediente(long idExpediente)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente);

        var formulario = entity != null
            ? _mapper.Map<revision_marcacion_cobertura_bbva>(entity)
            : new revision_marcacion_cobertura_bbva { id_expediente = idExpediente };

        // No existe actividad predecesora construida (BBV-104 "Validar Cumplimiento de
        // Políticas" aún no está desarrollada — ver sección 0.1 de la receta): la herencia
        // de CA02 se limita a los datos generales del encabezado del expediente.
        RevisionMarcacionCoberturaHerencia? herencia = null;
        try
        {
            var encabezado = await _encabezadoApplication
                .InformacionEncabezado(idExpediente, ActividadRevisarMarcacionCobertura);

            herencia = new RevisionMarcacionCoberturaHerencia
            {
                nombre_cliente        = encabezado?.nombre_completo_t1,
                numero_identificacion = encabezado?.numero_identificacion_t1,
                tipo_identificacion   = encabezado?.tipo_documento_id_t1
            };
        }
        catch
        {
            // Falla al obtener el encabezado: se muestran los campos como "-" en el frontend,
            // sin bloquear el resto de la pantalla (mismo patrón que VoboGerenciaCohApplication).
        }

        return new RevisionMarcacionCoberturaResponse
        {
            formulario = formulario,
            herencia   = herencia
        };
    }

    public async Task<revision_marcacion_cobertura_bbva> Guardar(revision_marcacion_cobertura_bbva request, int userId)
    {
        var existente = await RepositoryProvider.GetByExpediente(request.id_expediente);

        request.id_actividad = ActividadRevisarMarcacionCobertura;

        if (existente != null)
        {
            // Actualizar registro existente preservando auditoría de creación
            request.id           = existente.id;
            request.created_by   = existente.created_by;
            request.created_date = existente.created_date;
            request.is_active    = existente.is_active;
            request.row_status   = existente.row_status;
            return Update(request, userId);
        }

        request.is_active  = true;
        request.row_status = true;
        return Create(request, userId);
    }

    public async Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente)
            ?? throw new InvalidOperationException("Debe guardar la información antes de avanzar.");

        var formulario = _mapper.Map<revision_marcacion_cobertura_bbva>(entity);
        ValidarCamposObligatorios(formulario);
        ValidarEmail(formulario);

        // CA05 — Gatillador: envío de notificación al Área de Colocaciones antes de avanzar.
        await _emailNotificationApplication
            .EnviarNotificacionCobertura(idExpediente, formulario.email_area_colocaciones!, userId);

        FolioDTO folio = await _workflowApplication
            .CapturarDatosFolio(idExpediente, ActividadRevisarMarcacionCobertura);
        List<xpdl_transition_DTO> transitions = await _workflowApplication
            .GetTransitions(ActividadRevisarMarcacionCobertura);

        // Enrutamiento único: siempre avanza hacia Validar Condiciones Desembolso (CA06)
        string transitionId = transitions.FirstOrDefault(x => x.name == TransicionValidarCondiciones)?.transition_id
            ?? throw new InvalidOperationException($"No se encontró la transición '{TransicionValidarCondiciones}' en el workflow.");

        var resultado = await _workflowApplication
            .AvanzarActividad(transitionId, folio, userId);

        // Registro atómico en bitácora (fallo aquí revierte la transición)
        RegistrarBitacora(idExpediente, userId, formulario, "Validar Condiciones Desembolso");

        return resultado;
    }

    // ── Helpers privados ──────────────────────────────────────────────────────

    private static void ValidarCamposObligatorios(revision_marcacion_cobertura_bbva f)
    {
        var faltantes = new List<string>();

        if (string.IsNullOrWhiteSpace(f.tipo_documento)) faltantes.Add("Tipo de Documento");
        if (string.IsNullOrWhiteSpace(f.numero_documento)) faltantes.Add("C.C (Número)");
        if (string.IsNullOrWhiteSpace(f.tipo_tramite)) faltantes.Add("TT (Tipo Trámite)");
        if (string.IsNullOrWhiteSpace(f.nombre)) faltantes.Add("Nombre");
        if (string.IsNullOrWhiteSpace(f.constructora)) faltantes.Add("Constructora");
        if (string.IsNullOrWhiteSpace(f.proyecto)) faltantes.Add("Proyecto");
        if (!f.fecha_aceptacion_plataforma.HasValue) faltantes.Add("Fecha de Aceptación Plataforma");
        if (string.IsNullOrWhiteSpace(f.tipo_vivienda)) faltantes.Add("Tipo de Vivienda");
        if (!f.valor_subsidio.HasValue) faltantes.Add("Valor Subsidio");
        if (string.IsNullOrWhiteSpace(f.numero_obligacion)) faltantes.Add("N° Obligación");
        if (!f.fecha_desembolso.HasValue) faltantes.Add("Fecha de Desembolso");
        if (!f.fecha_proximo_canon.HasValue) faltantes.Add("Fecha Próximo Canon");
        if (!f.valor_desembolso.HasValue) faltantes.Add("Valor Desembolso");
        if (!f.fecha_solicitud_marcacion.HasValue) faltantes.Add("Fecha Solicitud Marcación");
        if (string.IsNullOrWhiteSpace(f.hora_solicitud_marcacion)) faltantes.Add("Hora Solicitud Marcación");
        if (string.IsNullOrWhiteSpace(f.responsable_m5)) faltantes.Add("Responsable M5");
        if (string.IsNullOrWhiteSpace(f.no_resolucion)) faltantes.Add("No Resolución");
        if (!f.fecha_resolucion.HasValue) faltantes.Add("Fecha de la Resolución");
        if (!f.fecha_envio_resolucion.HasValue) faltantes.Add("Fecha Envío Resolución");
        if (string.IsNullOrWhiteSpace(f.estado_proceso)) faltantes.Add("Estado Proceso");
        if (string.IsNullOrWhiteSpace(f.observaciones)) faltantes.Add("Observaciones");

        // Condicionado (CA04): fecha y hora de respuesta van juntas, o ninguna
        var tieneFechaRespuesta = f.fecha_respuesta_marcacion.HasValue;
        var tieneHoraRespuesta  = !string.IsNullOrWhiteSpace(f.hora_respuesta_marcacion);
        if (tieneFechaRespuesta != tieneHoraRespuesta)
            faltantes.Add("Fecha y Hora de Respuesta Marcación (ambas o ninguna)");

        if (faltantes.Count > 0)
            throw new InvalidOperationException(
                $"Campos obligatorios faltantes: {string.Join(", ", faltantes)}");
    }

    private static void ValidarEmail(revision_marcacion_cobertura_bbva f)
    {
        var email = f.email_area_colocaciones?.Trim();
        if (string.IsNullOrWhiteSpace(email) || !EmailRegex.IsMatch(email))
            throw new InvalidOperationException(
                "El correo del Área de Colocaciones es obligatorio y debe tener un formato de email válido.");
    }

    private void RegistrarBitacora(long idExpediente, int userId, revision_marcacion_cobertura_bbva f, string destinoActividad)
    {
        var obs = $"Revisión Marcación de Cobertura. Estado Proceso: {f.estado_proceso}. " +
                  $"Notificación enviada a: {f.email_area_colocaciones}. Destino: [{destinoActividad}].";
        if (!string.IsNullOrWhiteSpace(f.observaciones))
            obs += $" Observaciones: {f.observaciones}";

        _bitacoraApplication.Create(new bitacora
        {
            id_expediente = idExpediente,
            id_actividad  = ActividadRevisarMarcacionCobertura,
            id_usuario    = userId,
            fecha_alta    = DateTime.Now,
            observaciones = obs,
            is_active     = true,
            row_status    = true
        }, userId);
    }
}
