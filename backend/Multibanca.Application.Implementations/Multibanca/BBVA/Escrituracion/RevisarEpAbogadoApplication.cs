using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca.BBVA.Escrituracion;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Common;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

namespace Multibanca.Application.Implementations.Multibanca.BBVA.Escrituracion;

public class RevisarEpAbogadoApplication
    : MultibancaGenericApplication<revisar_ep_abogado_bbva, revisar_ep_abogado_entity, IRevisarEpAbogadoRepository>,
      IRevisarEpAbogadoApplication
{
    // Constantes de transición (workflow XPDL)
    private const string TransicionFirmarRepLegal = Constants.TransicionesBBVA.RevisarEPAFirmarRepLegal;
    private const string TransicionDevolucion = Constants.TransicionesBBVA.RevisarEPADevolucion;

    // ID de la actividad actual en el workflow
    private static readonly string ActividadRevisarEPAbogado = Constants.ActividadesBBVA.EscrituracionRevisarEPAbogado;

    private readonly IMapper _mapper;
    private readonly ICommonApplication _commonApplication;
    private readonly IWorkflowApplication _workflowApplication;
    private readonly IBitacoraApplication _bitacoraApplication;

    public RevisarEpAbogadoApplication(
        MultibancaDBContext multibancaDBContext,
        IRevisarEpAbogadoRepository revisarEpAbogadoRepository,
        IMapper mapper,
        ICommonApplication commonApplication,
        IWorkflowApplication workflowApplication,
        IBitacoraApplication bitacoraApplication)
        : base(multibancaDBContext, revisarEpAbogadoRepository, mapper)
    {
        _mapper = mapper;
        _commonApplication = commonApplication;
        _workflowApplication = workflowApplication;
        _bitacoraApplication = bitacoraApplication;
    }

    public async Task<revisar_ep_abogado_bbva?> GetByExpediente(long idExpediente)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente);
        var herencia = await RepositoryProvider.GetDatosHerencia(idExpediente);

        if (entity == null)
        {
            return new revisar_ep_abogado_bbva
            {
                id_expediente = idExpediente,
                notaria = herencia?.notaria,
                fecha_notaria = herencia?.fecha_notaria,
                numero_notaria = herencia?.numero_notaria,
                ciudad_notaria = herencia?.ciudad_notaria,
                numero_escritura = herencia?.numero_escritura,
                fecha_escritura = herencia?.fecha_escritura,
                representante_legal = herencia?.representante_legal
            };
        }

        var result = _mapper.Map<revisar_ep_abogado_bbva>(entity);

        // Overlay inherited fields from firmar_escritura_cliente
        result.notaria = herencia?.notaria;
        result.fecha_notaria = herencia?.fecha_notaria;
        result.numero_notaria = herencia?.numero_notaria;
        result.ciudad_notaria = herencia?.ciudad_notaria;
        result.numero_escritura = herencia?.numero_escritura;
        result.fecha_escritura = herencia?.fecha_escritura;

        // Si representante_legal no fue editado en esta actividad, pre-cargar desde herencia
        if (string.IsNullOrWhiteSpace(result.representante_legal))
            result.representante_legal = herencia?.representante_legal;

        return result;
    }

    public async Task<object> GetControles(long idExpediente)
    {
        var representantesLegales = await _commonApplication.GetCatalogoByType(Constants.Catalogo.RepresentanteLegal_L38);
        var tipologias = await _commonApplication.GetCatalogoByType(Constants.Catalogo.TipologiaCorreccionEP_L39);
        var casuisticas = await _commonApplication.GetCatalogoByType(Constants.Catalogo.CasuisticaCorreccionEP_L40);

        return new
        {
            representantes_legales = representantesLegales,
            tipologias = tipologias,
            casuisticas = casuisticas
        };
    }

    public async Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente)
            ?? throw new InvalidOperationException("Debe guardar la información antes de avanzar.");

        var formulario = _mapper.Map<revisar_ep_abogado_bbva>(entity);

        ValidarCamposObligatorios(formulario);

        var actividadesCreadas = new List<AssignActivityDTO>();
        List<xpdl_transition_DTO> transitions = await _workflowApplication.GetTransitions(ActividadRevisarEPAbogado);
        FolioDTO folio = await _workflowApplication.CapturarDatosFolio(idExpediente, ActividadRevisarEPAbogado);

        if (string.Equals(formulario.ep_conforme, "NO", StringComparison.OrdinalIgnoreCase))
        {
            // CA04 — EP No Conforme: enrutar a "Realizar Devolución EP" (Analista de Vivienda)
            var transitionId = transitions.FirstOrDefault(x => x.name == TransicionDevolucion)?.transition_id
                ?? throw new InvalidOperationException($"No se encontró la transición '{TransicionDevolucion}' en el workflow.");

            var resultado = await _workflowApplication.AvanzarActividad(transitionId, folio, userId);
            actividadesCreadas.AddRange(resultado);
        }
        else
        {
            // CA04, CA09 — EP Conforme: verificar estado de Carta de Aprobación
            string? estadoCarta = await RepositoryProvider.GetEstadoCartaAprobacion(idExpediente);

            bool cartaVencidaOPorVencer = string.Equals(estadoCarta, "Por_Vencer", StringComparison.OrdinalIgnoreCase)
                || string.Equals(estadoCarta, "Vencido", StringComparison.OrdinalIgnoreCase);

            if (cartaVencidaOPorVencer)
            {
                // CA09 — Carta vencida o por vencer: enrutar a "Realizar Devolución EP" (Analista de Vivienda)
                var transitionId = transitions.FirstOrDefault(x => x.name == TransicionDevolucion)?.transition_id
                    ?? throw new InvalidOperationException($"No se encontró la transición '{TransicionDevolucion}' en el workflow.");

                var resultado = await _workflowApplication.AvanzarActividad(transitionId, folio, userId);
                actividadesCreadas.AddRange(resultado);
            }
            else
            {
                // CA04 — Carta vigente: enrutar a "Firmar Rep. Legal" (Representante Legal)
                var transitionId = transitions.FirstOrDefault(x => x.name == TransicionFirmarRepLegal)?.transition_id
                    ?? throw new InvalidOperationException($"No se encontró la transición '{TransicionFirmarRepLegal}' en el workflow.");

                var resultado = await _workflowApplication.AvanzarActividad(transitionId, folio, userId);
                actividadesCreadas.AddRange(resultado);
            }
        }

        // CA07 — Registrar en bitácora
        RegistrarBitacora(idExpediente, userId, formulario);

        return actividadesCreadas;
    }

    // ======================== Métodos privados ========================

    private static void ValidarCamposObligatorios(revisar_ep_abogado_bbva formulario)
    {
        var camposFaltantes = new List<string>();

        // Campos siempre obligatorios
        if (string.IsNullOrWhiteSpace(formulario.representante_legal))
            camposFaltantes.Add("Representante Legal");

        if (string.IsNullOrWhiteSpace(formulario.ep_conforme))
            camposFaltantes.Add("EP Conforme");

        // Campos obligatorios solo cuando ep_conforme = "NO" (CA05, CA08)
        if (string.Equals(formulario.ep_conforme, "NO", StringComparison.OrdinalIgnoreCase))
        {
            if (string.IsNullOrWhiteSpace(formulario.tipologia))
                camposFaltantes.Add("Tipología");

            if (string.IsNullOrWhiteSpace(formulario.casuistica))
                camposFaltantes.Add("Casuística");

            if (string.IsNullOrWhiteSpace(formulario.observaciones_legales))
                camposFaltantes.Add("Observaciones Legales");
        }

        if (camposFaltantes.Count > 0)
        {
            throw new InvalidOperationException(
                $"Campos obligatorios faltantes: {string.Join(", ", camposFaltantes)}");
        }
    }

    private void RegistrarBitacora(long idExpediente, int userId, revisar_ep_abogado_bbva formulario)
    {
        var conformidad = formulario.ep_conforme ?? "N/A";
        var observaciones = $"Avance de Revisar EP Abogado. Conformidad EP: {conformidad}.";

        if (string.Equals(formulario.ep_conforme, "NO", StringComparison.OrdinalIgnoreCase))
        {
            observaciones += $" Tipología: {formulario.tipologia}. Casuística: {formulario.casuistica}.";

            if (!string.IsNullOrWhiteSpace(formulario.observaciones_legales))
                observaciones += $" Observaciones legales: {formulario.observaciones_legales}";
        }

        _bitacoraApplication.Create(new bitacora
        {
            id_expediente = idExpediente,
            id_actividad = ActividadRevisarEPAbogado,
            id_usuario = userId,
            fecha_alta = DateTime.Now,
            observaciones = observaciones,
            is_active = true,
            row_status = true
        }, userId);
    }
}
