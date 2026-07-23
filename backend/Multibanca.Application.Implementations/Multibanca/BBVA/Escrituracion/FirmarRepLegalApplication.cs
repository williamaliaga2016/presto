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

public class FirmarRepLegalApplication
    : MultibancaGenericApplication<firmar_rep_legal, firmar_rep_legal_entity, IFirmarRepLegalRepository>,
      IFirmarRepLegalApplication
{
    // Constantes de transición (workflow XPDL)
    // TODO: Reemplazar con los nombres reales de transición del XPDL
    private const string TransicionEntregaEP = Constants.TransicionesBBVA.FirmarRepLegalEntregaEP;
    private const string TransicionDevolucion = Constants.TransicionesBBVA.FirmarRepLegalDevolucion;

    // ID de la actividad actual en el workflow
    private static readonly string ActividadFirmarRepLegal = Constants.ActividadesBBVA.EscrituracionFirmarRepLegal;

    // Códigos de concepto de firma
    private const string ConceptoFirmadaConforme = "CRL-1";
    private const string ConceptoNoFirmada = "CRL-2";

    // Dependencias
    private readonly IMapper _mapper;
    private readonly ICommonApplication _commonApplication;
    private readonly IWorkflowApplication _workflowApplication;
    private readonly IBitacoraApplication _bitacoraApplication;

    public FirmarRepLegalApplication(
        MultibancaDBContext multibancaDBContext,
        IFirmarRepLegalRepository firmarRepLegalRepository,
        IMapper mapper,
        ICommonApplication commonApplication,
        IWorkflowApplication workflowApplication,
        IBitacoraApplication bitacoraApplication)
        : base(multibancaDBContext, firmarRepLegalRepository, mapper)
    {
        _mapper = mapper;
        _commonApplication = commonApplication;
        _workflowApplication = workflowApplication;
        _bitacoraApplication = bitacoraApplication;
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

        string destinoActividad;

        if (formulario.concepto_firma == ConceptoNoFirmada)
        {
            // Escritura NO firmada → Realizar Devolución EP (Analista de Vivienda)
            var transitionId = transitions.FirstOrDefault(x => x.name == TransicionDevolucion)?.transition_id
                ?? throw new InvalidOperationException($"No se encontró la transición '{TransicionDevolucion}' en el workflow.");

            var resultado = await _workflowApplication.AvanzarActividad(transitionId, folio, userId);
            actividadesCreadas.AddRange(resultado);
            destinoActividad = "Realizar Devolución EP";
        }
        else
        {
            // Escritura firmada Conforme → Realizar Entrega EP Firmada (Analista de Vivienda)
            var transitionId = transitions.FirstOrDefault(x => x.name == TransicionEntregaEP)?.transition_id
                ?? throw new InvalidOperationException($"No se encontró la transición '{TransicionEntregaEP}' en el workflow.");

            var resultado = await _workflowApplication.AvanzarActividad(transitionId, folio, userId);
            actividadesCreadas.AddRange(resultado);
            destinoActividad = "Realizar Entrega EP Firmada";
        }

        // Registrar bitácora
        RegistrarBitacora(idExpediente, userId, formulario, destinoActividad);

        return actividadesCreadas;
    }

    // ======================== Métodos privados ========================

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
