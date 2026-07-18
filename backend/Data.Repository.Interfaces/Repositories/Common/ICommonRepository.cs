using Framework.WorkFlow.Common.DTO;
using Multibanca.DTO.Common;

namespace Data.Repository.Interfaces.Repositories.Common
{
    public interface ICommonRepository
    {
        Task<FolioDTO> CapturarDatosFolio(long id_expediente, string id_actividad);
        Task<AssignActivityDTO> AsignarActividad(long id_expediente, string id_performer);
        Task<List<ControlBaseDTO>> GetCatalogoByType(string tipo, string? codigoPadre = null);
        /// <summary>
        /// Consulta catalogos e incluye el codigo del padre para dependencias entre listas.
        /// </summary>
        /// <param name="tipo">Tipo de catalogo que se debe consultar.</param>
        /// <returns>Opciones del catalogo con codigo padre cuando aplica.</returns>
        Task<List<ControlBaseDTO>> GetCatalogoByTypeWithParentCode(string tipo);
        Task<bool> ExisteActividadFolio(long idExpediente, string idActividad);
    }
}
