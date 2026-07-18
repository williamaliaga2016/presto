using Data.Repository.Interfaces.Entities.Multibanca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IRegistrarFirmaApoderadoBancoRepository : IMultibancaGenericRepository<registrar_firma_apoderado_banco_entity>, IDisposable
    {
        Task<registrar_firma_apoderado_banco_entity?> GetByExpediente(long id_expediente);
    }
}
