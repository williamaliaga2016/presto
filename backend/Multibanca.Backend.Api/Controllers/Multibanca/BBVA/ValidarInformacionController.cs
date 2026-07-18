using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Multibanca.BBVA;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca.BBVA;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca.BBVA;

[Authorize]
[ApiController]
[Route("api/validar-informacion")]
public class ValidarInformacionController : ControllerBase
{
    private readonly IValidarInformacionApplication ValidarInformacionAppProvider;

    public ValidarInformacionController(IValidarInformacionApplication validarInformacionApp)
    {
        ValidarInformacionAppProvider = validarInformacionApp;
    }

    [HttpGet("{idExpediente}")]
    public async Task<IActionResult> GetByExpediente(long idExpediente)
    {
        try
        {
            var result = await ValidarInformacionAppProvider.GetByExpediente(idExpediente);
            return Ok(new { status = true, detail = result, message = "Registro consultado." });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpPost("guardar")]
    public async Task<IActionResult> Guardar([FromBody] validar_informacion_bbva request)
    {
        try
        {
            var result = await ValidarInformacionAppProvider.Guardar(request, GetUserId());
            return Ok(new { status = true, detail = result, message = "Registro guardado." });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpPost("{idExpediente}/avanzar")]
    public async Task<IActionResult> Avanzar(long idExpediente)
    {
        try
        {
            var result = await ValidarInformacionAppProvider.Avanzar(
                idExpediente,
                GetUserId(),
                Constants.ActividadesBBVA.ValidarInformacion);
            return Ok(new { status = true, detail = result, message = "Actividad avanzada." });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("{idExpediente}/controles")]
    public async Task<IActionResult> GetControles(long idExpediente)
    {
        try
        {
            var result = await ValidarInformacionAppProvider.GetControles();
            return Ok(new { status = true, detail = result, message = "Controles consultados." });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("{idExpediente}/formulario-encabezado")]
    public async Task<IActionResult> GetFormularioConEncabezado(long idExpediente)
    {
        try
        {
            var result = await ValidarInformacionAppProvider.GetFormularioConEncabezado(idExpediente);
            return Ok(new { status = true, detail = result, message = "Formulario consultado." });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("{idExpediente}/con-encabezado")]
    public Task<IActionResult> GetConEncabezado(long idExpediente)
    {
        return GetFormularioConEncabezado(idExpediente);
    }

    [HttpGet("{idExpediente}/titulares")]
    public async Task<IActionResult> GetTitulares(long idExpediente)
    {
        try
        {
            var result = await ValidarInformacionAppProvider.GetTitulares(idExpediente);
            return Ok(new { status = true, detail = result, message = "Titulares consultados." });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpPost("{idExpediente}/titulares")]
    public async Task<IActionResult> AgregarTitular(long idExpediente, [FromBody] titular_bbva request)
    {
        try
        {
            var result = await ValidarInformacionAppProvider.AgregarTitular(idExpediente, request, GetUserId());
            return Ok(new { status = true, detail = result, message = "Titular registrado." });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    private IActionResult HandleException(Exception ex)
    {
        if (ex.Message.StartsWith("Datos Obligatorios Faltantes:", StringComparison.OrdinalIgnoreCase))
        {
            var campos = ex.Message
                .Replace("Datos Obligatorios Faltantes:", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            return BadRequest(new
            {
                status = false,
                detail = "Datos Obligatorios Faltantes",
                campos_faltantes = campos,
                message = "Datos Obligatorios Faltantes"
            });
        }

        return BadRequest(new { status = false, detail = ex.Message, message = ex.Message });
    }

    private int GetUserId()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userIdClaim = identity?.Claims.FirstOrDefault(q => q.Type == "user_id");

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            throw new InvalidOperationException("No fue posible identificar el usuario autenticado.");

        return userId;
    }
}
