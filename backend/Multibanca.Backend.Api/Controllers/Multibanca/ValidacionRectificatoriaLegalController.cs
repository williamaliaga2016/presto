using Multibanca.Application.Interfaces.Workflow;
using Framework.WorkFlow.Common.DTO;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.CargaOperacionBanco;
using Multibanca.Common;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;
using System.Security.Claims;
using Multibanca.Application.Interfaces.Multibanca.ValidacionRectificatoriaLegal;
using Multibanca.Domain.Models.Multibanca.ValidacionRectificatoriaLegal;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidacionRectificatoriaLegalController : ControllerBase
    {
        private readonly IValidacionRectificatoriaLegalApplication ValidacionRectificatoriaLegalApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;
        private readonly ICommonApplication CommonApplicationProvider;
        private readonly IValidacionRectificatoriaLegalDatosPersonalesApplication ValidacionRectificatoriaLegalDatosPersonalesApplicationProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;

        private readonly string ActividadID = Constants.Actividades.ValidacionRectificatoriaLegal;

        public ValidacionRectificatoriaLegalController(
            IValidacionRectificatoriaLegalApplication _validacionRectificatoriaLegalApplication,
           IValidacionRectificatoriaLegalDatosPersonalesApplication _validacionRectificatoriaLegalDatosPersonalesApplication,
            IBitacoraApplication _bitacoraApplication, ICommonApplication _commonApplication,
            IWorkflowApplication _workflowApplication)
        {
            ValidacionRectificatoriaLegalApplicationProvider = _validacionRectificatoriaLegalApplication;
            ValidacionRectificatoriaLegalDatosPersonalesApplicationProvider = _validacionRectificatoriaLegalDatosPersonalesApplication;
            WorkflowApplicationProvider = _workflowApplication;
            BitacoraApplicacionProvider = _bitacoraApplication;
            CommonApplicationProvider = _commonApplication;
        }

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await ValidacionRectificatoriaLegalApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Validacion Rectificatoria Legal obtenida correctamente."
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

        // ============================================================
        // ACTIVIDAD BASE: DATOS PERSONALES FALTANTES POR FIMAR
        // ============================================================

        [HttpGet, Route("GetFaltantesFirmaByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetFaltantesFirmaByExpediente(long idExpediente)
        {
            try
            {
                var result = await ValidacionRectificatoriaLegalDatosPersonalesApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Datos personales de Validacion Rectificatoria Legal obtenidos correctamente."
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
        public async Task<IActionResult> Save([FromBody] validacion_rectificatoria_legal validacion_rectificatoria_legal)
        {
            {
                IActionResult response = Unauthorized();

                try
                {
                    if (validacion_rectificatoria_legal.id_usuario_solicitante == 0)
                    {
                        return BadRequest(new
                        {
                            status = false,
                            message = "El usuario solicitante es obligatorio."
                        });
                    }

                    if (validacion_rectificatoria_legal.is_subsanar && string.IsNullOrWhiteSpace(validacion_rectificatoria_legal.observaciones))
                    {
                        return BadRequest(new
                        {
                            status = false,
                            message = "Las observaciones son obligatorias cuando se subsana el reparo."
                        });
                    }

                    validacion_rectificatoria_legal modelSaved;

                    if (validacion_rectificatoria_legal.id_validacion_rectificatoria_legal == 0)
                    {
                        validacion_rectificatoria_legal.row_status = true;
                        validacion_rectificatoria_legal.is_active = true;

                        modelSaved = ValidacionRectificatoriaLegalApplicationProvider.Create(
                            validacion_rectificatoria_legal,
                            GetUserId()
                        );
                    }
                    else
                    {
                        modelSaved = ValidacionRectificatoriaLegalApplicationProvider.Update(
                            validacion_rectificatoria_legal,
                            GetUserId()
                        );
                    }


                    if (validacion_rectificatoria_legal.validacion_rectificatoria_legal_datos_personales != null)
                    {
                        foreach (var faltante_firma in validacion_rectificatoria_legal.validacion_rectificatoria_legal_datos_personales)
                        {
                            faltante_firma.id_validacion_rectificatoria_legal = modelSaved.id_validacion_rectificatoria_legal;
                            faltante_firma.id_expediente = modelSaved.id_expediente;

                            if (faltante_firma.id_validacion_rectificatoria_legal_datos_personales == 0)
                            {
                                faltante_firma.is_active = true;
                                faltante_firma.row_status = true;

                                ValidacionRectificatoriaLegalDatosPersonalesApplicationProvider.Create(faltante_firma, GetUserId());
                            }
                            else
                            {
                                ValidacionRectificatoriaLegalDatosPersonalesApplicationProvider.Update(faltante_firma, GetUserId());
                            }
                        }
                    }

                    var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(validacion_rectificatoria_legal.id_expediente, ActividadID);

                    bitacora bitacora = new bitacora();
                    bitacora.id_expediente = validacion_rectificatoria_legal.id_expediente;
                    bitacora.id_actividad = ActividadID;
                    bitacora.id_usuario = GetUserId();
                    bitacora.fecha_alta = DateTime.Now;
                    bitacora.observaciones = "Se guardó la actividad Carga Operación Banco.";
                    bitacora.is_active = true;
                    bitacora.row_status = true;
                    if (resultBitacora.id_bitacora > 0)
                    {
                        bitacora.id_bitacora = resultBitacora.id_bitacora;
                        BitacoraApplicacionProvider.Update(bitacora, GetUserId());
                    }
                    else
                    {
                        BitacoraApplicacionProvider.Create(bitacora, GetUserId());
                    }
                    validacion_rectificatoria_legal? result = await ValidacionRectificatoriaLegalApplicationProvider
                        .GetByExpediente(modelSaved.id_expediente);

                    response = Ok(new
                    {
                        status = true,
                        detail = result,
                        message = "Carga Operación Banco guardada correctamente."
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
                    await ValidacionRectificatoriaLegalApplicationProvider.Avanzar(id_expediente, id_usuario, ActividadID);

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


        // ============================================================
        // CONTROLES / CATÁLOGOS
        // ============================================================

        [HttpGet, Route("GetControlesValidacionRectificatoriaLegal")]
        public async Task<IActionResult> GetControlesValidacionRectificatoriaLegal()
        {
            try
            {
                var rolComparecencia = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.RolComparecencia);

                var genero = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.Genero);
                var estadoCivil = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.EstadoCivil);
                var relacionTitular = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.RelacionTitular);
                var region = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.Region);
                var comuna = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.Comuna);
                var nacionalidad = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.Nacionalidad);
                var tipoRequerimientoDocumentacion = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.tipoRequerimientoDocumentacion);
                var realizaPago = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.realizaPago);
                return Ok(new
                {
                    status = true,
                    detail = new
                    {
                        rol_comparecencia = rolComparecencia,
                        genero = genero,
                        estado_civil = estadoCivil,
                        relacion_titular = relacionTitular,
                        region = region,
                        comuna = comuna,
                        nacionalidad = nacionalidad,
                        tipo_requerimiento_documentacion = tipoRequerimientoDocumentacion,
                        realiza_pago = realizaPago,

                    },
                    message = "Controles de Validacion Rectificatoria Legal obtenidos correctamente."
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
