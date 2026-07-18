using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.FuncTransversal;
using Data.Repository.Interfaces.Repositories.FuncTransversal;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Common;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.DTO.Common;
using Multibanca.DTO.FuncTransversal;
using System.Web;

namespace Multibanca.Application.Implementations.FuncTransversal
{
    public class S3Access
    {
        public AmazonS3Client Client { get; set; }
        public string BucketName { get; set; }
        public string PathPrefix { get; set; }
    }

    public class ExpedienteDigitalApplication : MultibancaGenericApplication<expediente_digital, expediente_digital_entity, IExpedienteDigitalRepository>, IExpedienteDigitalApplication
    {
        private readonly IExpedienteDigitalRepository ExpedienteDigitalRepositoryProvider;
        private readonly IExpedienteDigitalMongoRepository ExpedienteDigitalMongoRepositoryProvider;
        private readonly IMapper Mapper;

        public ExpedienteDigitalApplication(
            MultibancaDBContext _multibancaDBContext,
            IExpedienteDigitalRepository _expedienteDigitalRepository,
            IExpedienteDigitalMongoRepository _expedienteDigitalMongoRepository,
            IMapper _mapper) : base(_multibancaDBContext, _expedienteDigitalRepository, _mapper)
        {
            ExpedienteDigitalRepositoryProvider = _expedienteDigitalRepository;
            ExpedienteDigitalMongoRepositoryProvider = _expedienteDigitalMongoRepository;
            Mapper = _mapper;
        }

        public async Task<List<ControlBaseDTO>> GetCatalogoCategoriaExpedienteDigital()
        {
            return await ExpedienteDigitalRepositoryProvider.GetCatalogoCategoriaExpedienteDigital();
        }

        public async Task<List<ControlBaseDTO>> GetCatalogoDocumentos(int idExpedienteDigital, long idExpediente)
        {
            return await ExpedienteDigitalRepositoryProvider.GetCatalogoDocumentos(idExpedienteDigital, idExpediente);
        }

        public async Task<List<ControlBaseDTO>> GetCatalogoDocumentosCompleto()
        {
            return await ExpedienteDigitalRepositoryProvider.GetCatalogoDocumentosCompleto();
        }

        /// <summary>
        /// Consulta archivos del expediente digital y conserva el filtro por actividad solicitado por la capa API.
        /// </summary>
        /// <param name="idExpediente">Identificador del expediente.</param>
        /// <param name="activityId">Actividad workflow usada para limitar el listado.</param>
        /// <returns>Archivos del expediente que cumplen el filtro solicitado.</returns>
        public async Task<List<expediente_digital>> GetFilesExpedienteDigital(long idExpediente, string? activityId = null)
        {
            List<expediente_digital_mongo_entity> expedienteDigitalEntity =
                await ExpedienteDigitalMongoRepositoryProvider.GetFilesExpedienteDigital(idExpediente, activityId);

            return Mapper.Map<List<expediente_digital>>(expedienteDigitalEntity);
        }

        public async Task<List<expediente_digital>> GetFilesByActividad(long idExpediente, string actividadId)
        {
            List<expediente_digital_mongo_entity> entities =
                await ExpedienteDigitalMongoRepositoryProvider.GetFilesByActividad(idExpediente, actividadId);

            return Mapper.Map<List<expediente_digital>>(entities);
        }

        public async Task<expediente_digital?> GetFileExpedienteDigitalById(long idArchivo)
        {
            expediente_digital_mongo_entity? entity =
                await ExpedienteDigitalMongoRepositoryProvider.GetById(idArchivo);

            return Mapper.Map<expediente_digital?>(entity);
        }

