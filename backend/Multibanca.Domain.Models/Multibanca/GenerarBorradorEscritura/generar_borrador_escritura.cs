using Common.Domain.Models;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Domain.Models.Multibanca.GenerarBorradorEscritura
{
    public class generar_borrador_escritura : base_auditoria
    {
        public int id_generar_borrador_escritura { get; set; }
        public long id_expediente { get; set; }
        public bool existe_alzamiento { get; set; }
        public bool seguro_cesantia { get; set; }
        public bool mandato_judicial { get; set; }
        public string? beneficios { get; set; }
        public int id_notaria { get; set; }
        public bool enviar_reparo { get; set; }
        public string? observaciones { get; set; }

        public List<generar_borrador_escritura_detalle>? detalle { get; set; }

    }
}
