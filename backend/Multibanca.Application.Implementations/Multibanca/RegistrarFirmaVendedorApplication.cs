using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Implementations.Repositories.Multibanca;
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
    public class RegistrarFirmaVendedorApplication : MultibancaGenericApplication<firma_vendedor, firma_vendedor_entity, IRegistrarFirmaVendedorRepository>, IRegistrarFirmaVendedorApplication
    {
        private readonly IRegistrarFirmaVendedorRepository RegistrarFirmaVendedorRepositoryProvider;
        private readonly IRegistrarFirmaVendedorDetalleRepository RegistrarFirmaVendedorDetalleRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public RegistrarFirmaVendedorApplication(
            MultibancaDBContext _multibancaDBContext,
            IRegistrarFirmaVendedorRepository _registrarFirmaVendedorRepository,
            IRegistrarFirmaVendedorDetalleRepository _registrarFirmaVendedorDetalleRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _registrarFirmaVendedorRepository, _mapper)
        {
            RegistrarFirmaVendedorRepositoryProvider = _registrarFirmaVendedorRepository;
            RegistrarFirmaVendedorDetalleRepositoryProvider = _registrarFirmaVendedorDetalleRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<firma_vendedor?> GetByExpediente(long id_expediente)
        {
            var entity = await RegistrarFirmaVendedorRepositoryProvider.GetByExpedienteActividad(
                id_expediente);
            if (entity != null)
            {
                var model = Mapper.Map<firma_vendedor>(entity);

                var detalles = await RegistrarFirmaVendedorDetalleRepositoryProvider
                    .GetByIdFirmaVendedor(entity.id_firma_vendedor);

                model.firma_vendedor_detalle =
                    Mapper.Map<List<firma_vendedor_detalle>>(detalles);

                return model;
            }

            var defaults = await RegistrarFirmaVendedorDetalleRepositoryProvider
                .GetDefaultFromExpediente(id_expediente);

            if (defaults == null || !defaults.Any())
                return null;

            return new firma_vendedor
            {
                id_expediente = id_expediente,
                observaciones = "",
                is_active = true,
                row_status = true,
                firma_vendedor_detalle =
                    Mapper.Map<List<firma_vendedor_detalle>>(defaults)
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
                await RegistrarFirmaVendedorRepositoryProvider.GetByExpedienteActividad(
                    expediente_id
                );

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Registrar Firma Vendedor para el expediente {expediente_id}."
                );
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(
                expediente_id,
                actividad_id
            );

            List<xpdl_transition_DTO> listTransitions =
                await WorkflowApplicationProvider.GetTransitions(actividad_id);

            string transitionsID = listTransitions
                .Where(x => x.name == "RegistrarFirmaVendedor_RealizarControlCredito")
                .Select(x => x.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Registrar Firma Vendedor."
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
