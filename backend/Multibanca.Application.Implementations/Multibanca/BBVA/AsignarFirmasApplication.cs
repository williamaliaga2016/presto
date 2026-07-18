using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Framework.WorkFlow.Common.DTO;
using System.Transactions;
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

public class AsignarFirmasApplication(
    IAsignarFirmasRepository repository,
    IValidarInformacionRepository validarRepository,
    IEncabezadoApplication encabezadoApplication,
    ICommonApplication commonApplication,
    IWorkflowApplication workflowApplication,
    IActividadesApplication actividadesApplication,
    IBitacoraApplication bitacoraApplication,
    IAsignadorFirmasService asignadorService,
    IAccesoTemporalApplication accesoTemporalApplication) : IAsignarFirmasApplication
{
    public async Task<object> GetConEncabezado(long idExpediente)
    {
        var formulario = await repository.GetByExpediente(idExpediente);
        var heredados = await validarRepository.GetByExpediente(idExpediente)
            ?? new validar_informacion_bbva { id_expediente = idExpediente };
        var source = await encabezadoApplication.InformacionEncabezado(
            idExpediente, Constants.ActividadesBBVA.AsignarFirmas);

        var encabezado = new
        {
            scoring = source.id_scoring,
            id_tipo_sub_producto = source.id_tipo_sub_producto,
            monto_otorgado_original = source.monto_otorgado,
            plazo_meses = source.plazo,
            tasa = source.tasa,
            condiciones_organismo_decisor = source.condiciones_organismo_decisor,
            codigo_oficina = source.codigo_oficina_bbva,
            descripcion_oficina = source.descripcion_oficina_bbva,
            codigo_asesor = source.codigo_asesor_bbva,
            correo_declarativo_original = source.correo_declarativo,
            telefono_declarativo_original = source.telefono_declarativo
        };
        var datosFolio = new
        {
            fecha_asignacion = DateTime.Now,
            tipo_inmueble = heredados.tipo_inmueble,
            constructora = heredados.constructora,
            proyecto = heredados.descripcion_proyecto,
            identificacion_cliente = heredados.numero_id_t1,
            nombre_cliente = heredados.nombre_completo_t1,
            departamento_predio = heredados.departamento_inmueble,
            ciudad_predio = heredados.municipio_inmueble,
            direccion_predio = (string?)null,
            ubicacion_predio = heredados.estado_inmueble,
            valor_comercial_predio = (decimal?)null,
            usuario_solicitante = source.codigo_asesor_bbva
        };
        return new { formulario, datos_heredados = heredados, encabezado, datos_folio = datosFolio };
    }

    public Task<asignar_firmas_peritos_abogados> Guardar(
        asignar_firmas_peritos_abogados request, int userId) => repository.Guardar(request, userId);

    public Task<object> Calcular(CalcularAsignacionRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.tipo_cliente) ||
            string.IsNullOrWhiteSpace(request.codigo_ejecutivo) ||
            string.IsNullOrWhiteSpace(request.oficina) ||
            string.IsNullOrWhiteSpace(request.tipo_solicitud_avaluo) ||
            string.IsNullOrWhiteSpace(request.tipo_tramite_eett))
            throw new InvalidOperationException("Los datos requeridos para calcular la asignacion estan incompletos.");
        return asignadorService.Calcular(request);
    }

    public async Task<AsignarFirmasAvanzarResponseDTO> Avanzar(long idExpediente, int userId)
    {
        var form = await repository.GetByExpediente(idExpediente)
            ?? throw new InvalidOperationException("Debe guardar la informacion antes de avanzar.");
        if (string.IsNullOrWhiteSpace(form.nombre_firma_supervisor) || string.IsNullOrWhiteSpace(form.nombre_abogado))
            throw new InvalidOperationException("Debe calcular la asignacion antes de avanzar.");
        if (!form.requiere_envio_notificacion.HasValue)
            throw new InvalidOperationException("Debe indicar si requiere envio de notificacion.");
        if (form.requiere_envio_notificacion.Value &&
            (string.IsNullOrWhiteSpace(form.checklist_documentos_solicitar) ||
             form.checklist_documentos_solicitar.Trim() == "[]"))
            throw new InvalidOperationException("Debe seleccionar al menos un documento para solicitar.");
        if (string.IsNullOrWhiteSpace(form.observaciones))
            throw new InvalidOperationException("Las observaciones son obligatorias.");

        var conNotificacion = form.requiere_envio_notificacion.Value;
        var transitionName = conNotificacion ? "TR_014" : "TR_013";
        var transitions = await workflowApplication.GetTransitions(Constants.ActividadesBBVA.AsignarFirmas);
        var transitionId = transitions.FirstOrDefault(x => x.name == transitionName)?.transition_id
            ?? throw new InvalidOperationException($"No se encontro la transicion '{transitionName}' en el workflow XPDL.");
        var folio = await workflowApplication.CapturarDatosFolio(
            idExpediente, Constants.ActividadesBBVA.AsignarFirmas);

        var transactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.DefaultTimeout
        };

        AccesoTemporalGenerarResponseDTO? accesoTemporal = null;

        if (conNotificacion)
        {
            accesoTemporal = await accesoTemporalApplication.Generar(
                new AccesoTemporalGenerarRequestDTO
                {
                    id_expediente = idExpediente,
                    id_usuario_cliente = userId,
                    id_actividad = Constants.ActividadesBBVA.SoportesPago
                },
                userId);
        }

        var actividadesAsignadas = await workflowApplication.AvanzarActividad(transitionId, folio, userId);

        await ValidarActividadesCreadas(idExpediente, conNotificacion);
        RegistrarBitacora(idExpediente, userId, conNotificacion, accesoTemporal);

        return new AsignarFirmasAvanzarResponseDTO
        {
            workflow = actividadesAsignadas,
            acceso_temporal = accesoTemporal
        };
    }

    private async Task ValidarActividadesCreadas(long idExpediente, bool conNotificacion)
    {
        var existeGestionarFirma = await actividadesApplication.ExisteActividadActiva(
            idExpediente,
            Constants.ActividadesBBVA.GestionarFirma);

        if (!existeGestionarFirma)
            throw new InvalidOperationException(
                "El workflow avanzo, pero no dejo activa la actividad ACT_GESTIONAR_FIRMA. Revise la transicion XPDL de Asignar Firmas.");

        if (!conNotificacion)
            return;

        var existeSoportesPago = await actividadesApplication.ExisteActividadActiva(
            idExpediente,
            Constants.ActividadesBBVA.SoportesPago);

        if (!existeSoportesPago)
            throw new InvalidOperationException(
                "El workflow avanzo con notificacion, pero no dejo activa la actividad ACT_SOPORTES_PAGO. Revise la bifurcacion paralela XPDL.");
    }

    private void RegistrarBitacora(
        long idExpediente,
        int userId,
        bool conNotificacion,
        AccesoTemporalGenerarResponseDTO? accesoTemporal)
    {
        var actividadesDestino = conNotificacion
            ? $"{Constants.ActividadesBBVA.SoportesPago} + {Constants.ActividadesBBVA.GestionarFirma}"
            : Constants.ActividadesBBVA.GestionarFirma;

        var observaciones =
            $"Avance de Asignar Firmas, Peritos y Abogados. " +
            $"Requiere notificacion: {(conNotificacion ? "Si" : "No")}. " +
            $"Actividades destino: {actividadesDestino}.";

        if (accesoTemporal is not null)
            observaciones += $" Token temporal generado con vencimiento {accesoTemporal.fecha_expiracion:yyyy-MM-dd HH:mm}.";

        bitacoraApplication.Create(new bitacora
        {
            id_expediente = idExpediente,
            id_actividad = Constants.ActividadesBBVA.AsignarFirmas,
            id_usuario = userId,
            fecha_alta = DateTime.Now,
            observaciones = observaciones,
            is_active = true,
            row_status = true
        }, userId);
    }

    public async Task<object> GetControles()
    {
        return new
        {
            tipo_documento_id = await commonApplication.GetCatalogoByType(Constants.Catalogo.TipoDocumentoId),
            situacion_laboral = await commonApplication.GetCatalogoByType(Constants.Catalogo.SituacionLaboral),
            tipo_inmueble = await commonApplication.GetCatalogoByType(Constants.Catalogo.TipoInmueble),
            departamento = await commonApplication.GetCatalogoByType(Constants.Catalogo.Departamento),
            municipio = await commonApplication.GetCatalogoByType(Constants.Catalogo.Municipio),
            estatus_general = await commonApplication.GetCatalogoByType(Constants.Catalogo.EstatusGeneral),
            motivo_devolucion = await commonApplication.GetCatalogoByType(Constants.Catalogo.MotivoDevolucion),
            tipo_credito = await commonApplication.GetCatalogoByType(Constants.Catalogo.TipoCredito),
            documentos_solicitar = new[]
            {
                new { id = "SOPORTE_AVALUO", nombre = "Soporte de pago del avaluo" },
                new { id = "SOPORTE_ESTUDIO_TITULOS", nombre = "Soporte de pago del estudio de titulos" },
                new { id = "DOCUMENTO_IDENTIDAD", nombre = "Documento de identidad" }
            }
        };
    }
}
