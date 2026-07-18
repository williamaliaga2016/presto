using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Domain.Models.Multibanca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class ActividadesApplication : MultibancaGenericApplication<actividades, actividades_entity, IActividadesRepository>, IActividadesApplication
    {
        private readonly IActividadesRepository ActividadesRepositoryProvider;
        private readonly IMapper Mapper;

        public ActividadesApplication(MultibancaDBContext _multibancaDBContext, IActividadesRepository _actividadesRepository, IMapper _mapper) : base(_multibancaDBContext, _actividadesRepository, _mapper)
        {
            ActividadesRepositoryProvider = _actividadesRepository;
            Mapper = _mapper;
        }

        public async Task<actividades> ObtenerInformacionActividadPorId(long id)
        {
            actividades_entity actividad = await ActividadesRepositoryProvider.ObtenerInformacionActividadPorId(id);
            return Mapper.Map<actividades>(actividad);
        }

        public async Task<bool> CompletarActividad(long id, long id_usuario)
        {
            return await ActividadesRepositoryProvider.CompletarActividad(id, id_usuario);
        }

        public async Task<bool> ExisteActividad(long id_expediente, string id_actividad)
        {
            return await ActividadesRepositoryProvider.ExisteActividad(id_expediente, id_actividad);
        }

        public async Task<bool> ExisteActividadActiva(long id_expediente, string id_actividad)
        {
            return await ActividadesRepositoryProvider.ExisteActividadActiva(id_expediente, id_actividad);
        }

        public async Task<long> InsertActividad(actividades actividad)
        {
            actividades_entity actividad_entity = Mapper.Map<actividades_entity>(actividad);
            return await ActividadesRepositoryProvider.InsertActividad(actividad_entity);
        }

        public async Task<bool> ValidaEditActividad(long id_expediente, int id_usuario, string id_Actividad)
        {
            return await ActividadesRepositoryProvider.ValidaEditActividad(id_expediente, id_usuario, id_Actividad);
        }

        public async Task<bool> IsCompleteActivity(long id_expediente, string id_Actividad)
        {
            return await ActividadesRepositoryProvider.IsCompleteActivity(id_expediente, id_Actividad);
        }

        public async Task<actividades> ObtenerActividadPorExpedienteActividad(long id_expediente, string id_actividad)
        {
            var entity = await ActividadesRepositoryProvider.ObtenerActividadPorExpedienteActividad(id_expediente, id_actividad);
            return Mapper.Map<actividades>(entity);
        }
    }
}
