using Framework.WorkFlow.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Common;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]")]
    [ApiController]
    public class AprobacionComercialLegalCdRController : ControllerBase
    {
        private readonly IAprobacionComercialLegalCdRApplication AprobacionApplication;
        private readonly IBitacoraApplication BitacoraApplication;
        private readonly string ActividadID = Constants.Actividades.RealizarAprobacionComercialLegalCdR;

        public AprobacionComercialLegalCdRController(
            IAprobacionComercialLegalCdRApplication _aprobacionApplication,
            IBitacoraApplication _bitacoraApplication)
        {
            AprobacionApplication = _aprobacionApplication;
            BitacoraApplication = _bitacoraApplication;
        }

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await AprobacionApplication.GetByExpediente(idExpediente);
                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Aprobacion Comercial Legal CdR obtenido correctamente."
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
        public async Task<IActionResult> Save([FromBody] aprobacion_comercial_legal_cdr model)
        {
            try
            {
                if (model.id_aprobacion_comercial_legal_cdr == 0)
                {
                    model.row_status = true;
                    model.is_active = true;
                    model = AprobacionApplication.Create(model, GetUserId());
                }
                else
                {
                    model = AprobacionApplication.Update(model, GetUserId());
                }

                if (model == null)
                {
                    return BadRequest("Invalid client request");
                }

                var resultBitacora = await BitacoraApplication.GetByExpedienteActividad(model.id_expediente, ActividadID);

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = model.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = GetUserId();
                bitacora.fecha_alta = DateTime.Now;
                bitacora.observaciones = model.observaciones;
                bitacora.is_active = true;
                bitacora.row_status = true;

                if (resultBitacora.id_bitacora > 0)
                {
                    bitacora.id_bitacora = resultBitacora.id_bitacora;
                    BitacoraApplication.Update(bitacora, GetUserId());
                }
                else
                {
                    BitacoraApplication.Create(bitacora, GetUserId());
                }

                return Ok(new
                {
                    status = true,
                    detail = model,
                    message = "Aprobacion Comercial Legal CdR guardado correctamente."
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
        [HttpGet, Route("Avanzar/{id_expediente}")]
        public async Task<IActionResult> Avanzar(long id_expediente)
        {
            IActionResult response = Unauthorized();

            try
            {
                int id_usuario = GetUserId();

                List<AssignActivityDTO> listAssignActividadesDTO =
                    await AprobacionApplication.Avanzar(
                        id_expediente,
                        id_usuario,
                        ActividadID
                    );

                response = Ok(new
                {
                    status = true,
                    detail = listAssignActividadesDTO,
                    message = "Actividad avanzada correctamente"
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

        private int GetUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claims = identity.Claims.ToList();
            return int.Parse(claims[2].Value);
        }
    }
}
