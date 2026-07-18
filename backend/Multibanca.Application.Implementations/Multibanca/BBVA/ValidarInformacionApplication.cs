using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.BBVA;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Common;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca.BBVA;
using Multibanca.DTO.Multibanca;
using Multibanca.DTO.Multibanca.BBVA;

namespace Multibanca.Application.Implementations.Multibanca.BBVA;

public class ValidarInformacionApplication : IValidarInformacionApplication
{
    private readonly IValidarInformacionRepository ValidarInformacionRepositoryProvider;
    private readonly IWorkflowApplication WorkflowApplicationProvider;
    private readonly ICommonApplication CommonApplicationProvider;
    private readonly IEncabezadoApplication EncabezadoApplicationProvider;
    private readonly IAccesoTemporalApplication AccesoTemporalApplicationProvider;
    private readonly IBitacoraApplication BitacoraApplicationProvider;

    private static readonly Dictionary<string, string> TransicionPorActividadValidar = new(StringComparer.OrdinalIgnoreCase)
    {
        { "ACT_VALIDAR_INFO", "ValidarInformacion_Avanzar" },
        { "BBVA_CONTACTO_VALIDAR_INFORMACION_8DAA7A61", "BBVA_CONTACTO_TR_002" },
        { "BBVA_CONTACTO_VALIDAR_INFORMACION_1BAE684C", "BBVA_CONTACTO_TR_009" },
        { "BBVA_CONTACTO_VALIDAR_INFORMACION_F9EC31C7", "BBVA_CONTACTO_TR_LINK_F9EC31C7_1BAE684C" },
        { "BBVA_CONTACTO_VALIDAR_INFORMACION_3D2B0BD6", "BBVA_CONTACTO_TR_LINK_3D2B0BD6_1BAE684C" },
        { "BBVA_CONTACTO_VALIDAR_INFORMACION_4B9A6D24", "BBVA_CONTACTO_TR_LINK_4B9A6D24_1BAE684C" }
    };

    public ValidarInformacionApplication(
        IValidarInformacionRepository validarInformacionRepository,
        IWorkflowApplication workflowApplication,
        ICommonApplication commonApplication,
        IEncabezadoApplication encabezadoApplication,
        IAccesoTemporalApplication accesoTemporalApplication,
        IBitacoraApplication bitacoraApplication)
    {
        ValidarInformacionRepositoryProvider = validarInformacionRepository;
        WorkflowApplicationProvider = workflowApplication;
        CommonApplicationProvider = commonApplication;
        EncabezadoApplicationProvider = encabezadoApplication;
        AccesoTemporalApplicationProvider = accesoTemporalApplication;
        BitacoraApplicationProvider = bitacoraApplication;
    }

    public async Task<validar_informacion_bbva?> GetByExpediente(long idExpediente)
    {
        return await ValidarInformacionRepositoryProvider.GetByExpediente(idExpediente);
    }

    public async Task<validar_informacion_bbva> Guardar(validar_informacion_bbva request, int userId)
    {
        NormalizarReglasDerivadas(request);

        if (request.monto_otorgado_vi.HasValue)
        {
            var encabezado = await EncabezadoApplicationProvider.InformacionEncabezado(
                request.id_expediente, Constants.ActividadesBBVA.ValidarInformacion);

            request.monto_otorgado_vivienda_original ??= encabezado.monto_otorgado;

            if (request.monto_otorgado_vi > encabezado.monto_otorgado)
                throw new InvalidOperationException("El monto otorgado no puede ser superior al monto original del cargue");
        }

        ValidarObligatoriosCondicionales(request, validarAvance: false);

        return await ValidarInformacionRepositoryProvider.Guardar(request, userId);
    }

    public Task<IReadOnlyList<titular_bbva>> GetTitulares(long idExpediente)
    {
        return ValidarInformacionRepositoryProvider.GetTitulares(idExpediente);
    }

    public Task<titular_bbva> AgregarTitular(long idExpediente, titular_bbva request, int userId)
    {
        request.id_expediente = idExpediente;
        request.id_actividad ??= Constants.ActividadesBBVA.ValidarInformacion;
        ValidarTitular(request);
        return ValidarInformacionRepositoryProvider.AgregarTitular(request, userId);
    }

