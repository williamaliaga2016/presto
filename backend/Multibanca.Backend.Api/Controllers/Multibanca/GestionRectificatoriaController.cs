using Framework.WorkFlow.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Common;
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
    public class GestionRectificatoriaController : ControllerBase
    {
        private readonly IGestionRectificatoriaApplication GestionRectificatoriaApplication;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly ICommonApplication CommonApplicationProvider;
        private readonly string ActividadID = Constants.Actividades.GestionRectificatoria;

        public GestionRectificatoriaController(IGestionRectificatoriaApplication gestionRectificatoriaApplication, IBitacoraApplication bitacoraApplicacionProvider, IWorkflowApplication workflowApplicationProvider, ICommonApplication _commonApplication)
        {
            GestionRectificatoriaApplication = gestionRectificatoriaApplication;
            BitacoraApplicacionProvider = bitacoraApplicacionProvider;
            WorkflowApplicationProvider = workflowApplicationProvider;
            CommonApplicationProvider = _commonApplication;
        }
        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await GestionRectificatoriaApplication.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Gestion Rectificatoria obtenido correctamente."
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
        public async Task<IActionResult> Save([FromBody] gestion_rectificatoria gestion_rectificatoria)
        {
            IActionResult response = Unauthorized();
            try
            {
                if (gestion_rectificatoria.id_gestion_rectificatoria == 0)
                {
                    gestion_rectificatoria = GestionRectificatoriaApplication.Create(gestion_rectificatoria, GetUserId());
                }
                else
                {
                    gestion_rectificatoria = GestionRectificatoriaApplication.Update(gestion_rectificatoria, GetUserId());
                }

                if (gestion_rectificatoria == null)
                {
                    return BadRequest("Invalid client request");
                }
                else
                {
                    var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(gestion_rectificatoria.id_expediente, ActividadID);

                    bitacora bitacora = new bitacora();
                    bitacora.id_expediente = gestion_rectificatoria.id_expediente;
                    bitacora.id_actividad = ActividadID;
                    bitacora.id_usuario = GetUserId();
                    bitacora.fecha_alta = DateTime.Now;
                    bitacora.is_active = true;
                    bitacora.row_status = true;

                    if (resultBitacora.id_bitacora > 0)
                    {
                        bitacora.id_bitacora = resultBitacora.id_bitacora;
                        bitacora.observaciones = "Se actualizó la actividad Gestion Rectificatoria.";
                        BitacoraApplicacionProvider.Update(bitacora, GetUserId());
                    }
                    else
                    {
                        bitacora.observaciones = "Se guardó la actividad Gestion Rectificatoria.";
                        BitacoraApplicacionProvider.Create(bitacora, GetUserId());
                    }
                }

                response = Ok(new
                {
                    status = true,
                    detail = gestion_rectificatoria,
                    message = "Gestion Rectificatoria guardado correctamente."
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
                List<AssignActivityDTO> listAssignActividadesDTO = await GestionRectificatoriaApplication.Avanzar(id_expediente, id_usuario, ActividadID);

                if (listAssignActividadesDTO.Count() > 0)
                {
                    response = Ok(new
                    {
                        status = true,
                        detail = "",
                        message = "Actividad avanzada correctamente."
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
        [HttpGet, Route("GetControles")]
        public async Task<IActionResult> GetControlesPropiedad()
        {
            try
            {
                var tipoReparo = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.GestionRectificatoriaTipoReparo);

                return Ok(new
                {
                    status = true,
                    detail = new
                    {
                        tipo_reparo=tipoReparo
                    },
                    message = "Controles de Propiedad obtenidos correctamente."
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

