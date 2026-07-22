using Data.Repository.Implementations.EntityConfig.FuncTransversal;
using Data.Repository.Implementations.EntityConfig.Multibanca;
using Data.Repository.Implementations.EntityConfig.Multibanca.BBVA;
using Data.Repository.Implementations.EntityConfig.Multibanca.CargaOperacionBanco;
using Data.Repository.Implementations.EntityConfig.Multibanca.DatosOperacion;
using Data.Repository.Implementations.EntityConfig.Multibanca.GenerarBorradorEscritura;
using Data.Repository.Implementations.EntityConfig.Multibanca.RevisarDatosOperacion;
using Data.Repository.Implementations.EntityConfig.Multibanca.ValidacionRectificatoriaLegal;
using Data.Repository.Implementations.EntityConfig.Multibanca.ValidacionRectificatoriaLegalPostventa;
using Data.Repository.Implementations.EntityConfig.Security;
using Data.Repository.Interfaces;
using Data.Repository.Interfaces.Entities.FuncTransversal;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco;
using Data.Repository.Interfaces.Entities.Multibanca.DatosOperacion;
using Data.Repository.Interfaces.Entities.Multibanca.GenerarBorradorEscritura;
using Data.Repository.Interfaces.Entities.Multibanca.RevisarDatosOperacion;
using Data.Repository.Interfaces.Entities.Multibanca.ValidacionRectificatoriaLegal;
using Data.Repository.Interfaces.Entities.Multibanca.ValidacionRectificatoriaLegalPostventa;
using Data.Repository.Interfaces.Entities.Security;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.Domain.Models.Multibanca.BBVA;
using Microsoft.EntityFrameworkCore;
using Data.Repository.Interfaces.Entities.Multibanca.BBVA;
using Data.Repository.Interfaces.Entities.Multibanca.BBVA.ValidarIntegracionDocumentos;
using Data.Repository.Implementations.EntityConfig.Multibanca.BBVA.ValidarIntegracionDocumentos;

namespace Data.Repository.Implementations
{
    public class MultibancaDBContext : DbContext, IMultibancaDBContext
    {
        //Security
        public DbSet<users_entity> Users { get; set; }
        public DbSet<roles_entity> Roles { get; set; }
        public DbSet<menus_entity> Menus { get; set; }
        public DbSet<role_menu_entity> RoleMenus { get; set; }
        //FuncTransversal
        public DbSet<bitacora_entity> bitacora { get; set; }
        public DbSet<expediente_digital_entity> ExpedienteDigitalEntities { get; set; }
        //Multibanca
        public DbSet<reports_entity> reports { get; set; }
        public DbSet<actividades_entity> actividades { get; set; }
        public DbSet<carga_operacion_banco_entity> carga_operacion_banco { get; set; }
        public DbSet<carga_operacion_banco_datos_operacion_entity> carga_operacion_banco_datos_operacion { get; set; }
        public DbSet<carga_operacion_banco_antecedente_comprador_entity> carga_operacion_banco_antecedente_comprador { get; set; }
        public DbSet<carga_operacion_banco_antecedente_vendedor_entity> carga_operacion_banco_antecedente_vendedor { get; set; }
        public DbSet<carga_operacion_banco_antecedente_credito_entity> carga_operacion_banco_antecedente_credito { get; set; }
        public DbSet<carga_operacion_banco_datos_comercial_entity> carga_operacion_banco_datos_comercial { get; set; }
        public DbSet<recepcion_carga_fabrica_entity> recepcion_carga_fabrica { get; set; }
        public DbSet<corregir_reparo_fabrica_entity> corregir_reparo_fabrica { get; set; }
        public DbSet<corregir_reparo_tasacion_entity> corregir_reparo_tasacion { get; set; }
        public DbSet<datos_operacion_entity> datos_operacion { get; set; }
        public DbSet<datos_operacion_datos_credito_entity> datos_operacion_datos_credito { get; set; }
        public DbSet<datos_operacion_comprador_entity> datos_operacion_comprador { get; set; }
        public DbSet<datos_operacion_vendedor_entity> datos_operacion_vendedor { get; set; }
        public DbSet<datos_operacion_fiador_garante_entity> datos_operacion_fiador_garante { get; set; }
        public DbSet<datos_operacion_banco_acreedor_entity> datos_operacion_banco_acreedor { get; set; }
        public DbSet<datos_operacion_propiedad_entity> datos_operacion_propiedad { get; set; }
        public DbSet<revisar_datos_operacion_entity> revisar_datos_operacion { get; set; }
        public DbSet<revisar_datos_operacion_propiedad_entity> revisar_datos_operacion_propiedad { get; set; }
        public DbSet<revisar_datos_operacion_credito_entity> revisar_datos_operacion_credito { get; set; }
        public DbSet<revisar_datos_operacion_vendedor_entity> revisar_datos_operacion_vendedor { get; set; }
        public DbSet<revisar_datos_operacion_comprador_entity> revisar_datos_operacion_comprador { get; set; }
        public DbSet<asignar_escritura_entity> asignar_escritura { get; set; }
        public DbSet<asignar_estudio_titulos_entity> asignar_estudio_titulos { get; set; }
        public DbSet<generar_prefiniquito_entity> generar_prefiniquito { get; set; }
        public DbSet<corregir_reparo_prefiniquito_entity> corregir_reparo_prefiniquito { get; set; }
        public DbSet<corregir_reparo_calculo_doc_entity> corregir_reparo_calculo_doc { get; set; }
        public DbSet<revisar_ingreso_datos_credito_entity> revisar_ingreso_datos_credito { get; set; }
        public DbSet<corregir_reparo_datos_operacion_entity> corregir_reparo_datos_operacion { get; set; }
        public DbSet<corregir_reparo_cdr_entity> corregir_reparo_cdr { get; set; }
        public DbSet<corregir_reparo_copias_escrituras_entity> corregir_reparo_copias_escrituras { get; set; }
        public DbSet<visar_operacion_entity> visar_operacion { get; set; }
        public DbSet<aprobacion_comercial_legal_cdr_entity> aprobacion_comercial_legal_cdr { get; set; }

