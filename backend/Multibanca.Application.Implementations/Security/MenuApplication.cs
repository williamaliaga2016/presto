using AutoMapper;
using Common.Application.Implementations;
using Common.Application.Interfaces;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Security;
using Data.Repository.Interfaces.Repositories.Security;
using Multibanca.Application.Interfaces.Security;
using Multibanca.Domain.Models.Security;
using Multibanca.DTO.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Implementations.Security
{
    public class MenuApplication : MultibancaGenericApplication<menus, menus_entity, IMenuRepository>, IMenuApplication, IMultibancaGenericApplication<menus>
    {
        private readonly IMapper Mapper;

        public MenuApplication(MultibancaDBContext _multibancaDBContext, IMenuRepository _menuRepository, IMapper _mapper) : base(_multibancaDBContext, _menuRepository, _mapper)
        {
            Mapper = _mapper;
        }

        public async Task<List<MenuItemDTO>> GetMenuJerarquico()
        {
            List<MenuItemDTO> menus = await RepositoryProvider.GetMenuJerarquico();
            return menus;
        }

        public async Task<List<MenuItemDTO>> GetMenuByUserId(int userId)
        {
            List<MenuItemDTO> menus = await RepositoryProvider.GetMenuByUserId(userId);
            return menus;
        }

        public async Task<List<MenuItemDTO>> GetMenuByRoleId(int role_id)
        {
            List<MenuItemDTO> menus = await RepositoryProvider.GetMenuByRoleId(role_id);
            return menus;
        }
    }
}
