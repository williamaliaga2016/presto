using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Multibanca.BBVA;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca.BBVA;

[Authorize]
[ApiController]
[Route("api/CartaAprobacionBbva")]
public class CartaAprobacionBbvaController : ControllerBase
{
    private readonly ICartaAprobacionBbvaApplication CartaAprobacionApp;

    public CartaAprobacionBbvaController(ICartaAprobacionBbvaApplication cartaAprobacionApp)
    {
        CartaAprobacionApp = cartaAprobacionApp;
    }

    [HttpGet("{idExpediente}")]
    public async Task<IActionResult> GetByExpediente(long idExpediente)
    {
        try
        {
            var result = await CartaAprobacionApp.GetByExpediente(idExpediente);
            return Ok(new { status = true, detail = result });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { status = false, message = ex.Message });
        }
    }

    [HttpGet("{idExpediente}/historico")]
    public async Task<IActionResult> GetHistorico(long idExpediente)
    {
        try
        {
            var result = await CartaAprobacionApp.GetHistorico(idExpediente);
            return Ok(new { status = true, detail = result });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { status = false, message = ex.Message });
        }
    }

    [HttpGet("{idExpediente}/generar")]
    public async Task<IActionResult> Generar(long idExpediente)
    {
        try
        {
            var idUsuario = GetUserId();
            var (success, message) = await CartaAprobacionApp.Generar(idExpediente, idUsuario);

            if (!success)
                return BadRequest(new { status = false, message });

            return Ok(new { status = true, message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { status = false, message = ex.Message });
        }
    }

    private int GetUserId()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var claims = identity!.Claims.ToList();
        return int.Parse(claims[2].Value);
    }
}
