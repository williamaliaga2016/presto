using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Multibanca;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController: ControllerBase
    {
        private readonly IReportsApplication ReportsApplication;

        public ReportsController(IReportsApplication reportListApplication)
        {
            ReportsApplication = reportListApplication;
        }
        [HttpGet, Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = ReportsApplication.All();

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Reportes obtenidos correctamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    message = ex.Message
                });
            }
        }
    }
}
