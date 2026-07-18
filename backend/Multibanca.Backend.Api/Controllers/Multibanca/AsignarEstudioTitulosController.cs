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
    public class AsignarEstudioTitulosController : ControllerBase
    {
        private readonly IAsignarEstudioTitulosApplication AsignarEstudioTitulosApplicationProvider;
        private readonly ICommonApplication CommonApplicationProvider;

        private readonly string ActividadID = Constants.Actividades.AsignarEstudioTitulos;

        public AsignarEstudioTitulosController(
            IAsignarEstudioTitulosApplication _asignarEstudioTitulosApplication,
            ICommonApplication _commonApplication)
        {
            AsignarEstudioTitulosApplicationProvider = _asignarEstudioTitulosApplication;
            CommonApplicationProvider = _commonApplication;
        }

        [HttpGet, Route("GetByIdExpediente/{id_expediente}")]
        public async Task<IActionResult> GetByIdExpediente(long id_expediente)
        {
            try
            {
                var result = await AsignarEstudioTitulosApplicationProvider.GetByExpediente(id_expediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Asignar Estudio de Títulos obtenido correctamente."
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
        public async Task<IActionResult> Save([FromBody] asignar_estudio_titulos asignar_estudio_titulos)
        {
            IActionResult response = Unauthorized();

            try
            {
                if (asignar_estudio_titulos.id_expediente <= 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "No existe un id_expediente válido."
                    });
                }

                asignar_estudio_titulos.id_actividad = ActividadID;

                if (asignar_estudio_titulos.id_asignar_estudio_titulos == 0)
                {
                    asignar_estudio_titulos.row_status = true;
                    asignar_estudio_titulos.is_active = true;
                    asignar_estudio_titulos = AsignarEstudioTitulosApplicationProvider.Create(asignar_estudio_titulos, GetUserId());
                }
                else
                {
                    asignar_estudio_titulos = AsignarEstudioTitulosApplicationProvider.Update(asignar_estudio_titulos, GetUserId());
                }

                if (asignar_estudio_titulos == null)
                {
                    return BadRequest("Invalid client request");
                }

                response = Ok(new
                {
                    status = true,
                    detail = asignar_estudio_titulos,
                    message = "Asignar Estudio de Títulos guardado correctamente."
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

        [Authorize(Roles = "ADMINISTRADOR,EJECUTIVO_FORMALIZADOR")]
        [HttpGet, Route("Avanzar/{id_expediente}")]
        public async Task<IActionResult> Avanzar(long id_expediente)
        {
            try
            {
                int id_usuario = GetUserId();

                List<AssignActivityDTO> listAssignActividadesDTO = await AsignarEstudioTitulosApplicationProvider
                    .Avanzar(id_expediente, id_usuario, ActividadID);

                if (listAssignActividadesDTO.Count > 0)
                {
                    return Ok(new
                    {
                        status = true,
                        detail = "",
                        message = "Actividad avanzada correctamente."
                    });
                }

                return Ok(new
                {
                    status = false,
                    detail = "",
                    message = "No se pudo avanzar la actividad."
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    status = false,
                    detail = (object?)null,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    detail = (object?)null,
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

