using Multibanca.DTO.Multibanca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface IEncabezadoApplication
    {
        Task<EncabezadoDTO> InformacionEncabezado(long idExpediente, string? activityID);
    }
}
