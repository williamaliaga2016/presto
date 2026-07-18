using Data.Repository.Implementations;
using Data.Repository.Implementations.Repositories.Security;
using Data.Repository.Interfaces;
using Data.Repository.Interfaces.Repositories.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Multibanca.Application.Implementations.Security;
using Multibanca.Application.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Register.IoC.Security
{
    public static class IoCRegisterSecurity
    {
        public static IServiceCollection AddRegistration(this IServiceCollection services)
        {
            AddRegisterRespositories(services);
            AddRegisterApplications(services);
            return services;
        }

        public static IServiceCollection AddRegisterApplications(IServiceCollection services)
        {
            services.AddScoped<IUserApplication, UserApplication>();
            services.AddScoped<IRoleApplication, RoleApplication>();
            services.AddScoped<IMenuApplication, MenuApplication>();
            services.AddScoped<IRoleMenuApplication, RoleMenuApplication>();
            services.AddScoped<ILoginApplication, LoginApplication>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            return services;
        }

        public static IServiceCollection AddRegisterRespositories(IServiceCollection services)
        {
            services.AddScoped<IMultibancaDBContext, MultibancaDBContext>();
            services.AddScoped<DbContext, MultibancaDBContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IRoleMenuRepository, RoleMenuRepository>();
            services.AddScoped<ILoginRepository, LoginRepository>();

            return services;
        }
    }
}
