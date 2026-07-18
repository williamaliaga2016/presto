using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.BBVA;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca.BBVA;
using Multibanca.DTO.Multibanca.BBVA;

namespace Multibanca.Application.Implementations.Multibanca.BBVA;

/// <summary>
/// Orquesta las reglas de negocio de BBV-44 Definir Inmueble.
/// </summary>
public class DefinirInmuebleApplication(
    IDefinirInmuebleRepository repository,
    IValidarInformacionRepository validarRepository,
    IEncabezadoApplication encabezadoApplication,
    ICommonApplication commonApplication,
    IWorkflowApplication workflowApplication) : IDefinirInmuebleApplication
{
    private const string EstatusSinInmueble = "SIN_INM";

    /// <inheritdoc />
    public async Task<object> GetConEncabezado(long idExpediente)
    {
        var heredados = await validarRepository.GetByExpediente(idExpediente)
            ?? new validar_informacion_bbva { id_expediente = idExpediente };

        var formulario = await repository.GetByExpediente(idExpediente)
            ?? new definir_inmueble_bbva
            {
                id_expediente = idExpediente,
                id_actividad = Constants.ActividadesBBVA.DefinirInmueble,
                cliente_cuenta_inmueble_definido = heredados.inmueble_definido ?? false,
                constructora = heredados.constructora,
                fecha_estimada_entrega = heredados.fecha_estimada_entrega,
                estatus_general = EstatusSinInmueble
            };

        var source = await encabezadoApplication.InformacionEncabezado(
            idExpediente, Constants.ActividadesBBVA.DefinirInmueble);

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

        return new { formulario, datos_heredados = heredados, encabezado };
    }

    /// <inheritdoc />
    public async Task<object> GetControles()
    {
        return new
        {
            tipo_documento_id = await commonApplication.GetCatalogoByType(Constants.Catalogo.TipoDocumentoId),
            situacion_laboral = await commonApplication.GetCatalogoByType(Constants.Catalogo.SituacionLaboral),
            estatus_general = await commonApplication.GetCatalogoByType(Constants.Catalogo.EstatusGeneral),
            motivo_devolucion = await commonApplication.GetCatalogoByType(Constants.Catalogo.MotivoDevolucion),
            tipo_credito = await commonApplication.GetCatalogoByType(Constants.Catalogo.TipoCredito)
        };
    }

    /// <inheritdoc />
    public async Task<definir_inmueble_bbva> Guardar(definir_inmueble_bbva request, int userId)
    {
        Normalizar(request);
        ValidarDatosInmueble(request);
        return await repository.Guardar(request, userId);
    }

    /// <inheritdoc />
    public async Task<DefinirInmuebleAvanzarResponseDTO> Avanzar(
        long idExpediente,
        int userId,
        bool confirmar)
    {
        var form = await repository.GetByExpediente(idExpediente)
            ?? throw new InvalidOperationException("Debe guardar la informacion antes de avanzar.");

        ValidarDatosInmueble(form);

        if (string.IsNullOrWhiteSpace(form.estatus_general))
            throw new InvalidOperationException("Debe seleccionar el estatus general antes de avanzar.");

        if (EsSinInmueble(form) && !confirmar)
        {
            return new DefinirInmuebleAvanzarResponseDTO
            {
                requiere_confirmacion = true,
                mensaje = "El Inmueble aun no se encuentra definido, estas seguro de avanzar?"
            };
        }

        var transitions = await workflowApplication.GetTransitions(Constants.ActividadesBBVA.DefinirInmueble);
        var transitionId = transitions
            .FirstOrDefault(x => x.name == Constants.Transiciones.DefinirInmuebleAvanzar)
            ?.transition_id;

        if (string.IsNullOrWhiteSpace(transitionId))
            throw new InvalidOperationException(
                $"No se encontro la transicion '{Constants.Transiciones.DefinirInmuebleAvanzar}' en el workflow XPDL.");

        var folio = await workflowApplication.CapturarDatosFolio(
            idExpediente, Constants.ActividadesBBVA.DefinirInmueble);
        var workflow = await workflowApplication.AvanzarActividad(transitionId, folio, userId);

        return new DefinirInmuebleAvanzarResponseDTO
        {
            requiere_confirmacion = false,
            mensaje = "Actividad avanzada.",
            workflow = workflow
        };
    }

    private static void Normalizar(definir_inmueble_bbva request)
    {
        request.id_actividad = Constants.ActividadesBBVA.DefinirInmueble;

        if (request.cliente_cuenta_inmueble_definido == true &&
            string.Equals(request.estatus_general, EstatusSinInmueble, StringComparison.OrdinalIgnoreCase))
        {
            request.estatus_general = "LISTO";
        }
    }

    private static void ValidarDatosInmueble(definir_inmueble_bbva request)
    {
        if (request.cliente_cuenta_inmueble_definido != true)
            return;

        if (string.IsNullOrWhiteSpace(request.constructora))
            throw new InvalidOperationException("La constructora es obligatoria cuando el inmueble esta definido.");

        if (!request.fecha_estimada_entrega.HasValue)
            throw new InvalidOperationException(
                "La fecha estimada de entrega es obligatoria cuando el inmueble esta definido.");
    }

    private static bool EsSinInmueble(definir_inmueble_bbva form)
    {
        return form.cliente_cuenta_inmueble_definido != true ||
            string.Equals(form.estatus_general, EstatusSinInmueble, StringComparison.OrdinalIgnoreCase);
    }
}
