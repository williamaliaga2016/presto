using Framework.WorkFlow.Common.DTO;
using Multibanca.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Interfaces.Common
{
    public interface ICommonApplication
    {
        Task<FolioDTO> CapturarDatosFolio(long id_expediente, string ActivityID);
        Task<AssignActivityDTO> AsignarActividad(long id_expediente, string idPerformer);
        Task<List<ControlBaseDTO>> GetCatalogoByType(string type);
        /// <summary>
        /// Consulta catalogos e incluye el codigo del padre para dependencias entre listas.
        /// </summary>
        /// <param name="type">Tipo de catalogo que se debe consultar.</param>
        /// <returns>Opciones del catalogo con codigo padre cuando aplica.</returns>
        Task<List<ControlBaseDTO>> GetCatalogoByTypeWithParentCode(string type);
        Task<bool> ExisteActividadFolio(long idExpediente, string idActividad);
    }
}
