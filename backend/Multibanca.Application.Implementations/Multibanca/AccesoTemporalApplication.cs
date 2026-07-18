using Data.Repository.Interfaces.Repositories.Multibanca;
using Framework.WorkFlow.Application.Interfaces;
using Microsoft.Extensions.Options;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Security;
using Multibanca.Common;
using Multibanca.Common.Settings;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.DTO.Multibanca;

namespace Multibanca.Application.Implementations.Multibanca;

/// <summary>
/// Orquesta la generacion de links temporales y la emision de JWT para usuarios externos.
/// </summary>
public class AccesoTemporalApplication : IAccesoTemporalApplication
{
    private readonly IAccesoTemporalRepository AccesoTemporalRepositoryProvider;
    private readonly AccesoTemporalSettings AccesoTemporalSettings;
    private readonly IJwtTokenService JwtTokenService;
    private readonly ICaseApplication CaseApplicationProvider;

    /// <summary>
    /// Inicializa el servicio de aplicacion de acceso temporal.
    /// </summary>
    /// <param name="accesoTemporalRepository">Repositorio de tokens temporales.</param>
    /// <param name="accesoTemporalSettings">Configuracion de expiracion y URL base.</param>
    /// <param name="jwtTokenService">Servicio reutilizable para emitir JWTs temporales.</param>
    /// <param name="caseApplication">Servicio de workflow usado para confirmar la actividad vigente del caso.</param>
    public AccesoTemporalApplication(
        IAccesoTemporalRepository accesoTemporalRepository,
        IOptions<AccesoTemporalSettings> accesoTemporalSettings,
        IJwtTokenService jwtTokenService,
        ICaseApplication caseApplication)
    {
        AccesoTemporalRepositoryProvider = accesoTemporalRepository;
        AccesoTemporalSettings = accesoTemporalSettings.Value;
        JwtTokenService = jwtTokenService;
        CaseApplicationProvider = caseApplication;
    }