        public DbSet<corregir_reparo_generar_memo_escritura_entity> corregir_reparo_generar_memo_escritura { get; set; }
        public DbSet<generar_memo_escritura_entity> generar_memo_escritura { get; set; }
        public DbSet<calculo_generacion_documento_entity> calculo_generacion_documento { get; set; }
        public DbSet<valor_uf_entity> valor_uf { get; set; }
        public DbSet<generar_estudio_titulos_entity> generar_estudio_titulos { get; set; }
        public DbSet<corregir_reparo_visado_entity> corregir_reparo_visado { get; set; }
        public DbSet<corregir_carta_resguardo_entity> corregir_carta_resguardo { get; set; }
        public DbSet<corregir_reparo_estudio_titulos_entity> corregir_reparo_estudio_titulos { get; set; }
        public DbSet<reparo_estudio_titulos_detalle_entity> reparo_estudio_titulos_detalle { get; set; }
        public DbSet<generar_borrador_escritura_entity> generar_borrador_escritura { get; set; }
        public DbSet<generar_borrador_escritura_detalle_entity> generar_borrador_escritura_detalle { get; set; }

        public DbSet<tasacion_entity> tasacion { get; set; }
        public DbSet<tasacion_detalle_entity> tasacion_detalle { get; set; }
        public DbSet<verificar_reparo_estudio_titulo_entity> verificar_reparo_estudio_titulo { get; set; }
        public DbSet<verificar_reparo_datos_operacion_entity> verificar_reparo_datos_operacion { get; set; }
        //public DbSet<verificar_reparo_datos_operacion_detalle_entity> verificar_reparo_datos_operacion_detalle { get; set; }
        public DbSet<revisar_datos_operacion_fiador_garante_entity> revisar_datos_operacion_fiador_garante { get; set; }
        public DbSet<corregir_reparo_generar_borrador_escritura_entity> corregir_reparo_generar_borrador_escritura { get; set; }
        public DbSet<firma_comprador_entity> firma_comprador { get; set; }
        public DbSet<firma_comprador_detalle_entity> firma_comprador_detalle { get; set; }
        public DbSet<firma_vendedor_entity> firma_vendedor { get; set; }
        public DbSet<firma_vendedor_detalle_entity> firma_vendedor_detalle { get; set; }
        public DbSet<firma_banco_acreedor_cg_entity> firma_banco_acreedor_cg { get; set; }
        public DbSet<corregir_reparo_entregar_carpeta_entity> corregir_reparo_entregar_carpeta { get; set; }
        public DbSet<cierre_copias_notaria_entity> cierre_copias_notaria { get; set; }
        public DbSet<corregir_reparo_cierre_copias_notaria_entity> corregir_reparo_cierre_copias_notaria { get; set; }
        public DbSet<revisar_inscripcion_cbr_entity> revisar_inscripcion_cbr { get; set; }
        public DbSet<reingresar_escritura_cbr_entity> reingresar_escritura_cbr { get; set; }
        public DbSet<corregir_reparo_liquidacion_entity> corregir_reparo_liquidacion { get; set; }
        public DbSet<revisar_liquidacion_entity> revisar_liquidacion { get; set; }
        public DbSet<revisar_desembolso_entity> revisar_desembolso { get; set; }
        public DbSet<corregir_notaria_reparo_abogados_entity> corregir_notaria_reparo_abogados { get; set; }

