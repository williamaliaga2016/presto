using Data.Repository.Implementations;
using Data.Repository.Implementations.Repositories.FuncTransversal;
using Data.Repository.Interfaces;
using Data.Repository.Interfaces.Repositories.FuncTransversal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Multibanca.Application.Implementations.FuncTransversal;
using Multibanca.Application.Interfaces.FuncTransversal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Register.IoC.FuncTransversal
{
    public static class IoCRegisterFuncTransversal
    {
        public static IServiceCollection AddRegistration(this IServiceCollection services)
        {
            AddRegisterRespositories(services);
            AddRegisterApplications(services);
            return services;
        }

        public static IServiceCollection AddRegisterApplications(IServiceCollection services)
        {
            services.AddScoped<IExpedienteDigitalApplication, ExpedienteDigitalApplication>();
            services.AddScoped<IBitacoraApplication, BitacoraApplication>();
            services.AddScoped<IHistorialExpedienteApplication, HistorialExpedienteApplication>();

            return services;
        }

        public static IServiceCollection AddRegisterRespositories(IServiceCollection services)
        {
            services.AddSingleton<IMongoDbContext, MongoDbContext>();
            services.AddScoped<IExpedienteDigitalRepository, ExpedienteDigitalRepository>();
            services.AddScoped<IExpedienteDigitalMongoRepository, ExpedienteDigitalMongoRepository>();
            services.AddScoped<IBitacoraRepository, BitacoraRepository>();
            services.AddScoped<IHistorialExpedienteRepository, HistorialExpedienteRepository>();

            return services;
        }
    }
}
