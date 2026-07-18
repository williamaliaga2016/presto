using AutoMapper;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.BBVA;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca.BBVA;
using Multibanca.DTO.Multibanca;
using Multibanca.DTO.Multibanca.BBVA;
using Syncfusion.DocIO.DLS;

namespace Multibanca.Application.Implementations.Multibanca.BBVA;

/// <summary>
/// Implementa las reglas de negocio de Cargar Documentos Cliente y su integracion con encabezado y workflow.
/// </summary>
public class CargarDocumentosClienteApplication : ICargarDocumentosClienteApplication
{
    private readonly ICargarDocumentosClienteRepository CargarDocumentosClienteRepositoryProvider;
    private readonly IEncabezadoApplication EncabezadoApplicationProvider;
    private readonly IWorkflowApplication WorkflowApplicationProvider;
    private readonly IMapper Mapper;

    private const string TransitionName = Constants.Transiciones.DocsClienteAvanzar;

    /// <summary>
    /// Inicializa el servicio de aplicacion de Cargar Documentos Cliente.
    /// </summary>
    /// <param name="cargarDocumentosClienteRepository">Repositorio de confirmacion documental.</param>
    /// <param name="encabezadoApplication">Servicio de encabezado para obtener datos generales del expediente.</param>
    /// <param name="workflowApplication">Servicio de workflow para capturar folio y avanzar actividad.</param>
    /// <param name="mapper">Mapper para transformar modelos de dominio y entidades de repositorio.</param>
    public CargarDocumentosClienteApplication(
        ICargarDocumentosClienteRepository cargarDocumentosClienteRepository,
        IEncabezadoApplication encabezadoApplication,
        IWorkflowApplication workflowApplication,
        IMapper mapper)
    {
        CargarDocumentosClienteRepositoryProvider = cargarDocumentosClienteRepository;
        EncabezadoApplicationProvider = encabezadoApplication;
        WorkflowApplicationProvider = workflowApplication;
        Mapper = mapper;
    }

    /// <summary>
    /// Obtiene el registro activo de Cargar Documentos Cliente para un expediente.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente en Presto.</param>
    /// <returns>Registro activo de Cargar Documentos Cliente o `null` si no existe.</returns>
    public async Task<cargar_documentos_cliente?> GetByExpediente(long idExpediente)
    {
        cargar_documentos_cliente_entity? registro = await CargarDocumentosClienteRepositoryProvider.GetByExpediente(idExpediente);
        return Mapper.Map<cargar_documentos_cliente?>(registro);
    }

    /// <summary>
    /// Guarda los datos de confirmacion documental sin avanzar el workflow.
    /// </summary>
    /// <param name="request">Datos capturados en la actividad Cargar Documentos Cliente.</param>
    /// <param name="userId">Usuario autenticado que guarda el registro.</param>
    /// <returns>Registro creado o actualizado.</returns>
    public async Task<cargar_documentos_cliente> Guardar(
        cargar_documentos_cliente request,
        int userId)
    {
        request.id_actividad = Constants.ActividadesBBVA.DocsCliente;
        cargar_documentos_cliente_entity entity = Mapper.Map<cargar_documentos_cliente_entity>(request);

        cargar_documentos_cliente_entity saved = await CargarDocumentosClienteRepositoryProvider.Guardar(entity, userId);
        return Mapper.Map<cargar_documentos_cliente>(saved);
    }

    /// <summary>
    /// Obtiene informacion general de cliente y analista para presentacion enmascarada.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente en Presto.</param>
    /// <returns>Informacion general para la pantalla externa de Cargar Documentos Cliente.</returns>
    public async Task<CargarDocumentosClienteInfoDTO> GetInfoCliente(long idExpediente)
    {
        EncabezadoDTO encabezado = await EncabezadoApplicationProvider.InformacionEncabezado(
            idExpediente, Constants.ActividadesBBVA.DocsCliente);

        return new CargarDocumentosClienteInfoDTO
        {
            id_expediente = idExpediente,
            nombre_completo_t1 = encabezado.nombre_completo_t1,
            nombre_cliente = encabezado.nombre_completo_t1,
            correo_declarativo = encabezado.correo_declarativo,
            telefono_declarativo = encabezado.telefono_declarativo,
            nombre_analista = encabezado.usuario_asignado
        };
    }

    /// <summary>
    /// Valida que exista confirmacion documental y avanza Cargar Documentos Cliente hacia revision documental.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente en Presto.</param>
    /// <param name="userId">Usuario autenticado que ejecuta el avance.</param>
    /// <param name="actividadId">Identificador de actividad workflow actual.</param>
    /// <returns>Actividades asignadas por el motor de workflow despues del avance.</returns>
    public async Task<List<AssignActivityDTO>> Avanzar(
        long idExpediente,
        int userId,
        string actividadId)
    {
        cargar_documentos_cliente_entity? registro = await CargarDocumentosClienteRepositoryProvider.GetByExpediente(idExpediente);
        if (registro == null || !registro.documentos_adjuntos)
            throw new InvalidOperationException("Debe confirmar que adjunto los documentos antes de avanzar.");

        FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(idExpediente, actividadId);
        List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividadId);

        // Validamos si ya comenzo la actividad de comenzar firma
        bool yaInicioActividadGestionarFirma = await WorkflowApplicationProvider.ExisteActividadFolio(
            idExpediente,
            Constants.ActividadesBBVA.GestionarFirma);

        // Validamos si ya realizo la firma, si es asi, va a la actividad
        string nombreTransicionEsperada = yaInicioActividadGestionarFirma == true
                ? "TR_010"
                : "TR_009";

        string transitionId = listTransitions
            .Where(x => x.name == nombreTransicionEsperada)
            .Select(x => x.transition_id)
            .FirstOrDefault() ?? string.Empty;

        if (string.IsNullOrEmpty(transitionId))
            throw new InvalidOperationException(
                $"No se encontro la transicion '{nombreTransicionEsperada}' en el workflow para la actividad '{actividadId}'.");

        return await WorkflowApplicationProvider.AvanzarActividad(transitionId, folio, userId);
    }

}
