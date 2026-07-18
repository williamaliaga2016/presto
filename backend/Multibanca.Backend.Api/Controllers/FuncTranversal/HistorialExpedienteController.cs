using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.DTO.FuncTransversal;

namespace Multibanca.Backend.Api.Controllers.FuncTranversal
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialExpedienteController : ControllerBase
    {
        private readonly IHistorialExpedienteApplication HistorialExpedienteApplicationProvider;

        public HistorialExpedienteController(IHistorialExpedienteApplication _historialExpedienteProvider)
        {
            HistorialExpedienteApplicationProvider = _historialExpedienteProvider;
        }

        [HttpGet, Route("GetByIdExpediente/{idExpediente}")]
        public async Task<IActionResult> ObtenerHistorial(long idExpediente)
        {
            IActionResult response = Unauthorized();
            try
            {
                List<HistorialExpedienteDTO> result = await HistorialExpedienteApplicationProvider.CargaInicialHistorial(idExpediente); //idExpediente para pruebas

                if (result != null)
                {
                    response = Ok(new
                    {
                        status = true,
                        detail = result,
                        message = "Historial obtenido correctamente."
                    });
                }
                else
                {
                    response = Ok(new
                    {
                        status = false,
                        detail = new List<HistorialExpedienteDTO>(),
                        message = "No existen registros."
                    });
                }
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
