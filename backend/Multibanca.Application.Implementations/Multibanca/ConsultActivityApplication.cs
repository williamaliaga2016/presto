using Data.Repository.Interfaces.Repositories.Multibanca;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.DTO.Common;
using Multibanca.DTO.Multibanca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class ConsultActivityApplication : IConsultActivityApplication
    {
        private readonly IConsultActivityRepository ConsultActivityRepositoryProvider;
        public ConsultActivityApplication(IConsultActivityRepository _consultActivityRepository)
        {
            ConsultActivityRepositoryProvider = _consultActivityRepository;
        }

        public async Task<List<ControlBaseDTO>> GetCatalogoTipoBusqueda()
        {
            return await ConsultActivityRepositoryProvider.GetCatalogoTipoBusqueda();
        }

        public async Task<List<ConsultActivityDTO>> GetConsultActivity(SearchCriteriaDTO searchCriteria)
        {
            return await ConsultActivityRepositoryProvider.GetConsultActivity(searchCriteria);
        }

        public async Task<List<EtapaDTO>> GetConsultTrackinActivity(long idExpediente)
        {
            return await ConsultActivityRepositoryProvider.GetConsultTrackinActivity(idExpediente);
        }
    }
}
