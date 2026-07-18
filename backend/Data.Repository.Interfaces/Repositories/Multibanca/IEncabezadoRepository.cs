using Multibanca.DTO.Multibanca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Repositories.Multibanca
{
    public interface IEncabezadoRepository
    {
        Task<EncabezadoDTO> InformacionEncabezado(long idExpediente, string activityID);
    }
}
