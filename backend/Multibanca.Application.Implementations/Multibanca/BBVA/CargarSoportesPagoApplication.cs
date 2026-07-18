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

namespace Multibanca.Application.Implementations.Multibanca.BBVA;

/// <summary>
/// Implementa las reglas de negocio de Cargar Soportes de Pago y su integracion con encabezado y workflow.
/// </summary>
public class CargarSoportesPagoApplication : ICargarSoportesPagoApplication
{
    private readonly ICargarSoportesPagoRepository CargarSoportesPagoRepositoryProvider;
    private readonly IEncabezadoApplication EncabezadoApplicationProvider;
    private readonly IWorkflowApplication WorkflowApplicationProvider;
    private readonly IMapper Mapper;

    private const string TransitionName = Constants.Transiciones.SoportesPagoAvanzar;

    /// <summary>
    /// Inicializa el servicio de aplicacion de Cargar Soportes de Pago.
    /// </summary>
    /// <param name="cargarSoportesPagoRepository">Repositorio de confirmacion de soportes de pago.</param>
    /// <param name="encabezadoApplication">Servicio de encabezado para obtener datos generales del expediente.</param>
    /// <param name="workflowApplication">Servicio de workflow para capturar folio y avanzar actividad.</param>
    /// <param name="mapper">Mapper para transformar modelos de dominio y entidades de repositorio.</param>
    public CargarSoportesPagoApplication(
        ICargarSoportesPagoRepository cargarSoportesPagoRepository,
        IEncabezadoApplication encabezadoApplication,
        IWorkflowApplication workflowApplication,
        IMapper mapper)
    {
        CargarSoportesPagoRepositoryProvider = cargarSoportesPagoRepository;
        EncabezadoApplicationProvider = encabezadoApplication;
        WorkflowApplicationProvider = workflowApplication;
        Mapper = mapper;
    }

    /// <summary>
    /// Obtiene el registro activo de Cargar Soportes de Pago para un expediente.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente en Presto.</param>
    /// <returns>Registro activo de Cargar Soportes de Pago o `null` si no existe.</returns>
    public async Task<cargar_soportes_pago?> GetByExpediente(long idExpediente)
    {
        cargar_soportes_pago_entity? registro = await CargarSoportesPagoRepositoryProvider.GetByExpediente(idExpediente);
        return Mapper.Map<cargar_soportes_pago?>(registro);
    }

    /// <summary>
    /// Guarda los datos de confirmacion de soportes de pago sin avanzar el workflow.
    /// </summary>
    /// <param name="request">Datos capturados en la actividad Cargar Soportes de Pago.</param>
    /// <param name="userId">Usuario autenticado que guarda el registro.</param>
    /// <returns>Registro creado o actualizado.</returns>
    public async Task<cargar_soportes_pago> Guardar(
        cargar_soportes_pago request,
        int userId)
    {
        request.id_actividad = Constants.ActividadesBBVA.SoportesPago;
        cargar_soportes_pago_entity entity = Mapper.Map<cargar_soportes_pago_entity>(request);

        cargar_soportes_pago_entity saved = await CargarSoportesPagoRepositoryProvider.Guardar(entity, userId);
        return Mapper.Map<cargar_soportes_pago>(saved);
    }

    /// <summary>
    /// Obtiene informacion general de cliente y analista para la pantalla externa.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente en Presto.</param>
    /// <returns>Informacion general para la pantalla de Cargar Soportes de Pago.</returns>
    public async Task<CargarSoportesPagoInfoDTO> GetInfoCliente(long idExpediente)
    {
        EncabezadoDTO encabezado = await EncabezadoApplicationProvider.InformacionEncabezado(
            idExpediente, Constants.ActividadesBBVA.SoportesPago);

        return new CargarSoportesPagoInfoDTO
        {
            id_expediente = idExpediente,
            nombre_completo_t1 = encabezado.nombre_completo_t1,
            nombre_cliente = encabezado.nombre_completo_t1,
            nombre_analista = encabezado.usuario_asignado
        };
    }

    /// <summary>
    /// Valida que exista confirmacion de soportes de pago y avanza la actividad.
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
        cargar_soportes_pago_entity? registro = await CargarSoportesPagoRepositoryProvider.GetByExpediente(idExpediente);
        if (registro == null || !registro.documentos_adjuntos)
            throw new InvalidOperationException("Debe confirmar que adjunto los soportes de pago antes de avanzar.");

        FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(idExpediente, actividadId);
        List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividadId);

        string transitionId = listTransitions
            .Where(x => x.name == TransitionName)
            .Select(x => x.transition_id)
            .FirstOrDefault() ?? string.Empty;

        if (string.IsNullOrEmpty(transitionId))
            throw new InvalidOperationException(
                $"No se encontro la transicion '{TransitionName}' en el workflow para la actividad '{actividadId}'.");

        return await WorkflowApplicationProvider.AvanzarActividad(transitionId, folio, userId);
    }

}
