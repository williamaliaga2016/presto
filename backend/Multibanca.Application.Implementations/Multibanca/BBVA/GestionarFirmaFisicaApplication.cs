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

namespace Multibanca.Application.Implementations.Multibanca.BBVA;

/// <summary>
/// Orquesta la lógica de negocio para Gestionar Firma Física.
/// Coordina repositorio, workflow, y servicios transversales.
/// </summary>
public class GestionarFirmaFisicaApplication(
    IGestionarFirmaFisicaRepository repository,
    IValidarInformacionRepository validarInformacionRepository,
    IWorkflowApplication workflowApplication,
    ICommonApplication commonApplication,
    IEncabezadoApplication encabezadoApplication,
    IBitacoraApplication bitacoraApplication) : IGestionarFirmaFisicaApplication
{
    public Task<gestionar_firma_fisica_bbva?> GetByExpediente(long idExpediente)
    {
        return repository.GetByExpediente(idExpediente);
    }

    public async Task<gestionar_firma_fisica_bbva> Guardar(gestionar_firma_fisica_bbva request, int userId)
    {
        return await repository.Guardar(request, userId);
    }

    public async Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId)
    {
        var formulario = await repository.GetByExpediente(idExpediente)
            ?? throw new InvalidOperationException("Debe guardar la información antes de avanzar.");

        // Validar campos obligatorios: motorizado_asignado, fecha_gestoria, resultado_gestoria
        ValidarCamposObligatorios(formulario);

        // Obtener transición del workflow
        var transitions = await workflowApplication.GetTransitions(Constants.ActividadesBBVA.FirmaFisica);
        var transitionId = transitions.FirstOrDefault(x => x.name == "TR_018")?.transition_id
            ?? throw new InvalidOperationException("No se encontró la transición 'TR_018' en el workflow XPDL.");

        // Capturar datos del folio y avanzar actividad
        var folio = await workflowApplication.CapturarDatosFolio(
            idExpediente,
            Constants.ActividadesBBVA.FirmaFisica);

        var resultado = await workflowApplication.AvanzarActividad(transitionId, folio, userId);

        // Registrar bitácora
        RegistrarBitacora(idExpediente, userId, formulario);

        return resultado;
    }

    public async Task<object> GetControles(long idExpediente)
    {
        // Retorna catálogo L10 (Resultado de Gestoría)
        return new
        {
            resultado_gestoria = await commonApplication.GetCatalogoByType("L10")
        };
    }

    public async Task<object> GetFormularioConEncabezado(long idExpediente)
    {
        // Obtener formulario actual o crear uno vacío
        var formulario = await repository.GetByExpediente(idExpediente)
            ?? new gestionar_firma_fisica_bbva { id_expediente = idExpediente };

        // Obtener datos heredados de ValidarInformacion
        var heredados = await validarInformacionRepository.GetByExpediente(idExpediente)
            ?? new validar_informacion_bbva { id_expediente = idExpediente };

        // Obtener encabezado del expediente
        var encabezado = await encabezadoApplication.InformacionEncabezado(
            idExpediente,
            Constants.ActividadesBBVA.FirmaFisica);

        return new
        {
            formulario,
            datos_heredados = new
            {
                correo_declarativo = heredados.correo_declarativo,
                telefono_declarativo = heredados.telefono_declarativo,
                tipo_credito = heredados.tipo_credito,
                nombre_cliente = heredados.nombre_completo_t1,
                numero_identificacion = heredados.numero_id_t1,
                tipo_inmueble = heredados.tipo_inmueble,
                constructora = heredados.constructora,
                proyecto = heredados.descripcion_proyecto,
                ciudad_predio = heredados.municipio_inmueble,
                departamento_predio = heredados.departamento_inmueble
            },
            encabezado = new
            {
                scoring = encabezado.id_scoring,
                tipo_subproducto = encabezado.id_tipo_sub_producto,
                monto_otorgado_original = encabezado.monto_otorgado,
                plazo_meses = encabezado.plazo,
                tasa = encabezado.tasa,
                condiciones_organismo_decisor = encabezado.condiciones_organismo_decisor,
                codigo_oficina = encabezado.codigo_oficina_bbva,
                descripcion_oficina = encabezado.descripcion_oficina_bbva,
                codigo_asesor = encabezado.codigo_asesor_bbva,
                correo_declarativo_original = encabezado.correo_declarativo,
                telefono_declarativo_original = encabezado.telefono_declarativo
            }
        };
    }

    private static void ValidarCamposObligatorios(gestionar_firma_fisica_bbva formulario)
    {
        if (string.IsNullOrWhiteSpace(formulario.motorizado_asignado))
            throw new InvalidOperationException("El motorizado asignado es obligatorio para avanzar.");

        if (!formulario.fecha_gestoria.HasValue)
            throw new InvalidOperationException("La fecha de gestoría es obligatoria para avanzar.");

        if (string.IsNullOrWhiteSpace(formulario.resultado_gestoria))
            throw new InvalidOperationException("El resultado de gestoría es obligatorio para avanzar.");
    }

    private void RegistrarBitacora(long idExpediente, int userId, gestionar_firma_fisica_bbva formulario)
    {
        string observaciones = $"Avance de Gestionar Firma Física hacia ACT_VALIDAR_INTEGRACION. " +
            $"Motorizado: {formulario.motorizado_asignado}, " +
            $"Fecha Gestoría: {formulario.fecha_gestoria:yyyy-MM-dd}, " +
            $"Resultado: {formulario.resultado_gestoria}.";

        if (!string.IsNullOrWhiteSpace(formulario.observaciones))
            observaciones += $" Observaciones: {formulario.observaciones}";

        bitacoraApplication.Create(new bitacora
        {
            id_expediente = idExpediente,
            id_actividad = Constants.ActividadesBBVA.FirmaFisica,
            id_usuario = userId,
            fecha_alta = DateTime.Now,
            observaciones = observaciones,
            is_active = true,
            row_status = true
        }, userId);
    }
}
