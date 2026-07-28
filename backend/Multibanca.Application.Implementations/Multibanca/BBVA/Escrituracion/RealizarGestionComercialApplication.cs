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

public class RealizarGestionComercialApplication
    : MultibancaGenericApplication<realizar_gestion_comercial, realizar_gestion_comercial_entity, IRealizarGestionComercialRepository>,
      IRealizarGestionComercialApplication
{
    private const string TransicionRetornoFirmarEscritura = Constants.TransicionesBBVA.GestionComercialRetornoFirmarEscritura;
    private const string TransicionRetornoDevolucionEP = Constants.TransicionesBBVA.GestionComercialRetornoDevolucionEP;
    private const string TransicionRetornoValidarDesembolso = Constants.TransicionesBBVA.GestionComercialRetornoValidarDesembolso;
    private static readonly string ActividadGestionComercial = Constants.ActividadesBBVA.EscrituracionRealizarGestionComercial;

    private readonly IMapper _mapper;
    private readonly ICommonApplication _commonApplication;
    private readonly IWorkflowApplication _workflowApplication;
    private readonly IBitacoraApplication _bitacoraApplication;
    private readonly IActividadesApplication _actividadesApplication;
    private readonly IFirmarEscrituraClienteRepository _firmarEscrituraClienteRepository;
    private readonly IValidarCondicionesDesembolsoRepository _validarDesembolsoRepository;
    private readonly IRealizarVBFinalAbogadoRepository _vbFinalAbogadoRepository;
    private readonly IValidarInformacionRepository _validarInformacionRepository;

    public RealizarGestionComercialApplication(
        MultibancaDBContext multibancaDBContext,
        IRealizarGestionComercialRepository repository,
        IMapper mapper,
        ICommonApplication commonApplication,
        IWorkflowApplication workflowApplication,
        IBitacoraApplication bitacoraApplication,
        IActividadesApplication actividadesApplication,
        IFirmarEscrituraClienteRepository firmarEscrituraClienteRepository,
        IValidarCondicionesDesembolsoRepository validarDesembolsoRepository,
        IRealizarVBFinalAbogadoRepository vbFinalAbogadoRepository,
        IValidarInformacionRepository validarInformacionRepository)
        : base(multibancaDBContext, repository, mapper)
    {
        _mapper = mapper;
        _commonApplication = commonApplication;
        _workflowApplication = workflowApplication;
        _bitacoraApplication = bitacoraApplication;
        _actividadesApplication = actividadesApplication;
        _firmarEscrituraClienteRepository = firmarEscrituraClienteRepository;
        _validarDesembolsoRepository = validarDesembolsoRepository;
        _vbFinalAbogadoRepository = vbFinalAbogadoRepository;
        _validarInformacionRepository = validarInformacionRepository;
    }

    public async Task<object?> GetByExpediente(long idExpediente)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente);

        var formulario = entity != null
            ? _mapper.Map<realizar_gestion_comercial>(entity)
            : new realizar_gestion_comercial { id_expediente = idExpediente };

        // Calcular origen (CA05)
        var firmarEscritura = await _firmarEscrituraClienteRepository.GetByExpediente(idExpediente);
        bool vieneDeFirmarEscritura = firmarEscritura?.requiere_escalamiento_comercial == "SI";

        var validarDesembolso = await _validarDesembolsoRepository.GetByExpediente(idExpediente);
        bool vieneDeValidarDesembolso = validarDesembolso?.requiere_escalamiento_comercial == "SI";

        string origenEscalamiento = vieneDeFirmarEscritura ? "FIRMAR_ESCRITURA"
            : vieneDeValidarDesembolso ? "VALIDAR_DESEMBOLSO"
            : "DEVOLUCION_EP";

        formulario.origen_escalamiento = origenEscalamiento;

        // Datos heredados condicionales (CA02)
        object datosHeredados;
        if (origenEscalamiento == "FIRMAR_ESCRITURA")
        {
            var validarInfo = await _validarInformacionRepository.GetByExpediente(idExpediente);
            string? notariaDesc = null;
            if (!string.IsNullOrWhiteSpace(firmarEscritura?.notaria))
            {
                var cat = await _commonApplication.GetCatalogoByType(Constants.Catalogo.Notarias_L46);
                notariaDesc = cat.FirstOrDefault(c => c.code == firmarEscritura.notaria)?.description ?? firmarEscritura.notaria;
            }
            string? tipoCreditoDesc = null;
            if (!string.IsNullOrWhiteSpace(validarInfo?.tipo_credito))
            {
                var cat = await _commonApplication.GetCatalogoByType(Constants.Catalogo.TipoCredito);
                tipoCreditoDesc = cat.FirstOrDefault(c => c.code == validarInfo.tipo_credito)?.description ?? validarInfo.tipo_credito;
            }
            string? repLegalDesc = null;
            if (!string.IsNullOrWhiteSpace(firmarEscritura?.representante_legal))
            {
                var cat = await _commonApplication.GetCatalogoByType(Constants.Catalogo.RepresentanteLegal_L38);
                repLegalDesc = cat.FirstOrDefault(c => c.code == firmarEscritura.representante_legal)?.description ?? firmarEscritura.representante_legal;
            }

            datosHeredados = new
            {
                origen_label = "Desde Firmar Escritura Cliente",
                tipo_credito = tipoCreditoDesc,
                nombre_completo = validarInfo?.nombre_completo_t1,
                notaria = notariaDesc,
                numero_notaria = firmarEscritura?.numero_notaria,
                ciudad_notaria = firmarEscritura?.ciudad_notaria,
                numero_escritura = firmarEscritura?.numero_escritura,
                representante_legal = repLegalDesc,
            };
        }
        else if (origenEscalamiento == "DEVOLUCION_EP")
        {
            var vbFinal = await _vbFinalAbogadoRepository.GetByExpediente(idExpediente);

            // Resolver códigos a descripciones
            string? notariaDesc = null;
            if (!string.IsNullOrWhiteSpace(firmarEscritura?.notaria))
            {
                var cat = await _commonApplication.GetCatalogoByType(Constants.Catalogo.Notarias_L46);
                notariaDesc = cat.FirstOrDefault(c => c.code == firmarEscritura.notaria)?.description ?? firmarEscritura.notaria;
            }
            string? tipologiaDesc = null;
            if (!string.IsNullOrWhiteSpace(vbFinal?.tipologia))
            {
                var cat = await _commonApplication.GetCatalogoByType(Constants.Catalogo.TipologiaCorreccionEP_L39);
                tipologiaDesc = cat.FirstOrDefault(c => c.code == vbFinal.tipologia)?.description ?? vbFinal.tipologia;
            }
            string? casuisticaDesc = null;
            if (!string.IsNullOrWhiteSpace(vbFinal?.casuistica))
            {
                var cat = await _commonApplication.GetCatalogoByType(Constants.Catalogo.CasuisticaCorreccionEP_L40);
                casuisticaDesc = cat.FirstOrDefault(c => c.code == vbFinal.casuistica)?.description ?? vbFinal.casuistica;
            }

            datosHeredados = new
            {
                origen_label = "Desde Realizar Devolución EP",
                tipologia_rechazo = tipologiaDesc,
                casuistica_rechazo = casuisticaDesc,
                observaciones_abogado = vbFinal?.observaciones,
                notaria = notariaDesc,
                numero_escritura = firmarEscritura?.numero_escritura,
            };
        }
        else
        {
            var validarInfo = await _validarInformacionRepository.GetByExpediente(idExpediente);
            string? tipoCreditoDesc = null;
            if (!string.IsNullOrWhiteSpace(validarInfo?.tipo_credito))
            {
                var cat = await _commonApplication.GetCatalogoByType(Constants.Catalogo.TipoCredito);
                tipoCreditoDesc = cat.FirstOrDefault(c => c.code == validarInfo.tipo_credito)?.description ?? validarInfo.tipo_credito;
            }
            string? notariaDesc = null;
            if (!string.IsNullOrWhiteSpace(firmarEscritura?.notaria))
            {
                var cat = await _commonApplication.GetCatalogoByType(Constants.Catalogo.Notarias_L46);
                notariaDesc = cat.FirstOrDefault(c => c.code == firmarEscritura.notaria)?.description ?? firmarEscritura.notaria;
            }

            datosHeredados = new
            {
                origen_label = "Desde Validar Condiciones Desembolso",
                nombre_completo = validarInfo?.nombre_completo_t1,
                tipo_credito = tipoCreditoDesc,
                plan_pagos_confirmado = validarDesembolso?.confirmar_plan_pagos,
                requiere_escalamiento = validarDesembolso?.requiere_escalamiento_comercial,
                observaciones_condiciones = validarDesembolso?.observaciones,
                notaria = notariaDesc,
                ciudad_notaria = firmarEscritura?.ciudad_notaria,
                numero_notaria = firmarEscritura?.numero_notaria,
                numero_escritura = firmarEscritura?.numero_escritura,
            };
        }

        return new
        {
            formulario,
            origen_escalamiento = origenEscalamiento,
            datos_heredados = datosHeredados
        };
    }

    public async Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId)
    {
        var entity = await RepositoryProvider.GetByExpediente(idExpediente)
            ?? throw new InvalidOperationException("Debe guardar la información antes de avanzar.");

        var formulario = _mapper.Map<realizar_gestion_comercial>(entity);

        ValidarCamposObligatorios(formulario);

        // Si cliente desiste → cancelar caso
        if (formulario.cliente_desiste == "SI")
        {
            await _workflowApplication.CancelCase(idExpediente);

            // Marcar la actividad local como "Desistido" para que salga de bandeja
            var actividad = await _actividadesApplication.ObtenerActividadPorExpedienteActividad(idExpediente, ActividadGestionComercial);
            if (actividad != null && actividad.id > 0)
            {
                actividad.status = "Desistido";
                actividad.activo = false;
                _actividadesApplication.Update(actividad, userId);
            }

            _bitacoraApplication.Create(new bitacora
            {
                id_expediente = idExpediente,
                id_actividad = ActividadGestionComercial,
                id_usuario = userId,
                fecha_alta = DateTime.Now,
                observaciones = $"Fin Terminal. Cliente desistió del caso. Observaciones: {formulario.observaciones ?? "N/A"}",
                is_active = true,
                row_status = true
            }, userId);

            return new List<AssignActivityDTO>();
        }

        // Si NO desiste → retornar a origen
        var firmarEscritura = await _firmarEscrituraClienteRepository.GetByExpediente(idExpediente);
        bool vieneDeFirmarEscritura = firmarEscritura?.requiere_escalamiento_comercial == "SI";
        var validarDesembolso = await _validarDesembolsoRepository.GetByExpediente(idExpediente);
        bool vieneDeValidarDesembolso = validarDesembolso?.requiere_escalamiento_comercial == "SI";

        string origenEscalamiento = vieneDeFirmarEscritura ? "FIRMAR_ESCRITURA"
            : vieneDeValidarDesembolso ? "VALIDAR_DESEMBOLSO"
            : "DEVOLUCION_EP";

        string transicionSeleccionada = origenEscalamiento switch
        {
            "FIRMAR_ESCRITURA" => TransicionRetornoFirmarEscritura,
            "VALIDAR_DESEMBOLSO" => TransicionRetornoValidarDesembolso,
            _ => TransicionRetornoDevolucionEP
        };

        string destinoActividad = origenEscalamiento switch
        {
            "FIRMAR_ESCRITURA" => "Firmar Escritura Cliente",
            "VALIDAR_DESEMBOLSO" => "Validar Condiciones Desembolso",
            _ => "Realizar Devolución EP"
        };

        var actividadesCreadas = new List<AssignActivityDTO>();
        List<xpdl_transition_DTO> transitions = await _workflowApplication.GetTransitions(ActividadGestionComercial);
        FolioDTO folio = await _workflowApplication.CapturarDatosFolio(idExpediente, ActividadGestionComercial);

        var transitionId = transitions.FirstOrDefault(x => x.name == transicionSeleccionada)?.transition_id
            ?? throw new InvalidOperationException($"No se encontró la transición '{transicionSeleccionada}' en el workflow.");

        var resultado = await _workflowApplication.AvanzarActividad(transitionId, folio, userId);
        actividadesCreadas.AddRange(resultado);

        _bitacoraApplication.Create(new bitacora
        {
            id_expediente = idExpediente,
            id_actividad = ActividadGestionComercial,
            id_usuario = userId,
            fecha_alta = DateTime.Now,
            observaciones = $"Avance de Gestión Comercial. Cliente NO desiste. Retorno a: [{destinoActividad}]. {(formulario.observaciones != null ? $"Observaciones: {formulario.observaciones}" : "")}",
            is_active = true,
            row_status = true
        }, userId);

        return actividadesCreadas;
    }

    private static void ValidarCamposObligatorios(realizar_gestion_comercial formulario)
    {
        var camposFaltantes = new List<string>();
        if (string.IsNullOrWhiteSpace(formulario.cliente_desiste))
            camposFaltantes.Add("¿Cliente Desiste del Caso?");
        if (camposFaltantes.Count > 0)
            throw new InvalidOperationException($"Campos obligatorios faltantes: {string.Join(", ", camposFaltantes)}");
    }
}
