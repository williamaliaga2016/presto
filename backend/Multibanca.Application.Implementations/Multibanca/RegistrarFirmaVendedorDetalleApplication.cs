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
    public class RegistrarFirmaVendedorDetalleApplication : MultibancaGenericApplication<firma_vendedor_detalle, firma_vendedor_detalle_entity, IRegistrarFirmaVendedorDetalleRepository>, IRegistrarFirmaVendedorDetalleApplication
    {
        private readonly IRegistrarFirmaVendedorDetalleRepository RegistrarFirmaVendedorDetalleRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public RegistrarFirmaVendedorDetalleApplication(
            MultibancaDBContext _multibancaDBContext,
            IRegistrarFirmaVendedorDetalleRepository _registrarFirmaVendedorDetalleRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _registrarFirmaVendedorDetalleRepository, _mapper)
        {
            RegistrarFirmaVendedorDetalleRepositoryProvider = _registrarFirmaVendedorDetalleRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

    }
}
