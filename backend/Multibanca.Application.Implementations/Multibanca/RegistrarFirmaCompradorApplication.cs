using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Multibanca.Application.Interfaces.Workflow;
using Framework.WorkFlow.Common.DTO;
//using Framework.WorkFlow.Domain.Models;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class RegistrarFirmaCompradorApplication : MultibancaGenericApplication<firma_comprador, firma_comprador_entity, IRegistrarFirmaCompradorRepository>, IRegistrarFirmaCompradorApplication
    {
        private readonly IRegistrarFirmaCompradorRepository RegistrarFirmaCompradorRepositoryProvider;
        private readonly IRegistrarFirmaCompradorDetalleRepository RegistrarFirmaCompradorDetalleRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public RegistrarFirmaCompradorApplication(
            MultibancaDBContext _multibancaDBContext,
            IRegistrarFirmaCompradorRepository _registrarFirmaCompradorRepository,
            IRegistrarFirmaCompradorDetalleRepository _registrarFirmaCompradorDetalleRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _registrarFirmaCompradorRepository, _mapper)
        {
            RegistrarFirmaCompradorRepositoryProvider = _registrarFirmaCompradorRepository;
            RegistrarFirmaCompradorDetalleRepositoryProvider = _registrarFirmaCompradorDetalleRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<firma_comprador?> GetByExpediente(long id_expediente)
        {
            var entity = await RegistrarFirmaCompradorRepositoryProvider.GetByExpedienteActividad(
                id_expediente);

            if (entity != null)
            {
                var model = Mapper.Map<firma_comprador>(entity);

                var detalles = await RegistrarFirmaCompradorDetalleRepositoryProvider
                    .GetByIdFirmaComprador(entity.id_firma_comprador);

                model.firma_comprador_detalle =
                    Mapper.Map<List<firma_comprador_detalle>>(detalles);

                return model;
            }

            var defaults = await RegistrarFirmaCompradorDetalleRepositoryProvider
                .GetDefaultFromExpediente(id_expediente);

            if (defaults == null || !defaults.Any())
                return null;

            return new firma_comprador
            {
                id_expediente = id_expediente,
                observaciones = "",
                is_active = true,
                row_status = true,
                firma_comprador_detalle = Mapper.Map<List<firma_comprador_detalle>>(defaults)
                   .Select(x =>
                   {
                       x.fecha_firma = null;
                       return x;
                   })
                   .ToList()
            };
        }

        public async Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        )
        {
            var entity =
                await RegistrarFirmaCompradorRepositoryProvider.GetByExpedienteActividad(
                    expediente_id
                );

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Registrar firma comprador para el expediente {expediente_id}."
                );
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(
                expediente_id,
                actividad_id
            );

            List<xpdl_transition_DTO> listTransitions =
                await WorkflowApplicationProvider.GetTransitions(actividad_id);

            string transitionsID = listTransitions
                .Where(x => x.name == "RegistrarFirmaComprador_RealizarControlCredito")
                .Select(x => x.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Registrar Firma Comprador."
                );
            }

            return await WorkflowApplicationProvider.AvanzarActividad(
                transitionsID,
                folio,
                usuario_id
            );
        }
    

    }
}
