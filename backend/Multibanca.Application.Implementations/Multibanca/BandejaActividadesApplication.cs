using Data.Repository.Interfaces.Repositories.Multibanca;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.DTO.Multibanca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class BandejaActividadesApplication : IBandejaActividadesApplication
    {
        private readonly IBandejaActividadesRepository BandejaActividadesRepositoryProvider;
        public BandejaActividadesApplication(IBandejaActividadesRepository _bandejaActividadesRepository)
        {
            BandejaActividadesRepositoryProvider = _bandejaActividadesRepository;
        }

        public async Task<List<ActividadDTO>> GetInfoActivityByUser(long idUsuario, string workFlowProcessID)
        {
            return await BandejaActividadesRepositoryProvider.GetInfoActivityByUser(idUsuario, workFlowProcessID);
        }
    }
}