    public async Task<object> GetFormularioConEncabezado(long idExpediente)
    {
        var encabezado = await EncabezadoApplicationProvider.InformacionEncabezado(
            idExpediente, Constants.ActividadesBBVA.ValidarInformacion);

        var formulario = await ValidarInformacionRepositoryProvider.GetByExpediente(idExpediente);

        if (formulario == null)
        {
            formulario = new validar_informacion_bbva
            {
                id_expediente        = idExpediente,
                correo_declarativo   = encabezado.correo_declarativo,
                telefono_declarativo = encabezado.telefono_declarativo,
                estatus_general      = "SIN_INM"
            };
        }

        var encabezadoDTO = new ValidarInformacionEncabezadoDTO
        {
            scoring                       = encabezado.id_scoring,
            tipo_subproducto              = encabezado.id_tipo_sub_producto,
            monto_otorgado_original       = encabezado.monto_otorgado,
            plazo_meses                   = encabezado.plazo,
            tasa                          = encabezado.tasa,
            condiciones_organismo_decisor = encabezado.condiciones_organismo_decisor,
            codigo_oficina                = encabezado.codigo_oficina_bbva,
            descripcion_oficina           = encabezado.descripcion_oficina_bbva,
            codigo_asesor                 = encabezado.codigo_asesor_bbva,
            correo_declarativo_original   = encabezado.correo_declarativo,
            telefono_declarativo_original = encabezado.telefono_declarativo
        };

        return new { formulario, encabezado = encabezadoDTO };
    }

    /// <summary>
    /// Avanza Validar Informacion y devuelve el link temporal cuando el flujo principal requiere carga documental del cliente.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente que se avanza.</param>
    /// <param name="userId">Usuario autenticado que ejecuta el avance y genera el link temporal.</param>
    /// <param name="actividadId">Actividad funcional de origen para resolver la transicion.</param>
    /// <returns>Resultado del workflow y acceso temporal generado cuando corresponde.</returns>
    //public async Task<ValidarInformacionAvanzarResponseDTO> Avanzar(long idExpediente, int userId, string actividadId)
    //{
    //    var formulario = await ValidarInformacionRepositoryProvider.GetByExpediente(idExpediente);
    //    if (formulario == null)
    //        throw new InvalidOperationException(
    //            $"No existe registro de Validar Información para el expediente {idExpediente}.");

    //    NormalizarReglasDerivadas(formulario);
    //    ValidarObligatoriosCondicionales(formulario, validarAvance: true);

    //    string estatusGeneral = formulario.estatus_general ?? string.Empty;
    //    if (!EstatusPermitidos.Contains(estatusGeneral))
    //        throw new InvalidOperationException(
    //            $"El estatus '{estatusGeneral}' no tiene una transición configurada para avanzar la actividad.");

    //    FolioDTO folio = await CapturarFolioValidarInformacion(idExpediente, actividadId);
    //    string transitionName = ResolverTransicionValidarInformacion(folio.id_actividad);
    //    List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(folio.id_actividad);

    //    string transitionId = listTransitions
    //        .Where(x => x.transition_id == transitionName || x.name == transitionName)
    //        .Select(x => x.transition_id)
    //        .FirstOrDefault() ?? string.Empty;

    //    if (string.IsNullOrEmpty(transitionId))
    //        throw new InvalidOperationException(
    //            $"No se encontró la transición '{transitionName}' en el workflow para la actividad '{folio.id_actividad}'.");

    //    var result = await WorkflowApplicationProvider.AvanzarActividad(transitionId, folio, userId);
    //    RegistrarBitacoraAvance(formulario, userId, transitionName);
    //    AccesoTemporalGenerarResponseDTO? accesoTemporal = null;

    //    // Cargar Documentos Cliente requiere crear el acceso temporal cuando Validar Informacion avanza por el flujo principal.
    //    if (EstatusFlujoPrincipal.Contains(estatusGeneral) && formulario.requiere_carga_cliente == true)
    //    {
    //        accesoTemporal = await AccesoTemporalApplicationProvider.Generar(
    //            new AccesoTemporalGenerarRequestDTO
    //            {
    //                id_expediente = idExpediente,
    //                id_usuario_cliente = userId,
    //                id_actividad = Constants.ActividadesBBVA.DocsCliente
    //            },
    //            userId);
    //    }

    //    return new ValidarInformacionAvanzarResponseDTO
    //    {
    //        workflow = result,
    //        acceso_temporal = accesoTemporal
    //    };
    //}

