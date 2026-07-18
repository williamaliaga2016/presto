using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Security;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.Domain.Models.Security;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class RectificatoriaFirmaDetalleApplication
        : MultibancaGenericApplication<rectificatoria_firma_detalle, rectificatoria_firma_detalle_entity, IRectificatoriaFirmaDetalleRepository>,
          IRectificatoriaFirmaDetalleApplication
    {
        private readonly IRectificatoriaFirmaDetalleRepository RectificatoriaFirmaDetalleRepositoryProvider;
        private readonly IUserApplication UserApplicationProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public RectificatoriaFirmaDetalleApplication(
            MultibancaDBContext _multibancaDBContext,
            IRectificatoriaFirmaDetalleRepository _rectificatoriaFirmaDetalleRepository,
            IUserApplication _userApplication,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper
        ) : base(_multibancaDBContext, _rectificatoriaFirmaDetalleRepository, _mapper)
        {
            RectificatoriaFirmaDetalleRepositoryProvider = _rectificatoriaFirmaDetalleRepository;
            UserApplicationProvider = _userApplication;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

    }
}