        public async Task<expediente_digital> SaveMetadataMongo(expediente_digital expedienteDigital, int idUsuario)
        {
            expedienteDigital.id_usuario = idUsuario;
            expedienteDigital.extension = Path.GetExtension(expedienteDigital.nombre_archivo_original);
            expedienteDigital.version_archivo = await ExpedienteDigitalMongoRepositoryProvider.GetNextVersion(
                expedienteDigital.id_expediente,
                expedienteDigital.id_documento
            );

            if (string.IsNullOrWhiteSpace(expedienteDigital.nombre_archivo))
            {
                string fileNameVersion = await FileNameVersion(expedienteDigital.id_expediente, expedienteDigital.id_documento);
                expedienteDigital.nombre_archivo = $"{fileNameVersion}{expedienteDigital.extension}";
            }

            expedienteDigital.fecha_alta = DateTime.Now;
            expedienteDigital.storage_provider = string.IsNullOrWhiteSpace(expedienteDigital.storage_provider)
                ? "local"
                : expedienteDigital.storage_provider;
            expedienteDigital.storage_path = Path.Combine(
                expedienteDigital.id_expediente.ToString(),
                expedienteDigital.nombre_archivo_original
            );

            CompleteFileMetadata(expedienteDigital);

            expedienteDigital.is_active = true;
            expedienteDigital.row_status = true;
            expedienteDigital.created_by = idUsuario;
            expedienteDigital.created_date = DateTime.Now;

            expediente_digital_mongo_entity entity = Mapper.Map<expediente_digital_mongo_entity>(expedienteDigital);

            expediente_digital_mongo_entity result = await ExpedienteDigitalMongoRepositoryProvider.Create(entity);

            await ExpedienteDigitalMongoRepositoryProvider.DeactivatePreviousVersions(
                result.id_expediente,
                result.id_documento,
                result.id_archivo,
                idUsuario
            );

            return Mapper.Map<expediente_digital>(result);
        }

        public async Task<expediente_digital?> UpdateMetadataMongo(expediente_digital expedienteDigital, int idUsuario)
        {
            expediente_digital_mongo_entity? current =
                await ExpedienteDigitalMongoRepositoryProvider.GetById(expedienteDigital.id_archivo);

            if (current == null)
            {
                return null;
            }

            current.id_documento = expedienteDigital.id_documento;
            current.comentarios = expedienteDigital.comentarios;
            current.is_active = expedienteDigital.is_active;
            current.row_status = expedienteDigital.row_status;
            current.modified_by = idUsuario;
            current.modified_date = DateTime.Now;

            expediente_digital_mongo_entity? result =
                await ExpedienteDigitalMongoRepositoryProvider.Update(current);

            return Mapper.Map<expediente_digital?>(result);
        }

        public async Task<string> FileNameVersion(long idExpediente, long idDocumento)
        {
            string nombreDocumento = await ExpedienteDigitalRepositoryProvider.GetDocumentoNombreCorto(idDocumento);

            if (string.IsNullOrWhiteSpace(nombreDocumento))
            {
                nombreDocumento = $"documento_{idDocumento}";
            }

            nombreDocumento = Path.GetFileNameWithoutExtension(nombreDocumento);
            nombreDocumento = SanitizeFileName(nombreDocumento);

            int version = await ExpedienteDigitalMongoRepositoryProvider.GetNextVersion(
                idExpediente,
                Convert.ToInt32(idDocumento)
            );

            string nombreBase = $"{nombreDocumento}_{idExpediente}";

            if (version <= 1)
            {
                return nombreBase;
            }

            return $"{nombreBase}_{version}";
        }

        private static string SanitizeFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return "documento";
            }

            char[] invalidChars = Path.GetInvalidFileNameChars();

            string sanitized = new string(
                fileName.Select(character => invalidChars.Contains(character) ? '_' : character).ToArray()
            );

