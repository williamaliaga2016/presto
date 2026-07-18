using Data.Repository.Interfaces.Entities.Multibanca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ICorregirReparoControlCreditoRepository : IMultibancaGenericRepository<corregir_reparo_control_credito_entity>,IDisposable
    {
        Task<corregir_reparo_control_credito_entity?> GetByExpediente(long id_expediente);
    }
}
