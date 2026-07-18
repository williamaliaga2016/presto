using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca.BBVA;
using Multibanca.DTO.Multibanca.BBVA;
using Multibanca.DTO.Multibanca.BBVA.Multibanca.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Interfaces.Multibanca.BBVA
{
    public interface IDevolucionVbComercialApplication
    {
        Task<DevolucionVbComercialDTO> GetByExpedienteConEncabezado(long id_expediente);
        Task<object> GetControles();
        Task<GuardarDevolucionVbComercialDTO> Guardar(GuardarDevolucionVbComercialDTO dto, int usuarioId, string actividadId);
        Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id, bool confirmarCierre);
    }
}
