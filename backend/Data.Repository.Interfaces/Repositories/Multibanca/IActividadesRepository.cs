using Data.Repository.Interfaces.Entities.Multibanca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IActividadesRepository : IMultibancaGenericRepository<actividades_entity>, IDisposable
    {
        Task<actividades_entity> ObtenerInformacionActividadPorId(long id);
        Task<List<actividades_entity>> GetCurrentActivitiesByIdExpediente(long id_expediente);
        Task<bool> CompletarActividad(long id, long id_usuario);
        Task<bool> ExisteActividad(long id_expediente, string id_actividad);
        Task<bool> ExisteActividadActiva(long id_expediente, string id_actividad);
        Task<long> InsertActividad(actividades_entity actividadesEntity);
        Task<bool> ValidaEditActividad(long id_expediente, int id_usuario, string activityID);
        Task<bool> IsCompleteActivity(long id_expediente, string activityID);
        Task<actividades_entity> ObtenerActividadPorExpedienteActividad(long id_expediente, string id_actividad);
    }
}