        public DbSet<corregir_reparo_inst_pago_entity> corregir_reparo_inst_pago { get; set; }
        public DbSet<corregir_reparos_gestor_entity> corregir_reparos_gestor { get; set; }
        public DbSet<entregar_carpeta_entity> entregar_carpeta { get; set; }

        public DbSet<generar_carta_resguardo_entity> generar_carta_resguardo { get; set; }

        public DbSet<generar_recursos_pagos_cbr_entity> generar_recursos_pagos_cbr { get; set; }

        public DbSet<recepcionar_matriz_entity> recepcionar_matriz { get; set; }

        public DbSet<verificar_reparo_cbr_entity> verificar_reparo_cbr { get; set; }
        public DbSet<recibir_instruccion_pago_entity> recibir_instruccion_pago { get; set; }

        public DbSet<registrar_escritura_cbr_entity> registrar_escritura_cbr { get; set; }

        public DbSet<realizar_revision_previo_firma_banco_entity> realizar_revision_previo_firma_banco { get; set; }

        public DbSet<valorizar_cbr_entity> valorizar_cbr { get; set; }
        public DbSet<registrar_fecha_registro_cbr_entity> registrar_fecha_registro_cbr { get; set; }
        public DbSet<control_escritura_entity> control_escritura { get; set; }
        public DbSet<corregir_control_escritura_entity> corregir_control_escritura { get; set; }

        public DbSet<generar_finiquito_entity> generar_finiquito { get; set; }
        public DbSet<verificar_correccion_escritura_entity> verificar_correccion_escritura { get; set; }
        public DbSet<registrar_firma_apoderado_banco_entity> registrar_firma_apoderado_banco { get; set; }

        public DbSet<realizar_control_credito_entity> realizar_control_credito { get; set; }
        public DbSet<corregir_reparo_control_credito_entity> corregir_Reparo_Control_Credito { get; set; }
        public DbSet<gestion_rectificatoria_entity> gestion_rectificatoria_ { get; set; }
        public DbSet<validacion_rectificatoria_legal_entity> validacion_rectificatoria_legal { get; set; }
        public DbSet<validacion_rectificatoria_legal_datos_personales_entity> validacion_rectificatoria_legal_datos_personales { get; set; }
        public DbSet<gestion_rectificatoria_solucion_reparo_entity> gestion_rectificatoria_solucion_reparo { get; set; }
        public DbSet<gestion_reparo_entity> gestion_reparo { get; set; }

        public DbSet<reparo_formulario_entity> reparo_formulario { get; set; }
        public DbSet<rectificatoria_firma_entity> rectificatoria_firma { get; set; }
        public DbSet<rectificatoria_firma_detalle_entity> rectificatoria_firma_detalle { get; set; }
        public DbSet<rectificatoria_legal_carta_resguardo_entity> rectificatoria_legal_carta_resguardo { get; set; }
        public DbSet<rectificatoria_legal_firma_alzante_entity> rectificatoria_legal_firma_alzante { get; set; }
        public DbSet<rectificatoria_legal_cierre_copias_entity> rectificatoria_legal_cierre_copias { get; set; }
        public DbSet<rectificatoria_legal_cierre_copias_postventa_entity> rectificatoria_legal_cierre_copias_postventa { get; set; }
        public DbSet<gestion_rectificatoria_escritura_firmada_entity> gestion_rectificatoria_escritura_firmada { get; set; }
        public DbSet<rectificatoria_analisis_derivacion_reparo_postventa_entity> rectificatoria_analisis_derivacion_reparo_postventa { get; set; }
        public DbSet<rectificatoria_postventa_solucion_reparo_entity> rectificatoria_postventa_solucion_reparo { get; set; }
        public DbSet<validacion_rectificatoria_legal_postventa_entity> validacion_rectificatoria_legal_postventa { get; set; }
        public DbSet<validacion_rectificatoria_legal_postventa_datos_personales_entity> validacion_rectificatoria_legal_postventa_datos_personales { get; set; }
        public DbSet<rectificatoria_firma_post_venta_entity> rectificatoria_firma_post_venta { get; set; }
        public DbSet<rectificatoria_firma_post_venta_detalle_entity> rectificatoria_firma_post_venta_detalle { get; set; }
        public DbSet<gestion_rectificatoria_escritura_firmada_postventa_entity> gestion_rectificatoria_escritura_firmada_postventa { get; set; }
        public DbSet<revisar_copias_escrituras_entity> revisar_copias_escrituras { get; set; }
        public DbSet<idempotency_key_entity> idempotency_keys { get; set; }

