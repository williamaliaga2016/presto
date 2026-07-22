using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
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

public class FirmarEscrituraClienteApplication
    : MultibancaGenericApplication<firmar_escritura_cliente_bbva, firmar_escritura_cliente_entity, IFirmarEscrituraClienteRepository>,
      IFirmarEscrituraClienteApplication
{
    // Constantes de transición (workflow XPDL)
    private const string TransicionEscalamientoComercial = Constants.TransicionesBBVA.EscrituracionEscalamientoComercial;
    private const string TransicionRevisarEP = Constants.TransicionesBBVA.EscrituracionRevisarEP;
    private const string TransicionVBProrrata = Constants.TransicionesBBVA.EscrituracionVBProrrata;
    private const string TransicionCausacion = Constants.TransicionesBBVA.EscrituracionCausacion;

    // ID de la actividad actual en el workflow
    private static readonly string ActividadFirmarEscrituraCliente = Constants.ActividadesBBVA.EscrituracionFirmarEscrituraCliente;

    // IDs de actividad para verificar conceptos previos (deben coincidir con cat_actividades_ws)
    private const string ActividadRevisarEP = Constants.ActividadesBBVA.EscrituracionRevisarEPAbogado;
    private const string ActividadVBProrrata = Constants.ActividadesBBVA.EscrituracionVBProrrata;
    private const string ActividadCausacion = Constants.ActividadesBBVA.EscrituracionRealizarCausacion;

    // Tipos de crédito para enrutamiento (se consultan desde catálogo en BD)
    private const string CatalogoTipoLeasing = Constants.Catalogo.TipoCreditoLeasing;
    private const string CatalogoTipoCXI = Constants.Catalogo.TipoCreditoCXI;

    // Dependencias
    private readonly IMapper _mapper;
    private readonly ICommonApplication _commonApplication;
    private readonly IWorkflowApplication _workflowApplication;
    private readonly IBitacoraApplication _bitacoraApplication;
    private readonly IActividadesApplication _actividadesApplication;

    public FirmarEscrituraClienteApplication(
        MultibancaDBContext multibancaDBContext,
        IFirmarEscrituraClienteRepository firmarEscrituraClienteRepository,
        IMapper mapper,
        ICommonApplication commonApplication,
        IWorkflowApplication workflowApplication,
        IBitacoraApplication bitacoraApplication,
        IActividadesApplication actividadesApplication)
        : base(multibancaDBContext, firmarEscrituraClienteRepository, mapper)
    {
        _mapper = mapper;
        _commonApplication = commonApplication;
        _workflowApplication = workflowApplication;
        _bitacoraApplication = bitacoraApplication;
        _actividadesApplication = actividadesApplication;
    }

    public async Task<firmar_escritura_cliente_bbva?> GetByExpediente(long idExpediente)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente);

        if (entity == null)
        {
            var herencia = await RepositoryProvider.GetDatosNotariaHerencia(idExpediente);

            if (herencia == null)
                return null;

            dynamic datos = herencia;
            return new firmar_escritura_cliente_bbva
            {
                id_expediente = idExpediente,
                notaria = datos.notaria,
                fecha_notaria = datos.fecha_notaria,
                numero_notaria = datos.numero_notaria,
                ciudad_notaria = datos.ciudad_notaria
            };
        }

        return _mapper.Map<firmar_escritura_cliente_bbva>(entity);
    }

    public async Task<object> GetControles(long idExpediente)
    {
        var representantesLegales = await _commonApplication.GetCatalogoByType("REPRESENTANTE_LEGAL");
        var tipologias = await _commonApplication.GetCatalogoByType("TIPOLOGIA_ESCALAMIENTO");

        return new
        {
            representantes_legales = representantesLegales,
            tipologias = tipologias
        };
    }

    public async Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente)
            ?? throw new InvalidOperationException("Debe guardar la información antes de avanzar.");

        var formulario = _mapper.Map<firmar_escritura_cliente_bbva>(entity);

        // Cargar catálogos de tipo de crédito una sola vez
        var tiposLeasing = await _commonApplication.GetCatalogoByType(CatalogoTipoLeasing);
        var tiposCXI = await _commonApplication.GetCatalogoByType(CatalogoTipoCXI);

        // CA06, CA10 — Obligatoriedad condicionada según escalamiento comercial
        ValidarCamposObligatorios(formulario, tiposLeasing);

        var actividadesCreadas = new List<AssignActivityDTO>();
        List<xpdl_transition_DTO> transitions = await _workflowApplication.GetTransitions(ActividadFirmarEscrituraCliente);
        FolioDTO folio = await _workflowApplication.CapturarDatosFolio(idExpediente, ActividadFirmarEscrituraCliente);

        if (formulario.requiere_escalamiento_comercial == "SI")
        {
            // CA03 — Escalamiento Comercial: Sí → Realizar Gestión Comercial (rol Comercial)
            var transitionId = transitions.FirstOrDefault(x => x.name == TransicionEscalamientoComercial)?.transition_id
                ?? throw new InvalidOperationException($"No se encontró la transición '{TransicionEscalamientoComercial}' en el workflow.");

            var resultado = await _workflowApplication.AvanzarActividad(transitionId, folio, userId);
            actividadesCreadas.AddRange(resultado);
        }
        else
        {
            // CA03 — Escalamiento Comercial: No → Revisar EP Abogado (rol Abogado)
            // CA11 — Evitar re-escalamiento si ya existe concepto previo
            bool tieneConceptoEP = await _actividadesApplication.IsCompleteActivity(idExpediente, ActividadRevisarEP);

            if (!tieneConceptoEP)
            {
                var transitionIdEP = transitions.FirstOrDefault(x => x.name == TransicionRevisarEP)?.transition_id
                    ?? throw new InvalidOperationException($"No se encontró la transición '{TransicionRevisarEP}' en el workflow.");

                var resultadoEP = await _workflowApplication.AvanzarActividad(transitionIdEP, folio, userId);
                actividadesCreadas.AddRange(resultadoEP);
            }

            // CA04, CA09 — Producto CXI: disparar en paralelo Realizar VB Prorrata (rol Gestor Constructor)
            // CA11 — Evitar re-escalamiento si ya existe concepto previo
            if (EsTipoCXI(formulario.tipo_credito, tiposCXI))
            {
                bool tieneConceptoProrrata = await _actividadesApplication.IsCompleteActivity(idExpediente, ActividadVBProrrata);

                if (!tieneConceptoProrrata)
                {
                    var transitionIdProrrata = transitions.FirstOrDefault(x => x.name == TransicionVBProrrata)?.transition_id
                        ?? throw new InvalidOperationException($"No se encontró la transición '{TransicionVBProrrata}' en el workflow.");

                    try
                    {
                        var resultadoProrrata = await _workflowApplication.AvanzarActividad(transitionIdProrrata, folio, userId);
                        actividadesCreadas.AddRange(resultadoProrrata);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(
                            $"Error en enrutamiento paralelo CXI. No fue posible crear la actividad VB Prorrata: {ex.Message}");
                    }
                }
            }

            // CA04, CA09 — Producto Leasing + requiere_causar = SI: disparar Realizar Causación (rol Analista Leasing)
            // CA11 — Evitar re-escalamiento si ya existe concepto previo
            if (EsTipoLeasing(formulario.tipo_credito, tiposLeasing) && formulario.requiere_causar == "SI")
            {
                bool tieneConceptoCausacion = await _actividadesApplication.IsCompleteActivity(idExpediente, ActividadCausacion);

                if (!tieneConceptoCausacion)
                {
                    var transitionIdCausacion = transitions.FirstOrDefault(x => x.name == TransicionCausacion)?.transition_id
                        ?? throw new InvalidOperationException($"No se encontró la transición '{TransicionCausacion}' en el workflow.");

                    try
                    {
                        var resultadoCausacion = await _workflowApplication.AvanzarActividad(transitionIdCausacion, folio, userId);
                        actividadesCreadas.AddRange(resultadoCausacion);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(
                            $"Error en enrutamiento paralelo Leasing. No fue posible crear la actividad Causación: {ex.Message}");
                    }
                }
            }
        }

        // CA01.5 — Registrar en bitácora: fecha, actividad, usuario, decisiones, observaciones
        RegistrarBitacora(idExpediente, userId, formulario, actividadesCreadas, tiposLeasing, tiposCXI);

        return actividadesCreadas;
    }

    // ======================== Métodos privados ========================

    private static void ValidarCamposObligatorios(firmar_escritura_cliente_bbva formulario, IEnumerable<dynamic> tiposLeasing)
    {
        var camposFaltantes = new List<string>();

        if (string.IsNullOrWhiteSpace(formulario.notaria))
            camposFaltantes.Add("Notaría");

        if (!formulario.fecha_notaria.HasValue)
            camposFaltantes.Add("Fecha Notaría");

        if (!formulario.numero_notaria.HasValue)
            camposFaltantes.Add("Número Notaría");

        if (string.IsNullOrWhiteSpace(formulario.ciudad_notaria))
            camposFaltantes.Add("Ciudad Notaría");

        if (string.IsNullOrWhiteSpace(formulario.requiere_escalamiento_comercial))
            camposFaltantes.Add("¿Requiere Escalamiento Comercial?");

        if (formulario.requiere_escalamiento_comercial == "NO")
        {
            if (string.IsNullOrWhiteSpace(formulario.numero_escritura))
                camposFaltantes.Add("Número de la Escritura");

            if (!formulario.fecha_escritura.HasValue)
                camposFaltantes.Add("Fecha de la Escritura");
        }

        if (formulario.requiere_escalamiento_comercial == "SI")
        {
            if (string.IsNullOrWhiteSpace(formulario.tipologia))
                camposFaltantes.Add("Tipología");
        }

        if (EsTipoLeasing(formulario.tipo_credito, tiposLeasing))
        {
            if (string.IsNullOrWhiteSpace(formulario.requiere_causar))
                camposFaltantes.Add("¿Requiere Causar?");
        }

        if (camposFaltantes.Count > 0)
        {
            throw new InvalidOperationException(
                $"Campos obligatorios faltantes: {string.Join(", ", camposFaltantes)}");
        }
    }

    private static bool EsTipoLeasing(string? tipoCredito, IEnumerable<dynamic> tiposLeasing)
    {
        if (string.IsNullOrWhiteSpace(tipoCredito))
            return false;

        return tiposLeasing.Any(t => string.Equals((string)t.valor, tipoCredito, StringComparison.OrdinalIgnoreCase));
    }

    private static bool EsTipoCXI(string? tipoCredito, IEnumerable<dynamic> tiposCXI)
    {
        if (string.IsNullOrWhiteSpace(tipoCredito))
            return false;

        return tiposCXI.Any(t => string.Equals((string)t.valor, tipoCredito, StringComparison.OrdinalIgnoreCase));
    }

    private void RegistrarBitacora(
        long idExpediente,
        int userId,
        firmar_escritura_cliente_bbva formulario,
        List<AssignActivityDTO> actividadesCreadas,
        IEnumerable<dynamic> tiposLeasing,
        IEnumerable<dynamic> tiposCXI)
    {
        List<string> decisiones = new List<string>();

        if (formulario.requiere_escalamiento_comercial == "SI")
            decisiones.Add($"Escalamiento Comercial: SÍ, Tipología: {formulario.tipologia}");
        else
            decisiones.Add("Escalamiento Comercial: NO → Revisar EP Abogado");

        if (EsTipoCXI(formulario.tipo_credito, tiposCXI))
            decisiones.Add("Producto CXI → Realizar VB Prorrata (paralela)");

        if (EsTipoLeasing(formulario.tipo_credito, tiposLeasing))
        {
            decisiones.Add(formulario.requiere_causar == "SI"
                ? "Leasing → Requiere Causar: SÍ → Realizar Causación (paralela)"
                : "Leasing → Requiere Causar: NO");
        }

        var actividadesStr = actividadesCreadas.Count > 0
            ? string.Join(", ", actividadesCreadas.Select(a => a.display_name ?? a.id_actividad ?? "N/A"))
            : "Ninguna";

        var observacionesBitacora = $"Avance de Firmar Escritura Cliente. " +
            $"Decisiones: [{string.Join("; ", decisiones)}]. " +
            $"Actividades creadas: [{actividadesStr}].";

        if (!string.IsNullOrWhiteSpace(formulario.observaciones))
            observacionesBitacora += $" Observaciones del analista: {formulario.observaciones}";

        _bitacoraApplication.Create(new bitacora
        {
            id_expediente = idExpediente,
            id_actividad = ActividadFirmarEscrituraCliente,
            id_usuario = userId,
            fecha_alta = DateTime.Now,
            observaciones = observacionesBitacora,
            is_active = true,
            row_status = true
        }, userId);
    }
}
