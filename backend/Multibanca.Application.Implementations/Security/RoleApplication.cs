using AutoMapper;
using Common.Application.Implementations;
using Common.Application.Interfaces;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Security;
using Data.Repository.Interfaces.Repositories.Security;
using Multibanca.Application.Interfaces.Security;
using Multibanca.Domain.Models.Security;
using Multibanca.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Implementations.Security
{
    public class RoleApplication : MultibancaGenericApplication<roles, roles_entity, IRoleRepository>, IRoleApplication, IMultibancaGenericApplication<roles>
    {
        private readonly IMapper Mapper;

        public RoleApplication(MultibancaDBContext _multibancaDBContext, IRoleRepository _roleRepository, IMapper _mapper) : base(_multibancaDBContext, _roleRepository, _mapper)
        {
            Mapper = _mapper;
        }

        public async Task<List<ControlBaseDTO>> GetControlRole()
        {
            return await RepositoryProvider.GetControlRole();
        }
    }
}
