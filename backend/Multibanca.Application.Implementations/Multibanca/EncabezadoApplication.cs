using Data.Repository.Interfaces.Repositories.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.CargaOperacionBanco;
using Multibanca.DTO.Multibanca;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Application.Interfaces.Multibanca
{
    public class EncabezadoApplication : IEncabezadoApplication
    {
        private readonly IEncabezadoRepository EncabezadoRepositoryProvider;
        private readonly ICargaOperacionBancoDatosOperacionApplication CargaOperacionBancoDatosOperacionApplicationProvider;
        private readonly ICargaOperacionBancoAntecedenteCompradorApplication CargaOperacionBancoAntecedenteCompradorApplicationProvider;
        private readonly ICargaOperacionBancoAntecedenteCreditoApplication CargaOperacionBancoAntecedenteCreditoApplicationProvider;
        private readonly ICargaOperacionBancoDatosComercialApplication CargaOperacionBancoDatosComercialApplicationProvider;

        public EncabezadoApplication(
            IEncabezadoRepository _encabezadoRepository,
            ICargaOperacionBancoDatosOperacionApplication _cargaOperacionBancoDatosOperacionApplication,
            ICargaOperacionBancoAntecedenteCompradorApplication _cargaOperacionBancoAntecedenteCompradorApplication,
            ICargaOperacionBancoAntecedenteCreditoApplication _cargaOperacionBancoAntecedenteCreditoApplication,
            ICargaOperacionBancoDatosComercialApplication _cargaOperacionBancoDatosComercialApplication)
        {
            EncabezadoRepositoryProvider = _encabezadoRepository;
            CargaOperacionBancoDatosOperacionApplicationProvider = _cargaOperacionBancoDatosOperacionApplication;
            CargaOperacionBancoAntecedenteCompradorApplicationProvider = _cargaOperacionBancoAntecedenteCompradorApplication;
            CargaOperacionBancoAntecedenteCreditoApplicationProvider = _cargaOperacionBancoAntecedenteCreditoApplication;
            CargaOperacionBancoDatosComercialApplicationProvider = _cargaOperacionBancoDatosComercialApplication;
        }

        public async Task<EncabezadoDTO> InformacionEncabezado(long idExpediente, string? activityID)
        {
            EncabezadoDTO encabezado = await EncabezadoRepositoryProvider.InformacionEncabezado(idExpediente, activityID ?? string.Empty)
                ?? new EncabezadoDTO();

            encabezado.id_expediente = idExpediente;

            await PoblarCamposBbva(encabezado, idExpediente);

            return encabezado;
        }

        private async Task PoblarCamposBbva(EncabezadoDTO encabezado, long idExpediente)
        {
            carga_operacion_banco_datos_operacion datosOperacion =
                await CargaOperacionBancoDatosOperacionApplicationProvider.GetByExpediente(idExpediente);

            if (datosOperacion != null)
            {
                encabezado.id_scoring = datosOperacion.id_scoring;
                encabezado.codigo_asesor_bbva = datosOperacion.codigo_asesor;
                encabezado.codigo_oficina_bbva = datosOperacion.codigo_oficina;
                encabezado.descripcion_oficina_bbva = datosOperacion.descripcion_oficina;
                encabezado.canal_originacion = datosOperacion.canal_originacion;
                encabezado.tipo_inmueble = datosOperacion.tipo_inmueble;
                encabezado.estado_inmueble = datosOperacion.estado_inmueble;
                encabezado.codigo_proyecto = datosOperacion.codigo_proyecto;
                encabezado.descripcion_proyecto = datosOperacion.descripcion_proyecto;
            }

            List<carga_operacion_banco_antecedente_comprador> antecedentesComprador =
                await CargaOperacionBancoAntecedenteCompradorApplicationProvider.GetByExpediente(idExpediente);

            List<carga_operacion_banco_antecedente_comprador> compradoresActivos = (antecedentesComprador ?? new())
                .Where(c => c.is_active && c.row_status)
                .OrderBy(c => c.id_carga_operacion_banco_antecedente_comprador)
                .ToList();

            carga_operacion_banco_antecedente_comprador? Titular(string tipoTitular, int fallbackIndex) =>
                compradoresActivos.FirstOrDefault(c => string.Equals(c.tipo_titular, tipoTitular, StringComparison.OrdinalIgnoreCase))
                ?? (compradoresActivos.Count > fallbackIndex ? compradoresActivos[fallbackIndex] : null);

            carga_operacion_banco_antecedente_comprador? titular1 = Titular("T1", 0);
            carga_operacion_banco_antecedente_comprador? titular2 = Titular("T2", 1);

            if (titular1 != null)
            {
                encabezado.tipo_documento_id_t1 = titular1.tipo_documento_id;
                encabezado.numero_identificacion_t1 = titular1.numero_identificacion;
                encabezado.nombre_completo_t1 = titular1.nombre_completo;
                encabezado.celular_t1 = titular1.celular;
                encabezado.situacion_laboral = titular1.situacion_laboral;
                encabezado.cliente_nomina = titular1.cliente_nomina;
            }

            if (titular2 != null)
            {
                encabezado.numero_identificacion_t2 = titular2.numero_identificacion;
                encabezado.nombre_completo_t2 = titular2.nombre_completo;
            }

            carga_operacion_banco_antecedente_credito antecedenteCredito =
                await CargaOperacionBancoAntecedenteCreditoApplicationProvider.GetByExpediente(idExpediente);

            if (antecedenteCredito != null)
            {
                encabezado.id_tipo_sub_producto = antecedenteCredito.id_tipo_sub_producto;
                encabezado.monto_otorgado = antecedenteCredito.monto_otorgado;
                encabezado.fecha_aprobacion = antecedenteCredito.fecha_aprobacion;
                encabezado.condiciones_organismo_decisor = antecedenteCredito.condiciones_organismo_decisor;
                encabezado.plazo = antecedenteCredito.plazo ?? 0;
            }

            carga_operacion_banco_datos_comercial datosComercial =
                await CargaOperacionBancoDatosComercialApplicationProvider.GetByExpediente(idExpediente);

            if (datosComercial != null)
            {
                encabezado.correo_declarativo = datosComercial.correo_declarativo_cliente;
                encabezado.telefono_declarativo = datosComercial.numero_telefono_declarativo;
            }
        }
    }
}
