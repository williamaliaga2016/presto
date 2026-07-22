using AutoMapper;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Entities.Multibanca.BBVA;
using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Data.Repository.Interfaces.Entities.Multibanca.BBVA.ValidarIntegracionDocumentos;
using Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco;
using Data.Repository.Interfaces.Entities.Multibanca.DatosOperacion;
using Data.Repository.Interfaces.Entities.Multibanca.GenerarBorradorEscritura;
using Data.Repository.Interfaces.Entities.Multibanca.RevisarDatosOperacion;
using Data.Repository.Interfaces.Entities.Multibanca.ValidacionRectificatoriaLegal;
using Data.Repository.Interfaces.Entities.Multibanca.ValidacionRectificatoriaLegalPostventa;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.Domain.Models.Multibanca.BBVA;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;
using Multibanca.Domain.Models.Multibanca.BBVA.ValidarIntegracionDocumentos;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;
using Multibanca.Domain.Models.Multibanca.DatosOperacion;
using Multibanca.Domain.Models.Multibanca.GenerarBorradorEscritura;
using Multibanca.Domain.Models.Multibanca.RevisarDatosOperacion;
using Multibanca.Domain.Models.Multibanca.ValidacionRectificatoriaLegal;
using Multibanca.Domain.Models.Multibanca.ValidacionRectificatoriaLegalPostventa;

