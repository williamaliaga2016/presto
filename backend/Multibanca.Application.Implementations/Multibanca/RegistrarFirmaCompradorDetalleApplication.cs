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
    public class RegistrarFirmaCompradorDetalleApplication : MultibancaGenericApplication<firma_comprador_detalle, firma_comprador_detalle_entity, IRegistrarFirmaCompradorDetalleRepository>, IRegistrarFirmaCompradorDetalleApplication
    {
        private readonly IRegistrarFirmaCompradorDetalleRepository RegistrarFirmaCompradorDetalleRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public RegistrarFirmaCompradorDetalleApplication(
            MultibancaDBContext _multibancaDBContext,
            IRegistrarFirmaCompradorDetalleRepository _registrarFirmaCompradorDetalleRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _registrarFirmaCompradorDetalleRepository, _mapper)
        {
            RegistrarFirmaCompradorDetalleRepositoryProvider = _registrarFirmaCompradorDetalleRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

    }
}
