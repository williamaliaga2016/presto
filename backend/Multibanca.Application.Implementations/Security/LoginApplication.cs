using AutoMapper;
using Data.Repository.Implementations;
using Data.Repository.Interfaces;
using Data.Repository.Interfaces.Entities.Security;
using Multibanca.Application.Interfaces.Security;
using Multibanca.Domain.Models.Security;
using Multibanca.DTO.Security;

namespace Multibanca.Application.Implementations.Security
{
    public class LoginApplication : ILoginApplication
    {
        private readonly IMultibancaUnitOfWork _UoW;
        private readonly IMapper Mapper;

        public LoginApplication(MultibancaDBContext _multibancaDBContext, IMapper _mapper)
        {
            _UoW = new MultibancaUnitOfWork(_multibancaDBContext);
            Mapper = _mapper;
        }

        private MultibancaUnitOfWork UoW
        {
            get
            {
                return _UoW as MultibancaUnitOfWork;
            }
        }

        public async Task<AuthUserDTO> Login(LoginDTO loginDTO)
        {
            users_entity userEntity = await UoW.LoginRepositoryProvider.Login(loginDTO);
            roles_entity roleEntity = UoW.RoleRepositoryProvider.FindById(userEntity.role_id);
            AuthUserDTO authUserDTO = new AuthUserDTO
            {
                user_id = userEntity.user_id,
                role_id = userEntity.role_id,
                code = roleEntity.code,
                user_name = userEntity.user_name,
                role_name = roleEntity.name,
                name_complete = userEntity.name + " " + userEntity.last_name_first + " " + userEntity.last_name_second,
                email = userEntity.email,
                status = userEntity != null,
                message = userEntity != null ? "Login successful" : "Invalid username or password"
            };
            return authUserDTO;
        }

        public async Task<users> GetUserByUserName(string user_name)
        {
            users_entity userEntity = await UoW.LoginRepositoryProvider.GetUserByUserName(user_name);
            return Mapper.Map<users>(userEntity);
        }
    }
}
