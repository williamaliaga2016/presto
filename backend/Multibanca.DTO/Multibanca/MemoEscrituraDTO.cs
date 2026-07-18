using System;
using System.Collections.Generic;

namespace Multibanca.DTO.Multibanca
{
    public class MemoEscrituraDTO
    {
        public long id_expediente { get; set; }
        public string observaciones { get; set; }
        public bool is_enviar_reparo { get; set; }
    }

    public class MemoEscrituraDataDTO
    {
        public long id_expediente { get; set; }
        public EncabezadoMemoDTO encabezado { get; set; }
        public List<AntecedentePersonalMemoDTO> antecedentes_personales { get; set; }
        public AntecedentesPrestamoMemoDTO antecedentes_prestamo { get; set; }
        public DeudaCompradoresMemoDTO deuda_compradores { get; set; }
        public AntecedentesCreditoMemoDTO antecedentes_credito { get; set; }
        public List<SeguroMemoDTO> seguros { get; set; }
        public decimal impuesto_al_mutuo { get; set; }
        public GastosOperacionalesMemoDTO gastos_operacionales { get; set; }
        public DividendosMemoDTO dividendos { get; set; }
        public MedioPagoPACMemoDTO medio_pago_pac { get; set; }
        public List<AntecedentePropiedadMemoDTO> antecedentes_propiedad { get; set; }
        public OtrosAntecedentesMemoDTO otros_antecedentes { get; set; }
        public ResolucionMemoDTO resolucion { get; set; }
    }

    public class EncabezadoMemoDTO
    {
        public string nro_solicitud { get; set; }
        public string nro_mutuo { get; set; }
        public string mes_calculo { get; set; }
        public decimal monto_prestamo_uf { get; set; }
        public decimal tasa_porcentaje { get; set; }
        public int plazo_anios { get; set; }
        public decimal costo_total_credito_uf { get; set; }
        public decimal cae_porcentaje { get; set; }
        public decimal valor_uf_hoy { get; set; }
        public DateTime fecha_uf_hoy { get; set; }
        public decimal valor_uf_calculo { get; set; }
        public DateTime fecha_uf_calculo { get; set; }
        public string oficina_origen { get; set; }
        public bool credito_en_uf { get; set; }
        public string fecha_escritura_texto { get; set; }
    }

    public class AntecedentePersonalMemoDTO
    {
        public string relacion { get; set; }
        public string rut { get; set; }
        public string nombre_razon_social { get; set; }
        public string estado_civil { get; set; }
        public string regimen_bienes { get; set; }
    }

    public class AntecedentesPrestamoMemoDTO
    {
        public string tipo_prestamo { get; set; }
        public string tipo_prestamo_subproducto { get; set; }
        public string destino_prestamo { get; set; }
        public decimal precio_venta_uf { get; set; }
        public decimal precio_venta_clp { get; set; }
        public decimal valor_tasacion_uf { get; set; }
        public decimal valor_tasacion_clp { get; set; }
        public decimal valor_asegurable_uf { get; set; }
        public decimal valor_asegurable_clp { get; set; }
        public DateTime fecha_tasacion { get; set; }
        public decimal valor_uf_tasacion { get; set; }
        public decimal prestamo_maximo_clp { get; set; }
        public decimal monto_solicitado_uf { get; set; }
        public decimal monto_solicitado_clp { get; set; }
        public decimal dividendo_pagar_uf { get; set; }
        public decimal dividendo_pagar_clp { get; set; }
        public decimal renta_liquida_ajustada_clp { get; set; }
        public string tipo_comision_tasa { get; set; }
        public string meses_sabaticos { get; set; }
        public string periodo_gracia { get; set; }
    }

    public class DeudaCompradoresMemoDTO
    {
        public decimal monto_total_pension_uf { get; set; }
        public decimal monto_total_pension_clp { get; set; }
        public int total_cuotas_impagas { get; set; }
        public decimal valor_cuota_pension_uf { get; set; }
        public decimal valor_cuota_pension_clp { get; set; }
    }

    public class AntecedentesCreditoMemoDTO
    {
        public string serie_codigo_bursatil_serie { get; set; }
        public string serie_codigo_bursatil_codigo { get; set; }
        public decimal monto_credito_uf { get; set; }
        public decimal monto_credito_clp { get; set; }
        public decimal monto_cuota_contado_uf { get; set; }
        public decimal monto_cuota_contado_clp { get; set; }
        public decimal precio_venta_uf { get; set; }
        public decimal precio_venta_clp { get; set; }
        public int plazo_anios { get; set; }
        public decimal tasa_preferencial { get; set; }
        public decimal tasa_estandar { get; set; }
    }

    public class SeguroMemoDTO
    {
        public string descripcion { get; set; }
        public decimal monto_uf { get; set; }
    }

    public class GastosOperacionalesMemoDTO
    {
        public decimal conservador_bienes_raices { get; set; }
        public decimal escrituracion { get; set; }
        public decimal estudio_titulos { get; set; }
        public decimal gastos_notariales { get; set; }
        public decimal servicio_inscripcion_cbr { get; set; }
        public decimal tasacion { get; set; }
        public decimal total_gastos_operacionales { get; set; }
    }

    public class DividendosMemoDTO
    {
        public List<RangoDividendoMemoDTO> rangos { get; set; }
    }

    public class RangoDividendoMemoDTO
    {
        public int del { get; set; }
        public int al { get; set; }
        public decimal uf { get; set; }
    }

    public class MedioPagoPACMemoDTO
    {
        public string numero_pac { get; set; }
        public string tipo_medio_pago { get; set; }
    }

    public class AntecedentePropiedadMemoDTO
    {
        public string tipo { get; set; }
        public string direccion { get; set; }
        public string rol_avaluo { get; set; }
        public string estado { get; set; }
    }

    public class OtrosAntecedentesMemoDTO
    {
        public string beneficio_tributario { get; set; }
        public string beneficiario { get; set; }
        public string destino_fondos { get; set; }
        public string hipoteca_mandato { get; set; }
        public string observaciones { get; set; }
    }

    public class ResolucionMemoDTO
    {
        public string texto_aprobado { get; set; }
    }
}
