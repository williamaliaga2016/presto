using Data.Repository.Interfaces.Entities.Multibanca;
using System;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IAprobacionComercialLegalCdRRepository : IMultibancaGenericRepository<aprobacion_comercial_legal_cdr_entity>, IDisposable
    {
        Task<aprobacion_comercial_legal_cdr_entity?> GetByExpediente(long id_expediente);
    }
}
