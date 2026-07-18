using Framework.WorkFlow.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsignarEscrituraController : ControllerBase
    {
        private readonly IAsignarEscrituraApplication AsignarEscrituraApplicationProvider;
        private readonly ICommonApplication CommonApplicationProvider;

        private readonly string ActividadID = Constants.Actividades.AsignarEscritura;

        public AsignarEscrituraController(
            IAsignarEscrituraApplication _asignarEscrituraApplication,
            ICommonApplication _commonApplication)
        {
            AsignarEscrituraApplicationProvider = _asignarEscrituraApplication;
            CommonApplicationProvider = _commonApplication;
        }

        [HttpGet, Route("GetByIdExpediente/{id_expediente}")]
        public async Task<IActionResult> GetByIdExpediente(long id_expediente)
        {
            try
            {
                var result = await AsignarEscrituraApplicationProvider.GetByExpediente(id_expediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Asignar Escritura obtenida correctamente."
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

        [HttpGet, Route("GetCatalogoAbogado")]
        public async Task<IActionResult> GetCatalogoAbogado()
        {
            try
            {
                var result = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.NombreAbogado);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Catálogo de abogados obtenido correctamente."
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

        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpPost, Route("Save")]
        public async Task<IActionResult> Save([FromBody] asignar_escritura asignar_escritura)
        {
            IActionResult response = Unauthorized();

            try
            {
                if (asignar_escritura.id_expediente <= 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "No existe un id_expediente válido."
                    });
                }

                asignar_escritura.id_actividad = ActividadID;

                if (asignar_escritura.id_asignar_escritura == 0)
                {
                    asignar_escritura.row_status = true;
                    asignar_escritura.is_active = true;
                    asignar_escritura = AsignarEscrituraApplicationProvider.Create(asignar_escritura, GetUserId());
                }
                else
                {
                    asignar_escritura = AsignarEscrituraApplicationProvider.Update(asignar_escritura, GetUserId());
                }

                if (asignar_escritura == null)
                {
                    return BadRequest("Invalid client request");
                }

                response = Ok(new
                {
                    status = true,
                    detail = asignar_escritura,
                    message = "Asignar Escritura guardada correctamente."
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

        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpGet, Route("Avanzar/{id_expediente}")]
        public async Task<IActionResult> Avanzar(long id_expediente)
        {
            IActionResult response = Unauthorized();

            try
            {
                int id_usuario = GetUserId();

                List<AssignActivityDTO> listAssignActividadesDTO =
                    await AsignarEscrituraApplicationProvider.Avanzar(id_expediente, id_usuario, ActividadID);

                if (listAssignActividadesDTO.Count() > 0)
                {
                    response = Ok(new
                    {
                        status = true,
                        detail = "",
                        message = "Actividad avanzada correctamente"
                    });
                }
                else
                {
                    response = Ok(new
                    {
                        status = true,
                        detail = "",
                        message = "Actividad avanzada correctamente"
                    });
                }

                return response;
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
