using Multibanca.Application.Interfaces.Workflow;
using Framework.WorkFlow.Common.DTO;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Backend.Api.Extensions;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.CargaOperacionBanco;
using Multibanca.Common;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;
using Newtonsoft.Json;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargaOperacionBancoController : ControllerBase
    {
        private readonly ICargaOperacionBancoApplication CargaOperacionBancoApplicationProvider;
        private readonly ICargaOperacionBancoDatosOperacionApplication CargaOperacionBancoDatosOperacionApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;
        private readonly ICommonApplication CommonApplicationProvider;
        private readonly ICargaOperacionBancoAntecedenteCompradorApplication CargaOperacionBancoAntecedenteCompradorApplicationProvider;
        private readonly ICargaOperacionBancoAntecedenteVendedorApplication CargaOperacionBancoAntecedenteVendedorApplicationProvider;
        private readonly ICargaOperacionBancoAntecedenteCreditoApplication CargaOperacionBancoAntecedenteCreditoApplicationProvider;
        private readonly ICargaOperacionBancoDatosComercialApplication CargaOperacionBancoDatosComercialApplicationProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IIdempotencyStore IdempotencyStoreProvider;

        private readonly string ActividadID = Constants.Actividades.CargaOperacionBanco;

        public CargaOperacionBancoController(
            ICargaOperacionBancoApplication _cargaOperacionBancoApplication,
            ICargaOperacionBancoDatosOperacionApplication _cargaOperacionBancoDatosOperacionApplication,
            ICargaOperacionBancoAntecedenteCompradorApplication _cargaOperacionBancoAntecedenteCompradorApplication,
            IBitacoraApplication _bitacoraApplication, ICommonApplication _commonApplication,
            ICargaOperacionBancoAntecedenteVendedorApplication _cargaOperacionBancoAntecedenteVendedorApplication,
            ICargaOperacionBancoAntecedenteCreditoApplication _cargaOperacionBancoAntecedenteCreditoApplication,
            ICargaOperacionBancoDatosComercialApplication _cargaOperacionBancoDatosComercialApplication,
            IWorkflowApplication _workflowApplication,
            IIdempotencyStore _idempotencyStore)
        {
            CargaOperacionBancoApplicationProvider = _cargaOperacionBancoApplication;
            CargaOperacionBancoDatosOperacionApplicationProvider = _cargaOperacionBancoDatosOperacionApplication;
            CargaOperacionBancoAntecedenteCompradorApplicationProvider = _cargaOperacionBancoAntecedenteCompradorApplication;
            CargaOperacionBancoAntecedenteVendedorApplicationProvider = _cargaOperacionBancoAntecedenteVendedorApplication;
            CargaOperacionBancoAntecedenteCreditoApplicationProvider = _cargaOperacionBancoAntecedenteCreditoApplication;
            CargaOperacionBancoDatosComercialApplicationProvider = _cargaOperacionBancoDatosComercialApplication;
            WorkflowApplicationProvider = _workflowApplication;
            BitacoraApplicacionProvider = _bitacoraApplication;
            CommonApplicationProvider = _commonApplication;
            IdempotencyStoreProvider = _idempotencyStore;
        }

        // ============================================================
        // ACTIVIDAD BASE: CARGA OPERACIÓN BANCO
        // ============================================================

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await CargaOperacionBancoApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Carga Operación Banco obtenida correctamente."
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
        // ACTIVIDAD BASE: ANTECEDENTES DEL COMPRADOR
        // ============================================================

        [HttpGet, Route("GetAntecedentesCompradorByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetAntecedentesCompradorByExpediente(long idExpediente)
        {
            try
            {
                var result = await CargaOperacionBancoAntecedenteCompradorApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Antecedentes del Comprador obtenidos correctamente."
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
        public async Task<IActionResult> Save([FromBody] carga_operacion_banco carga_operacion_banco)
        {
            IActionResult response = Unauthorized();
            FolioDTO folio = new FolioDTO();
            try
            {
                int userId = GetUserId();

                string? idempotencyKey = Request.Headers["Idempotency-Key"].FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(idempotencyKey))
                {
                    var existing = await IdempotencyStoreProvider.Get(idempotencyKey);
                    if (existing is not null)
                    {
                        var cachedEntity = JsonConvert.DeserializeObject<carga_operacion_banco>(existing.ResponseSnapshot);
                        return Ok(new
                        {
                            status = true,
                            detail = cachedEntity,
                            message = "Carga Operación Banco guardada correctamente."
                        });
                    }
                }

                string? validationError = ValidateCargaOperacionBanco(carga_operacion_banco);
                if (validationError != null)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = validationError
                    });
                }

                if (carga_operacion_banco.id_expediente == 0)
                {
                    FolioDTO AuxfolioDTO = await WorkflowApplicationProvider.CreateCase("Principal", Constants.WorkFlowMultibanca.WorkFlowID, GetPerformerId(), GetUserId());
                    carga_operacion_banco.id_expediente = AuxfolioDTO.id_expediente;
                    List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(AuxfolioDTO.id_actividad);

                    var result = CargaOperacionBancoApplicationProvider.Create(carga_operacion_banco, GetUserId());
                    carga_operacion_banco.id_carga_operacion_banco = result.id_carga_operacion_banco;
                    carga_operacion_banco.id_expediente = result.id_expediente;
                    
                    // AVANCE DE ACTIVIDAD  
                    folio = await WorkflowApplicationProvider.CapturarDatosFolio(carga_operacion_banco.id_expediente, string.Empty);
                    string transitionsID = listTransitions.Where(x => x.name == "StartEvent_CargaOperacionBanco").Select(q => q.transition_id).FirstOrDefault() ?? "";
                    await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, carga_operacion_banco.created_by);
                }
                else
                {
                    var result = CargaOperacionBancoApplicationProvider.Update(carga_operacion_banco,userId);
                    carga_operacion_banco.id_carga_operacion_banco = result.id_carga_operacion_banco;
                    carga_operacion_banco.id_expediente = result.id_expediente;                    
                }

                if (carga_operacion_banco.datos_operacion != null)
                {
                    carga_operacion_banco.datos_operacion.id_carga_operacion_banco = carga_operacion_banco.id_carga_operacion_banco;

                    carga_operacion_banco.datos_operacion.id_expediente = carga_operacion_banco.id_expediente;

                    if (carga_operacion_banco.datos_operacion.id_carga_operacion_banco_datos_operacion == 0)
                    {
                        carga_operacion_banco.datos_operacion.is_active = true;
                        carga_operacion_banco.datos_operacion.row_status = true;

                        carga_operacion_banco.datos_operacion = CargaOperacionBancoDatosOperacionApplicationProvider.Create(carga_operacion_banco.datos_operacion, userId);
                    }
                    else
                    {
                        carga_operacion_banco.datos_operacion =
                            CargaOperacionBancoDatosOperacionApplicationProvider.Update(carga_operacion_banco.datos_operacion, userId);
                    }
                }

                if (carga_operacion_banco.antecedentes_comprador != null)
                {
                    for (int i = 0; i < carga_operacion_banco.antecedentes_comprador.Count; i++)
                    {
                        var comprador = carga_operacion_banco.antecedentes_comprador[i];
                        comprador.id_carga_operacion_banco = carga_operacion_banco.id_carga_operacion_banco;
                        comprador.id_expediente = carga_operacion_banco.id_expediente;

                        if (comprador.id_carga_operacion_banco_antecedente_comprador == 0)
                        {
                            comprador.is_active = true;
                            comprador.row_status = true;

                            carga_operacion_banco.antecedentes_comprador[i] =
                                CargaOperacionBancoAntecedenteCompradorApplicationProvider.Create(comprador, userId);
                        }
                        else
                        {
                            carga_operacion_banco.antecedentes_comprador[i] =
                                CargaOperacionBancoAntecedenteCompradorApplicationProvider.Update(comprador, userId);
                        }
                    }
                }

                if (carga_operacion_banco.antecedentes_vendedor != null)
                {
                    for (int i = 0; i < carga_operacion_banco.antecedentes_vendedor.Count; i++)
                    {
                        var vendedor = carga_operacion_banco.antecedentes_vendedor[i];
                        vendedor.id_carga_operacion_banco = carga_operacion_banco.id_carga_operacion_banco;
                        vendedor.id_expediente = carga_operacion_banco.id_expediente;

                        if (vendedor.id_carga_operacion_banco_antecedente_vendedor == 0)
                        {
                            vendedor.is_active = true;
                            vendedor.row_status = true;

                            carga_operacion_banco.antecedentes_vendedor[i] =
                                CargaOperacionBancoAntecedenteVendedorApplicationProvider.Create(vendedor, userId);
                        }
                        else
                        {
                            carga_operacion_banco.antecedentes_vendedor[i] =
                                CargaOperacionBancoAntecedenteVendedorApplicationProvider.Update(vendedor, userId);
                        }
                    }
                }

                if (carga_operacion_banco.antecedente_credito != null)
                {
                    carga_operacion_banco.antecedente_credito.id_carga_operacion_banco = carga_operacion_banco.id_carga_operacion_banco;

                    carga_operacion_banco.antecedente_credito.id_expediente = carga_operacion_banco.id_expediente;

                    if (carga_operacion_banco.antecedente_credito.id_carga_operacion_banco_antecedente_credito == 0)
                    {
                        carga_operacion_banco.antecedente_credito.is_active = true;
                        carga_operacion_banco.antecedente_credito.row_status = true;

                        carga_operacion_banco.antecedente_credito = CargaOperacionBancoAntecedenteCreditoApplicationProvider.Create(carga_operacion_banco.antecedente_credito, userId);
                    }
                    else
                    {
                        carga_operacion_banco.antecedente_credito = CargaOperacionBancoAntecedenteCreditoApplicationProvider.Update(carga_operacion_banco.antecedente_credito, userId);
                    }
                }

                if (carga_operacion_banco.datos_comercial != null)
                {
                    carga_operacion_banco.datos_comercial.id_carga_operacion_banco = carga_operacion_banco.id_carga_operacion_banco;

                    carga_operacion_banco.datos_comercial.id_expediente = carga_operacion_banco.id_expediente;

                    if (carga_operacion_banco.datos_comercial.id_carga_operacion_banco_datos_comercial == 0)
                    {
                        carga_operacion_banco.datos_comercial.is_active = true;
                        carga_operacion_banco.datos_comercial.row_status = true;

                        carga_operacion_banco.datos_comercial = CargaOperacionBancoDatosComercialApplicationProvider.Create(carga_operacion_banco.datos_comercial, userId);
                    }
                    else
                    {
                        carga_operacion_banco.datos_comercial = CargaOperacionBancoDatosComercialApplicationProvider.Update(carga_operacion_banco.datos_comercial, userId);
                    }
                }

                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(carga_operacion_banco.id_expediente, ActividadID);

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = carga_operacion_banco.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = userId;
                bitacora.fecha_alta = DateTime.Now;
                bitacora.observaciones = "Se guardó la actividad Carga Operación Banco.";
                bitacora.is_active = true;
                bitacora.row_status = true;
                if (resultBitacora.id_bitacora > 0)
                {
                    bitacora.id_bitacora = resultBitacora.id_bitacora;
                    BitacoraApplicacionProvider.Update(bitacora, userId);
                }
                else
                    BitacoraApplicacionProvider.Create(bitacora, userId);

                if (!string.IsNullOrWhiteSpace(idempotencyKey))
                {
                    string snapshot = JsonConvert.SerializeObject(carga_operacion_banco);
                    await IdempotencyStoreProvider.Save(idempotencyKey, carga_operacion_banco.id_expediente, snapshot);
                }

                response = Ok(new
                {
                    status = true,
                    detail = carga_operacion_banco,
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

        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpGet, Route("Avanzar/{id_expediente}")]
        public async Task<IActionResult> Avanzar(long id_expediente)
        {
            IActionResult response = Unauthorized();

            try
            {
                int id_usuario = GetUserId();

                List<AssignActivityDTO> listAssignActividadesDTO =
                    await CargaOperacionBancoApplicationProvider.Avanzar(id_expediente, id_usuario, ActividadID);

                if (listAssignActividadesDTO.Count() > 0)
                {
                    response = Ok(new
                    {
                        status = true,
                        detail = "",
                        message = "El expediente: " + id_expediente + " fue avanzado correctamente a la siguiente etapa del proceso"
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
        // SECCIÓN: DATOS DE OPERACIÓN
        // ============================================================

        [HttpGet, Route("GetDatosOperacionByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetDatosOperacionByExpediente(long idExpediente)
        {
            try
            {
                var result = await CargaOperacionBancoDatosOperacionApplicationProvider.GetByExpediente(idExpediente);

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

        [HttpGet, Route("GetAntecedentesVendedorByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetAntecedentesVendedorByExpediente(long idExpediente)
        {
            try
            {
                var result = await CargaOperacionBancoAntecedenteVendedorApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Antecedentes del Vendedor obtenidos correctamente."
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

        [HttpGet, Route("GetAntecedenteCreditoByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetAntecedenteCreditoByExpediente(long idExpediente)
        {
            try
            {
                var result = await CargaOperacionBancoAntecedenteCreditoApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Antecedente del Crédito obtenido correctamente."
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

        [HttpGet, Route("GetDatosComercialByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetDatosComercialByExpediente(long idExpediente)
        {
            try
            {
                var result = await CargaOperacionBancoDatosComercialApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Datos de Comercial obtenidos correctamente."
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
        // CONTROLES / CATÁLOGOS
        // ============================================================

        [HttpGet, Route("GetControlesDatosOperacion")]
        public async Task<IActionResult> GetControlesDatosOperacion()
        {
            try
            {
                var canalOriginacion = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.CanalOriginacion);
                var tipoInmueble = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.TipoInmueble);
                var estadoInmueble = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.EstadoInmueble);
                var codigoOficina = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.CodigoOficina);

                return Ok(new
                {
                    status = true,
                    detail = new
                    {
                        canal_originacion = canalOriginacion,
                        tipo_inmueble = tipoInmueble,
                        estado_inmueble = estadoInmueble,
                        codigo_oficina = codigoOficina,
                    },
                    message = "Controles de Datos de Operación obtenidos correctamente."
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

        [HttpGet, Route("GetControlesAntecedenteComprador")]
        public async Task<IActionResult> GetControlesAntecedenteComprador()
        {
            try
            {
                var tipoDocumentoId = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.TipoDocumentoId);
                var situacionLaboral = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.SituacionLaboral);

                return Ok(new
                {
                    status = true,
                    detail = new
                    {
                        tipo_documento_id = tipoDocumentoId,
                        situacion_laboral = situacionLaboral
                    },
                    message = "Controles de Antecedente del Comprador obtenidos correctamente."
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

        [HttpGet, Route("GetControlesAntecedenteVendedor")]
        public async Task<IActionResult> GetControlesAntecedenteVendedor()
        {
            try
            {
                var tipoVendedor = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.TipoVendedor);
                var genero = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.Genero);
                var estadoCivil = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.EstadoCivil);
                var relacionTitular = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.RelacionTitular);
                var region = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.Region);
                var comuna = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.Comuna);
                var nacionalidad = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.Nacionalidad);

                return Ok(new
                {
                    status = true,
                    detail = new
                    {
                        tipo_vendedor = tipoVendedor,
                        genero = genero,
                        estado_civil = estadoCivil,
                        relacion_titular = relacionTitular,
                        region = region,
                        comuna = comuna,
                        nacionalidad = nacionalidad
                    },
                    message = "Controles de Antecedente del Vendedor obtenidos correctamente."
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

        [HttpGet, Route("GetControlesAntecedenteCredito")]
        public async Task<IActionResult> GetControlesAntecedenteCredito()
        {
            try
            {
                var tipoSubproducto = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.TipoSubproducto);

                return Ok(new
                {
                    status = true,
                    detail = new
                    {
                        tipo_subproducto = tipoSubproducto
                    },
                    message = "Controles de Antecedente del Crédito obtenidos correctamente."
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

        [HttpGet, Route("GetControlesDatosComercial")]
        public async Task<IActionResult> GetControlesDatosComercial()
        {
            try
            {
                var tipoHipoteca = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.TipoHipoteca);

                return Ok(new
                {
                    status = true,
                    detail = new
                    {
                        tipo_hipoteca = tipoHipoteca
                    },
                    message = "Controles de Datos de Comercial obtenidos correctamente."
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


        #region Validaciones de negocio (BBV-76)
        private string? ValidateCargaOperacionBanco(carga_operacion_banco carga_operacion_banco)
        {
            if (carga_operacion_banco.antecedentes_comprador != null)
            {
                if (!carga_operacion_banco.antecedentes_comprador.Any())
                {
                    return "Debe existir al menos un titular activo por folio.";
                }

                var titular1 = carga_operacion_banco.antecedentes_comprador
                    .FirstOrDefault(c => string.Equals(c.tipo_titular, "T1", StringComparison.OrdinalIgnoreCase))
                    ?? carga_operacion_banco.antecedentes_comprador.First();

                if (string.IsNullOrWhiteSpace(titular1.tipo_documento_id) ||
                    string.IsNullOrWhiteSpace(titular1.numero_identificacion) ||
                    string.IsNullOrWhiteSpace(titular1.nombre_completo))
                {
                    return "El Titular 1 requiere como mínimo tipo_documento_id, numero_identificacion y nombre_completo.";
                }
            }

            if (carga_operacion_banco.antecedente_credito != null)
            {
                if (string.IsNullOrWhiteSpace(carga_operacion_banco.antecedente_credito.id_tipo_sub_producto) ||
                    !carga_operacion_banco.antecedente_credito.monto_otorgado.HasValue)
                {
                    return "id_tipo_sub_producto y monto_otorgado son obligatorios para Guardar/Avanzar.";
                }
            }

            return null;
        }
        #endregion

        #region GetUserId
        private int GetUserId()
        {
            return HttpContext.User.GetUserId();
        }
        #endregion

        #region GetPerformerId
        private string GetPerformerId()
        {
            return HttpContext.User.GetPerformerId();
        }
        #endregion
    }
}
