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
    public class CalculoGeneracionDocumentoApplication : MultibancaGenericApplication<calculo_generacion_documento, calculo_generacion_documento_entity, ICalculoGeneracionDocumentoRepository>, ICalculoGeneracionDocumentoApplication
    {
        private readonly ICalculoGeneracionDocumentoRepository CalculoRepositoryProvider;
        private readonly IDatosOperacionPropiedadApplication DatosOperacionPropiedadProvider;
        private readonly IValorUfRepository ValorUfRepositoryProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;
        private readonly IMapper Mapper;

        public CalculoGeneracionDocumentoApplication(
            MultibancaDBContext _multibancaDBContext,
            ICalculoGeneracionDocumentoRepository _calculoRepository,
            IDatosOperacionPropiedadApplication _datosOperacionPropiedadApplication,
            IValorUfRepository _valorUfRepository,
            IWorkflowApplication _workflowApplication,
            IMapper _mapper) : base(_multibancaDBContext, _calculoRepository, _mapper)
        {
            CalculoRepositoryProvider = _calculoRepository;
            DatosOperacionPropiedadProvider = _datosOperacionPropiedadApplication;
            ValorUfRepositoryProvider = _valorUfRepository;
            WorkflowApplicationProvider = _workflowApplication;
            Mapper = _mapper;
        }

        public async Task<calculo_generacion_documento?> GetByExpediente(long id_expediente)
        {
            var entity = await CalculoRepositoryProvider.GetByExpediente(id_expediente);
            if (entity == null)
                return null;
            var datos_Operacion_Propiedad = await DatosOperacionPropiedadProvider.GetByExpediente(id_expediente);

            var model = Mapper.Map<calculo_generacion_documento?>(entity);

            var ufHoy = await ValorUfRepositoryProvider.GetByFecha(DateTime.Today);
            if (model != null)
                model.valor_uf_fecha_hoy = ufHoy?.valor;

            if (model != null && datos_Operacion_Propiedad != null)
            {
                model.tipo_propiedad = datos_Operacion_Propiedad.tipo_propiedad;
                model.tipo_direccion = datos_Operacion_Propiedad.tipo_direccion;
                model.direccion = datos_Operacion_Propiedad.direccion;
                model.region = datos_Operacion_Propiedad.region;
                model.comuna = datos_Operacion_Propiedad.comuna;
                model.rol_avaluo = datos_Operacion_Propiedad.rol_avaluo_1 +' '+ datos_Operacion_Propiedad.rol_avaluo_2;
            }

            return model;





            //var entity = await GestionRectificatoriaSolucionReparoProvider.GetByExpediente(id_expediente);

            //if (entity == null)
            //    return null;

            //var gestionRectificatoria = await GestionRectificatoriaApplicationProvider.GetByExpediente(id_expediente);

            //var result = Mapper.Map<gestion_rectificatoria_solucion_reparo?>(entity);

            //if (result != null && gestionRectificatoria != null)
            //{
            //    var user = UserApplicationProvider.FindId(gestionRectificatoria.created_by);
            //    result.id_solicitud = gestionRectificatoria.id_gestion_rectificatoria;
            //    result.id_solicitante = user.user_id;
            //    result.solicitante = user != null
            //? $"{user?.name} {user?.last_name_first} {user?.last_name_second}".Trim()
            //: gestionRectificatoria.created_by.ToString();
            //    result.observaciones_reparo = gestionRectificatoria.observaciones;
            //    result.fecha_ingreso = gestionRectificatoria.created_date;
            //}

            //return result;
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long expediente_id, int usuario_id, string actividad_id)
        {
            var entity = await CalculoRepositoryProvider.GetByExpediente(expediente_id);

            if (entity == null)
            {
                throw new InvalidOperationException($"No existe registro de Cálculo y Generación Documento para el expediente {expediente_id}.");
            }

            if (string.IsNullOrEmpty(entity.revision_rol_propiedad))
            {
                throw new InvalidOperationException("El campo 'Revisión Rol Propiedad' es obligatorio para avanzar.");
            }

            if (entity.revision_rol_propiedad != "ROL_INCORRECTO" && entity.revision_rol_propiedad != "ACEPTAR")
            {
                throw new InvalidOperationException("El valor de 'Revisión Rol Propiedad' no es válido. Valores aceptados: ROL_INCORRECTO, ACEPTAR.");
            }

            var folio = await WorkflowApplicationProvider.CapturarDatosFolio(expediente_id, actividad_id);

            var listTransitions = await WorkflowApplicationProvider.GetTransitions(actividad_id);

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
                transitionsID = listTransitions
                    .Where(x =>
                        x.name == "CalculoGeneracionDocumento_GenerarMemoEscritura_ReparoNO")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }

            return await WorkflowApplicationProvider.AvanzarActividad(transitionsID, folio, usuario_id);
        }

        public async Task<decimal?> CalcularUF(DateTime fecha)
        {
            var uf = await ValorUfRepositoryProvider.GetByFecha(fecha);
            return uf?.valor;
        }
    }
}
