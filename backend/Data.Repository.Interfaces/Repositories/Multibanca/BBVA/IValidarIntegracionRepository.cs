using Data.Repository.Interfaces.Entities.Multibanca.BBVA.ValidarIntegracionDocumentos;
using Multibanca.Domain.Models.Multibanca.BBVA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.Multibanca.BBVA
{
    public interface IValidarIntegracionRepository
    {
        Task<validar_integracion_documentos_entity?> GetByExpediente(long id_expediente);
        Task<validar_integracion_documentos_entity> Guardar(validar_integracion_documentos_entity actividad, int userId);

        #region INTERVINIENTE
        Task<List<interviniente_bbva_entity>> GetIntervinientes(long idExpediente);
        Task<interviniente_bbva_entity?> GetInterviniente(int id);
        Task<interviniente_bbva_entity> GuardarInterviniente(interviniente_bbva_entity interviniente, int userId);
        Task<bool> CambiarEstadoInterviniente(int id, bool activo, int userId);
        #endregion
    }
}
