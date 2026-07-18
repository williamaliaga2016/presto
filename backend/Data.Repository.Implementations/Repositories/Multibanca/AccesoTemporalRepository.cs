using Data.Repository.Interfaces.Repositories.Multibanca;
using Microsoft.EntityFrameworkCore;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.DTO.Multibanca;

namespace Data.Repository.Implementations.Repositories.Multibanca;

/// <summary>
/// Administra la persistencia y consumo atomico de tokens de acceso temporal.
/// </summary>
public class AccesoTemporalRepository : IAccesoTemporalRepository
{
    private readonly MultibancaDBContext MultibancaDBContext;

    /// <summary>
    /// Inicializa el repositorio de acceso temporal.
    /// </summary>
    /// <param name="multibancaDBContext">Contexto EF Core de Multibanca.</param>
    public AccesoTemporalRepository(MultibancaDBContext multibancaDBContext)
    {
        MultibancaDBContext = multibancaDBContext;
    }

    /// <summary>
    /// Persiste un token temporal activo y disponible para un unico consumo posterior.
    /// </summary>
    /// <param name="request">Entidad de token temporal a crear.</param>
    /// <returns>Entidad persistida con UUID, estado activo y fecha de creacion.</returns>
    public async Task<token_acceso_temporal> Crear(token_acceso_temporal request)
    {
        request.token = request.token == Guid.Empty ? Guid.NewGuid() : request.token;
        request.usado = false;
        request.is_active = true;
        request.created_date = request.created_date == default ? DateTime.Now : request.created_date;

        MultibancaDBContext.TokenAccesoTemporal.Add(request);
        await MultibancaDBContext.SaveChangesAsync();
        return request;
    }

    /// <summary>
    /// Obtiene datos de contexto del token si esta activo, no fue usado y no expiro.
    /// </summary>
    /// <param name="token">UUID recibido desde el link temporal.</param>
    /// <param name="fechaActual">Fecha de referencia para validar expiracion.</param>
    /// <returns>Datos minimos requeridos para construir el JWT temporal.</returns>
    public async Task<AccesoTemporalTokenDTO> ObtenerTokenDisponible(Guid token, DateTime fechaActual)
    {
        var available = await MultibancaDBContext.TokenAccesoTemporal
            .AsNoTracking()
            .Where(q => q.token == token
                && q.is_active
                && !q.usado
                && q.fecha_expiracion > fechaActual)
            .Select(q => new AccesoTemporalTokenDTO
            {
                token = q.token,
                id_usuario = q.id_usuario,
                id_expediente = q.id_expediente,
                id_actividad = q.id_actividad
            })
            .FirstOrDefaultAsync();

        if (available != null)
            return available;

        await ThrowTokenValidationError(token, fechaActual);
        throw new InvalidOperationException("No fue posible validar el token de acceso temporal.");
    }

    /// <summary>
    /// Marca el token como usado solo si coincide con el expediente y actividad autenticados.
    /// </summary>
    /// <param name="token">UUID autenticado en el JWT temporal.</param>
    /// <param name="idExpediente">Expediente asociado al token temporal.</param>
    /// <param name="idActividad">Actividad asociada al token temporal.</param>
    /// <param name="fechaActual">Fecha de referencia para validar expiracion y registrar uso.</param>
    public async Task ConsumirToken(
        Guid token,
        long idExpediente,
        string idActividad,
        DateTime fechaActual)
    {
        // ExecuteUpdate aplica el filtro y el cambio en una sola sentencia para evitar doble consumo concurrente.
        var updated = await MultibancaDBContext.TokenAccesoTemporal
            .Where(q => q.token == token
                && q.id_expediente == idExpediente
                && q.id_actividad == idActividad
                && q.is_active
                && !q.usado
                && q.fecha_expiracion > fechaActual)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(q => q.usado, true)
                .SetProperty(q => q.fecha_uso, fechaActual));

        if (updated == 1)
            return;

        await ThrowTokenValidationError(token, fechaActual);
        throw new InvalidOperationException("No fue posible consumir el token de acceso temporal.");
    }

    /// <summary>
    /// Determina el motivo funcional por el cual un token no pudo consumirse.
    /// </summary>
    /// <param name="token">UUID del token evaluado.</param>
    /// <param name="fechaActual">Fecha usada para distinguir tokens expirados.</param>
    private async Task ThrowTokenValidationError(Guid token, DateTime fechaActual)
    {
        // Separar el diagnostico conserva el update atomico y permite mensajes funcionales precisos.
        var existing = await MultibancaDBContext.TokenAccesoTemporal
            .AsNoTracking()
            .FirstOrDefaultAsync(q => q.token == token);

        if (existing == null || !existing.is_active)
            throw new InvalidOperationException(Constants.AccesoTemporal.TokenInvalido);

        if (existing.usado)
            throw new InvalidOperationException(Constants.AccesoTemporal.TokenUsado);

        if (existing.fecha_expiracion <= fechaActual)
            throw new InvalidOperationException(Constants.AccesoTemporal.TokenExpirado);
    }
}
