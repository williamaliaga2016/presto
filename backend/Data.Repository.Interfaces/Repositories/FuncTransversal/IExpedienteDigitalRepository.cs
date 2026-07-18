using Data.Repository.Interfaces.Entities.FuncTransversal;
using Multibanca.DTO.Common;
using Multibanca.DTO.FuncTransversal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.FuncTransversal
{
    public interface IExpedienteDigitalRepository : IMultibancaGenericRepository<expediente_digital_entity>
    {
        Task<bool> ExistsByIdAsync(long idExpediente);
        Task<List<ControlBaseDTO>> GetCatalogoCategoriaExpedienteDigital();
        Task<List<ControlBaseDTO>> GetCatalogoDocumentos(int idExpedienteDigital, long idExpediente);
        Task<List<ControlBaseDTO>> GetCatalogoDocumentosCompleto();
        Task<List<expediente_digital_entity>> GetFilesExpedienteDigital(long idExpediente);
        Task<string> FileNameVersion(long idExpediente, long idDocumento);
        Task<string> GetDocumentoNombreCorto(long idDocumento);
        Task<UserResponsibleDTO> GetUserResponsibleByIdExpediente(long idExpediente);
        Task<string> GetTemplateFileName(int idCatExpedienteDigital, int idCatExpedienteDigitalDocumento);
    }
}
