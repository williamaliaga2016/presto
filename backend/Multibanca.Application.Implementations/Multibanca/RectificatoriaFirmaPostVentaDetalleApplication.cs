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
    public class RectificatoriaFirmaPostVentaDetalleApplication
        : MultibancaGenericApplication<rectificatoria_firma_post_venta_detalle, rectificatoria_firma_post_venta_detalle_entity, IRectificatoriaFirmaPostVentaDetalleRepository>,
          IRectificatoriaFirmaPostVentaDetalleApplication
    {
        private readonly IRectificatoriaFirmaPostVentaDetalleRepository RectificatoriaFirmaPostVentaDetalleRepositoryProvider;
        private readonly IUserApplication UserApplicationProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public RectificatoriaFirmaPostVentaDetalleApplication(
            MultibancaDBContext _multibancaDBContext,
            IRectificatoriaFirmaPostVentaDetalleRepository _rectificatoriaFirmaPostVentaDetalleRepository,
            IUserApplication _userApplication,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper
        ) : base(_multibancaDBContext, _rectificatoriaFirmaPostVentaDetalleRepository, _mapper)
        {
            RectificatoriaFirmaPostVentaDetalleRepositoryProvider = _rectificatoriaFirmaPostVentaDetalleRepository;
            UserApplicationProvider = _userApplication;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

    }
}
