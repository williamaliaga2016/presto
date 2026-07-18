using Common.Application.Interfaces;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface IRegistrarFirmaApoderadoBancoApplication : IMultibancaGenericApplication<registrar_firma_apoderado_banco>
    {
        Task<registrar_firma_apoderado_banco?> GetByExpediente(long id_expediente);
        Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id);
    }
}