namespace Multibanca.Automapper.Multibanca
{
    public class AutoMapperProfileMultibanca : Profile
    {
        public AutoMapperProfileMultibanca()
        {
            CreateMap<reports, reports_entity>().ReverseMap();
            CreateMap<actividades, actividades_entity>().ReverseMap();

            CreateMap<carga_operacion_banco, carga_operacion_banco_entity>().ReverseMap();
            CreateMap<carga_operacion_banco_datos_operacion, carga_operacion_banco_datos_operacion_entity>().ReverseMap();
            CreateMap<carga_operacion_banco_antecedente_comprador, carga_operacion_banco_antecedente_comprador_entity>().ReverseMap();
            CreateMap<carga_operacion_banco_antecedente_vendedor_entity, carga_operacion_banco_antecedente_vendedor>().ReverseMap();
            CreateMap<carga_operacion_banco_antecedente_credito_entity, carga_operacion_banco_antecedente_credito>().ReverseMap();
            CreateMap<carga_operacion_banco_datos_comercial_entity, carga_operacion_banco_datos_comercial>().ReverseMap();
            CreateMap<recepcion_carga_fabrica, recepcion_carga_fabrica_entity>().ReverseMap();
            CreateMap<corregir_reparo_fabrica, corregir_reparo_fabrica_entity>().ReverseMap();
            CreateMap<corregir_carta_resguardo, corregir_carta_resguardo_entity>().ReverseMap();
            CreateMap<corregir_reparo_tasacion, corregir_reparo_tasacion_entity>().ReverseMap();
            CreateMap<asignar_escritura, asignar_escritura_entity>().ReverseMap();
            CreateMap<asignar_estudio_titulos, asignar_estudio_titulos_entity>().ReverseMap();
            CreateMap<generar_prefiniquito, generar_prefiniquito_entity>().ReverseMap();
            CreateMap<corregir_reparo_prefiniquito, corregir_reparo_prefiniquito_entity>().ReverseMap();
            CreateMap<datos_operacion, datos_operacion_entity>().ReverseMap();
            CreateMap<datos_operacion_datos_credito, datos_operacion_datos_credito_entity>().ReverseMap();
            CreateMap<datos_operacion_comprador, datos_operacion_comprador_entity>().ReverseMap();
            CreateMap<datos_operacion_vendedor, datos_operacion_vendedor_entity>().ReverseMap();
            CreateMap<datos_operacion_fiador_garante, datos_operacion_fiador_garante_entity>().ReverseMap();
            CreateMap<datos_operacion_banco_acreedor, datos_operacion_banco_acreedor_entity>().ReverseMap();
            CreateMap<datos_operacion_propiedad, datos_operacion_propiedad_entity>().ReverseMap();
            CreateMap<corregir_reparo_calculo_doc, corregir_reparo_calculo_doc_entity>().ReverseMap();
            CreateMap<revisar_datos_operacion_propiedad, revisar_datos_operacion_propiedad_entity>().ReverseMap();
            CreateMap<revisar_datos_operacion_credito, revisar_datos_operacion_credito_entity>().ReverseMap();
            CreateMap<revisar_datos_operacion_comprador, revisar_datos_operacion_comprador_entity>().ReverseMap();
            CreateMap<revisar_datos_operacion_vendedor, revisar_datos_operacion_vendedor_entity>().ReverseMap();
            CreateMap<corregir_reparo_generar_memo_escritura, corregir_reparo_generar_memo_escritura_entity>().ReverseMap();
            CreateMap<verificar_reparo_estudio_titulo, verificar_reparo_estudio_titulo_entity>().ReverseMap();
            CreateMap<corregir_reparo_datos_operacion, corregir_reparo_datos_operacion_entity>().ReverseMap();
            CreateMap<corregir_reparo_cdr, corregir_reparo_cdr_entity>().ReverseMap();
            CreateMap<calculo_generacion_documento, calculo_generacion_documento_entity>().ReverseMap();
            CreateMap<corregir_reparo_estudio_titulos, corregir_reparo_estudio_titulos_entity>().ReverseMap();
            CreateMap<generar_estudio_titulos, generar_estudio_titulos_entity>().ReverseMap();
            CreateMap<corregir_reparo_visado, corregir_reparo_visado_entity>().ReverseMap();
            CreateMap<revisar_datos_operacion, revisar_datos_operacion_entity>().ReverseMap();
            CreateMap<corregir_reparo_datos_operacion, corregir_reparo_datos_operacion_entity>().ReverseMap();
            CreateMap<verificar_reparo_datos_operacion, verificar_reparo_datos_operacion_entity>().ReverseMap();
            CreateMap<revisar_ingreso_datos_credito, revisar_ingreso_datos_credito_entity>().ReverseMap();
            CreateMap<tasacion, tasacion_entity>().ReverseMap();
            CreateMap<tasacion_detalle, tasacion_detalle_entity>().ReverseMap();
            CreateMap<revisar_datos_operacion_banco, revisar_datos_operacion_banco_entity>().ReverseMap();
            CreateMap<revisar_datos_operacion_fiador_garante, revisar_datos_operacion_fiador_garante_entity>().ReverseMap();
            CreateMap<corregir_reparo_generar_borrador_escritura, corregir_reparo_generar_borrador_escritura_entity>().ReverseMap();
            CreateMap<generar_borrador_escritura, generar_borrador_escritura_entity>().ReverseMap();
            CreateMap<generar_borrador_escritura_detalle, generar_borrador_escritura_detalle_entity>().ReverseMap();
            CreateMap<generar_memo_escritura, generar_memo_escritura_entity>().ReverseMap();
            CreateMap<visar_operacion, visar_operacion_entity>().ReverseMap();
            CreateMap<firma_comprador, firma_comprador_entity>().ReverseMap();
            CreateMap<firma_comprador_detalle, firma_comprador_detalle_entity>().ReverseMap();
            CreateMap<firma_vendedor, firma_vendedor_entity>().ReverseMap();
            CreateMap<firma_vendedor_detalle, firma_vendedor_detalle_entity>().ReverseMap();
            CreateMap<firma_banco_acreedor_cg, firma_banco_acreedor_cg_entity>().ReverseMap();
            CreateMap<cierre_copias_notaria, cierre_copias_notaria_entity>().ReverseMap();
            CreateMap<corregir_reparo_cierre_copias_notaria, corregir_reparo_cierre_copias_notaria_entity>().ReverseMap();
            CreateMap<aprobacion_comercial_legal_cdr, aprobacion_comercial_legal_cdr_entity>().ReverseMap();
            CreateMap<revisar_inscripcion_cbr, revisar_inscripcion_cbr_entity>().ReverseMap();
            CreateMap<reingresar_escritura_cbr, reingresar_escritura_cbr_entity>().ReverseMap();
            CreateMap<corregir_reparo_liquidacion, corregir_reparo_liquidacion_entity>().ReverseMap();
            CreateMap<revisar_liquidacion, revisar_liquidacion_entity>().ReverseMap();
            CreateMap<control_escritura, control_escritura_entity>().ReverseMap();
            CreateMap<corregir_control_escritura, corregir_control_escritura_entity>().ReverseMap();
            CreateMap<revisar_desembolso, revisar_desembolso_entity>().ReverseMap();
            CreateMap<corregir_notaria_reparo_abogados, corregir_notaria_reparo_abogados_entity>().ReverseMap();

            CreateMap<corregir_reparo_entregar_carpeta, corregir_reparo_entregar_carpeta_entity>().ReverseMap();
            CreateMap<corregir_reparo_inst_pago, corregir_reparo_inst_pago_entity>().ReverseMap();
            CreateMap<corregir_reparos_gestor, corregir_reparos_gestor_entity>().ReverseMap();

            CreateMap<entregar_carpeta, entregar_carpeta_entity>().ReverseMap();
            CreateMap<visar_operacion, visar_operacion_entity>().ReverseMap(); 
            CreateMap<generar_carta_resguardo, generar_carta_resguardo_entity>().ReverseMap();

            CreateMap<generar_recursos_pagos_cbr, generar_recursos_pagos_cbr_entity>().ReverseMap();

            CreateMap<recepcionar_matriz, recepcionar_matriz_entity>().ReverseMap();
            CreateMap<verificar_reparo_cbr, verificar_reparo_cbr_entity>().ReverseMap(); 
            CreateMap<recibir_instruccion_pago, recibir_instruccion_pago_entity>().ReverseMap();

            CreateMap<registrar_escritura_cbr, registrar_escritura_cbr_entity>().ReverseMap();

            CreateMap<realizar_revision_previo_firma_banco, realizar_revision_previo_firma_banco_entity>().ReverseMap();
            CreateMap<valorizar_cbr, valorizar_cbr_entity>().ReverseMap();
            CreateMap<registrar_fecha_registro_cbr, registrar_fecha_registro_cbr_entity>().ReverseMap();

            CreateMap<generar_finiquito,generar_finiquito_entity>().ReverseMap();
            CreateMap<verificar_correccion_escritura, verificar_correccion_escritura_entity>().ReverseMap();
            CreateMap<realizar_control_credito,realizar_control_credito_entity>().ReverseMap();
            CreateMap<corregir_reparo_control_credito, corregir_reparo_control_credito_entity>().ReverseMap();

            CreateMap<registrar_firma_apoderado_banco, registrar_firma_apoderado_banco_entity>().ReverseMap();
            CreateMap<gestion_rectificatoria,gestion_rectificatoria_entity>().ReverseMap();
            CreateMap<registrar_firma_apoderado_banco, registrar_firma_apoderado_banco_entity>().ReverseMap(); 
            CreateMap<registrar_firma_apoderado_banco, registrar_firma_apoderado_banco_entity>().ReverseMap();
            CreateMap<validacion_rectificatoria_legal, validacion_rectificatoria_legal_entity>().ReverseMap();
            CreateMap<validacion_rectificatoria_legal_datos_personales, validacion_rectificatoria_legal_datos_personales_entity>().ReverseMap();
            CreateMap<gestion_rectificatoria_solucion_reparo, gestion_rectificatoria_solucion_reparo_entity>().ReverseMap();
            CreateMap<gestion_reparo, gestion_reparo_entity>().ReverseMap();

            CreateMap<reparo_formulario, reparo_formulario_entity>().ReverseMap();
            CreateMap<rectificatoria_firma, rectificatoria_firma_entity>().ReverseMap();
            CreateMap<rectificatoria_firma_detalle, rectificatoria_firma_detalle_entity>().ReverseMap();
            CreateMap<rectificatoria_legal_carta_resguardo, rectificatoria_legal_carta_resguardo_entity>().ReverseMap();
            CreateMap<rectificatoria_legal_firma_alzante, rectificatoria_legal_firma_alzante_entity>().ReverseMap();
            CreateMap<rectificatoria_legal_cierre_copias, rectificatoria_legal_cierre_copias_entity>().ReverseMap();
            CreateMap<rectificatoria_legal_cierre_copias_postventa, rectificatoria_legal_cierre_copias_postventa_entity>().ReverseMap();
            CreateMap<gestion_rectificatoria_escritura_firmada,gestion_rectificatoria_escritura_firmada_entity>().ReverseMap();
            CreateMap<rectificatoria_analisis_derivacion_reparo_postventa, rectificatoria_analisis_derivacion_reparo_postventa_entity>().ReverseMap();
            CreateMap<rectificatoria_postventa_solucion_reparo, rectificatoria_postventa_solucion_reparo_entity>().ReverseMap();
            CreateMap<validacion_rectificatoria_legal_postventa, validacion_rectificatoria_legal_postventa_entity>().ReverseMap();
            CreateMap<validacion_rectificatoria_legal_postventa_datos_personales, validacion_rectificatoria_legal_postventa_datos_personales_entity>().ReverseMap();
            CreateMap<rectificatoria_firma_post_venta, rectificatoria_firma_post_venta_entity>().ReverseMap();
            CreateMap<rectificatoria_firma_post_venta_detalle, rectificatoria_firma_post_venta_detalle_entity>().ReverseMap();
            CreateMap<gestion_rectificatoria_escritura_firmada_postventa, gestion_rectificatoria_escritura_firmada_postventa_entity>().ReverseMap();
            CreateMap<revisar_copias_escrituras, revisar_copias_escrituras_entity>().ReverseMap();
            CreateMap<corregir_reparo_copias_escrituras, corregir_reparo_copias_escrituras_entity>().ReverseMap();

            // BBVA Colombia — Presto Legalización
            CreateMap<cargar_documentos_constructora, cargar_documentos_constructora_entity>().ReverseMap();
            CreateMap<revisar_documentos_inmueble, revisar_documentos_inmueble_entity>().ReverseMap();
            CreateMap<cargar_documentos_cliente, cargar_documentos_cliente_entity>().ReverseMap();
            CreateMap<cargar_soportes_pago, cargar_soportes_pago_entity>().ReverseMap();
            CreateMap<validar_integracion_documentos, validar_integracion_documentos_entity>().ReverseMap();
            CreateMap<interviniente_bbva, interviniente_bbva_entity>().ReverseMap();
            CreateMap<devolucion_vb_comercial, devolucion_vb_comercial_entity>().ReverseMap();
            CreateMap<firmar_escritura_cliente_bbva, firmar_escritura_cliente_entity>().ReverseMap();
        }
    }
}
