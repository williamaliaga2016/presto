using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.DTO.Multibanca.BBVA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Interfaces.Multibanca.BBVA
{
    public interface IValidarIntegracionApplication
    {
        Task<ValidarIntegracionDocumentosDTO> GetByExpedienteConEncabezado(long id_expediente);
        Task<object> GetControles();
        Task<GuardarValidarIntegracionDocumentosDTO> Guardar(GuardarValidarIntegracionDocumentosDTO dto, int usuario_id, string actividad_id);
        Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id);

        #region INTERVINIENTE

        Task<List<IntervinienteDTO>> GetIntervinientes(long idExpediente);
        Task<IntervinienteDTO> GuardarInterviniente(IntervinienteDTO dto, int usuario_id, string actividad_id);

        #endregion
    }
}
