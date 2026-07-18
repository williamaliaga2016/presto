using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.DatosOperacion;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Domain.Models.Multibanca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class GenerarFiniquitoApplication : MultibancaGenericApplication<generar_finiquito, generar_finiquito_entity, IGenerarFiniquitoRepository>, IGenerarFiniquitoApplication
    {
        private readonly IGenerarFiniquitoRepository GenerarFiniquitoRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IDatosOperacionPropiedadApplication _propiedadApplication;
        private readonly ITasacionApplication _tasacionApplication;
        private readonly IMapper Mapper;

        public GenerarFiniquitoApplication(
            MultibancaDBContext multibancaDBContext,
            IGenerarFiniquitoRepository generarFiniquitoRepositoryProvider,
            IWorkflowApplication workflowApplicationProvider,
            IDatosOperacionPropiedadApplication propiedadApplication,
            ITasacionApplication tasacionApplication,
            IMapper mapper) : base(multibancaDBContext, generarFiniquitoRepositoryProvider, mapper)
        {
            GenerarFiniquitoRepositoryProvider = generarFiniquitoRepositoryProvider;
            WorkflowApplicationProvider = workflowApplicationProvider;
            _propiedadApplication = propiedadApplication;
            _tasacionApplication = tasacionApplication;
            Mapper = mapper;
        }

        public async Task<generar_finiquito?> GetByExpediente(long id_expediente)
        {
            var finiquitoEntity = await GenerarFiniquitoRepositoryProvider.GetByExpediente(id_expediente);
            if (finiquitoEntity == null) return null;

            var finiquito = Mapper.Map<generar_finiquito>(finiquitoEntity);

            try
            {
                var propiedad = await _propiedadApplication.GetByExpediente(id_expediente);
                if (propiedad != null)
                {
                    finiquito.tipo_propiedad = propiedad.tipo_propiedad;
                    finiquito.direccion = propiedad.direccion;
                    finiquito.comuna = propiedad.comuna;
                    finiquito.rol_avaluo = propiedad.rol_avaluo_1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener propiedad: {ex.Message}");
            }

            try
            {
                var tasacion = await _tasacionApplication.GetByExpediente(id_expediente);
                if (tasacion?.detalles != null)
                {
                    // Filtrar tasación individual (tipo_tasacion = "INDIVIDUAL")
                    var tasacionIndividual = tasacion.detalles
                        .FirstOrDefault(d => d.tipo_tasacion == "INDIVIDUAL");

                    if (tasacionIndividual != null)
                    {
                        finiquito.tipo_tasacion = tasacionIndividual.tipo_tasacion;
                        finiquito.fecha_informe_tasacion = tasacionIndividual.fecha_informe_tasacion;
                        finiquito.fecha_solicitud_tasacion = tasacionIndividual.fecha_solicitud_tasacion;
                        finiquito.fecha_recepcion_tasacion = tasacionIndividual.fecha_recepcion_tasacion;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener tasación: {ex.Message}");
            }

            return finiquito;
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            var entity = await GenerarFiniquitoRepositoryProvider.GetByExpediente(expediente_id);

            if (entity == null)
            {
                throw new InvalidOperationException($"No existe registro de Generar Finiquito para el expediente {expediente_id}.");
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);

            List<xpdl_transition_DTO> listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);

            string transitionsID = listTransitions
                .Where(x => x.name == "GenerarFiniquito_RecibirInstruccionPago")
                .Select(x => x.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException("No se encontró transición configurada para Generar Finiquito.");
            }

            return await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
        }
    }
}