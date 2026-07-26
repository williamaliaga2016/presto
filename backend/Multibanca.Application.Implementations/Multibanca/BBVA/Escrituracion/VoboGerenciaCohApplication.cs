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

public class VoboGerenciaCohApplication
    : MultibancaGenericApplication<vobo_gerencia_coh_bbva, vobo_gerencia_coh_entity,
        IVoboGerenciaCohRepository>,
      IVoboGerenciaCohApplication
{
    // Constante de transición (workflow XPDL)
    private const string TransicionExcepcionDesembolso = Constants.TransicionesBBVA.VoboGerenciaCohExcepcionDesembolso;

    // ID de la actividad actual en el workflow
    private static readonly string ActividadVoboGerenciaCoh = Constants.ActividadesBBVA.EscrituracionVoBoGerenciaApplication;

    private readonly IMapper _mapper;
    private readonly IWorkflowApplication _workflowApplication;
    private readonly IBitacoraApplication _bitacoraApplication;
    private readonly IEncabezadoApplication _encabezadoApplication;

    public VoboGerenciaCohApplication(
        MultibancaDBContext multibancaDBContext,
        IVoboGerenciaCohRepository repository,
        IMapper mapper,
        IWorkflowApplication workflowApplication,
        IBitacoraApplication bitacoraApplication,
        IEncabezadoApplication encabezadoApplication)
        : base(multibancaDBContext, repository, mapper)
    {
        _mapper                = mapper;
        _workflowApplication   = workflowApplication;
        _bitacoraApplication   = bitacoraApplication;
        _encabezadoApplication = encabezadoApplication;
    }

    public async Task<VoboGerenciaCohResponse> GetByExpediente(long idExpediente)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente);

        var formulario = entity != null
            ? _mapper.Map<vobo_gerencia_coh_bbva>(entity)
            : new vobo_gerencia_coh_bbva { id_expediente = idExpediente };

        var herencia = await RepositoryProvider.GetDatosHerencia(idExpediente);

        // Datos del cliente (titular 1): se obtienen del encabezado ya existente,
        // no de una tabla propia de esta actividad.
        if (herencia != null)
        {
            try
            {
                var encabezado = await _encabezadoApplication.InformacionEncabezado(idExpediente, ActividadVoboGerenciaCoh);
                herencia.nombre_cliente        = encabezado?.nombre_completo_t1;
                herencia.numero_identificacion = encabezado?.numero_identificacion_t1;
                herencia.tipo_identificacion   = encabezado?.tipo_documento_id_t1;
            }
            catch
            {
                // Falla al enriquecer con datos de cliente: se muestran los campos como "-"
                // en el frontend (Req 2.3), sin bloquear el resto de los Datos_Heredados.
            }
        }

        return new VoboGerenciaCohResponse
        {
            formulario = formulario,
            herencia   = herencia
        };
    }

    public async Task<vobo_gerencia_coh_bbva> Guardar(vobo_gerencia_coh_bbva request, int userId)
    {
        var existente = await RepositoryProvider.GetByExpediente(request.id_expediente);

        request.id_actividad = ActividadVoboGerenciaCoh;

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

        var formulario = _mapper.Map<vobo_gerencia_coh_bbva>(entity);
        ValidarCamposObligatorios(formulario);

        FolioDTO folio = await _workflowApplication
            .CapturarDatosFolio(idExpediente, ActividadVoboGerenciaCoh);
        List<xpdl_transition_DTO> transitions = await _workflowApplication
            .GetTransitions(ActividadVoboGerenciaCoh);

        // Enrutamiento único: siempre regresa a Realizar Excepción Desembolso (Comercial)
        string transitionId = transitions.FirstOrDefault(x => x.name == TransicionExcepcionDesembolso)?.transition_id
            ?? throw new InvalidOperationException($"No se encontró la transición '{TransicionExcepcionDesembolso}' en el workflow.");

        var resultado = await _workflowApplication
            .AvanzarActividad(transitionId, folio, userId);

        // Registro atómico en bitácora (fallo aquí revierte la transición)
        RegistrarBitacora(idExpediente, userId, formulario, "Realizar Excepción Desembolso");

        return resultado;
    }

    // ── Helpers privados ──────────────────────────────────────────────────────

    private static void ValidarCamposObligatorios(vobo_gerencia_coh_bbva f)
    {
        var faltantes = new List<string>();

        if (string.IsNullOrWhiteSpace(f.concepto_vobo))
            faltantes.Add("Concepto VoBo");

        if (f.concepto_vobo == "No Favorable" &&
            string.IsNullOrWhiteSpace(f.observaciones))
            faltantes.Add("Observaciones (obligatorio cuando concepto es No Favorable)");

        if (faltantes.Count > 0)
            throw new InvalidOperationException(
                $"Campos obligatorios faltantes: {string.Join(", ", faltantes)}");
    }

    private void RegistrarBitacora(long idExpediente, int userId, vobo_gerencia_coh_bbva f, string destinoActividad)
    {
        var obs = $"VoBo Gerencia COH. Concepto: {f.concepto_vobo}. Destino: [{destinoActividad}].";
        if (!string.IsNullOrWhiteSpace(f.observaciones))
            obs += $" Observaciones: {f.observaciones}";

        _bitacoraApplication.Create(new bitacora
        {
            id_expediente = idExpediente,
            id_actividad  = ActividadVoboGerenciaCoh,
            id_usuario    = userId,
            fecha_alta    = DateTime.Now,
            observaciones = obs,
            is_active     = true,
            row_status    = true
        }, userId);
    }
}
