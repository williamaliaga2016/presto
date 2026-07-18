using Common.Application.Interfaces;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;
using Multibanca.Domain.Models.Multibanca.GenerarBorradorEscritura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Interfaces.Multibanca.GenerarBorradorEscritura
{
    public interface IGenerarBorradorEscrituraDetalleApplication : IMultibancaGenericApplication<generar_borrador_escritura_detalle>
    {
        Task<List<generar_borrador_escritura_detalle>> GetList(int id_generar_borrador_escritura, long id_expediente);
    }
}
