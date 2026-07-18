using AutoMapper;
using Common.Application.Implementations;
using Common.Application.Interfaces;
using Data.Repository.Implementations;
using Data.Repository.Implementations.Repositories.Security;
using Data.Repository.Interfaces.Entities.Security;
using Data.Repository.Interfaces.Repositories.Security;
using Multibanca.Application.Interfaces.Security;
using Multibanca.Domain.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Implementations.Security
{
    public class RoleMenuApplication : MultibancaGenericApplication<role_menu, role_menu_entity, IRoleMenuRepository>, IRoleMenuApplication, IMultibancaGenericApplication<role_menu>
    {
        private readonly IMapper Mapper;

        public RoleMenuApplication(MultibancaDBContext _multibancaDBContext, IRoleMenuRepository _roleMenuRepository, IMapper _mapper) : base(_multibancaDBContext, _roleMenuRepository, _mapper)
        {
            Mapper = _mapper;
        }

        public async Task<List<role_menu>> GetAllRoleMenuAssign(int userId)
        {
            var listEntities = await RepositoryProvider.GetAllRoleMenuAssign(userId);
            return Mapper.Map<List<role_menu>>(listEntities);
        }
    }
}
