using Multibanca.DTO.Common;
using Multibanca.DTO.Multibanca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IConsultActivityRepository
    {
        Task<List<ControlBaseDTO>> GetCatalogoTipoBusqueda();
        Task<List<ConsultActivityDTO>> GetConsultActivity(SearchCriteriaDTO searchCriteria);
        Task<List<EtapaDTO>> GetConsultTrackinActivity(long idExpediente);
    }
}
