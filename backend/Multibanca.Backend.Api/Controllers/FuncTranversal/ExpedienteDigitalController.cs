using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Multibanca.Backend.Api.Extensions;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Common;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.DTO.Common;
using Multibanca.DTO.FuncTransversal;

namespace Multibanca.Backend.Api.Controllers.FuncTranversal
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpedienteDigitalController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly IExpedienteDigitalApplication ExpedienteDigitalApplicationProvider;
        private readonly ICommonApplication CommonApplicationProvider;

        public ExpedienteDigitalController(IConfiguration _configuration, IExpedienteDigitalApplication _expedienteDigitalApplicationProvider, ICommonApplication _commonApplication)
        {
            Configuration = _configuration;
            ExpedienteDigitalApplicationProvider = _expedienteDigitalApplicationProvider;
            CommonApplicationProvider = _commonApplication;
        }

        [HttpGet, Route("GetFilesExpedienteDigital/{idExpediente}")]
        /// <summary>
        /// Consulta archivos del expediente digital con filtro opcional por actividad workflow.
        /// </summary>
        /// <param name="idExpediente">Identificador del expediente.</param>
        /// <param name="activityId">Actividad workflow usada para limitar el listado.</param>
        /// <returns>Respuesta HTTP con los archivos del expediente que cumplen el filtro solicitado.</returns>
        public async Task<IActionResult> GetFilesExpedienteDigital(long idExpediente, [FromQuery] string? activityId = null)
        {
            IActionResult response = Unauthorized();
            try
            {
                List<expediente_digital> result = await ExpedienteDigitalApplicationProvider.GetFilesExpedienteDigital(idExpediente, activityId);

                response = Ok(new
                {
                    status = true,
                    detail = result != null ? result : new List<expediente_digital>(),
                    message = "Get Archivos Expediente Digital cargado correctamente."
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

        [HttpGet, Route("GetFilesByActividad/{idExpediente}/{actividadId}")]
        public async Task<IActionResult> GetFilesByActividad(long idExpediente, string actividadId)
        {
            IActionResult response = Unauthorized();
            try
            {
                List<expediente_digital> result = await ExpedienteDigitalApplicationProvider.GetFilesByActividad(idExpediente, actividadId);

                response = Ok(new
                {
                    status = true,
                    detail = result != null ? result : new List<expediente_digital>(),
                    message = "Get Archivos por Actividad cargado correctamente."
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

        [HttpGet, Route("GetCatalogoDocumentos/{idExpediente}/{idExpedienteDigital}")]
        public async Task<IActionResult> GetCatalogoDocumentos(long idExpediente, int idExpedienteDigital)
        {
            IActionResult response = Unauthorized();
            try
            {
                List<ControlBaseDTO> result = await ExpedienteDigitalApplicationProvider.GetCatalogoDocumentos(idExpedienteDigital, idExpediente);

                if (result != null)
                {
                    response = Ok(new
                    {
                        status = true,
                        detail = result,
                        message = "Control Documento cargado correctamente."
                    });
                }
                else
                {
                    response = Ok(new
                    {
                        status = false,
                        detail = new List<ControlBaseDTO>(),
                        message = "No existen registros."
                    });
                }
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

        [HttpGet, Route("GetControlCatExpedienteDigital")]
        public async Task<IActionResult> GetControlCatExpedienteDigital()
        {
            IActionResult response = Unauthorized();
            try
            {
                List<ControlBaseDTO> result = await ExpedienteDigitalApplicationProvider.GetCatalogoCategoriaExpedienteDigital();

                if (result != null)
                {
                    response = Ok(new
                    {
                        status = true,
                        detail = result,
                        message = "Control CatExpedienteDigital cargada correctamente."
                    });
                }
                else
                {
                    response = Ok(new
                    {
                        status = false,
                        detail = new List<ControlBaseDTO>(),
                        message = "No existen registros."
                    });
                }
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

        [HttpPost, Route("UploadFile"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile(IFormCollection data, IFormFile expedienteDigitalFormData)
        {
            IActionResult response = Unauthorized();

            string idExpedienteStr = data["idExpedienteStr"];
            string activityID = data["activityID"];
            string docName = data["docName"];
            string workFlowProcessID = data["workFlowProcessID"];
            long idExpediente = long.Parse(idExpedienteStr);
            bool result = false;
            try
            {
                string server = Configuration["Server_UploadDownload:Server"];
                string fileName = expedienteDigitalFormData.FileName;

                string tempFileName = Path.GetTempFileName();

                using (FileStream outputFileStream = new FileStream(tempFileName, FileMode.Create))
                {
                    await expedienteDigitalFormData.CopyToAsync(outputFileStream);
                }

                if (server == "Local")
                {
                    result = await ExpedienteDigitalApplicationProvider.UploadToLocal(idExpediente, fileName, System.IO.File.OpenRead(tempFileName));

                    response = Ok(new
                    {
                        status = true,
                        detail = result,
                        message = "Archivo subido correctamente."
                    });
                }
                else if (server == "AWS")
                {
                    result = await ExpedienteDigitalApplicationProvider.UploadToAWS(idExpediente, fileName, System.IO.File.OpenRead(tempFileName));

                    response = Ok(new
                    {
                        status = true,
                        detail = result,
                        message = "Archivo subido correctamente."
                    });
                }
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

        [HttpPost, Route("Save")]
        public async Task<IActionResult> SaveExpedienteDigital([FromBody] expediente_digital expedienteDigital)
        {
            IActionResult response = Unauthorized();
            try
            {
                string fileNameVersion = await ExpedienteDigitalApplicationProvider.FileNameVersion(expedienteDigital.id_expediente, expedienteDigital.id_documento);
                expedienteDigital.nombre_archivo = string.Format("{0}{1}", fileNameVersion, Path.GetExtension(expedienteDigital.nombre_archivo_original));
                expediente_digital expediente_Digital = await ExpedienteDigitalApplicationProvider.SaveMetadataMongo(expedienteDigital, User.GetUserId());

                if (expediente_Digital == null) return BadRequest("invalid client Request");


                UserResponsibleDTO userResponsibleDTO = await ExpedienteDigitalApplicationProvider.GetUserResponsibleByIdExpediente(expedienteDigital.id_expediente);
                string message = "Se ha adjuntado un documento en el exp. Digital de la actividad: " + userResponsibleDTO.descripcion;
                string title = "EXP. DIGITAL";
                //await SendNotifications(userResponsibleDTO.idUsuario, message, title);

                response = Ok(new
                {
                    status = true,
                    detail = expediente_Digital,
                    message = "Expediente digital guardada correctamente"
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

        [HttpGet, Route("DownloadFile/{idDocument}"), DisableRequestSizeLimit]
        public async Task<IActionResult> DownloadFile(long idDocument)
        {
            IActionResult response = Unauthorized();
            bool result = false;
            try
            {
                expediente_digital? metadata = await ExpedienteDigitalApplicationProvider.GetFileExpedienteDigitalById(idDocument);
                if (metadata == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                string nameFile = metadata.nombre_archivo_original;
                string server = Configuration["Server_UploadDownload:Server"];
                Stream fileStream = null;
                if (server == "Local")
                {
                    fileStream = await ExpedienteDigitalApplicationProvider.DownloadFromLocal(idDocument);
                }
                else if (server == "AWS")
                {
                    fileStream = await ExpedienteDigitalApplicationProvider.DownloadFromAWS(idDocument);
                }
                if (fileStream != null)
                {
                    MemoryStream memory = new MemoryStream();
                    await fileStream.CopyToAsync(memory);
                    memory.Position = 0;
                    string contentType = GetContentType(nameFile);
                    return File(memory, contentType, nameFile);
                }
                else
                {
                    response = StatusCode(StatusCodes.Status404NotFound);
                }
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

        [HttpGet, Route("GetTemplateFileName/{idCatExpedienteDigital}/{idCatExpedienteDigitalDocumento}")]
        public async Task<IActionResult> GetTemplateFileName(int idCatExpedienteDigital, int idCatExpedienteDigitalDocumento)
        {
            IActionResult response = Unauthorized();
            try
            {
                string templates = await ExpedienteDigitalApplicationProvider.GetTemplateFileName(idCatExpedienteDigital, idCatExpedienteDigitalDocumento);
                response = Ok(new
                {
                    status = true,
                    detail = templates,
                    message = "Template cargado correctamente."
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

        [HttpGet, Route("DownloadTemplateFile/{templateFileName}"), DisableRequestSizeLimit]
        public async Task<IActionResult> DownloadTemplateFile(string templateFileName)
        {
            IActionResult response = Unauthorized();
            bool result = false;
            try
            {
                Stream fileStream = null;
                string currentDirectory = Directory.GetCurrentDirectory();

                if (System.IO.File.Exists(Path.Combine(currentDirectory, Constants.Path.templates, Constants.PathFolder.expediente_digital, templateFileName)))
                {
                    fileStream = System.IO.File.OpenRead(Path.Combine(currentDirectory, Constants.Path.templates, Constants.PathFolder.expediente_digital, templateFileName));
                }

                MemoryStream memory = new MemoryStream();
                await fileStream.CopyToAsync(memory);
                memory.Position = 0;
                string contentType = GetContentType(templateFileName);
                return File(memory, contentType, templateFileName);
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

        [HttpGet, Route("GetCatalogoTipoDocumentoValido")]
        public async Task<IActionResult> GetCatalogoTipoDocumentoValido()
        {
            IActionResult response = Unauthorized();
            try
            {
                List<ControlBaseDTO> listCatalogos = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.TipoDocumentoValido);

                response = Ok(new
                {
                    status = true,
                    detail = listCatalogos,
                    message = "catalogo cargado correctamente"
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

        [HttpPost, Route("UpdEstadoDocumento")]
        public async Task<IActionResult> UpdEstadoDocumento([FromBody] expediente_digital expedienteDigital)
        {
            IActionResult response = Unauthorized();

            try
            {
                expediente_digital? result = await ExpedienteDigitalApplicationProvider.UpdateMetadataMongo(expedienteDigital, User.GetUserId());
                if (result == null) return BadRequest("invalid client Request");

                response = Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Expediente digital actualizada correctamente"
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

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
    }
}