    public async Task<ValidarInformacionAvanzarResponseDTO> Avanzar(
        long idExpediente,
        int userId,
        string actividadId)
    {
        var formulario = await ValidarInformacionRepositoryProvider.GetByExpediente(idExpediente);

        if (formulario == null)
        {
            throw new InvalidOperationException(
                $"No existe registro de Validar Información para el expediente {idExpediente}.");
        }

        // Aplica reglas automáticas de negocio
        NormalizarReglasDerivadas(formulario);

        // Valida información obligatoria antes de cambiar de actividad
        ValidarObligatoriosCondicionales(formulario, validarAvance: true);


        string estatusGeneral = formulario.estatus_general?.Trim()
            ?? string.Empty;

        // Validamos si ya comenzo la actividad de comenzar firma
        bool yaInicioActividadGestionarFirma = await WorkflowApplicationProvider.ExisteActividadFolio(
            idExpediente,
            Constants.ActividadesBBVA.GestionarFirma);


        string transitionName = estatusGeneral.ToUpperInvariant() switch
        {
            // Cliente no tiene inmueble definido
            "SIN_INM" =>
                "TR_001",

            // Escala devolución pendiente VB Comercial
            "ESCALAR_COMERCIAL" =>
                "TR_002",

            // Flujo normal de avance
            "AVANZAR" when formulario.requiere_carga_constructora == true =>
                "TR_003",

            "AVANZAR" when formulario.requiere_carga_cliente == true =>
                "TR_004",

            // Si ya comenzó Gestionar Firma, continúa por la transición 005
            "AVANZAR" when yaInicioActividadGestionarFirma =>
                "TR_005",

            // Si aún no ha comenzado Gestionar Firma, envía a la 026
            "AVANZAR" =>
                "TR_026",

            _ =>
                throw new InvalidOperationException(
                    $"El estatus general '{estatusGeneral}' no tiene una transición configurada para avanzar la actividad.")
        };


        FolioDTO folio =
            await WorkflowApplicationProvider.CapturarDatosFolio(
                idExpediente,
                actividadId);


        if (folio == null || folio.id <= 0)
        {
            throw new InvalidOperationException(
                $"No se encontró actividad activa de Validar Información para el expediente {idExpediente}.");
        }


        var transitions =
            await WorkflowApplicationProvider.GetTransitions(
                folio.id_actividad);


        string transitionId = transitions
            .Where(x =>
                x.transition_id.Equals(
                    transitionName,
                    StringComparison.OrdinalIgnoreCase)
                ||
                x.name.Equals(
                    transitionName,
                    StringComparison.OrdinalIgnoreCase))
            .Select(x => x.transition_id)
            .FirstOrDefault();


        if (string.IsNullOrWhiteSpace(transitionId))
        {
            throw new InvalidOperationException(
                $"No se encontró la transición '{transitionName}' para la actividad '{folio.id_actividad}'.");
        }


        var resultado =
            await WorkflowApplicationProvider.AvanzarActividad(
                transitionId,
                folio,
                userId);


        RegistrarBitacoraAvance(
            formulario,
            userId,
            transitionName);


        AccesoTemporalGenerarResponseDTO? accesoTemporal = null;


        // Solo cuando el flujo avanza a carga de documentos cliente
        if (estatusGeneral.Equals("AVANZAR", StringComparison.OrdinalIgnoreCase)
            && formulario.requiere_carga_cliente == true)
        {
            accesoTemporal =
                await AccesoTemporalApplicationProvider.Generar(
                    new AccesoTemporalGenerarRequestDTO
                    {
                        id_expediente = idExpediente,
                        id_usuario_cliente = userId,
                        id_actividad = Constants.ActividadesBBVA.DocsCliente
                    },
                    userId);
        }


        return new ValidarInformacionAvanzarResponseDTO
        {
            workflow = resultado,
            acceso_temporal = accesoTemporal
        };
    }

