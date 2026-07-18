using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Implementations.Repositories.Multibanca;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class ReportsApplication : MultibancaGenericApplication<reports, reports_entity, IReportsRepository>, IReportsApplication
    {
        private readonly IReportsRepository ReportsRepositoryProvider;
        private readonly IMapper Mapper;
        public ReportsApplication(MultibancaDBContext _multibancaDBContext, IReportsRepository _reportsRepository, IMapper _mapper) : base(_multibancaDBContext, _reportsRepository, _mapper)
        {
            ReportsRepositoryProvider = _reportsRepository;
            Mapper = _mapper;
        }
    }
}
