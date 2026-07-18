using Framework.WorkFlow.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Implementations.Multibanca.GenerarBorradorEscritura;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca.DatosOperacion;
using Multibanca.Application.Interfaces.Multibanca.GenerarBorradorEscritura;
using Multibanca.Common;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca.GenerarBorradorEscritura;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerarBorradorEscrituraController : ControllerBase
    {
        private readonly IGenerarBorradorEscrituraApplication GenerarBorradorEscrituraApplicationProvider;
        private readonly IGenerarBorradorEscrituraDetalleApplication GenerarBorradorEscrituraDetalleApplicationProvider;
        private readonly ICommonApplication CommonApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = Constants.Actividades.GenerarBorradorEscritura;

        public GenerarBorradorEscrituraController(IGenerarBorradorEscrituraApplication generarBorradorEscrituraApplicationProvider, IGenerarBorradorEscrituraDetalleApplication generarBorradorEscrituraDetalleApplicationProvider, ICommonApplication commonApplicationProvider, IBitacoraApplication bitacoraApplicacionProvider)
        {
            GenerarBorradorEscrituraApplicationProvider = generarBorradorEscrituraApplicationProvider;
            GenerarBorradorEscrituraDetalleApplicationProvider = generarBorradorEscrituraDetalleApplicationProvider;
            CommonApplicationProvider = commonApplicationProvider;
            BitacoraApplicacionProvider = bitacoraApplicacionProvider;
        }

        // ============================================================
        // ACTIVIDAD BASE: GENERAR BORRADOR ESCRITURA - 5.20
        // ============================================================

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await GenerarBorradorEscrituraApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Datos de Operación obtenidos correctamente."
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
        public async Task<IActionResult> Save([FromBody] generar_borrador_escritura request)
        {
            IActionResult response = Unauthorized();

            try
            {
                if (request == null)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "El modelo de Generar Borrador Escritura es obligatorio."
                    });
                }

                if (request.id_expediente <= 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "El expediente es obligatorio."
                    });
                }

                int userId = GetUserId();

                // 1. Guardar cabecera
                if (request.id_generar_borrador_escritura == 0)
                {
                    request.is_active = true;
                    request.row_status = true;

                    var result = GenerarBorradorEscrituraApplicationProvider.Create(request, userId);
                    request.id_generar_borrador_escritura = result.id_generar_borrador_escritura;
                    request.id_expediente = result.id_expediente;
                }
                else
                {
                    var result = GenerarBorradorEscrituraApplicationProvider.Update(request, userId);
                    request.id_generar_borrador_escritura = result.id_generar_borrador_escritura;
                    request.id_expediente = result.id_expediente;
                }

                // 2. Guardar detalle (Fiadores con roles y firma)
                if (request.detalle != null && request.detalle.Any())
                {
                    for (int i = 0; i < request.detalle.Count; i++)
                    {
                        request.detalle[i].id_generar_borrador_escritura = request.id_generar_borrador_escritura;
                        request.detalle[i].id_expediente = request.id_expediente;

                        if (request.detalle[i].id_generar_borrador_escritura_detalle_entity == 0)
                        {
                            request.detalle[i].is_active = true;
                            request.detalle[i].row_status = true;
                            request.detalle[i] = GenerarBorradorEscrituraDetalleApplicationProvider.Create(request.detalle[i], userId);
                        }
                        else
                        {
                            request.detalle[i] = GenerarBorradorEscrituraDetalleApplicationProvider.Update(request.detalle[i], userId);
                        }
                    }
                }

                // 3. Registrar en bitácora
                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(request.id_expediente, ActividadID);

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = request.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = userId;
                bitacora.fecha_alta = DateTime.Now;
                bitacora.observaciones = "Se guardó la actividad Generar Borrador Escritura.";
                bitacora.is_active = true;
                bitacora.row_status = true;

                if (resultBitacora.id_bitacora > 0)
                {
                    bitacora.id_bitacora = resultBitacora.id_bitacora;
                    BitacoraApplicacionProvider.Update(bitacora, userId);
                }
                else
                {
                    BitacoraApplicacionProvider.Create(bitacora, userId);
                }

                response = Ok(new
                {
                    status = true,
                    detail = request,
                    message = "Generar Borrador Escritura guardado correctamente."
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
                    await GenerarBorradorEscrituraApplicationProvider.Avanzar(id_expediente, id_usuario, ActividadID);

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

        // ============================================================
        // CONTROLES / CATÁLOGOS
        // ============================================================

        [HttpGet, Route("GetControlesGenerarBorradorEscritura")]
        public async Task<IActionResult> GetControlesGenerarBorradorEscritura()
        {
            try
            {
                var _notaria = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.Notaria);
                var _rolcomparecencia = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.RolComparecencia);


                return Ok(new
                {
                    status = true,
                    detail = new
                    {
                        notaria = _notaria,
                        rolcomparecencia=_rolcomparecencia
                        
                    },
                    message = "Controles de Generar Borrador Escritura obtenidos correctamente."
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

        #region GetPerformerId
        private string GetPerformerId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claims = identity.Claims.ToList();
            return claims[5].Value;
        }
        #endregion
    }
}
