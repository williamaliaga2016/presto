using Data.Repository.Interfaces.Entities.Multibanca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IVerificarCorreccionEscrituraRepository : IMultibancaGenericRepository<verificar_correccion_escritura_entity>,IDisposable
    {
        Task<verificar_correccion_escritura_entity?> GetByExpediente(long id_expediente);
    }
}
