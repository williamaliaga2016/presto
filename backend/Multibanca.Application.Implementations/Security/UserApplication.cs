using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
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
    public class UserApplication : MultibancaGenericApplication<users, users_entity, IUserRepository>, IUserApplication
    {
        private readonly IMapper Mapper;
        //private readonly IRoleApplication RoleApplicationProvider;

        public UserApplication(MultibancaDBContext _multibancaDBContext, IUserRepository _userRepository, IMapper _mapper) : base(_multibancaDBContext, _userRepository, _mapper)//IRoleApplication _roleApplication,
        {
            //RoleApplicationProvider = _roleApplication;
            Mapper = _mapper;
        }
    }
}
