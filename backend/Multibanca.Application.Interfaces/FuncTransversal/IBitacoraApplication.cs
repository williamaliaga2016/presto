using Common.Application.Interfaces;
using Multibanca.Domain.Models.FuncTransversal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Interfaces.FuncTransversal
{
    public interface IBitacoraApplication : IMultibancaGenericApplication<bitacora>
    {
        Task<bitacora> GetByExpedienteActividad(long id_expediente, string id_actividad);
    }
}
