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
using Multibanca.DTO.Common;

namespace Multibanca.Application.Implementations.Multibanca.BBVA;

public class GestionarFirmaApplication(
    IGestionarFirmaRepository repository,
    IValidarInformacionRepository validarInformacionRepository,
    IWorkflowApplication workflowApplication,
    ICommonApplication commonApplication,
    IEncabezadoApplication encabezadoApplication,
    IExpedienteDigitalApplication expedienteDigitalApplication,
    IBitacoraApplication bitacoraApplication) : IGestionarFirmaApplication
{
    private const int CategoriaExpedienteDigitalCa8 = 8;
    private const string AlertaFirmaElectronica = "No puede avanzar el caso como correcto si existen documentos obligatorios rechazados o faltantes en el expediente";

    public Task<gestionar_firma_bbva?> GetByExpediente(long idExpediente)
    {
        return repository.GetByExpediente(idExpediente);
    }

    public async Task<gestionar_firma_bbva> Guardar(gestionar_firma_bbva request, int userId)
    {
  
        return await repository.Guardar(request, userId);
    }

    public async Task<List<AssignActivityDTO>> Avanzar(long idExpediente, int userId)
    {
        var formulario = await repository.GetByExpediente(idExpediente)
            ?? throw new InvalidOperationException("Debe guardar la información antes de avanzar.");

 
        if (!formulario.requiere_firma_electronica.HasValue)
            throw new InvalidOperationException("Debe indicar si requiere firma electrónica.");

        if (formulario.requiere_firma_electronica == true)
        {
           

            if (!formulario.firma_electronica_realizada)
                throw new InvalidOperationException("Debe confirmar que la firma electrónica fue realizada antes de avanzar.");

            if (string.IsNullOrWhiteSpace(formulario.observaciones))
                throw new InvalidOperationException("Las observaciones son obligatorias para avanzar por firma electrónica.");
        }
        else
        {
            ValidarCamposFirmaFisica(formulario);
        }

        string transitionName = formulario.requiere_firma_electronica == true
            ? "TR_016"
            : "TR_015";

        var transitions = await workflowApplication.GetTransitions(Constants.ActividadesBBVA.GestionarFirma);
        var transitionId = transitions.FirstOrDefault(x => x.name == transitionName)?.transition_id
            ?? throw new InvalidOperationException($"No se encontró la transición '{transitionName}' en el workflow XPDL.");

        var folio = await workflowApplication.CapturarDatosFolio(
            idExpediente,
            Constants.ActividadesBBVA.GestionarFirma);

        var resultado = await workflowApplication.AvanzarActividad(transitionId, folio, userId);

        RegistrarBitacora(idExpediente, userId, formulario);

        return resultado;
    }

    public async Task<object> GetControles(long idExpediente)
    {
 
        return new
        {
            tipo_credito = await commonApplication.GetCatalogoByType(Constants.Catalogo.TipoCredito),
            
        };
    }

    public async Task<object> GetFormularioConEncabezado(long idExpediente)
    {
        var formulario = await repository.GetByExpediente(idExpediente)
            ?? new gestionar_firma_bbva { id_expediente = idExpediente };

        var heredados = await validarInformacionRepository.GetByExpediente(idExpediente)
            ?? new validar_informacion_bbva { id_expediente = idExpediente };

        var encabezado = await encabezadoApplication.InformacionEncabezado(
            idExpediente,
            Constants.ActividadesBBVA.GestionarFirma);

 
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

    private static void ValidarCamposFirmaFisica(gestionar_firma_bbva formulario)
    {
        if (//string.IsNullOrWhiteSpace(formulario.nombre_cliente_firma) ||
            //string.IsNullOrWhiteSpace(formulario.nombre_solicitante_firma) ||
            !formulario.franja_horaria.HasValue ||
            string.IsNullOrWhiteSpace(formulario.direccion_firma) ||
            string.IsNullOrWhiteSpace(formulario.descripcion_tramite) ||
            !formulario.fecha_programacion.HasValue ||
            string.IsNullOrWhiteSpace(formulario.ciudad_cliente)
            // || string.IsNullOrWhiteSpace(formulario.tipo_credito_firma)
            )
        {
            throw new InvalidOperationException(
                "Debe completar todos los datos de firma física antes de avanzar.");
        }
    }
 
    private void RegistrarBitacora(long idExpediente, int userId, gestionar_firma_bbva formulario)
    {
        string observaciones = formulario.requiere_firma_electronica == true
            ? "Avance de Gestionar Firma por camino electrónico hacia ACT_VALIDAR_INTEGRACION. Notificación al cliente pendiente de integración."
            : "Avance de Gestionar Firma por camino físico hacia ACT_FIRMA_FISICA. Notificación al Coordinador de Firmas pendiente de integración.";

        if (!string.IsNullOrWhiteSpace(formulario.observaciones))
            observaciones += $" Observaciones: {formulario.observaciones}";

        bitacoraApplication.Create(new bitacora
        {
            id_expediente = idExpediente,
            id_actividad = Constants.ActividadesBBVA.GestionarFirma,
            id_usuario = userId,
            fecha_alta = DateTime.Now,
            observaciones = observaciones,
            is_active = true,
            row_status = true
        }, userId);
    }
}