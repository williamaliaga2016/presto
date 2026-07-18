using AutoMapper;
using Data.Repository.Interfaces.Entities.Security;
using Multibanca.Domain.Models.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Automapper.Security
{
    public class AutoMapperProfileSecurity : Profile
    {
        public AutoMapperProfileSecurity()
        {
            CreateMap<users, users_entity>().ReverseMap();
            CreateMap<roles, roles_entity>().ReverseMap();
            CreateMap<menus, menus_entity>().ReverseMap();
            CreateMap<role_menu, role_menu_entity>().ReverseMap();
        }
    }
}
