
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interface.Utilidades;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Common;
using Multibanca.DTO.Common;
using Multibanca.DTO.Utilidades;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilidadesController : ControllerBase
    {
        private readonly IUtilidadesApplication utilidadesApplicationProvider;
        private readonly ICommonApplication CommonApplicationProvider;

        public UtilidadesController(IUtilidadesApplication utilidadesApplication, ICommonApplication _commonApplication)
        {
            utilidadesApplicationProvider = utilidadesApplication;
            CommonApplicationProvider = _commonApplication;
        }

        [HttpPost, Route("GetActivities")]
        public async Task<IActionResult> GetActivities([FromBody] UtilidadesDTO model)
        {
            try
            {
                var listGetActivities = await utilidadesApplicationProvider.GetActivities(model.id_expediente);

                return Ok(new
                {
                    status = true,
                    detail = listGetActivities
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

        [HttpGet, Route("ValidateRequestNumber/{idExpediente}")]
        public async Task<IActionResult> ValidateRequestNumber(long idExpediente)
        {
            try
            {
                var requestNumber = await utilidadesApplicationProvider.Validate_RequestNumber(idExpediente);

                if (requestNumber >= 1)
                {
                    return Ok(new
                    {
                        status = true,
                        detail = requestNumber,
                        message = "Nro de solicitud válido, escoga la acción a realizar..."
                    });
                }

                return Ok(new
                {
                    status = false,
                    detail = requestNumber,
                    message = "Nro. de Solicitud no válido"
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

        [HttpGet, Route("CatalogTypeUtility")]
        public async Task<IActionResult> CatalogTypeUtility()
        {
            try
            {
                var listCatalogos = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.TipoUtilidades);

                return Ok(new
                {
                    status = true,
                    detail = listCatalogos,
                    message = "catálogo cargado correctamente"
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

        [HttpPost, Route("GetUserActivity")]
        public async Task<IActionResult> GetUserActivity([FromBody] UtilidadesDTO model)
        {
            try
            {
                if (model.accion_id == 1)
                {
                    var listGetUserActivity = await utilidadesApplicationProvider.GetActivitiesUsers(model.id_expediente);

                    return Ok(new
                    {
                        status = true,
                        detail = listGetUserActivity,
                        message = string.Empty
                    });
                }

                if (model.accion_id == 2)
                {
                    var usersActivity = await utilidadesApplicationProvider.GetUsersActivities(model.id_expediente, model.actividad_id);

                    return Ok(new
                    {
                        status = true,
                        detail = usersActivity,
                        message = string.Empty
                    });
                }

                return Ok(new
                {
                    status = false,
                    detail = new List<ControlBaseDTO>(),
                    message = "Acción no válida."
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

        [HttpPost, Route("save")]
        public async Task<IActionResult> Save([FromBody] UtilidadesDTO util)
        {
            try
            {
                util.user_id = GetUserId();

                bool request;

                if (util.accion_id == 1)
                {
                    request = await utilidadesApplicationProvider.Save_CancelActivity(util.id_expediente);
                }
                else if (util.accion_id == 2)
                {
                    request = await utilidadesApplicationProvider.Save_DeleteReassign(util, GetUserId());
                }
                else
                {
                    return Ok(new
                    {
                        status = false,
                        detail = false,
                        message = "Acción no válida."
                    });
                }

                if (!request)
                {
                    return Ok(new
                    {
                        status = false,
                        detail = false,
                        message = "No se pudo realizar la operación."
                    });
                }

                return Ok(new
                {
                    status = true,
                    detail = true,
                    message = "Operación realizada correctamente"
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

        #region GetUserId
        private int GetUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claims = identity.Claims.ToList();
            return int.Parse(claims[2].Value);
        }
        #endregion
    }
}
