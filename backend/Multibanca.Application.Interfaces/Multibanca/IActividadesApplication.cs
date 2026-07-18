using Common.Application.Interfaces;
using Multibanca.Domain.Models.Multibanca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface IActividadesApplication : IMultibancaGenericApplication<actividades>
    {
        Task<actividades> ObtenerInformacionActividadPorId(long id);
        Task<bool> CompletarActividad(long id, long id_usuario);
        Task<bool> ExisteActividad(long id_expediente, string id_actividad);
        Task<bool> ExisteActividadActiva(long id_expediente, string id_actividad);
        Task<long> InsertActividad(actividades actividad);
        Task<bool> ValidaEditActividad(long id_expediente, int id_usuario, string id_Actividad);
        Task<bool> IsCompleteActivity(long id_expediente, string id_Actividad);
        Task<actividades> ObtenerActividadPorExpedienteActividad(long id_expediente, string id_actividad);
    }
}