        // BBVA Colombia — Presto Legalización
        public DbSet<validar_integracion_documentos_entity> validar_integracion_documentos_entity { get; set; }
        public DbSet<interviniente_bbva_entity> interviniente_bbva_entity { get; set; } // Relacionado a validar_integracion_documentos
        public DbSet<devolucion_vb_comercial_entity> devolucion_vb_comercial_entity { get; set; }
        public DbSet<validar_informacion_bbva> ValidarInformacionBbva { get; set; }
        public DbSet<definir_inmueble_bbva> DefinirInmuebleBbva { get; set; }
        public DbSet<asignar_firmas_peritos_abogados> AsignarFirmasBbva { get; set; }
        public DbSet<carta_aprobacion_bbva> CartaAprobacionBbva { get; set; }
        public DbSet<registro_contacto_bbva> RegistroContactoBbva { get; set; }
        public DbSet<titular_bbva> TitularBbva { get; set; }
        public DbSet<cargar_documentos_constructora_entity> CargarDocumentosConstructora { get; set; }
        public DbSet<revisar_documentos_inmueble_entity> RevisarDocumentosInmueble { get; set; }
        public DbSet<token_acceso_temporal> TokenAccesoTemporal { get; set; }
        public DbSet<cargar_documentos_cliente_entity> CargarDocumentosCliente { get; set; }
        public DbSet<cargar_soportes_pago_entity> CargarSoportesPago { get; set; }
        public DbSet<gestionar_firma_bbva> GestionarFirmaBbva { get; set; }
        public DbSet<gestionar_firma_fisica_bbva> GestionarFirmaFisicaBbva { get; set; }
        public DbSet<firmar_escritura_cliente_entity> firmar_escritura_cliente { get; set; }

