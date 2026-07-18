using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.DTO.Multibanca;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncabezadoController : ControllerBase
    {
        private readonly IEncabezadoApplication EncabezadoApplicationProvider;

        public EncabezadoController(IEncabezadoApplication _encabezadoApplicationProvider)
        {
            EncabezadoApplicationProvider = _encabezadoApplicationProvider;
        }

        [HttpGet, Route("infoEncabezado/{idExpediente}")]
        public async Task<IActionResult> InfoEncabezado(long idExpediente, [FromQuery] string? activityID)
        {
            IActionResult response = Unauthorized();

            try
            {
                EncabezadoDTO result = await EncabezadoApplicationProvider.InformacionEncabezado(idExpediente, activityID);

                if (result == null) return BadRequest("Invalid client request");

                response = Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Información del Encabezado obtenido correctamente"
                });
            }
            catch (Exception ex)
            {
                response = StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    message = ex.Message
                });
            }

            return response;
        }
    }
}
