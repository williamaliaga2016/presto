using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Data.Repository.Implementations.Repositories.Multibanca
{
    public class ReportsRepository : MultibancaGenericRepository<reports_entity>, IReportsRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public ReportsRepository(MultibancaDBContext _multibancaDBContext) : base(_multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }
    }
}