using Framework.WorkFlow.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Implementations.Multibanca;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrarFirmaVendedorController : ControllerBase
    {
        private readonly IRegistrarFirmaVendedorApplication RegistrarFirmaVendedorApplicationProvider;
        private readonly IRegistrarFirmaVendedorDetalleApplication RegistrarFirmaVendedorDetalleApplicationProvider;
        private readonly ICommonApplication CommonApplicationProvider;

        private readonly string ActividadID = Constants.Actividades.RegistrarFirmaVendedor;

        public RegistrarFirmaVendedorController(
            IRegistrarFirmaVendedorApplication _registrarFirmaVendedorApplication,
            IRegistrarFirmaVendedorDetalleApplication _registrarFirmaVendedorDetalleApplication,
            ICommonApplication _commonApplication)
        {
            RegistrarFirmaVendedorApplicationProvider = _registrarFirmaVendedorApplication;
            RegistrarFirmaVendedorDetalleApplicationProvider = _registrarFirmaVendedorDetalleApplication;
            CommonApplicationProvider = _commonApplication;
        }

        [HttpGet, Route("GetByIdExpediente/{id_expediente}")]
        public async Task<IActionResult> GetByIdExpediente(long id_expediente)
        {
            try
            {
                var result = await RegistrarFirmaVendedorApplicationProvider.GetByExpediente(id_expediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "firma vendedor obtenida correctamente."
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
        public async Task<IActionResult> Save([FromBody] firma_vendedor firma_vendedor)
        {
            IActionResult response = Unauthorized();

            try
            {
                if (firma_vendedor == null)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "El modelo de Datos de Operación es obligatorio."
                    });
                }

                if (firma_vendedor.id_expediente <= 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "El expediente es obligatorio."
                    });
                }

                int userId = GetUserId();

                if (firma_vendedor.id_firma_vendedor == 0)
                {
                    firma_vendedor.is_active = true;
                    firma_vendedor.row_status = true;

                    var result = RegistrarFirmaVendedorApplicationProvider.Create(firma_vendedor, userId);
                    firma_vendedor.id_firma_vendedor = result.id_firma_vendedor;
                    firma_vendedor.id_expediente = result.id_expediente;
                }
                else
                {
                    var result = RegistrarFirmaVendedorApplicationProvider.Update(firma_vendedor, userId);
                    firma_vendedor.id_firma_vendedor = result.id_firma_vendedor;
                    firma_vendedor.id_expediente = result.id_expediente;
                }

                if (firma_vendedor.firma_vendedor_detalle != null)
                {
                    foreach (var detalle in firma_vendedor.firma_vendedor_detalle)
                    {
                        detalle.id_firma_vendedor = firma_vendedor.id_firma_vendedor;

                        if (detalle.id_firma_vendedor_detalle == 0)
                        {
                            detalle.is_active = true;
                            detalle.row_status = true;
                            RegistrarFirmaVendedorDetalleApplicationProvider.Create(detalle, userId);
                        }
                        else
                        {
                            RegistrarFirmaVendedorDetalleApplicationProvider.Update(detalle, userId);
                        }
                    }
                }

                response = Ok(new
                {
                    status = true,
                    detail = firma_vendedor,
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

                List<AssignActivityDTO> listAssignActividadesDTO = await RegistrarFirmaVendedorApplicationProvider
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
