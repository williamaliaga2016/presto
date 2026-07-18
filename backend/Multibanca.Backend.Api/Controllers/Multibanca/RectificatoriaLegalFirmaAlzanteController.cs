using Framework.WorkFlow.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Common;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]")]
    [ApiController]
    public class RectificatoriaLegalFirmaAlzanteController : ControllerBase
    {
        private readonly IRectificatoriaLegalFirmaAlzanteApplication RectificatoriaLegalFirmaAlzanteApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = Constants.Actividades.RectificatoriaLegalFirmaAlzante;

        public RectificatoriaLegalFirmaAlzanteController(
            IRectificatoriaLegalFirmaAlzanteApplication _rectificatoriaLegalFirmaAlzanteApplication,
            IBitacoraApplication _bitacoraApplication,
            IWorkflowApplication _workflowApplication)
        {
            RectificatoriaLegalFirmaAlzanteApplicationProvider = _rectificatoriaLegalFirmaAlzanteApplication;
            BitacoraApplicacionProvider = _bitacoraApplication;
        }

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await RectificatoriaLegalFirmaAlzanteApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Rectificatoria Legal Firma Alzante obtenida correctamente."
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
        public async Task<IActionResult> Save([FromBody] rectificatoria_legal_firma_alzante rectificatoria_legal_firma_alzante)
        {
            IActionResult response = Unauthorized();

            try
            {
                if (rectificatoria_legal_firma_alzante.fecha_firma_alzante == null)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "La fecha de firma del alzante es obligatoria."
                    });
                }

                if (string.IsNullOrWhiteSpace(rectificatoria_legal_firma_alzante.observaciones))
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "Las observaciones son obligatorias."
                    });
                }

                if (rectificatoria_legal_firma_alzante.id_rectificatoria_legal_firma_alzante == 0)
                {
                    rectificatoria_legal_firma_alzante.row_status = true;
                    rectificatoria_legal_firma_alzante.is_active = true;
                    rectificatoria_legal_firma_alzante = RectificatoriaLegalFirmaAlzanteApplicationProvider.Create(rectificatoria_legal_firma_alzante, GetUserId());
                }
                else
                {
                    rectificatoria_legal_firma_alzante = RectificatoriaLegalFirmaAlzanteApplicationProvider.Update(rectificatoria_legal_firma_alzante, GetUserId());
                }

                if (rectificatoria_legal_firma_alzante == null)
                {
                    return BadRequest("Invalid client request");
                }

                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(rectificatoria_legal_firma_alzante.id_expediente, ActividadID);

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = rectificatoria_legal_firma_alzante.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = GetUserId();
                bitacora.fecha_alta = DateTime.Now;
                bitacora.observaciones = rectificatoria_legal_firma_alzante.observaciones;
                bitacora.is_active = true;
                bitacora.row_status = true;

                if (resultBitacora.id_bitacora > 0)
                {
                    bitacora.id_bitacora = resultBitacora.id_bitacora;
                    BitacoraApplicacionProvider.Update(bitacora, GetUserId());
                }
                else
                    BitacoraApplicacionProvider.Create(bitacora, GetUserId());

                response = Ok(new
                {
                    status = true,
                    detail = rectificatoria_legal_firma_alzante,
                    message = "Rectificatoria Legal Firma Alzante guardada correctamente."
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

                List<AssignActivityDTO> listAssignActividadesDTO = await RectificatoriaLegalFirmaAlzanteApplicationProvider.Avanzar(id_expediente, id_usuario, ActividadID);

                if (listAssignActividadesDTO.Count() > 0)
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
