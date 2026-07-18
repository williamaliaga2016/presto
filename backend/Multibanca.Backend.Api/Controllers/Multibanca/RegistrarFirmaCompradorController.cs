using Framework.WorkFlow.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.Domain.Models.Multibanca.DatosOperacion;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrarFirmaCompradorController : ControllerBase
    {
        private readonly IRegistrarFirmaCompradorApplication RegistrarFirmaCompradorApplicationProvider;
        private readonly IRegistrarFirmaCompradorDetalleApplication RegistrarFirmaCompradorDetalleApplicationProvider;
        private readonly ICommonApplication CommonApplicationProvider;

        private readonly string ActividadID = Constants.Actividades.RegistrarFirmaComprador;

        public RegistrarFirmaCompradorController(
            IRegistrarFirmaCompradorApplication _registrarFirmaCompradorApplication,
            IRegistrarFirmaCompradorDetalleApplication _registrarFirmaCompradorDetalleApplication,
            ICommonApplication _commonApplication)
        {
            RegistrarFirmaCompradorApplicationProvider = _registrarFirmaCompradorApplication;
            RegistrarFirmaCompradorDetalleApplicationProvider = _registrarFirmaCompradorDetalleApplication;
            CommonApplicationProvider = _commonApplication;
        }

        [HttpGet, Route("GetByIdExpediente/{id_expediente}")]
        public async Task<IActionResult> GetByIdExpediente(long id_expediente)
        {
            try
            {
                var result = await RegistrarFirmaCompradorApplicationProvider.GetByExpediente(id_expediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "firma comprador obtenida correctamente."
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
        public async Task<IActionResult> Save([FromBody] firma_comprador firma_comprador)
        {
            IActionResult response = Unauthorized();

            try
            {
                if (firma_comprador == null)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "El modelo de Datos de Operación es obligatorio."
                    });
                }

                if (firma_comprador.id_expediente <= 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "El expediente es obligatorio."
                    });
                }

                int userId = GetUserId();

                if (firma_comprador.id_firma_comprador == 0)
                {
                    firma_comprador.is_active = true;
                    firma_comprador.row_status = true;

                    var result = RegistrarFirmaCompradorApplicationProvider.Create(firma_comprador, userId);
                    firma_comprador.id_firma_comprador = result.id_firma_comprador;
                    firma_comprador.id_expediente = result.id_expediente;
                }
                else
                {
                    var result = RegistrarFirmaCompradorApplicationProvider.Update(firma_comprador, userId);
                    firma_comprador.id_firma_comprador = result.id_firma_comprador;
                    firma_comprador.id_expediente = result.id_expediente;
                }

                if (firma_comprador.firma_comprador_detalle != null)
                {
                    foreach (var detalle in firma_comprador.firma_comprador_detalle)
                    {
                        detalle.id_firma_comprador = firma_comprador.id_firma_comprador;

                        if (detalle.id_firma_comprador_detalle == 0)
                        {
                            detalle.is_active = true;
                            detalle.row_status = true;
                            RegistrarFirmaCompradorDetalleApplicationProvider.Create(detalle, userId);
                        }
                        else
                        {
                            RegistrarFirmaCompradorDetalleApplicationProvider.Update(detalle, userId);
                        }
                    }
                }

                response = Ok(new
                {
                    status = true,
                    detail = firma_comprador,
                    message = "Firma Comprador guardada correctamente."
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
            try
            {
                int id_usuario = GetUserId();

                List<AssignActivityDTO> listAssignActividadesDTO = await RegistrarFirmaCompradorApplicationProvider
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
