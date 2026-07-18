using Framework.WorkFlow.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca.RevisarDatosOperacion;

using Multibanca.Common;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca.RevisarDatosOperacion;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevisarDatosOperacionController : ControllerBase
    {
        private readonly IRevisarDatosOperacionApplication RevisarDatosOperacionApplicationProvider;
        private readonly IRevisarDatosOperacionPropiedadApplication RevisarDatosOperacionPropiedadApplicationProvider;
        private readonly IRevisarDatosOperacionCreditoApplication RevisarDatosOperacionCreditoApplicationProvider;
        private readonly IRevisarDatosOperacionBancoApplication RevisarDatosOperacionBancoApplicationProvider;
        private readonly IRevisarDatosOperacionVendedorApplication DatosDelVendedorProvider;
        private readonly IRevisarDatosOperacionCompradorApplication DatosDelCompradorProvider;
        private readonly IRevisarDatosOperacionFiadorGaranteApplication RevisarDatosOperacionFiadorGaranteApplicationProvider;
        private readonly ICommonApplication CommonApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = Constants.Actividades.RevisarIngresoDatosOperacion;

        public RevisarDatosOperacionController(
            IRevisarDatosOperacionApplication revisarDatosOperacionApplication,
            IRevisarDatosOperacionPropiedadApplication revisarDatosOperacionPropiedadApplication,
            IRevisarDatosOperacionCreditoApplication revisarDatosOperacionCreditoApplication,
            IRevisarDatosOperacionBancoApplication revisarDatosOperacionBancoApplication,
            IRevisarDatosOperacionVendedorApplication datosDelVendedorApplication,
            IRevisarDatosOperacionCompradorApplication datosDelCompradorApplication,
            IRevisarDatosOperacionFiadorGaranteApplication revisarDatosOperacionFiadorGaranteApplication,
            IBitacoraApplication bitacoraApplication,
            ICommonApplication commonApplication)
        {
            RevisarDatosOperacionApplicationProvider = revisarDatosOperacionApplication;
            RevisarDatosOperacionPropiedadApplicationProvider = revisarDatosOperacionPropiedadApplication;
            RevisarDatosOperacionCreditoApplicationProvider = revisarDatosOperacionCreditoApplication;
            RevisarDatosOperacionBancoApplicationProvider = revisarDatosOperacionBancoApplication;
            DatosDelVendedorProvider = datosDelVendedorApplication;
            DatosDelCompradorProvider = datosDelCompradorApplication;
            RevisarDatosOperacionFiadorGaranteApplicationProvider = revisarDatosOperacionFiadorGaranteApplication;
            BitacoraApplicacionProvider = bitacoraApplication;
            CommonApplicationProvider = commonApplication;
        }

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await RevisarDatosOperacionApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Revisar Datos Operación obtenido correctamente."
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

        [HttpGet, Route("GetRevisarDatosOperacionByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetRevisarDatosOperacionByExpediente(long idExpediente)
        {
            try
            {
                var result = await RevisarDatosOperacionBancoApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Banco Acreedor obtenido correctamente."
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

        [HttpGet, Route("GetPropiedadByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetPropiedadByExpediente(long idExpediente)
        {
            try
            {
                var result = await RevisarDatosOperacionPropiedadApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Propiedad de revisión obtenida correctamente."
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

        [HttpGet, Route("GetCreditoByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetCreditoByExpediente(long idExpediente)
        {
            try
            {
                var result = await RevisarDatosOperacionCreditoApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Crédito de revisión obtenido correctamente."
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

        [HttpGet, Route("GetControlesCredito")]
        public async Task<IActionResult> GetControlesCredito()
        {
            try
            {
                var tipoOperacion = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionTipoOperacion);
                var siNo = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionSiNo);

                return Ok(new
                {
                    status = true,
                    detail = new
                    {
                        tipo_operacion = tipoOperacion,
                        si_no = siNo
                    },
                    message = "Controles de Crédito obtenidos correctamente."
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


        [HttpGet, Route("GetFiadoresByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetFiadoresByExpediente(long idExpediente)
        {
            try
            {
                var result = await RevisarDatosOperacionFiadorGaranteApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Fiadores/Garantes obtenidos correctamente."
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

        [HttpGet, Route("GetControlesRevisarDatosOperacionBanco")]
        public async Task<IActionResult> GetControlesRevisarDatosOperacionBanco()
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

        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpPost, Route("Save")]
        public async Task<IActionResult> Save([FromBody] revisar_datos_operacion revisar_datos_operacion)
        {
            IActionResult response = Unauthorized();

            try
            {
                if (revisar_datos_operacion == null)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "El modelo de Revisar Datos Operación es obligatorio."
                    });
                }

                if (revisar_datos_operacion.id_expediente <= 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "El expediente es obligatorio."
                    });
                }

                int userId = GetUserId();

                if (revisar_datos_operacion.id_revisar_datos_operacion == 0)
                {
                    revisar_datos_operacion.is_active = true;
                    revisar_datos_operacion.row_status = true;

                    var result = RevisarDatosOperacionApplicationProvider.Create(revisar_datos_operacion, userId);
                    revisar_datos_operacion.id_revisar_datos_operacion = result.id_revisar_datos_operacion;
                    revisar_datos_operacion.id_expediente = result.id_expediente;
                }
                else
                {
                    var result = RevisarDatosOperacionApplicationProvider.Update(revisar_datos_operacion, userId);
                    revisar_datos_operacion.id_revisar_datos_operacion = result.id_revisar_datos_operacion;
                    revisar_datos_operacion.id_expediente = result.id_expediente;
                }

                if (revisar_datos_operacion.credito != null)
                {
                    revisar_datos_operacion.credito.id_revisar_datos_operacion = revisar_datos_operacion.id_revisar_datos_operacion;
                    revisar_datos_operacion.credito.id_expediente = revisar_datos_operacion.id_expediente;

                    if (revisar_datos_operacion.credito.id_revisar_datos_operacion_credito == 0)
                    {
                        revisar_datos_operacion.credito.is_active = true;
                        revisar_datos_operacion.credito.row_status = true;
                        revisar_datos_operacion.credito = RevisarDatosOperacionCreditoApplicationProvider.Create(revisar_datos_operacion.credito, userId);
                    }
                    else
                    {
                        revisar_datos_operacion.credito = RevisarDatosOperacionCreditoApplicationProvider.Update(revisar_datos_operacion.credito, userId);
                    }
                }

                if (revisar_datos_operacion.propiedad != null)
                {
                    revisar_datos_operacion.propiedad.id_revisar_datos_operacion = revisar_datos_operacion.id_revisar_datos_operacion;
                    revisar_datos_operacion.propiedad.id_expediente = revisar_datos_operacion.id_expediente;

                    if (revisar_datos_operacion.propiedad.id_revisar_datos_operacion_propiedad == 0)
                    {
                        revisar_datos_operacion.propiedad.is_active = true;
                        revisar_datos_operacion.propiedad.row_status = true;
                        revisar_datos_operacion.propiedad = RevisarDatosOperacionPropiedadApplicationProvider.Create(revisar_datos_operacion.propiedad, userId);
                    }
                    else
                    {
                        revisar_datos_operacion.propiedad = RevisarDatosOperacionPropiedadApplicationProvider.Update(revisar_datos_operacion.propiedad, userId);
                    }
                }

                if (revisar_datos_operacion.revisar_datos_operacion_banco != null)
                {
                    revisar_datos_operacion.revisar_datos_operacion_banco.id_revisar_datos_operacion = revisar_datos_operacion.id_revisar_datos_operacion;
                    revisar_datos_operacion.revisar_datos_operacion_banco.id_expediente = revisar_datos_operacion.id_expediente;

                    if (revisar_datos_operacion.revisar_datos_operacion_banco.id_revisar_datos_operacion_banco == 0)
                    {
                        revisar_datos_operacion.revisar_datos_operacion_banco.is_active = true;
                        revisar_datos_operacion.revisar_datos_operacion_banco.row_status = true;
                        revisar_datos_operacion.revisar_datos_operacion_banco = RevisarDatosOperacionBancoApplicationProvider.Create(revisar_datos_operacion.revisar_datos_operacion_banco, userId);
                    }
                    else
                    {
                        revisar_datos_operacion.revisar_datos_operacion_banco = RevisarDatosOperacionBancoApplicationProvider.Update(revisar_datos_operacion.revisar_datos_operacion_banco, userId);
                    }
                }

                if (revisar_datos_operacion.fiadores_garantes != null)
                {
                    for (int i = 0; i < revisar_datos_operacion.fiadores_garantes.Count; i++)
                    {
                        revisar_datos_operacion.fiadores_garantes[i].id_revisar_datos_operacion = revisar_datos_operacion.id_revisar_datos_operacion;
                        revisar_datos_operacion.fiadores_garantes[i].id_expediente = revisar_datos_operacion.id_expediente;
                        if (revisar_datos_operacion.fiadores_garantes[i].id_revisar_datos_operacion_fiador_garante <= 0)
                        {
                            if (revisar_datos_operacion.fiadores_garantes[i].row_status == false)
                                continue;
                            revisar_datos_operacion.fiadores_garantes[i].id_revisar_datos_operacion_fiador_garante = 0;
                            revisar_datos_operacion.fiadores_garantes[i].is_active = true;
                            revisar_datos_operacion.fiadores_garantes[i].row_status = true;
                            revisar_datos_operacion.fiadores_garantes[i] = RevisarDatosOperacionFiadorGaranteApplicationProvider.Create(revisar_datos_operacion.fiadores_garantes[i], userId);
                        }
                        else
                        {
                            revisar_datos_operacion.fiadores_garantes[i] = RevisarDatosOperacionFiadorGaranteApplicationProvider.Update(revisar_datos_operacion.fiadores_garantes[i], userId);
                        }
                    }
                }

                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(
                    revisar_datos_operacion.id_expediente,
                    ActividadID);

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = revisar_datos_operacion.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = userId;
                bitacora.fecha_alta = DateTime.Now;
                bitacora.is_active = true;
                bitacora.row_status = true;

                if (resultBitacora.id_bitacora > 0)
                {
                    bitacora.id_bitacora = resultBitacora.id_bitacora;
                    bitacora.observaciones = "Se actualizó la actividad Revisar Datos Operacion.";
                    BitacoraApplicacionProvider.Update(bitacora, userId);
                }
                else
                {
                    bitacora.observaciones = "Se guardó la actividad Revisar Datos Operacion.";
                    BitacoraApplicacionProvider.Create(bitacora, userId);
                }

                response = Ok(new
                {
                    status = true,
                    detail = revisar_datos_operacion,
                    message = "Revisar Datos Operación guardado correctamente."
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
                    await RevisarDatosOperacionApplicationProvider.Avanzar(id_expediente, id_usuario, ActividadID);

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


        [HttpGet, Route("GetVendedoresByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetVendedoresByExpediente(long idExpediente)
        {
            try
            {
                var result = await DatosDelVendedorProvider.GetListByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Datos del Vendedor obtenidos correctamente."
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
        [HttpPost, Route("SaveVendedor")]
        public async Task<IActionResult> SaveVendedor([FromBody] revisar_datos_operacion_vendedor revisar_datos_operacion_vendedor)
        {
            IActionResult response = Unauthorized();

            try
            {
                int userId = GetUserId();

                if (revisar_datos_operacion_vendedor.id_revisar_datos_operacion_vendedor == 0)
                {
                    revisar_datos_operacion_vendedor.is_active = true;
                    revisar_datos_operacion_vendedor.row_status = true;
                    revisar_datos_operacion_vendedor = DatosDelVendedorProvider.Create(revisar_datos_operacion_vendedor, userId);
                }
                else
                {
                    revisar_datos_operacion_vendedor = DatosDelVendedorProvider.Update(revisar_datos_operacion_vendedor, userId);
                }

                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(
                    revisar_datos_operacion_vendedor.id_expediente,
                    ActividadID);

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = revisar_datos_operacion_vendedor.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = userId;
                bitacora.fecha_alta = DateTime.Now;
                bitacora.observaciones = "Se guardó la actividad Datos del Vendedor.";
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
                    detail = revisar_datos_operacion_vendedor,
                    message = "Datos del Vendedor guardados correctamente."
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
        [HttpDelete, Route("DeleteVendedor/{id}")]
        public IActionResult DeleteVendedor(int id)
        {
            try
            {
                int userId = GetUserId();
                DatosDelVendedorProvider.LogicalDeleteById(id, userId);

                return Ok(new
                {
                    status = true,
                    message = "Vendedor eliminado correctamente."
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
                var siNo = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionSiNo);
                var tipoVendedor = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionTipoVendedor);
                var genero = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionGenero);
                var estadoCivil = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionEstadoCivil);
                var relacionTitular = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.RelacionTitular);
                var region = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionRegion);
                var comuna = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionComuna);
                var nacionalidad = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionNacionalidad);
                var tipoDireccionDividendo = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionTipoDireccionDividendo);

                return Ok(new
                {
                    status = true,
                    detail = new
                    {
                        si_no = siNo,
                        tipo_vendedor = tipoVendedor,
                        genero,
                        estado_civil = estadoCivil,
                        relacion_titular = relacionTitular,
                        region,
                        comuna,
                        nacionalidad,
                        tipo_direccion_dividendo = tipoDireccionDividendo
                    },
                    message = "Controles de Datos del Vendedor obtenidos correctamente."
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


        [HttpGet, Route("GetCompradoresByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetCompradoresByExpediente(long idExpediente)
        {
            try
            {
                var result = await DatosDelCompradorProvider.GetListByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Datos del Comprador obtenidos correctamente."
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
        [HttpPost, Route("SaveComprador")]
        public async Task<IActionResult> SaveComprador([FromBody] revisar_datos_operacion_comprador revisar_datos_operacion_comprador)
        {
            IActionResult response = Unauthorized();

            try
            {
                int userId = GetUserId();

                if (revisar_datos_operacion_comprador.id_revisar_datos_operacion_comprador == 0)
                {
                    revisar_datos_operacion_comprador.is_active = true;
                    revisar_datos_operacion_comprador.row_status = true;
                    revisar_datos_operacion_comprador = DatosDelCompradorProvider.Create(revisar_datos_operacion_comprador, userId);
                }
                else
                {
                    revisar_datos_operacion_comprador = DatosDelCompradorProvider.Update(revisar_datos_operacion_comprador, userId);
                }

                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(
                    revisar_datos_operacion_comprador.id_expediente,
                    ActividadID);

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = revisar_datos_operacion_comprador.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = userId;
                bitacora.fecha_alta = DateTime.Now;
                bitacora.observaciones = "Se guardó la actividad Datos del Comprador.";
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
                    detail = revisar_datos_operacion_comprador,
                    message = "Datos del Comprador guardados correctamente."
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
        [HttpDelete, Route("DeleteComprador/{id}")]
        public IActionResult DeleteComprador(int id)
        {
            try
            {
                int userId = GetUserId();
                DatosDelCompradorProvider.LogicalDeleteById(id, userId);

                return Ok(new
                {
                    status = true,
                    message = "Comprador eliminado correctamente."
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
                var siNo = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionSiNo);
                var tipoComprador = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionTipoVendedor);
                var genero = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionGenero);
                var estadoCivil = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionEstadoCivil);
                var relacionTitular = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.RelacionTitular);
                var region = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionRegion);
                var comuna = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionComuna);
                var nacionalidad = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionNacionalidad);
                var tipoDireccionDividendo = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionTipoDireccionDividendo);

                return Ok(new
                {
                    status = true,
                    detail = new
                    {
                        si_no = siNo,
                        tipo_comprador = tipoComprador,
                        genero,
                        estado_civil = estadoCivil,
                        relacion_titular = relacionTitular,
                        region,
                        comuna,
                        nacionalidad,
                        tipo_direccion_dividendo = tipoDireccionDividendo
                    },
                    message = "Controles de Datos del Comprador obtenidos correctamente."
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
    }
}