    /// <summary>
    /// Crea un token UUID de un solo uso y construye la URL publica que se enviara al cliente.
    /// </summary>
    /// <param name="request">Expediente, actividad, usuario cliente y expiracion opcional.</param>
    /// <param name="userId">Usuario autenticado que genera el link temporal.</param>
    /// <returns>Token, fecha de expiracion y URL completa para el frontend.</returns>
    public async Task<AccesoTemporalGenerarResponseDTO> Generar(
        AccesoTemporalGenerarRequestDTO request,
        int userId)
    {
        if (request.id_expediente <= 0)
            throw new InvalidOperationException("El expediente es obligatorio.");

        if (request.id_usuario_cliente <= 0)
            throw new InvalidOperationException("El usuario cliente es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.id_actividad))
            throw new InvalidOperationException("La actividad es obligatoria.");

        int horasExpiracion = request.horas_expiracion.GetValueOrDefault(
            AccesoTemporalSettings.HorasExpiracion);

        if (horasExpiracion <= 0)
            throw new InvalidOperationException("Las horas de expiracion deben ser mayores a cero.");

        // El consumo ocurre al completar la accion externa o cuando la actividad temporal deja de estar vigente.
        var token = new token_acceso_temporal
        {
            id_expediente = request.id_expediente,
            id_usuario = request.id_usuario_cliente,
            id_actividad = request.id_actividad,
            fecha_expiracion = DateTime.Now.AddHours(horasExpiracion),
            created_by = userId
        };

        var created = await AccesoTemporalRepositoryProvider.Crear(token);

        return new AccesoTemporalGenerarResponseDTO
        {
            token = created.token,
            fecha_expiracion = created.fecha_expiracion,
            url = BuildAccessUrl(created.token)
        };
    }

    /// <summary>
    /// Valida un token temporal disponible para emitir un JWT con claims minimos de usuario externo.
    /// </summary>
    /// <param name="request">Token temporal recibido desde la ruta publica.</param>
    /// <returns>JWT temporal y ruta protegida a la que debe navegar el frontend.</returns>
    public async Task<AccesoTemporalValidarResponseDTO> Validar(AccesoTemporalValidarRequestDTO request)
    {
        if (!Guid.TryParse(request.token, out Guid token))
            throw new InvalidOperationException(Constants.AccesoTemporal.TokenInvalido);

        // Evita consumir el token si el backend no puede emitir el JWT temporal.
        JwtTokenService.ValidateTemporalJwtConfiguration();

        AccesoTemporalTokenDTO consumed = await AccesoTemporalRepositoryProvider.ObtenerTokenDisponible(token, DateTime.Now);

        if (!await CasoPermaneceEnActividadTemporal(consumed))
        {
            await AccesoTemporalRepositoryProvider.ConsumirToken(
                consumed.token,
                consumed.id_expediente,
                consumed.id_actividad,
                DateTime.Now);

            throw new InvalidOperationException(Constants.AccesoTemporal.TokenUsado);
        }
        
        string jwt = JwtTokenService.BuildTemporalJwt(consumed);

        return new AccesoTemporalValidarResponseDTO
        {
            jwt = jwt,
            id_expediente = consumed.id_expediente,
            id_actividad = consumed.id_actividad,
            url_redirect = BuildRedirectUrl(consumed.id_actividad, consumed.id_expediente)
        };
    }

    /// <summary>
    /// Consume el token temporal solamente despues de que la accion externa fue completada correctamente.
    /// </summary>
    /// <param name="token">UUID autenticado en el JWT temporal.</param>
    /// <param name="idExpediente">Expediente asociado al token.</param>
    /// <param name="idActividad">Actividad asociada al token.</param>
    public async Task ConsumirDespuesDeAccionExitosa(
        Guid token,
        long idExpediente,
        string idActividad)
    {
        await AccesoTemporalRepositoryProvider.ConsumirToken(
            token,
            idExpediente,
            idActividad,
            DateTime.Now);
    }

    /// <summary>
    /// Construye la URL publica que contiene el token UUID.
    /// </summary>
    /// <param name="token">Token UUID que sera enviado al cliente externo.</param>
    /// <returns>URL absoluta de aterrizaje en el frontend.</returns>
    private string BuildAccessUrl(Guid token)
    {
        string baseUrl = AccesoTemporalSettings.FrontendBaseUrl.TrimEnd('/');
        return $"{baseUrl}/acceso-temporal?token={token}";
    }

    /// <summary>
    /// Resuelve la ruta protegida de destino segun la actividad asociada al token.
    /// </summary>
    /// <param name="idActividad">Identificador funcional de la actividad.</param>
    /// <param name="idExpediente">Identificador del expediente asociado al acceso temporal.</param>
    /// <returns>Ruta frontend a la que debe navegar el usuario externo.</returns>
    private string BuildRedirectUrl(string idActividad, long idExpediente)
    {
        return idActividad switch
        {
            Constants.ActividadesBBVA.DocsCliente =>
                $"/home/cargar_documentos_cliente/{idExpediente}",
            Constants.ActividadesBBVA.SoportesPago =>
                $"/home/cargar_soportes_pago/{idExpediente}",
            _ => $"/home/bandeja"
        };
    }

    /// <summary>
    /// Confirma que el caso aun tenga vigente la actividad asociada al link temporal antes de emitir el JWT.
    /// </summary>
    /// <param name="token">Contexto del token temporal disponible.</param>
    /// <returns>True cuando la actividad del token sigue siendo una actividad corriente del caso.</returns>
    private async Task<bool> CasoPermaneceEnActividadTemporal(AccesoTemporalTokenDTO token)
    {
        var workflowCase = await CaseApplicationProvider.GetCase(token.id_expediente);

        return workflowCase?.current_activities?.Any(activity =>
            string.Equals(activity.activity_id, token.id_actividad, StringComparison.OrdinalIgnoreCase)) == true;
    }
}
