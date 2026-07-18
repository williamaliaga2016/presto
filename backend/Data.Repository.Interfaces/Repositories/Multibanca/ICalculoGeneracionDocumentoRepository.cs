using Data.Repository.Interfaces.Entities.Multibanca;
using System;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface ICalculoGeneracionDocumentoRepository : IMultibancaGenericRepository<calculo_generacion_documento_entity>, IDisposable
    {
        Task<calculo_generacion_documento_entity?> GetByExpediente(long id_expediente);
    }
}
