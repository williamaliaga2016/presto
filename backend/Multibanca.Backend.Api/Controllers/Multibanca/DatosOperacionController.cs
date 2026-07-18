using Framework.WorkFlow.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca.DatosOperacion;
using Multibanca.Common;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;
using Multibanca.Domain.Models.Multibanca.DatosOperacion;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatosOperacionController : ControllerBase
    {
        private readonly IDatosOperacionApplication DatosOperacionApplicationProvider;
        private readonly IDatosOperacionDatosCreditoApplication DatosOperacionDatosCreditoApplicationProvider;
        private readonly IDatosOperacionCompradorApplication DatosOperacionCompradorApplicationProvider;
        private readonly IDatosOperacionVendedorApplication DatosOperacionVendedorApplicationProvider;
        private readonly IDatosOperacionFiadorGaranteApplication DatosOperacionFiadorGaranteApplicationProvider;
        private readonly IDatosOperacionBancoAcreedorApplication DatosOperacionBancoAcreedorApplicationProvider;
        private readonly IDatosOperacionPropiedadApplication DatosOperacionPropiedadApplicationProvider;
        private readonly ICommonApplication CommonApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = Constants.Actividades.DatosOperacion;

        public DatosOperacionController(
            IDatosOperacionApplication _datosOperacionApplication,
            IDatosOperacionDatosCreditoApplication _datosOperacionDatosCreditoApplication,
            IDatosOperacionCompradorApplication _datosOperacionCompradorApplication,
            IDatosOperacionVendedorApplication _datosOperacionVendedorApplication,
            IDatosOperacionFiadorGaranteApplication _datosOperacionFiadorGaranteApplication,
            IDatosOperacionBancoAcreedorApplication _datosOperacionBancoAcreedorApplication,
            IDatosOperacionPropiedadApplication _datosOperacionPropiedadApplication,
            IBitacoraApplication _bitacoraApplication,
            ICommonApplication _commonApplication)
        {
            DatosOperacionApplicationProvider = _datosOperacionApplication;
            DatosOperacionDatosCreditoApplicationProvider = _datosOperacionDatosCreditoApplication;
            DatosOperacionCompradorApplicationProvider = _datosOperacionCompradorApplication;
            DatosOperacionVendedorApplicationProvider = _datosOperacionVendedorApplication;
            DatosOperacionFiadorGaranteApplicationProvider = _datosOperacionFiadorGaranteApplication;
            DatosOperacionBancoAcreedorApplicationProvider = _datosOperacionBancoAcreedorApplication;
            DatosOperacionPropiedadApplicationProvider = _datosOperacionPropiedadApplication;
            BitacoraApplicacionProvider = _bitacoraApplication;
            CommonApplicationProvider = _commonApplication;
        }

        // ============================================================
        // ACTIVIDAD BASE: DATOS DE OPERACIÓN - 5.4
        // ============================================================

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await DatosOperacionApplicationProvider.GetByExpediente(idExpediente);

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
        public async Task<IActionResult> Save([FromBody] datos_operacion datos_operacion)
        {
            IActionResult response = Unauthorized();

            try
            {
                if (datos_operacion == null)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "El modelo de Datos de Operación es obligatorio."
                    });
                }

                if (datos_operacion.id_expediente <= 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "El expediente es obligatorio."
                    });
                }

                int userId = GetUserId();

                // 1. Guardar cabecera.
                if (datos_operacion.id_datos_operacion == 0)
                {
                    datos_operacion.is_active = true;
                    datos_operacion.row_status = true;

                    var result = DatosOperacionApplicationProvider.Create(datos_operacion, userId);
                    datos_operacion.id_datos_operacion = result.id_datos_operacion;
                    datos_operacion.id_expediente = result.id_expediente;
                }
                else
                {
                    var result = DatosOperacionApplicationProvider.Update(datos_operacion, userId);
                    datos_operacion.id_datos_operacion = result.id_datos_operacion;
                    datos_operacion.id_expediente = result.id_expediente;
                }

                // 2. Guardar tab Datos Crédito.
                if (datos_operacion.datos_credito != null)
                {
                    datos_operacion.datos_credito.id_datos_operacion = datos_operacion.id_datos_operacion;
                    datos_operacion.datos_credito.id_expediente = datos_operacion.id_expediente;

                    if (datos_operacion.datos_credito.id_datos_operacion_datos_credito == 0)
                    {
                        datos_operacion.datos_credito.is_active = true;
                        datos_operacion.datos_credito.row_status = true;
                        datos_operacion.datos_credito = DatosOperacionDatosCreditoApplicationProvider.Create(datos_operacion.datos_credito, userId);
                    }
                    else
                    {
                        datos_operacion.datos_credito = DatosOperacionDatosCreditoApplicationProvider.Update(datos_operacion.datos_credito, userId);
                    }
                }

                // 3. Guardar tab Compradores.
                if (datos_operacion.compradores != null)
                {
                    for (int i = 0; i < datos_operacion.compradores.Count; i++)
                    {
                        datos_operacion.compradores[i].id_datos_operacion = datos_operacion.id_datos_operacion;
                        datos_operacion.compradores[i].id_expediente = datos_operacion.id_expediente;

                        if (datos_operacion.compradores[i].id_datos_operacion_comprador == 0)
                        {
                            datos_operacion.compradores[i].is_active = true;
                            datos_operacion.compradores[i].row_status = true;
                            datos_operacion.compradores[i] = DatosOperacionCompradorApplicationProvider.Create(datos_operacion.compradores[i], userId);
                        }
                        else
                        {
                            datos_operacion.compradores[i] = DatosOperacionCompradorApplicationProvider.Update(datos_operacion.compradores[i], userId);
                        }
                    }
                }

                // 4. Guardar tab Vendedores.
                if (datos_operacion.vendedores != null)
                {
                    for (int i = 0; i < datos_operacion.vendedores.Count; i++)
                    {
                        datos_operacion.vendedores[i].id_datos_operacion = datos_operacion.id_datos_operacion;
                        datos_operacion.vendedores[i].id_expediente = datos_operacion.id_expediente;

                        if (datos_operacion.vendedores[i].id_datos_operacion_vendedor == 0)
                        {
                            datos_operacion.vendedores[i].is_active = true;
                            datos_operacion.vendedores[i].row_status = true;
                            datos_operacion.vendedores[i] = DatosOperacionVendedorApplicationProvider.Create(datos_operacion.vendedores[i], userId);
                        }
                        else
                        {
                            datos_operacion.vendedores[i] = DatosOperacionVendedorApplicationProvider.Update(datos_operacion.vendedores[i], userId);
                        }
                    }
                }

                // 5. Guardar tab Fiador/Garante.
                if (datos_operacion.fiadores_garantes != null)
                {
                    for (int i = 0; i < datos_operacion.fiadores_garantes.Count; i++)
                    {
                        datos_operacion.fiadores_garantes[i].id_datos_operacion = datos_operacion.id_datos_operacion;
                        datos_operacion.fiadores_garantes[i].id_expediente = datos_operacion.id_expediente;

                        if (datos_operacion.fiadores_garantes[i].id_datos_operacion_fiador_garante == 0)
                        {
                            datos_operacion.fiadores_garantes[i].is_active = true;
                            datos_operacion.fiadores_garantes[i].row_status = true;
                            datos_operacion.fiadores_garantes[i] = DatosOperacionFiadorGaranteApplicationProvider.Create(datos_operacion.fiadores_garantes[i], userId);
                        }
                        else
                        {
                            datos_operacion.fiadores_garantes[i] = DatosOperacionFiadorGaranteApplicationProvider.Update(datos_operacion.fiadores_garantes[i], userId);
                        }
                    }
                }

                // 6. Guardar tab Banco Acreedor.
                if (datos_operacion.banco_acreedor != null)
                {
                    datos_operacion.banco_acreedor.id_datos_operacion = datos_operacion.id_datos_operacion;
                    datos_operacion.banco_acreedor.id_expediente = datos_operacion.id_expediente;

                    if (datos_operacion.banco_acreedor.id_datos_operacion_banco_acreedor == 0)
                    {
                        datos_operacion.banco_acreedor.is_active = true;
                        datos_operacion.banco_acreedor.row_status = true;
                        datos_operacion.banco_acreedor = DatosOperacionBancoAcreedorApplicationProvider.Create(datos_operacion.banco_acreedor, userId);
                    }
                    else
                    {
                        datos_operacion.banco_acreedor = DatosOperacionBancoAcreedorApplicationProvider.Update(datos_operacion.banco_acreedor, userId);
                    }
                }

                // 7. Guardar tab Propiedad.
                if (datos_operacion.propiedad != null)
                {
                    datos_operacion.propiedad.id_datos_operacion = datos_operacion.id_datos_operacion;
                    datos_operacion.propiedad.id_expediente = datos_operacion.id_expediente;

                    if (datos_operacion.propiedad.id_datos_operacion_propiedad == 0)
                    {
                        datos_operacion.propiedad.is_active = true;
                        datos_operacion.propiedad.row_status = true;
                        datos_operacion.propiedad = DatosOperacionPropiedadApplicationProvider.Create(datos_operacion.propiedad, userId);
                    }
                    else
                    {
                        datos_operacion.propiedad = DatosOperacionPropiedadApplicationProvider.Update(datos_operacion.propiedad, userId);
                    }
                }

                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(datos_operacion.id_expediente, ActividadID);

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = datos_operacion.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = userId;
                bitacora.fecha_alta = DateTime.Now;
                bitacora.observaciones = "Se guardó la actividad Datos de Operación.";
                bitacora.is_active = true;
                bitacora.row_status = true;

                if (resultBitacora.id_bitacora > 0)
                {
                    bitacora.id_bitacora = resultBitacora.id_bitacora;
                    BitacoraApplicacionProvider.Update(bitacora, userId);
                }
                else
                    BitacoraApplicacionProvider.Create(bitacora, userId);

                response = Ok(new
                {
                    status = true,
                    detail = datos_operacion,
                    message = "Datos de Operación guardados correctamente."
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
                    await DatosOperacionApplicationProvider.Avanzar(id_expediente, id_usuario, ActividadID);

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
        // CONSULTAS POR TAB
        // ============================================================

        [HttpGet, Route("GetDatosCreditoByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetDatosCreditoByExpediente(long idExpediente)
        {
            try
            {
                var result = await DatosOperacionDatosCreditoApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Datos de Crédito obtenidos correctamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = ex.Message });
            }
        }

        [HttpGet, Route("GetCompradoresByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetCompradoresByExpediente(long idExpediente)
        {
            try
            {
                var result = await DatosOperacionCompradorApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Compradores obtenidos correctamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = ex.Message });
            }
        }

        [HttpGet, Route("GetVendedoresByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetVendedoresByExpediente(long idExpediente)
        {
            try
            {
                var result = await DatosOperacionVendedorApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Vendedores obtenidos correctamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = ex.Message });
            }
        }

        [HttpGet, Route("GetFiadoresGarantesByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetFiadoresGarantesByExpediente(long idExpediente)
        {
            try
            {
                var result = await DatosOperacionFiadorGaranteApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Fiadores/Garantes obtenidos correctamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = ex.Message });
            }
        }

        [HttpGet, Route("GetBancoAcreedorByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetBancoAcreedorByExpediente(long idExpediente)
        {
            try
            {
                var result = await DatosOperacionBancoAcreedorApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Banco Acreedor obtenido correctamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = ex.Message });
            }
        }

        [HttpGet, Route("GetPropiedadByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetPropiedadByExpediente(long idExpediente)
        {
            try
            {
                var result = await DatosOperacionPropiedadApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Propiedad obtenida correctamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = ex.Message });
            }
        }


        //Region controles

        [HttpGet, Route("GetControlesDatosCredito")]
        public async Task<IActionResult> GetControlesDatosCredito()
        {
            try
            {
                var siNo = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionSiNo);
                var tipoOperacion = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionTipoOperacion);
                var tipoDireccionDividendo = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionTipoDireccionDividendo);

                return Ok(new
                {
                    status = true,
                    detail = new
                    {
                        si_no = siNo,
                        tipo_operacion = tipoOperacion,
                        tipo_direccion_dividendo = tipoDireccionDividendo
                    },
                    message = "Controles de Datos Crédito obtenidos correctamente."
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


        [HttpGet, Route("GetControlesComprador")]
        public async Task<IActionResult> GetControlesComprador()
        {
            try
            {
                var relacionTitular = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.RelacionTitular);
                var genero = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionGenero);
                var estadoCivil = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionEstadoCivil);
                var region = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionRegion);
                var comuna = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionComuna);
                var nacionalidad = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionNacionalidad);
                var siNo = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionSiNo);
                var tipo_direccion_dividendo = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionTipoDireccionDividendo);

                return Ok(new
                {
                    status = true,
                    detail = new
                    {
                        relacion_titular = relacionTitular,
                        genero = genero,
                        estado_civil = estadoCivil,
                        region = region,
                        comuna = comuna,
                        nacionalidad = nacionalidad,
                        si_no = siNo,
                        tipo_direccion_dividendo = tipo_direccion_dividendo
                    },
                    message = "Controles de Comprador obtenidos correctamente."
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


        [HttpGet, Route("GetControlesVendedor")]
        public async Task<IActionResult> GetControlesVendedor()
        {
            try
            {
                var tipoVendedor = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionTipoVendedor);
                var relacionTitular = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.RelacionTitular);
                var genero = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionGenero);
                var estadoCivil = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionEstadoCivil);
                var region = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionRegion);
                var comuna = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionComuna);
                var nacionalidad = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionNacionalidad);
                var siNo = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionSiNo);
                var tipo_direccion_dividendo = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionTipoDireccionDividendo);

                return Ok(new
                {
                    status = true,
                    detail = new
                    {
                        tipo_vendedor = tipoVendedor,
                        relacion_titular = relacionTitular,
                        genero = genero,
                        estado_civil = estadoCivil,
                        region = region,
                        comuna = comuna,
                        nacionalidad = nacionalidad,
                        si_no = siNo,
                        tipo_direccion_dividendo = tipo_direccion_dividendo
                    },
                    message = "Controles de Vendedor obtenidos correctamente."
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


        [HttpGet, Route("GetControlesFiadorGarante")]
        public async Task<IActionResult> GetControlesFiadorGarante()
        {
            try
            {
                var relacionTitular = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.RelacionTitular);
                var genero = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionGenero);
                var estadoCivil = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionEstadoCivil);
                var region = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionRegion);
                var comuna = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionComuna);
                var nacionalidad = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionNacionalidad);
                var siNo = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionSiNo);

                return Ok(new
                {
                    status = true,
                    detail = new
                    {
                        relacion_titular = relacionTitular,
                        genero = genero,
                        estado_civil = estadoCivil,
                        region = region,
                        comuna = comuna,
                        nacionalidad = nacionalidad,
                        si_no = siNo
                    },
                    message = "Controles de Fiador/Garante obtenidos correctamente."
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


        [HttpGet, Route("GetControlesBancoAcreedor")]
        public async Task<IActionResult> GetControlesBancoAcreedor()
        {
            try
            {
                var bancoAcreedorInstitucion = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionBancoAcreedorInstitucion);
                var siNo = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionSiNo);

                return Ok(new
                {
                    status = true,
                    detail = new
                    {
                        banco_acreedor_institucion = bancoAcreedorInstitucion,
                        si_no = siNo
                    },
                    message = "Controles de Banco Acreedor obtenidos correctamente."
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


        [HttpGet, Route("GetControlesPropiedad")]
        public async Task<IActionResult> GetControlesPropiedad()
        {
            try
            {
                var tipoPropiedad = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionTipoPropiedad);
                var estadoPropiedad = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionEstadoPropiedad);
                var tipoVenta = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionTipoVenta);
                var tipoConstruccion = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionTipoConstruccion);
                var tipoDireccion = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionTipoDireccion);
                var existeRolAvaluo = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionExisteRolAvaluo);
                var region = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionRegion);
                var comuna = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionComuna);
                var siNo = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionSiNo);

                return Ok(new
                {
                    status = true,
                    detail = new
                    {
                        tipo_propiedad = tipoPropiedad,
                        estado_propiedad = estadoPropiedad,
                        tipo_venta = tipoVenta,
                        tipo_construccion = tipoConstruccion,
                        tipo_direccion = tipoDireccion,
                        existe_rol_avaluo = existeRolAvaluo,
                        region = region,
                        comuna = comuna,
                        si_no = siNo
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
