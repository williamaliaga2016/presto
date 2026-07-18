using Framework.WorkFlow.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Common;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerarRecursosPagosCbrController : ControllerBase
    {
        private readonly IGenerarRecursosPagosCbrApplication GenerarRecursosPagosCbrApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = Constants.Actividades.VisarOperacion;

        public GenerarRecursosPagosCbrController(
            IGenerarRecursosPagosCbrApplication _generarRecursosPagosCbrApplication,
            IBitacoraApplication _bitacoraApplication,
            IWorkflowApplication _workflowApplication)
        {
            GenerarRecursosPagosCbrApplicationProvider = _generarRecursosPagosCbrApplication;
            BitacoraApplicacionProvider = _bitacoraApplication;
        }

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await GenerarRecursosPagosCbrApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Generar Recursos Pagos CBR obtenido correctamente."
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
        public async Task<IActionResult> Save([FromBody] generar_recursos_pagos_cbr generar_recursos_pagos_cbr)
        {
            IActionResult response = Unauthorized();

            try
            {

                if (generar_recursos_pagos_cbr.id_generar_recursos_pagos_cbr == 0)
                {
                    generar_recursos_pagos_cbr.row_status = true;
                    generar_recursos_pagos_cbr.is_active = true;
                    generar_recursos_pagos_cbr = GenerarRecursosPagosCbrApplicationProvider.Create(generar_recursos_pagos_cbr, GetUserId());
                }
                else
                {
                    generar_recursos_pagos_cbr = GenerarRecursosPagosCbrApplicationProvider.Update(generar_recursos_pagos_cbr, GetUserId());
                }
                if (generar_recursos_pagos_cbr == null)
                {
                    return BadRequest("Invalid client request");
                }

                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(generar_recursos_pagos_cbr.id_expediente, ActividadID);

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = generar_recursos_pagos_cbr.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = GetUserId();
                bitacora.fecha_alta = DateTime.Now;
                bitacora.observaciones = generar_recursos_pagos_cbr.observaciones;
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
                    detail = generar_recursos_pagos_cbr,
                    message = "Generar Recursos Pagos CBR guardado correctamente."
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

        //[Authorize(Roles = "ADMINISTRADOR")]
        [AllowAnonymous]
        [HttpGet, Route("Avanzar/{id_expediente}")]
        public async Task<IActionResult> Avanzar(long id_expediente)
        {
            IActionResult response = Unauthorized();

            try
            {
                //int id_usuario = GetUserId();
                int id_usuario = 1;

                List<AssignActivityDTO> listAssignActividadesDTO = await GenerarRecursosPagosCbrApplicationProvider.Avanzar(id_expediente, id_usuario, ActividadID);

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
