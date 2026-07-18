using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Multibanca.Application.Interfaces.Security;
using Multibanca.Common;
using Multibanca.Common.Settings;
using Multibanca.DTO.Multibanca;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Multibanca.Application.Implementations.Security;

/// <summary>
/// Construye JWTs firmados para funcionalidades reutilizables de Multibanca.
/// </summary>
public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration Configuration;
    private readonly AccesoTemporalSettings AccesoTemporalSettings;

    /// <summary>
    /// Inicializa el servicio de emision de tokens JWT.
    /// </summary>
    /// <param name="configuration">Configuracion general usada para firmar JWT.</param>
    /// <param name="accesoTemporalSettings">Configuracion de expiracion del JWT temporal.</param>
    public JwtTokenService(
        IConfiguration configuration,
        IOptions<AccesoTemporalSettings> accesoTemporalSettings)
    {
        Configuration = configuration;
        AccesoTemporalSettings = accesoTemporalSettings.Value;
    }

    /// <summary>
    /// Emite un JWT temporal con claims minimos para navegar como usuario externo.
    /// </summary>
    /// <param name="token">Datos de contexto obtenidos al consumir el token temporal.</param>
    /// <returns>JWT firmado para la sesion temporal.</returns>
    public string BuildTemporalJwt(AccesoTemporalTokenDTO token)
    {
        ValidateTemporalJwtConfiguration();

        string jwtKey = Configuration["Jwt:Key"]!;
        string jwtIssuer = Configuration["Jwt:Issuer"]!;
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Claims limitados al contexto necesario para autorizar la navegacion temporal.
        Claim[] claims = new[]
        {
            new Claim("user_id", token.id_usuario.ToString()),
            new Claim("id_expediente", token.id_expediente.ToString()),
            new Claim("id_actividad", token.id_actividad),
            new Claim(Constants.AccesoTemporal.ClaimToken, token.token.ToString()),
            new Claim(
                Constants.AccesoTemporal.ClaimTipoAcceso,
                Constants.AccesoTemporal.TipoAccesoTemporal),
            new Claim(ClaimTypes.Role, Constants.AccesoTemporal.RolCliente),
            new Claim(ClaimTypes.NameIdentifier, token.id_usuario.ToString())
        };

        var jwt = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtIssuer,
            claims,
            expires: DateTime.UtcNow.AddHours(AccesoTemporalSettings.JwtHorasExpiracion),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    /// <summary>
    /// Valida que exista la configuracion necesaria para emitir JWTs temporales.
    /// </summary>
    public void ValidateTemporalJwtConfiguration()
    {
        if (string.IsNullOrWhiteSpace(Configuration["Jwt:Key"]) ||
            string.IsNullOrWhiteSpace(Configuration["Jwt:Issuer"]))
            throw new InvalidOperationException(
                "La configuracion JWT es obligatoria para emitir acceso temporal.");
    }
}
