using AutoMapper;
using Data.Repository.Interfaces.Repositories.Common;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Common;
using Multibanca.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Implementations.Common
{
    public class CommonApplication : ICommonApplication
    {
        private readonly ICommonRepository CommonRepositoryProvider;
        private readonly IMapper Mapper;

        public CommonApplication(ICommonRepository _commonRepository, IMapper _mapper)
        {
            CommonRepositoryProvider = _commonRepository;
            Mapper = _mapper;
        }

        public async Task<FolioDTO> CapturarDatosFolio(long id_expediente, string ActivityID)
        {
            return await CommonRepositoryProvider.CapturarDatosFolio(id_expediente, ActivityID);
        }

        public async Task<AssignActivityDTO> AsignarActividad(long id_expediente, string idPerformer)
        {
            return await CommonRepositoryProvider.AsignarActividad(id_expediente, idPerformer);
        }

        public async Task<List<ControlBaseDTO>> GetCatalogoByType(string type)
        {
            return await CommonRepositoryProvider.GetCatalogoByType(type);
        }

        /// <inheritdoc />
        public async Task<List<ControlBaseDTO>> GetCatalogoByTypeWithParentCode(string type)
        {
            return await CommonRepositoryProvider.GetCatalogoByTypeWithParentCode(type);
        }

        public async Task<bool> ExisteActividadFolio(long idExpediente, string idActividad)
        {
            return await CommonRepositoryProvider.ExisteActividadFolio(idExpediente, idActividad);
        }
    }
}
