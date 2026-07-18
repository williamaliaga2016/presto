using Common.Application.Interfaces;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.DTO.Common;
using Multibanca.DTO.FuncTransversal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Interfaces.FuncTransversal
{
    public interface IExpedienteDigitalApplication : IMultibancaGenericApplication<expediente_digital>
    {
        Task<List<ControlBaseDTO>> GetCatalogoCategoriaExpedienteDigital();
        Task<List<ControlBaseDTO>> GetCatalogoDocumentos(int idExpedienteDigital, long idExpediente);
        Task<List<ControlBaseDTO>> GetCatalogoDocumentosCompleto();
        Task<List<expediente_digital>> GetFilesByActividad(long idExpediente, string actividadId);
        Task<List<expediente_digital>> GetFilesExpedienteDigital(long idExpediente, string? activityId = null);
        Task<expediente_digital?> GetFileExpedienteDigitalById(long idArchivo);
        Task<expediente_digital> SaveMetadataMongo(expediente_digital expedienteDigital, int idUsuario);
        Task<expediente_digital?> UpdateMetadataMongo(expediente_digital expedienteDigital, int idUsuario);
        Task<string> FileNameVersion(long idExpediente, long idDocumento);
        Task<UserResponsibleDTO> GetUserResponsibleByIdExpediente(long idExpediente);
        Task<string> GetTemplateFileName(int idCatExpedienteDigital, int idCatExpedienteDigitalDocumento);

        #region AWS S3       
        Task<Stream> DownloadFromAWS(long idDocument);
        Task<bool> UploadToAWS(long idExpediente, string fileName, Stream fileContent);
        Task<bool> ExistsInRepositoryAWS(long idExpediente, string fileName);
        #endregion

        #region Local
        Task<Stream> DownloadFromLocal(long idDocument);
        Task<bool> UploadToLocal(long idExpediente, string fileName, Stream fileContent);
        #endregion
    }
}