    public async Task<object> GetControles()
    {
        var tipoDocumentoId   = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.TipoDocumentoId);
        var situacionLaboral  = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.SituacionLaboral);
        var tipoInmueble      = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.TipoInmueble);
        var departamento      = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.Departamento);
        var municipio         = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.Municipio);
        var estatusGeneral    = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.EstatusGeneral);
        var motivoDevolucion  = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.MotivoDevolucion);
        var canalContacto     = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.CanalContacto);
        var resultadoContacto = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.ResultadoContacto);
        var detalleContacto   = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DetalleContacto);
        var tipoCredito       = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.TipoCredito);

        return new
        {
            tipo_documento_id  = tipoDocumentoId,
            situacion_laboral  = situacionLaboral,
            tipo_inmueble      = tipoInmueble,
            departamento       = departamento,
            municipio          = municipio,
            estatus_general    = estatusGeneral,
            motivo_devolucion  = motivoDevolucion,
            canal_contacto     = canalContacto,
            resultado_contacto = resultadoContacto,
            detalle_contacto   = detalleContacto,
            tipo_credito       = tipoCredito
        };
    }

    private async Task<FolioDTO> CapturarFolioValidarInformacion(long idExpediente, string actividadId)
    {
        var actividadesCandidatas = new List<string> { actividadId };
        actividadesCandidatas.AddRange(TransicionPorActividadValidar.Keys);

        Exception? lastException = null;
        foreach (string actividadCandidata in actividadesCandidatas.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            try
            {
                FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(idExpediente, actividadCandidata);
                if (folio != null && folio.id > 0)
                {
                    return folio;
                }
            }
            catch (Exception ex)
            {
                lastException = ex;
            }
        }

        throw new InvalidOperationException(
            $"No se encontro una actividad activa de Validar Informacion para el expediente {idExpediente}.",
            lastException);
    }

    private static string ResolverTransicionValidarInformacion(string actividadId)
    {
        if (TransicionPorActividadValidar.TryGetValue(actividadId, out string? transitionId))
        {
            return transitionId;
        }

        throw new InvalidOperationException(
            $"La actividad '{actividadId}' no tiene una transicion de Validar Informacion configurada.");
    }

    private void RegistrarBitacoraAvance(
        validar_informacion_bbva formulario,
        int userId,
        string transitionName)
    {
        var observaciones = new List<string>
        {
            $"Decision: {formulario.estatus_general}",
            $"Transicion: {transitionName}"
        };

        if (!string.IsNullOrWhiteSpace(formulario.motivo_devolucion))
            observaciones.Add($"Motivo Devolucion: {formulario.motivo_devolucion}");

        if (!string.IsNullOrWhiteSpace(formulario.observaciones))
            observaciones.Add($"Observaciones: {formulario.observaciones}");

        var bitacora = new bitacora
        {
            id_expediente = formulario.id_expediente,
            id_actividad = Constants.ActividadesBBVA.ValidarInformacion,
            id_usuario = userId,
            fecha_alta = DateTime.Now,
            observaciones = string.Join(" | ", observaciones),
            is_active = true,
            row_status = true
        };

        BitacoraApplicationProvider.Create(bitacora, userId);
    }

    private static void NormalizarReglasDerivadas(validar_informacion_bbva formulario)
    {
        if (formulario.inmueble_definido == false)
        {
            formulario.estatus_general = "SIN_INMUEBLE_DEFINIDO";
            formulario.requiere_definir_inmueble = true;
            formulario.requiere_carga_cliente = false;
            formulario.requiere_carga_constructora = false;
        }

        formulario.garantia_constituida ??= formulario.tiene_garantia;
    }

    private static void ValidarObligatoriosCondicionales(
        validar_informacion_bbva formulario,
        bool validarAvance)
    {
        if (string.Equals(formulario.tipo_inmueble, "NUEVO", StringComparison.OrdinalIgnoreCase) &&
            (string.IsNullOrWhiteSpace(formulario.constructora) ||
             formulario.fecha_estimada_entrega is null))
        {
            throw new InvalidOperationException("Constructora y Fecha estimada de entrega son obligatorios para inmueble Nuevo");
        }

        if (!validarAvance) return;

        var faltantes = new List<string>();
        AddIfEmpty(faltantes, formulario.tipo_id_t1, "tipo_id_t1");
        AddIfEmpty(faltantes, formulario.numero_id_t1, "numero_id_t1");
        AddIfEmpty(faltantes, formulario.nombre_completo_t1, "nombre_completo_t1");
        AddIfEmpty(faltantes, formulario.celular_t1, "celular_t1");
        AddIfEmpty(faltantes, formulario.telefono_t1, "telefono_t1");
        AddIfEmpty(faltantes, formulario.email_t1, "email_t1");
        AddIfEmpty(faltantes, formulario.direccion_t1, "direccion_t1");
        AddIfEmpty(faltantes, formulario.tipo_credito, "tipo_credito");
        AddIfEmpty(faltantes, formulario.estatus_general, "estatus_general");

        if (formulario.garantia_constituida is null) faltantes.Add("garantia_constituida");
        if (formulario.monto_otorgado_vi is null) faltantes.Add("monto_otorgado_vi");

        if (faltantes.Count > 0)
            throw new InvalidOperationException($"Datos Obligatorios Faltantes: {string.Join(", ", faltantes)}");
    }

    private static void AddIfEmpty(List<string> faltantes, string? value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value)) faltantes.Add(fieldName);
    }

    private static void ValidarTitular(titular_bbva titular)
    {
        var faltantes = new List<string>();
        AddIfEmpty(faltantes, titular.tipo_identificacion, "tipo_identificacion");
        AddIfEmpty(faltantes, titular.numero_identificacion, "numero_identificacion");
        AddIfEmpty(faltantes, titular.nombre_completo, "nombre_completo");
        AddIfEmpty(faltantes, titular.celular_cliente, "celular_cliente");
        AddIfEmpty(faltantes, titular.telefono_residente, "telefono_residente");
        AddIfEmpty(faltantes, titular.email, "email");
        AddIfEmpty(faltantes, titular.direccion_residencia, "direccion_residencia");

        if (faltantes.Count > 0)
            throw new InvalidOperationException($"Datos Obligatorios Faltantes: {string.Join(", ", faltantes)}");
    }
}
