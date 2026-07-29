using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
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

public class ExcepcionDesembolsoApplication
    : MultibancaGenericApplication<excepcion_desembolso_bbva, excepcion_desembolso_entity, IExcepcionDesembolsoRepository>,
      IExcepcionDesembolsoApplication
{
    // Constantes de transición (workflow XPDL)
    private const string TransicionVoBoGerencia = Constants.TransicionesBBVA.ExcepcionDesembolsoVoBoGerencia;
    private const string TransicionValidarCondicionesDesembolso = Constants.TransicionesBBVA.ExcepcionDesembolsoValidarCondiciones;

    // ID de la actividad actual en el workflow
    private static readonly string ActividadExcepcionDesembolso = Constants.ActividadesBBVA.EscrituracionRealizarExcepcionDesembolso;

    // Dependencias
    private readonly IMapper _mapper;
    private readonly ICommonApplication _commonApplication;

    private readonly IWorkflowApplication _workflowApplication;
    private readonly IBitacoraApplication _bitacoraApplication;

    public ExcepcionDesembolsoApplication(
        MultibancaDBContext multibancaDBContext,
        IExcepcionDesembolsoRepository repository,
        IMapper mapper,
        ICommonApplication commonApplication,
        IWorkflowApplication workflowApplication,
        IBitacoraApplication bitacoraApplication)
        : base(multibancaDBContext, repository, mapper)
    {
        _mapper = mapper;
        _commonApplication = commonApplication;
        _workflowApplication = workflowApplication;
        _bitacoraApplication = bitacoraApplication;
    }

    public async Task<ExcepcionDesembolsoResponse> GetByExpediente(long idExpediente)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente);

        var formulario = entity != null
            ? _mapper.Map<excepcion_desembolso_bbva>(entity)
            : new excepcion_desembolso_bbva { id_expediente = idExpediente };

        var origenCaso = await RepositoryProvider.DeterminarOrigenCaso(idExpediente);
        var herencia   = await RepositoryProvider.GetDatosHerencia(idExpediente);

        if(OrigenCasoEnum.FIRMAR_REP_LEGAL.Equals(origenCaso) && herencia != null)
        {
            herencia.conceptoFirmaDesc = await Helpers.CatalogHelper.GetDescFromCatalog(
                _commonApplication,
                Constants.Catalogo.ConceptoFirmaRepLegal_L41,
                herencia.conceptoFirma,
                herencia.conceptoFirma
            );
        }

        return new ExcepcionDesembolsoResponse
        {
            formulario = formulario,
            herencia   = herencia,
            _OrigenCaso = origenCaso //autom. setea el valor en string
        };
    }

    public async Task<excepcion_desembolso_bbva> Guardar(excepcion_desembolso_bbva request, int userId)
    {
        excepcion_desembolso_entity? existente = await RepositoryProvider.GetByExpediente(request.id_expediente);

        // Determinar origen si no está establecido
        if (string.IsNullOrWhiteSpace(request.origen_caso))
        {
            var origen = await RepositoryProvider.DeterminarOrigenCaso(request.id_expediente);
            request.origen_caso = origen?.ToString();
        }

        if (existente != null)
        {
            // Actualizar registro existente preservando auditoría de creación
            request.id = existente.id;
            request.created_by = existente.created_by;
            request.created_date = existente.created_date;
            request.modified_by = userId;
            request.modified_date = DateTime.UtcNow;
            request.is_active = existente.is_active;
            request.row_status = existente.row_status;
            return Update(request, userId);
        }
        else
        {
            // Crear nuevo registro
            request.is_active = true;
            request.row_status = true;
            request.id_actividad = ActividadExcepcionDesembolso;
            return Create(request, userId);
        }
    }

    public async Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente)
            ?? throw new InvalidOperationException("Debe guardar la información antes de avanzar.");

        var formulario = _mapper.Map<excepcion_desembolso_bbva>(entity);

        // Validar campos obligatorios
        ValidarCamposObligatorios(formulario);

        var actividadesCreadas = new List<AssignActivityDTO>();
        List<xpdl_transition_DTO> transitions = await _workflowApplication.GetTransitions(ActividadExcepcionDesembolso);
        FolioDTO folio = await _workflowApplication.CapturarDatosFolio(idExpediente, ActividadExcepcionDesembolso);

        string destinoActividad;

        if (formulario.requiere_vobo_gerencia == "SI")
        {
            // Requiere VoBo Gerencia → Realizar Vobo Gerencia COH
            var transitionId = transitions.FirstOrDefault(x => x.name == TransicionVoBoGerencia)?.transition_id
                ?? throw new InvalidOperationException($"No se encontró la transición '{TransicionVoBoGerencia}' en el workflow.");

            var resultado = await _workflowApplication.AvanzarActividad(transitionId, folio, userId);
            actividadesCreadas.AddRange(resultado);
            destinoActividad = "Realizar Vobo Gerencia COH";
        }
        else
        {
            // No requiere VoBo → Validar Condiciones Desembolso
            var transitionId = transitions.FirstOrDefault(x => x.name == TransicionValidarCondicionesDesembolso)?.transition_id
                ?? throw new InvalidOperationException($"No se encontró la transición '{TransicionValidarCondicionesDesembolso}' en el workflow.");

            var resultado = await _workflowApplication.AvanzarActividad(transitionId, folio, userId);
            actividadesCreadas.AddRange(resultado);
            destinoActividad = "Validar Condiciones Desembolso";
        }

        // Registrar bitácora
        RegistrarBitacora(idExpediente, userId, formulario, destinoActividad);

        return actividadesCreadas;
    }

    // ======================== Métodos privados ========================

    private static void ValidarCamposObligatorios(excepcion_desembolso_bbva formulario)
    {
        var camposFaltantes = new List<string>();

        if (formulario.excepcion_autorizada != "SI" && formulario.excepcion_autorizada != "NO")
            camposFaltantes.Add("Excepción Autorizada");

        if (formulario.requiere_vobo_gerencia != "SI" && formulario.requiere_vobo_gerencia != "NO")
            camposFaltantes.Add("Requiere VoBo Gerencia");

        if (!formulario.confirmacion_excepcion)
            camposFaltantes.Add("Confirmación Excepción (debe estar marcada)");

        if (camposFaltantes.Count > 0)
        {
            throw new InvalidOperationException(
                $"Campos obligatorios faltantes: {string.Join(", ", camposFaltantes)}");
        }
    }

    private void RegistrarBitacora(
        long idExpediente,
        int userId,
        excepcion_desembolso_bbva formulario,
        string destinoActividad)
    {
        var decisión = formulario.requiere_vobo_gerencia == "SI" ? "Gerencia" : "Desembolso";
        var observacionesBitacora = $"Avance de Realizar Excepción Desembolso. " +
            $"Excepción Autorizada: {formulario.excepcion_autorizada}. " +
            $"Decisión de Enrutamiento: {decisión}. " +
            $"Destino: [{destinoActividad}].";

        if (!string.IsNullOrWhiteSpace(formulario.observaciones_excepcion))
            observacionesBitacora += $" Observaciones: {formulario.observaciones_excepcion}";

        _bitacoraApplication.Create(new bitacora
        {
            id_expediente = idExpediente,
            id_actividad = ActividadExcepcionDesembolso,
            id_usuario = userId,
            fecha_alta = DateTime.Now,
            observaciones = observacionesBitacora,
            is_active = true,
            row_status = true
        }, userId);
    }
}