            return sanitized.Trim();
        }

        public async Task<UserResponsibleDTO> GetUserResponsibleByIdExpediente(long idExpediente)
        {
            return await ExpedienteDigitalRepositoryProvider.GetUserResponsibleByIdExpediente(idExpediente);
        }

        public async Task<string> GetTemplateFileName(int idCatExpedienteDigital, int idCatExpedienteDigitalDocumento)
        {
            return await ExpedienteDigitalRepositoryProvider.GetTemplateFileName(idCatExpedienteDigital, idCatExpedienteDigitalDocumento);
        }

        private static void CompleteFileMetadata(expediente_digital expedienteDigital)
        {
            if (string.IsNullOrWhiteSpace(expedienteDigital.mime_type))
            {
                expedienteDigital.mime_type = GetMimeType(expedienteDigital.nombre_archivo_original);
            }

            if (!expedienteDigital.file_size.HasValue || expedienteDigital.file_size <= 0)
            {
                expedienteDigital.file_size = GetLocalFileSize(expedienteDigital);
            }
        }

        private static long? GetLocalFileSize(expediente_digital expedienteDigital)
        {
            if (!string.Equals(expedienteDigital.storage_provider, "local", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(expedienteDigital.storage_path))
            {
                return null;
            }

            string fullPath = Path.Combine(Constants.StorageLocal.filePath, expedienteDigital.storage_path);

            if (!File.Exists(fullPath))
            {
                return null;
            }

            return new FileInfo(fullPath).Length;
        }

        private static string GetMimeType(string fileName)
        {
            string extension = Path.GetExtension(fileName)?.ToLowerInvariant() ?? string.Empty;

            return extension switch
            {
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".ppt" => "application/vnd.ms-powerpoint",
                ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                ".txt" => "text/plain",
                ".csv" => "text/csv",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".tif" or ".tiff" => "image/tiff",
                ".zip" => "application/zip",
                ".rar" => "application/vnd.rar",
                ".7z" => "application/x-7z-compressed",
                ".xml" => "application/xml",
                ".json" => "application/json",
                _ => "application/octet-stream"
            };
        }

        #region AWS S3
        public S3Access getS3Client()
        {
            var bucketConfig = Constants.AWS3.BucketConfig;
            string[] configComponents = bucketConfig.Split('|');
            Uri S3AccessUri = new Uri(configComponents[0]);
            string bucketName = "";
            string pathPrefix = "";
            if (configComponents.Length > 1)
            {
                bucketName = configComponents[1];
            }
            if (configComponents.Length > 2)
            {
                pathPrefix = configComponents[2];
            }

            var config = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.USEast1,
            };
            if (S3AccessUri.Scheme.StartsWith("http"))
            {
                config = new AmazonS3Config
                {
                    RegionEndpoint = Amazon.RegionEndpoint.USEast1,
                    ServiceURL = S3AccessUri.Scheme + "://" + S3AccessUri.Host + ":" + S3AccessUri.Port,
                    ForcePathStyle = true,
                    Timeout = new TimeSpan(0, 0, 10),
                };
            }

            S3Access s3Access = new S3Access();
            s3Access.Client = new AmazonS3Client(HttpUtility.UrlDecode(S3AccessUri.UserInfo.Split(':')[0]), HttpUtility.UrlDecode(S3AccessUri.UserInfo.Split(':')[1]), config);
            s3Access.BucketName = bucketName;
            s3Access.PathPrefix = pathPrefix;
            return s3Access;
        }

        public async Task<Stream> DownloadFromAWS(long idDocument)
        {
            expediente_digital? expedienteDigital = await GetFileExpedienteDigitalById(idDocument);
            if (expedienteDigital == null) throw new Exception("No existe metadata del archivo solicitado.");
            string fullFileName = expedienteDigital.id_expediente.ToString() + "/" + expedienteDigital.nombre_archivo_original;
            S3Access clientsS3 = getS3Client();

            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = clientsS3.BucketName,
                Key = (clientsS3.PathPrefix == "") ? fullFileName : clientsS3.PathPrefix + "/" + fullFileName,
            };

            AmazonS3Client client = clientsS3.Client;
            try
            {
                GetObjectResponse response = await client.GetObjectAsync(request);
                return response.ResponseStream;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UploadToAWS(long idExpediente, string fileName, Stream fileContent)
        {
            try
            {
                S3Access clientS3 = getS3Client();

                string fullFileName = idExpediente.ToString() + "/" + fileName;
                using (var fileTransferUtility = new TransferUtility(clientS3.Client))
                {
                    fileTransferUtility.Upload(fileContent, clientS3.BucketName, fullFileName);
                }

                return await ExistsInRepositoryAWS(idExpediente, fileName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> ExistsInRepositoryAWS(long idExpediente, string fileName)
        {
            S3Access clientS3 = getS3Client();
            try
            {
                string fullFileName = idExpediente.ToString() + "/" + fileName;
                AmazonS3Client client = clientS3.Client;
                GetObjectMetadataResponse response = await client.GetObjectMetadataAsync(clientS3.BucketName, (clientS3.PathPrefix == "") ? fullFileName : clientS3.PathPrefix + "/" + fullFileName);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Local
        public async Task<Stream> DownloadFromLocal(long idDocument)
        {
            string pathLocal = Constants.StorageLocal.filePath;
            expediente_digital? expedienteDigital = await GetFileExpedienteDigitalById(idDocument);
            FileStream fs = null;

            if (expedienteDigital == null)
            {
                return fs;
            }

            if (File.Exists(Path.Combine(pathLocal, expedienteDigital.id_expediente.ToString(), expedienteDigital.nombre_archivo_original)))
            {
                fs = File.OpenRead(Path.Combine(pathLocal, expedienteDigital.id_expediente.ToString(), expedienteDigital.nombre_archivo_original));
            }
            return fs;
        }

        public async Task<bool> UploadToLocal(long idExpediente, string fileName, Stream fileContent)
        {
            try
            {
                string path = Path.Combine(Constants.StorageLocal.filePath, idExpediente.ToString());

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using (var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    await fileContent.CopyToAsync(stream);

                    return File.Exists(Path.Combine(path, fileName));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