        public MultibancaDBContext(DbContextOptions<MultibancaDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Security
            user_entity_config.SetEntityBuilder(modelBuilder.Entity<users_entity>());
            menus_entity_config.SetEntityBuilder(modelBuilder.Entity<menus_entity>());
            roles_entity_config.SetEntityBuilder(modelBuilder.Entity<roles_entity>());
            role_menu_entity_config.SetEntityBuilder(modelBuilder.Entity<role_menu_entity>());
            //FuncTransversal
            BitacoraEntityConfig.SetEntityBuilder(modelBuilder.Entity<bitacora_entity>());
            ExpedienteDigitalEntityConfig.SetEntityBuilder(modelBuilder.Entity<expediente_digital_entity>());
            //Multibanca
            reports_entity_config.SetEntityBuilder(modelBuilder.Entity<reports_entity>());
            ActividadesEntityConfig.SetEntityBuilder(modelBuilder.Entity<actividades_entity>());
            carga_operacion_banco_entity_config.SetEntityBuilder(modelBuilder.Entity<carga_operacion_banco_entity>());
            carga_operacion_banco_datos_operacion_entity_config.SetEntityBuilder(modelBuilder.Entity<carga_operacion_banco_datos_operacion_entity>());
            carga_operacion_banco_antecedente_comprador_entity_config.SetEntityBuilder(modelBuilder.Entity<carga_operacion_banco_antecedente_comprador_entity>());
            carga_operacion_banco_antecedente_vendedor_entity_config.SetEntityBuilder(modelBuilder.Entity<carga_operacion_banco_antecedente_vendedor_entity>());
            carga_operacion_banco_antecedente_credito_entity_config.SetEntityBuilder(modelBuilder.Entity<carga_operacion_banco_antecedente_credito_entity>());
            carga_operacion_banco_datos_comercial_entity_config.SetEntityBuilder(modelBuilder.Entity<carga_operacion_banco_datos_comercial_entity>());
            recepcion_carga_fabrica_entity_config.SetEntityBuilder(modelBuilder.Entity<recepcion_carga_fabrica_entity>());
            corregir_reparo_fabrica_entity_config.SetEntityBuilder(modelBuilder.Entity<corregir_reparo_fabrica_entity>());
            corregir_reparo_tasacion_entity_config.SetEntityBuilder(modelBuilder.Entity<corregir_reparo_tasacion_entity>());
            datos_operacion_entity_config.SetEntityBuilder(modelBuilder.Entity<datos_operacion_entity>());
            datos_operacion_datos_credito_entity_config.SetEntityBuilder(modelBuilder.Entity<datos_operacion_datos_credito_entity>());
            datos_operacion_comprador_entity_config.SetEntityBuilder(modelBuilder.Entity<datos_operacion_comprador_entity>());
            datos_operacion_vendedor_entity_config.SetEntityBuilder(modelBuilder.Entity<datos_operacion_vendedor_entity>());
            datos_operacion_fiador_garante_entity_config.SetEntityBuilder(modelBuilder.Entity<datos_operacion_fiador_garante_entity>());
            datos_operacion_banco_acreedor_entity_config.SetEntityBuilder(modelBuilder.Entity<datos_operacion_banco_acreedor_entity>());
            datos_operacion_propiedad_entity_config.SetEntityBuilder(modelBuilder.Entity<datos_operacion_propiedad_entity>());
            revisar_datos_operacion_entity_config.SetEntityBuilder(modelBuilder.Entity<revisar_datos_operacion_entity>());
            revisar_datos_operacion_propiedad_entity_config.SetEntityBuilder(modelBuilder.Entity<revisar_datos_operacion_propiedad_entity>());
            revisar_datos_operacion_credito_entity_config.SetEntityBuilder(modelBuilder.Entity<revisar_datos_operacion_credito_entity>());
            revisar_datos_operacion_vendedor_entity_config.SetEntityBuilder(modelBuilder.Entity<revisar_datos_operacion_vendedor_entity>());
            revisar_datos_operacion_comprador_entity_config.SetEntityBuilder(modelBuilder.Entity<revisar_datos_operacion_comprador_entity>());
            asignar_escritura_entity_config.SetEntityBuilder(modelBuilder.Entity<asignar_escritura_entity>());
            asignar_estudio_titulos_entity_config.SetEntityBuilder(modelBuilder.Entity<asignar_estudio_titulos_entity>());
            generar_prefiniquito_entity_config.SetEntityBuilder(modelBuilder.Entity<generar_prefiniquito_entity>());
            corregir_reparo_prefiniquito_entity_config.SetEntityBuilder(modelBuilder.Entity<corregir_reparo_prefiniquito_entity>());
            corregir_reparo_calculo_doc_entity_config.SetEntityBuilder(modelBuilder.Entity<corregir_reparo_calculo_doc_entity>());
            corregir_reparo_generar_memo_escritura_entity_config.SetEntityBuilder(modelBuilder.Entity<corregir_reparo_generar_memo_escritura_entity>());
            verificar_reparo_estudio_titulo_config.SetEntityBuilder(modelBuilder.Entity<verificar_reparo_estudio_titulo_entity>());
            verificar_reparo_datos_operacion_entity_config.SetEntityBuilder(modelBuilder.Entity<verificar_reparo_datos_operacion_entity>());
            corregir_reparo_datos_operacion_entity_config.SetEntityBuilder(modelBuilder.Entity<corregir_reparo_datos_operacion_entity>());
            corregir_reparo_cdr_entity_config.SetEntityBuilder(modelBuilder.Entity<corregir_reparo_cdr_entity>());
            corregir_reparo_copias_escrituras_entity_config.SetEntityBuilder(modelBuilder.Entity<corregir_reparo_copias_escrituras_entity>());
            CalculoGeneracionDocumentoEntityConfig.SetEntityBuilder(modelBuilder.Entity<calculo_generacion_documento_entity>());
            ValorUfEntityConfig.SetEntityBuilder(modelBuilder.Entity<valor_uf_entity>());
            corregir_reparo_estudio_titulos_entity_config.SetEntityBuilder(modelBuilder.Entity<corregir_reparo_estudio_titulos_entity>());
            reparo_estudio_titulos_detalle_entity_config.SetEntityBuilder(modelBuilder.Entity<reparo_estudio_titulos_detalle_entity>());
            tasacion_entity_config.SetEntityBuilder(modelBuilder.Entity<tasacion_entity>());
            tasacion_detalle_entity_config.SetEntityBuilder(modelBuilder.Entity<tasacion_detalle_entity>());
            generar_estudio_titulos_entity_config.SetEntityBuilder(modelBuilder.Entity<generar_estudio_titulos_entity>());
            corregir_reparo_visado_entity_config.SetEntityBuilder(modelBuilder.Entity<corregir_reparo_visado_entity>());
            corregir_carta_resguardo_entity_config.SetEntityBuilder(modelBuilder.Entity<corregir_carta_resguardo_entity>());
            corregir_reparo_datos_operacion_entity_config.SetEntityBuilder(modelBuilder.Entity<corregir_reparo_datos_operacion_entity>());
            revisar_datos_operacion_banco_entity_config.SetEntityBuilder(modelBuilder.Entity<revisar_datos_operacion_banco_entity>());
            revisar_datos_operacion_fiador_garante_entity_config.SetEntityBuilder(modelBuilder.Entity<revisar_datos_operacion_fiador_garante_entity>());
            corregir_reparo_generar_borrador_escritura_entity_config.SetEntityBuilder(modelBuilder.Entity<corregir_reparo_generar_borrador_escritura_entity>());
            generar_borrador_escritura_entity_config.SetEntityBuilder(modelBuilder.Entity<generar_borrador_escritura_entity>());
            generar_borrador_escritura_detalle_entity_config.SetEntityBuilder(modelBuilder.Entity<generar_borrador_escritura_detalle_entity>());
            generar_memo_escritura_entity_config.SetEntityBuilder(modelBuilder.Entity<generar_memo_escritura_entity>());
            visar_operacion_entity_config.SetEntityBuilder(modelBuilder.Entity<visar_operacion_entity>());
            firma_comprador_entity_config.SetEntityBuilder(modelBuilder.Entity<firma_comprador_entity>());
            firma_comprador_detalle_entity_config.SetEntityBuilder(modelBuilder.Entity<firma_comprador_detalle_entity>());
            firma_vendedor_entity_config.SetEntityBuilder(modelBuilder.Entity<firma_vendedor_entity>());
            firma_vendedor_detalle_entity_config.SetEntityBuilder(modelBuilder.Entity<firma_vendedor_detalle_entity>());
            firma_banco_acreedor_cg_entity_config.SetEntityBuilder(modelBuilder.Entity<firma_banco_acreedor_cg_entity>());
            corregir_reparo_entregar_carpeta_entity_config.SetEntityBuilder(modelBuilder.Entity<corregir_reparo_entregar_carpeta_entity>());
            cierre_copias_notaria_entity_config.SetEntityBuilder(modelBuilder.Entity<cierre_copias_notaria_entity>());
            corregir_reparo_cierre_copias_notaria_entity_config.SetEntityBuilder(modelBuilder.Entity<corregir_reparo_cierre_copias_notaria_entity>());
            aprobacion_comercial_legal_cdr_entity_config.SetEntityBuilder(modelBuilder.Entity<aprobacion_comercial_legal_cdr_entity>());
            revisar_inscripcion_cbr_entity_config.SetEntityBuilder(modelBuilder.Entity<revisar_inscripcion_cbr_entity>());
            reingresar_escritura_cbr_entity_config.SetEntityBuilder(modelBuilder.Entity<reingresar_escritura_cbr_entity>());
            corregir_reparo_liquidacion_entity_config.SetEntityBuilder(modelBuilder.Entity<corregir_reparo_liquidacion_entity>());
            revisar_liquidacion_entity_config.SetEntityBuilder(modelBuilder.Entity<revisar_liquidacion_entity>());
            revisar_desembolso_entity_config.SetEntityBuilder(modelBuilder.Entity<revisar_desembolso_entity>());
            corregir_notaria_reparo_abogados_entity_config.SetEntityBuilder(modelBuilder.Entity<corregir_notaria_reparo_abogados_entity>());
            corregir_reparo_inst_pago_entity_config.SetEntityBuilder(modelBuilder.Entity<corregir_reparo_inst_pago_entity>());
            corregir_reparos_gestor_entity_config.SetEntityBuilder(modelBuilder.Entity<corregir_reparos_gestor_entity>());
            entregar_carpeta_entity_config.SetEntityBuilder(modelBuilder.Entity<entregar_carpeta_entity>());
            generar_carta_resguardo_entity_config.SetEntityBuilder(modelBuilder.Entity<generar_carta_resguardo_entity>());
            generar_recursos_pagos_cbr_entity_config.SetEntityBuilder(modelBuilder.Entity<generar_recursos_pagos_cbr_entity>());
            recepcionar_matriz_entity_config.SetEntityBuilder(modelBuilder.Entity<recepcionar_matriz_entity>());
            verificar_reparo_cbr_entity_config.SetEntityBuilder(modelBuilder.Entity<verificar_reparo_cbr_entity>());
            recibir_instruccion_pago_entity_config.SetEntityBuilder(modelBuilder.Entity<recibir_instruccion_pago_entity>());
            registrar_escritura_cbr_entity_config.SetEntityBuilder(modelBuilder.Entity<registrar_escritura_cbr_entity>());
            realizar_revision_previo_firma_banco_entity_config.SetEntityBuilder(modelBuilder.Entity<realizar_revision_previo_firma_banco_entity>());
            valorizar_cbr_entity_config.SetEntityBuilder(modelBuilder.Entity<valorizar_cbr_entity>());
            registrar_fecha_registro_cbr_entity_config.SetEntityBuilder(modelBuilder.Entity<registrar_fecha_registro_cbr_entity>());
            generar_finiquito_entity_config.SetEntityBuilder(modelBuilder.Entity<generar_finiquito_entity>());
            verificar_correccion_escritura_entity_config.SetEntityBuilder(modelBuilder.Entity<verificar_correccion_escritura_entity>());
            registrar_firma_apoderado_banco_entity_config.SetEntityBuilder(modelBuilder.Entity<registrar_firma_apoderado_banco_entity>());
            realizar_control_credito_entity_config.SetEntityBuilder(modelBuilder.Entity<realizar_control_credito_entity>());
            corregir_reparo_control_credito_entity_config.SetEntityBuilder(modelBuilder.Entity<corregir_reparo_control_credito_entity>());
            control_escritura_entity_config.SetEntityBuilder(modelBuilder.Entity<control_escritura_entity>());
            corregir_control_escritura_entity_config.SetEntityBuilder(modelBuilder.Entity<corregir_control_escritura_entity>());
            gestion_rectificatoria_entity_config.SetEntityBuilder(modelBuilder.Entity<gestion_rectificatoria_entity>());
            validacion_rectificatoria_legal_entity_config.SetEntityBuilder(modelBuilder.Entity<validacion_rectificatoria_legal_entity>());
            validacion_rectificatoria_legal_datos_personales_entity_config.SetEntityBuilder(modelBuilder.Entity<validacion_rectificatoria_legal_datos_personales_entity>());
            gestion_rectificatoria_solucion_reparo_entity_config.SetEntityBuilder(modelBuilder.Entity<gestion_rectificatoria_solucion_reparo_entity>());
            gestion_reparo_entity_config.SetEntityBuilder(modelBuilder.Entity<gestion_reparo_entity>());
            reparo_formulario_entity_config.SetEntityBuilder(modelBuilder.Entity<reparo_formulario_entity>());
            rectificatoria_firma_entity_config.SetEntityBuilder(modelBuilder.Entity<rectificatoria_firma_entity>());
            rectificatoria_firma_detalle_entity_config.SetEntityBuilder(modelBuilder.Entity<rectificatoria_firma_detalle_entity>());
            rectificatoria_legal_carta_resguardo_entity_config.SetEntityBuilder(modelBuilder.Entity<rectificatoria_legal_carta_resguardo_entity>());
            rectificatoria_legal_firma_alzante_entity_config.SetEntityBuilder(modelBuilder.Entity<rectificatoria_legal_firma_alzante_entity>());
            rectificatoria_legal_cierre_copias_entity_config.SetEntityBuilder(modelBuilder.Entity<rectificatoria_legal_cierre_copias_entity>());
            rectificatoria_legal_cierre_copias_postventa_entity_config.SetEntityBuilder(modelBuilder.Entity<rectificatoria_legal_cierre_copias_postventa_entity>());
            gestion_rectificatoria_escritura_firmada_entity_config.SetEntityBuilder(modelBuilder.Entity<gestion_rectificatoria_escritura_firmada_entity>());
            rectificatoria_analisis_derivacion_reparo_postventa_entity_config.SetEntityBuilder(modelBuilder.Entity<rectificatoria_analisis_derivacion_reparo_postventa_entity>());
            rectificatoria_postventa_solucion_reparo_entity_config.SetEntityBuilder(modelBuilder.Entity<rectificatoria_postventa_solucion_reparo_entity>());
            validacion_rectificatoria_legal_postventa_entity_config.SetEntityBuilder(modelBuilder.Entity<validacion_rectificatoria_legal_postventa_entity>());
            validacion_rectificatoria_legal_postventa_datos_personales_entity_config.SetEntityBuilder(modelBuilder.Entity<validacion_rectificatoria_legal_postventa_datos_personales_entity>());
            rectificatoria_firma_post_venta_entity_config.SetEntityBuilder(modelBuilder.Entity<rectificatoria_firma_post_venta_entity>());
            rectificatoria_firma_post_venta_detalle_entity_config.SetEntityBuilder(modelBuilder.Entity<rectificatoria_firma_post_venta_detalle_entity>());
            revisar_copias_escrituras_entity_config.SetEntityBuilder(modelBuilder.Entity<revisar_copias_escrituras_entity>());
            IdempotencyKeyEntityConfig.SetEntityBuilder(modelBuilder.Entity<idempotency_key_entity>());

            // BBVA Colombia
            validar_integracion_documentos_entity_config.SetEntityBuilder(
                modelBuilder.Entity<validar_integracion_documentos_entity>());
            interviniente_bbva_entity_config.SetEntityBuilder(
                modelBuilder.Entity<interviniente_bbva_entity>());
            devolucion_vb_comercial_entity_config.SetEntityBuilder(
                modelBuilder.Entity<devolucion_vb_comercial_entity>());
            validar_informacion_bbva_entity_config.SetEntityBuilder(
                modelBuilder.Entity<validar_informacion_bbva>());
            definir_inmueble_bbva_entity_config.SetEntityBuilder(
                modelBuilder.Entity<definir_inmueble_bbva>());
            asignar_firmas_entity_config.SetEntityBuilder(
                modelBuilder.Entity<asignar_firmas_peritos_abogados>());
            carta_aprobacion_bbva_entity_config.SetEntityBuilder(
                modelBuilder.Entity<carta_aprobacion_bbva>());
            registro_contacto_bbva_entity_config.SetEntityBuilder(
                modelBuilder.Entity<registro_contacto_bbva>());
            titular_bbva_entity_config.SetEntityBuilder(
                modelBuilder.Entity<titular_bbva>());
            cargar_documentos_constructora_entity_config.SetEntityBuilder(
                modelBuilder.Entity<cargar_documentos_constructora_entity>());
            revisar_documentos_inmueble_entity_config.SetEntityBuilder(
                modelBuilder.Entity<revisar_documentos_inmueble_entity>());
            token_acceso_temporal_entity_config.SetEntityBuilder(
                modelBuilder.Entity<token_acceso_temporal>());
            cargar_documentos_cliente_entity_config.SetEntityBuilder(
                modelBuilder.Entity<cargar_documentos_cliente_entity>());
            cargar_soportes_pago_entity_config.SetEntityBuilder(
                modelBuilder.Entity<cargar_soportes_pago_entity>());
            gestionar_firma_bbva_entity_config.SetEntityBuilder(
                modelBuilder.Entity<gestionar_firma_bbva>());
            gestionar_firma_fisica_bbva_entity_config.SetEntityBuilder(
                modelBuilder.Entity<gestionar_firma_fisica_bbva>());
            firmar_escritura_cliente_entity_config.SetEntityBuilder(
                modelBuilder.Entity<firmar_escritura_cliente_entity>());

            base.OnModelCreating(modelBuilder);
            gestion_rectificatoria_escritura_firmada_postventa_entity_config.SetEntityBuilder(modelBuilder.Entity<gestion_rectificatoria_escritura_firmada_postventa_entity>());
        }
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
