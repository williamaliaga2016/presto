using Data.Repository.Interfaces.Entities.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IGenerarRecursosPagosCbrRepository : IMultibancaGenericRepository<generar_recursos_pagos_cbr_entity>, IDisposable
    {
        Task<generar_recursos_pagos_cbr_entity?> GetByExpediente(long id_expediente);
    }
}
