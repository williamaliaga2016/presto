using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca.CargaOperacionBanco;
using Multibanca.Application.Interfaces.Workflow;
using Framework.WorkFlow.Common.DTO;

using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Domain.Models.Multibanca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class TasacionApplication :
        MultibancaGenericApplication<tasacion, tasacion_entity, ITasacionRepository>,
        ITasacionApplication
    {
        private const decimal PORCENTAJE_FINANCIAMIENTO_MH90 = 0.90m;
        private const decimal PORCENTAJE_FINANCIAMIENTO_DEFAULT = 0.80m;
        private const string GLOSA_MH90_PREFIX = "MH 90";

        private readonly ITasacionRepository TasacionRepositoryProvider;
        private readonly ITasacionDetalleApplication TasacionDetalleApplicationProvider;
        private readonly ICargaOperacionBancoAntecedenteCreditoRepository AntecedenteCreditoRepositoryProvider;
        private readonly ICargaOperacionBancoDatosOperacionRepository DatosOperacionBancoRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public TasacionApplication(
            MultibancaDBContext _multibancaDBContext,
            ITasacionRepository _tasacionRepository,
            ITasacionDetalleApplication _tasacionDetalleApplication,
            ICargaOperacionBancoAntecedenteCreditoRepository _antecedenteCreditoRepository,
            ICargaOperacionBancoDatosOperacionRepository _datosOperacionBancoRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper)
            : base(_multibancaDBContext, _tasacionRepository, _mapper)
        {
            TasacionRepositoryProvider = _tasacionRepository;
            TasacionDetalleApplicationProvider = _tasacionDetalleApplication;
            AntecedenteCreditoRepositoryProvider = _antecedenteCreditoRepository;
            DatosOperacionBancoRepositoryProvider = _datosOperacionBancoRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<tasacion?> GetByExpediente(long id_expediente)
        {
            var entity = await TasacionRepositoryProvider.GetByExpediente(id_expediente);

            if (entity == null)
            {
                return null;
            }

            var model = Mapper.Map<tasacion>(entity);
            model.detalles = await TasacionDetalleApplicationProvider.GetByExpediente(id_expediente);

            return model;
        }

        public async Task<List<tasacion_detalle>> GetDetallesByExpediente(long id_expediente)
        {
            return await TasacionDetalleApplicationProvider.GetByExpediente(id_expediente);
        }

        public async Task<tasacion> Save(tasacion model, int userId)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.id_expediente <= 0)
            {
                throw new InvalidOperationException("El expediente es obligatorio.");
            }

            if (model.is_enviar_reparo && string.IsNullOrWhiteSpace(model.observaciones))
            {
                throw new InvalidOperationException("Las observaciones son obligatorias cuando se envía a reparo.");
            }

            tasacion saved;

            if (model.id_tasacion == 0)
            {
                model.is_active = true;
                model.row_status = true;
                saved = Create(model, userId);
            }
            else
            {
                saved = Update(model, userId);
            }

            if (model.detalles != null && model.detalles.Count > 0)
            {
                foreach (var detalle in model.detalles)
                {
                    detalle.id_tasacion = saved.id_tasacion;
                    detalle.id_expediente = saved.id_expediente;

                    if (detalle.id_tasacion_detalle == 0)
                    {
                        detalle.is_active = true;
                        detalle.row_status = true;
                        TasacionDetalleApplicationProvider.Create(detalle, userId);
                    }
                    else
                    {
                        TasacionDetalleApplicationProvider.Update(detalle, userId);
                    }
                }
            }

            saved.detalles = await TasacionDetalleApplicationProvider.GetByTasacion(saved.id_tasacion);

            return saved;
        }

        public async Task<bool> DeleteDetalle(int id_tasacion_detalle, int userId)
        {
            if (id_tasacion_detalle <= 0)
            {
                throw new InvalidOperationException("El id_tasacion_detalle es obligatorio.");
            }

            TasacionDetalleApplicationProvider.LogicalDeleteById(id_tasacion_detalle, userId);
            await Task.CompletedTask;
            return true;
        }

        public async Task<EvaluarReparoAutomaticoDTO> EvaluarReparoAutomatico(long id_expediente)
        {
            var dto = new EvaluarReparoAutomaticoDTO();

            var antecedenteCredito = await AntecedenteCreditoRepositoryProvider.GetByExpediente(id_expediente);
            var datosOperacionBanco = await DatosOperacionBancoRepositoryProvider.GetByExpediente(id_expediente);
            var detalles = await TasacionDetalleApplicationProvider.GetByExpediente(id_expediente);

            if (antecedenteCredito == null || datosOperacionBanco == null || detalles == null || detalles.Count == 0)
            {
                dto.aplica_reparo_automatico = false;
                return dto;
            }

            decimal? valorTasacionUf = detalles
                .Where(q => q.valor_tasacion_uf.HasValue)
                .Select(q => q.valor_tasacion_uf)
                .Max();

            string? tipoTasacion = detalles
                .OrderByDescending(q => q.id_tasacion_detalle)
                .Select(q => q.tipo_tasacion)
                .FirstOrDefault();

            decimal? precioVentaMonedaOriginal = ParseDecimal(antecedenteCredito.precio_venta_moneda_original);
            decimal? prestamoMaximo = antecedenteCredito.prestamo_maximo;
            string? glosaProducto = datosOperacionBanco.glosa_producto;

            dto.precio_venta_moneda_original = precioVentaMonedaOriginal;
            dto.valor_tasacion_uf = valorTasacionUf;
            dto.prestamo_maximo = prestamoMaximo;
            dto.glosa_producto = glosaProducto;
            dto.tipo_tasacion = tipoTasacion;

            if (!precioVentaMonedaOriginal.HasValue || !valorTasacionUf.HasValue || !prestamoMaximo.HasValue)
            {
                dto.aplica_reparo_automatico = false;
                return dto;
            }

            bool esMH90 = !string.IsNullOrWhiteSpace(glosaProducto)
                && glosaProducto.Trim().StartsWith(GLOSA_MH90_PREFIX, StringComparison.OrdinalIgnoreCase);

            decimal porcentaje = esMH90 ? PORCENTAJE_FINANCIAMIENTO_MH90 : PORCENTAJE_FINANCIAMIENTO_DEFAULT;
            decimal menorValor = Math.Min(precioVentaMonedaOriginal.Value, valorTasacionUf.Value);
            decimal montoCalculado = menorValor * porcentaje;

            dto.porcentaje_financiamiento = porcentaje;
            dto.monto_calculado = montoCalculado;

            if (montoCalculado > prestamoMaximo.Value)
            {
                dto.aplica_reparo_automatico = true;
                dto.mensaje = "El valor de la propiedad es mayor al valor comercial de tasación, y el monto del crédito otorgado supera el % máximo de financiamiento sobre el valor de tasación. Por tal razón, se activará un reparo en el sistema, a fin de gestionar su corrección";
            }
            else
            {
                dto.aplica_reparo_automatico = false;
            }

            return dto;
        }

        public async Task<bool> MarcarReparoSubsanado(long id_expediente, int userId)
        {
            var entity = await TasacionRepositoryProvider.GetByExpediente(id_expediente);
            if (entity == null || !entity.is_enviar_reparo)
            {
                return false;
            }

            var model = Mapper.Map<tasacion>(entity);
            model.is_enviar_reparo = false;
            Update(model, userId);
            return true;
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            var entity = await TasacionRepositoryProvider.GetByExpediente(expediente_id);

            if (entity == null)
            {
                throw new InvalidOperationException($"No existe registro de Tasación para el expediente {expediente_id}.");
            }

            var detalles = await TasacionDetalleApplicationProvider.GetByExpediente(expediente_id);

            if (detalles == null || detalles.Count == 0)
            {
                throw new InvalidOperationException("Debe registrar al menos una tasación para avanzar la actividad.");
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);
            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);

            bool envioReparo = entity.is_enviar_reparo;

            string transitionsID;

            if (envioReparo)
            {
                transitionsID = listTransitions
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }
            else
            {
                string tipoTasacion = detalles.FirstOrDefault()?.tipo_tasacion ?? "";
                bool esColectiva = tipoTasacion == "COLECTIVA";

                if (esColectiva)
                {
                    transitionsID = listTransitions
                        .Where(x =>
                            x.name == "RegistrarTasacion_RevisarIngresoDatosOperacion_ReparoNO_Asociado")
                        .Select(x => x.transition_id)
                        .FirstOrDefault() ?? string.Empty;
                }
                else
                {
                    transitionsID = listTransitions
                        .Where(x =>
                            x.name == "RegistrarTasacion_GenerarEstudioTitulos_ReparoNO_Particular")
                        .Select(x => x.transition_id)
                        .FirstOrDefault() ?? string.Empty;
                }
            }

            if (string.IsNullOrEmpty(transitionsID))
            {
                throw new InvalidOperationException("No se encontró una transición válida para avanzar la actividad.");
            }

            return await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
        }

        private static decimal? ParseDecimal(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            string clean = value.Replace("$", "").Replace(",", "").Trim();
            if (decimal.TryParse(clean, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal parsed))
            {
                return parsed;
            }
            return null;
        }
    }
}