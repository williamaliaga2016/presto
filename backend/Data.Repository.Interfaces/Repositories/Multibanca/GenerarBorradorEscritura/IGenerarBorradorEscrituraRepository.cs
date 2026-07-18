using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco;
using Data.Repository.Interfaces.Entities.Multibanca.GenerarBorradorEscritura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.Multibanca.GenerarBorradorEscritura
{
    public interface IGenerarBorradorEscrituraRepository : IMultibancaGenericRepository<generar_borrador_escritura_entity>, IDisposable
    {
        Task<generar_borrador_escritura_entity> GetByExpediente(long id_expediente);
    }
}
