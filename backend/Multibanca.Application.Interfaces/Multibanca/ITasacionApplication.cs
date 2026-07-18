using Common.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Domain.Models.Multibanca;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public interface ITasacionApplication : IMultibancaGenericApplication<tasacion>
    {
        Task<tasacion?> GetByExpediente(long id_expediente);
        Task<List<tasacion_detalle>> GetDetallesByExpediente(long id_expediente);
        Task<tasacion> Save(tasacion model, int userId);
        Task<bool> DeleteDetalle(int id_tasacion_detalle, int userId);
        Task<EvaluarReparoAutomaticoDTO> EvaluarReparoAutomatico(long id_expediente);
        Task<bool> MarcarReparoSubsanado(long id_expediente, int userId);
        Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id);
    }

    public class EvaluarReparoAutomaticoDTO
    {
        public bool aplica_reparo_automatico { get; set; }
        public string? mensaje { get; set; }
        public decimal? precio_venta_moneda_original { get; set; }
        public decimal? valor_tasacion_uf { get; set; }
        public decimal? prestamo_maximo { get; set; }
        public decimal? porcentaje_financiamiento { get; set; }
        public decimal? monto_calculado { get; set; }
        public string? glosa_producto { get; set; }
        public string? tipo_tasacion { get; set; }
    }
}
