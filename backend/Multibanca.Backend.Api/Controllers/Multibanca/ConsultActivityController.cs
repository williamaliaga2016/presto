using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.DTO.Common;
using Multibanca.DTO.Multibanca;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultActivityController : ControllerBase
    {
        private readonly ICommonApplication CommonApplicationProvider;
        private readonly IConsultActivityApplication ConsultActivityApplicationProvider;

        public ConsultActivityController(ICommonApplication _commonApplication, IConsultActivityApplication _consultActivityApplication)
        {
            CommonApplicationProvider = _commonApplication;
            ConsultActivityApplicationProvider = _consultActivityApplication;
        }

        [HttpGet, Route("GetCatalogoTipoBusqueda")]
        public async Task<IActionResult> GetCatalogoTipoBusqueda()
        {
            IActionResult response = Unauthorized();
            try
            {
                List<ControlBaseDTO> listCatalogos = await ConsultActivityApplicationProvider.GetCatalogoTipoBusqueda();

                response = Ok(new
                {
                    status = true,
                    detail = listCatalogos,
                    message = "catalogo cargado correctamente"
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

        [HttpPost, Route("GetConsultActivity")]
        public async Task<IActionResult> GetConsultActivity([FromBody] SearchCriteriaDTO searchCriteria)
        {
            IActionResult response = Unauthorized();
            try
            {
                List<ConsultActivityDTO> listConsultActivityDTO = await ConsultActivityApplicationProvider.GetConsultActivity(searchCriteria);

                response = Ok(new
                {
                    status = true,
                    detail = listConsultActivityDTO,
                    message = "consulta de actividades cargada con éxito"
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

        [HttpGet, Route("GetConsultTrackinActivity/{idExpediente}")]
        public async Task<IActionResult> GetConsultTrackinActivity(long idExpediente)
        {
            IActionResult response = Unauthorized();
            try
            {
                response = Ok(new
                {
                    status = true,
                    detail = await ConsultActivityApplicationProvider.GetConsultTrackinActivity(idExpediente),
                    message = "consulta de actividad con éxito"
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
