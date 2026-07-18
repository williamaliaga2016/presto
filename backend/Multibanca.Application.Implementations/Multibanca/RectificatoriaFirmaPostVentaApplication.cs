using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Entities.Multibanca.ValidacionRectificatoriaLegal;
using Data.Repository.Interfaces.Entities.Multibanca.ValidacionRectificatoriaLegalPostventa;
using Data.Repository.Interfaces.Repositories.Common;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca.ValidacionRectificatoriaLegal;
using Data.Repository.Interfaces.Repositories.Multibanca.ValidacionRectificatoriaLegalPostventa;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.ValidacionRectificatoriaLegal;
using Multibanca.Application.Interfaces.Security;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.Domain.Models.Multibanca.ValidacionRectificatoriaLegal;
using Multibanca.Domain.Models.Security;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class RectificatoriaFirmaPostVentaApplication
        : MultibancaGenericApplication<rectificatoria_firma_post_venta, rectificatoria_firma_post_venta_entity, IRectificatoriaFirmaPostVentaRepository>,
          IRectificatoriaFirmaPostVentaApplication
    {
        private readonly IRectificatoriaFirmaPostVentaRepository RectificatoriaFirmaPostVentaRepositoryProvider;
        private readonly IValidacionRectificatoriaLegalPostventaRepository ValidacionRectificatoriaLegalPostventaRepositoryProvider;
        private readonly IValidacionRectificatoriaLegalPostventaDatosPersonalesRepository ValidacionRectificatoriaLegalPostventaDatosPersonalesRepositoryProvider;
        private readonly IUserApplication UserApplicationProvider;
        private readonly ICommonRepository CommonRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public RectificatoriaFirmaPostVentaApplication(
            MultibancaDBContext _multibancaDBContext,
            IRectificatoriaFirmaPostVentaRepository _rectificatoriaFirmaPostVentaRepository,
            IValidacionRectificatoriaLegalPostventaRepository _validacionRectificatoriaLegalPostventarepository,
            IValidacionRectificatoriaLegalPostventaDatosPersonalesRepository _validacionRectificatoriaLegalPostventaDatosPersonalesRepository,
            IUserApplication _userApplication,
            ICommonRepository _commonRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper
        ) : base(_multibancaDBContext, _rectificatoriaFirmaPostVentaRepository, _mapper)
        {
            RectificatoriaFirmaPostVentaRepositoryProvider = _rectificatoriaFirmaPostVentaRepository;
            ValidacionRectificatoriaLegalPostventaRepositoryProvider = _validacionRectificatoriaLegalPostventarepository;
            ValidacionRectificatoriaLegalPostventaDatosPersonalesRepositoryProvider = _validacionRectificatoriaLegalPostventaDatosPersonalesRepository;
            UserApplicationProvider = _userApplication;
            CommonRepositoryProvider = _commonRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<rectificatoria_firma_post_venta?> GetByExpediente(long id_expediente)
        {
            var entity = await RectificatoriaFirmaPostVentaRepositoryProvider.GetByExpediente(id_expediente);

            if (entity != null)
            {
                entity.id_expediente = id_expediente;
            }

            var domain = Mapper.Map<rectificatoria_firma_post_venta?>(entity)
                ?? new rectificatoria_firma_post_venta();

            domain.id_expediente = id_expediente;

            await CompletarDatosHeredados(domain);

            return domain;
        }

        public async Task<List<rectificatoria_firma_post_venta_detalle>>GetRectificatoriaDetByExpediente(long id_expediente,string rol_comparecencia)
        {
            List<rectificatoria_firma_post_venta_detalle_entity> entity = await RectificatoriaFirmaPostVentaRepositoryProvider.GetRectificatoriaDetByExpediente(id_expediente);

            var domain = Mapper.Map<List<rectificatoria_firma_post_venta_detalle>>(entity) ?? new List<rectificatoria_firma_post_venta_detalle>();

            List<validacion_rectificatoria_legal_postventa_datos_personales_entity> entityDet =
                await ValidacionRectificatoriaLegalPostventaDatosPersonalesRepositoryProvider.GetByExpediente(id_expediente);

            var domainDet = Mapper.Map<List<validacion_rectificatoria_legal_datos_personales>>(entityDet);

            var catalogoRelacionTitular = await CommonRepositoryProvider.GetCatalogoByType("RELACION_TITULAR");

            var relacionTitularMap = catalogoRelacionTitular
                                        .Where(x => !string.IsNullOrEmpty(x.code))
                                        .ToDictionary(x => x.code!, x => x.description ?? string.Empty);

            if (!domain.Any())
            {
                domain = domainDet
                    .Where(x => x.rol_comparecencia == rol_comparecencia)
                    .Select(x => new rectificatoria_firma_post_venta_detalle
                    {
                        id_expediente = id_expediente,
                        rut = x.rut,
                        relacion_titular = relacionTitularMap.TryGetValue(
                                                x.relacion_titular ?? string.Empty,
                                                out var descripcion)
                                                    ? descripcion
                                                    : x.relacion_titular,
                        nombres = x.nombres,
                        apellido_paterno = x.apellido_paterno,
                        apellido_materno = x.apellido_materno,
                        rol_comparecencia = x.rol_comparecencia
                    })
                    .ToList();

                return domain;
            }

            foreach (var item in domain)
            {
                var persona = domainDet.FirstOrDefault(x => x.rut == item.rut);

                if (persona != null)
                {
                    item.relacion_titular = persona.relacion_titular;
                    item.nombres = persona.nombres;
                    item.apellido_paterno = persona.apellido_paterno;
                    item.apellido_materno = persona.apellido_materno;
                    item.rol_comparecencia=persona.rol_comparecencia;
                }
                if (!string.IsNullOrEmpty(item.relacion_titular) &&
                        relacionTitularMap.TryGetValue(item.relacion_titular, out var descripcion))
                {
                    item.relacion_titular = descripcion;
                }
            }

            return domain;

        }

        public async Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        )
        {

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(
                expediente_id,
                actividad_id
            );

            List<xpdl_transition_DTO> listTransitions =
                await WorkflowApplicationProvider.GetTransitions(actividad_id);

            var entity = await RectificatoriaFirmaPostVentaRepositoryProvider.GetByExpediente(expediente_id);

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Rectificatoria Firma PostVenta para el expediente {expediente_id}."
                );
            }

            if (!entity.is_subsanar)
            {
                throw new InvalidOperationException(
                    "Debe marcar el reparo como subsanado antes de avanzar."
                );
            }

            string transitionsID = listTransitions
                .Select(q => q.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Rectificatoria Firma PostVenta."
                );
            }

            return await WorkflowApplicationProvider.AvanzarActividad(
                transitionsID,
                folio,
                usuario_id
            );
        }

        private async Task CompletarDatosHeredados(rectificatoria_firma_post_venta domain)
        {
            if (domain == null || domain.id_expediente <= 0)
            {
                return;
            }

            validacion_rectificatoria_legal_postventa_entity? actividadOrigen =
                await ValidacionRectificatoriaLegalPostventaRepositoryProvider.GetByExpediente(domain.id_expediente);

            if (actividadOrigen != null)
            {
                domain.observaciones_reparo = actividadOrigen.observaciones;
                domain.fecha_ingreso = actividadOrigen.modified_date ?? actividadOrigen.created_date;

                if (domain.id_usuario_solicitante <= 0)
                {
                    domain.id_usuario_solicitante =
                        actividadOrigen.modified_by ?? actividadOrigen.created_by;
                }
            }

            if (domain.id_usuario_solicitante > 0)
            {
                users usuario = UserApplicationProvider.FindId(domain.id_usuario_solicitante);

                if (usuario != null)
                {
                    domain.solicitante =
                        $"{usuario.name} {usuario.last_name_first} {usuario.last_name_second}"
                        .Replace("  ", " ")
                        .Trim();
                }
            }
        }
    }
}
