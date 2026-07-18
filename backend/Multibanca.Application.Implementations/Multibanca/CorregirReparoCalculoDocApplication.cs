using AutoMapper;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.DatosOperacion;
using Multibanca.Application.Interfaces.Security;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.Domain.Models.Multibanca.DatosOperacion;
using Multibanca.Domain.Models.Security;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class CorregirReparoCalculoDocApplication
        : MultibancaGenericApplication<corregir_reparo_calculo_doc, corregir_reparo_calculo_doc_entity, ICorregirReparoCalculoDocRepository>,
          ICorregirReparoCalculoDocApplication
    {
        private readonly ICorregirReparoCalculoDocRepository CorregirReparoCalculoDocRepositoryProvider;
        private readonly ICalculoGeneracionDocumentoApplication CalculoGeneracionDocumentoApplicationProvider;
        private readonly IDatosOperacionPropiedadApplication DatosOperacionPropiedadApplicationProvider;
        private readonly IUserApplication UserApplicationProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public CorregirReparoCalculoDocApplication(
            MultibancaDBContext _multibancaDBContext,
            ICorregirReparoCalculoDocRepository _corregirReparoCalculoDocRepository,
            ICalculoGeneracionDocumentoApplication _calculoGeneracionDocumentoApplication,
            IDatosOperacionPropiedadApplication _datosOperacionPropiedadApplication,
            IUserApplication _userApplication,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper
        ) : base(_multibancaDBContext, _corregirReparoCalculoDocRepository, _mapper)
        {
            CorregirReparoCalculoDocRepositoryProvider = _corregirReparoCalculoDocRepository;
            CalculoGeneracionDocumentoApplicationProvider = _calculoGeneracionDocumentoApplication;
            DatosOperacionPropiedadApplicationProvider = _datosOperacionPropiedadApplication;
            UserApplicationProvider = _userApplication;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<corregir_reparo_calculo_doc?> GetByExpediente(long id_expediente)
        {
            var entity =
                await CorregirReparoCalculoDocRepositoryProvider.GetByExpediente(
                    id_expediente
                );

            var domain = Mapper.Map<corregir_reparo_calculo_doc?>(entity)
                ?? new corregir_reparo_calculo_doc();

            domain.id_expediente = id_expediente;

            await CompletarDatosHeredados(domain, id_expediente);
            await CompletarSolicitante(domain);

            return domain;
        }

        public async Task<List<AssignActivityDTO>> Avanzar(
            long expediente_id,
            int usuario_id,
            string actividad_id
        )
        {
            var entity =
                await CorregirReparoCalculoDocRepositoryProvider.GetByExpediente(
                    expediente_id
                );

            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Corregir Reparo Cálculo y Generación Documento para el expediente {expediente_id}."
                );
            }

            if (!entity.is_subsanar)
            {
                throw new InvalidOperationException(
                    "Debe marcar el reparo como subsanado antes de avanzar."
                );
            }

            FolioDTO folio = await WorkflowApplicationProvider.CapturarDatosFolio(
                expediente_id,
                actividad_id
            );

            List<xpdl_transition_DTO> listTransitions =
                await WorkflowApplicationProvider.GetTransitions(actividad_id);

            string transitionsID = listTransitions
                .Select(q => q.transition_id)
                .FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(transitionsID))
            {
                throw new InvalidOperationException(
                    "No se encontró transición configurada para Corregir Reparo Cálculo y Generación Documento."
                );
            }

            return await WorkflowApplicationProvider.AvanzarActividad(
                transitionsID,
                folio,
                usuario_id
            );
        }

        private async Task CompletarDatosHeredados(
            corregir_reparo_calculo_doc domain,
            long id_expediente
        )
        {
            calculo_generacion_documento? actividadOrigen =
                await CalculoGeneracionDocumentoApplicationProvider.GetByExpediente(
                    id_expediente
                );

            datos_operacion_propiedad? propiedadOrigen = null;

            try
            {
                propiedadOrigen =
                    await DatosOperacionPropiedadApplicationProvider.GetByExpediente(
                        id_expediente
                    );
            }
            catch
            {
                propiedadOrigen = null;
            }

            domain.tipo_propiedad = PreferirValor(
                actividadOrigen?.tipo_propiedad,
                propiedadOrigen?.tipo_propiedad
            );

            domain.tipo_direccion = PreferirValor(
                actividadOrigen?.tipo_direccion,
                propiedadOrigen?.tipo_direccion
            );

            domain.direccion = PreferirValor(
                actividadOrigen?.direccion,
                propiedadOrigen?.direccion
            );

            domain.region = PreferirValor(
                actividadOrigen?.region,
                propiedadOrigen?.region
            );

            domain.comuna = PreferirValor(
                actividadOrigen?.comuna,
                propiedadOrigen?.comuna
            );

            domain.rol_avaluo = PreferirValor(
                actividadOrigen?.rol_avaluo,
                propiedadOrigen?.rol_avaluo_1 ?? propiedadOrigen?.rol_avaluo_2
            );

            if (actividadOrigen != null)
            {
                domain.observaciones_reparo = actividadOrigen.observaciones;
                domain.fecha_ingreso =
                    actividadOrigen.modified_date ?? actividadOrigen.created_date;

                if (domain.id_usuario_solicitante == 0)
                {
                    domain.id_usuario_solicitante =
                        actividadOrigen.modified_by ?? actividadOrigen.created_by;
                }
            }

            if (string.IsNullOrWhiteSpace(domain.existe_rol_avaluo))
            {
                domain.existe_rol_avaluo = ObtenerExisteRolAvaluo(
                    actividadOrigen,
                    propiedadOrigen,
                    domain.rol_avaluo
                );
            }

            if (string.IsNullOrWhiteSpace(domain.rol_avaluo_editado))
            {
                domain.rol_avaluo_editado = domain.rol_avaluo;
            }
        }

        private async Task CompletarSolicitante(corregir_reparo_calculo_doc domain)
        {
            if (domain.id_usuario_solicitante <= 0)
            {
                domain.solicitante = string.Empty;
                return;
            }

            users? user = UserApplicationProvider.FindId(domain.id_usuario_solicitante);

            domain.solicitante = user == null
                ? string.Empty
                : $"{user.name} {user.last_name_first} {user.last_name_second}".Trim();
        }

        private static string? PreferirValor(string? valorPrincipal, string? valorRespaldo)
        {
            return !string.IsNullOrWhiteSpace(valorPrincipal)
                ? valorPrincipal
                : valorRespaldo;
        }

        private static string ObtenerExisteRolAvaluo(
            calculo_generacion_documento? actividadOrigen,
            datos_operacion_propiedad? propiedadOrigen,
            string? rolAvaluo
        )
        {
            if (string.Equals(
                    actividadOrigen?.revision_rol_propiedad,
                    "ROL_INCORRECTO",
                    StringComparison.OrdinalIgnoreCase
                ))
            {
                return "E/T";
            }

            if (!string.IsNullOrWhiteSpace(propiedadOrigen?.existe_rol_avaluo))
            {
                return propiedadOrigen!.existe_rol_avaluo!;
            }

            return string.IsNullOrWhiteSpace(rolAvaluo) ? "E/T" : "SI";
        }
    }
}
