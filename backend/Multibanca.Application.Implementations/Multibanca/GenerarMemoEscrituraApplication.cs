using AutoMapper;
using BoldReports.Web;
using BoldReports.Writer;
using Common.Application.Implementations;
using Data.Repository.Implementations;
using Data.Repository.Interfaces.Entities.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Data.Repository.Interfaces.Repositories.Multibanca.DatosOperacion;
using Framework.WorkFlow.Common.DTO;
using Microsoft.Extensions.Configuration;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Common;
using Multibanca.Application.Interfaces.Multibanca.CargaOperacionBanco;
using Multibanca.DTO.Common;
using Multibanca.Application.Interfaces.Multibanca.DatosOperacion;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;
using Multibanca.Domain.Models.Multibanca.DatosOperacion;
using Multibanca.DTO.Multibanca;
using System.Globalization;

namespace Multibanca.Application.Implementations.Multibanca
{
    public class GenerarMemoEscrituraApplication
        : MultibancaGenericApplication<generar_memo_escritura, generar_memo_escritura_entity, IGenerarMemoEscrituraRepository>,
          IGenerarMemoEscrituraApplication
    {
        private readonly IGenerarMemoEscrituraRepository RepositoryProvider;
        private readonly IExpedienteDigitalApplication ExpedienteDigitalApp;
        private readonly IWorkflowApplication WorkflowApp;
        private readonly IConfiguration Configuration;
        private readonly IMapper Mapper;

        private readonly IDatosOperacionCompradorApplication CompradorApp;
        private readonly IDatosOperacionVendedorApplication VendedorApp;
        private readonly IDatosOperacionDatosCreditoApplication DatosCreditoApp;
        private readonly IDatosOperacionPropiedadApplication PropiedadApp;
        private readonly ICargaOperacionBancoAntecedenteCreditoApplication CargaCreditoApp;
        private readonly ICargaOperacionBancoDatosOperacionApplication CargaOperacionApp;
        private readonly ITasacionDetalleApplication TasacionDetalleApp;
        private readonly IValorUfRepository ValorUfRepo;
        private readonly ICommonApplication CommonApp;

        public GenerarMemoEscrituraApplication(
            MultibancaDBContext _multibancaDBContext,
            IGenerarMemoEscrituraRepository _repository,
            IExpedienteDigitalApplication _expedienteDigitalApp,
            IWorkflowApplication _workflowApp,
            IConfiguration _configuration,
            IMapper _mapper,
            IDatosOperacionCompradorApplication _compradorApp,
            IDatosOperacionVendedorApplication _vendedorApp,
            IDatosOperacionDatosCreditoApplication _datosCreditoApp,
            IDatosOperacionPropiedadApplication _propiedadApp,
            ICargaOperacionBancoAntecedenteCreditoApplication _cargaCreditoApp,
            ICargaOperacionBancoDatosOperacionApplication _cargaOperacionApp,
            ITasacionDetalleApplication _tasacionDetalleApp,
            IValorUfRepository _valorUfRepo,
            ICommonApplication _commonApp)
            : base(_multibancaDBContext, _repository, _mapper)
        {
            RepositoryProvider = _repository;
            ExpedienteDigitalApp = _expedienteDigitalApp;
            WorkflowApp = _workflowApp;
            Configuration = _configuration;
            Mapper = _mapper;
            CompradorApp = _compradorApp;
            VendedorApp = _vendedorApp;
            DatosCreditoApp = _datosCreditoApp;
            PropiedadApp = _propiedadApp;
            CargaCreditoApp = _cargaCreditoApp;
            CargaOperacionApp = _cargaOperacionApp;
            TasacionDetalleApp = _tasacionDetalleApp;
            ValorUfRepo = _valorUfRepo;
            CommonApp = _commonApp;
        }

        public async Task<generar_memo_escritura?> GetByExpediente(long id_expediente)
        {
            var entity = await RepositoryProvider.GetByExpediente(id_expediente);
            if (entity == null) return null;
            return Mapper.Map<generar_memo_escritura>(entity);
        }

        public Task<MemoEscrituraDataDTO> GetDatosMemo(long id_expediente)
            => BuildMemoData(id_expediente);

        public async Task<expediente_digital> GenerarPdf(MemoEscrituraDTO request, int id_usuario)
        {
            int idDocumento = Configuration.GetValue<int>("MemoEscritura:IdDocumento", Constants.Documentos.MemorandumEscrituracionDefault);
            string reportFile = Configuration["MemoEscritura:ReportFileName"] ?? "MemoEscritura.rdl";

            int proximaVersion = CalcularProximaVersion(request.id_expediente, idDocumento);
            string nombreArchivo = $"{Constants.Documentos.MemorandumEscrituracionDescripcion}_v{proximaVersion}.pdf";

            MemoEscrituraDataDTO data = await BuildMemoData(request.id_expediente);
            if (!string.IsNullOrWhiteSpace(request.observaciones))
            {
                data.otros_antecedentes ??= new OtrosAntecedentesMemoDTO();
                data.otros_antecedentes.observaciones = request.observaciones;
            }

            byte[] pdfBytes = RenderRdlToPdf(reportFile, data, proximaVersion);
            ValidarPdf(pdfBytes);

            string server = Configuration["Server_UploadDownload:Server"] ?? "Local";
            using (var uploadStream = new MemoryStream(pdfBytes))
            {
                if (server == "AWS")
                    await ExpedienteDigitalApp.UploadToAWS(request.id_expediente, nombreArchivo, uploadStream);
                else
                    await ExpedienteDigitalApp.UploadToLocal(request.id_expediente, nombreArchivo, uploadStream);
            }

            expediente_digital expedienteDigital = new expediente_digital
            {
                id_expediente = request.id_expediente,
                id_documento = idDocumento,
                id_usuario = id_usuario,
                nombre_archivo = nombreArchivo,
                nombre_archivo_original = nombreArchivo,
                extension = ".pdf",
                version_archivo = proximaVersion,
                fecha_alta = DateTime.Now,
                comentarios = string.IsNullOrWhiteSpace(request.observaciones)
                    ? $"Memo generado por sistema (v{proximaVersion})"
                    : request.observaciones,
                is_active = true,
                row_status = true
            };

            return ExpedienteDigitalApp.Create(expedienteDigital, id_usuario);
        }

        public Task<List<expediente_digital>> ListarVersiones(long id_expediente)
        {
            int idDocumento = Configuration.GetValue<int>("MemoEscritura:IdDocumento", Constants.Documentos.MemorandumEscrituracionDefault);

            var versiones = ExpedienteDigitalApp.All()
                .Where(a => a.id_expediente == id_expediente
                         && a.id_documento == idDocumento
                         && a.is_active
                         && a.row_status)
                .OrderByDescending(a => a.version_archivo)
                .ThenByDescending(a => a.fecha_alta)
                .ToList();

            return Task.FromResult(versiones);
        }

        public async Task<List<AssignActivityDTO>> Avanzar(long id_expediente, int id_usuario, string id_actividad)
        {
            var entity = await RepositoryProvider.GetByExpediente(id_expediente);
            if (entity == null)
            {
                throw new InvalidOperationException(
                    $"No existe registro de Generar Memo Escritura para el expediente {id_expediente}. Debe guardar la actividad antes de avanzar.");
            }

            FolioDTO folio = await WorkflowApp.CapturarDatosFolio(id_expediente, id_actividad);
            List<xpdl_transition_DTO> listTransitions = await WorkflowApp.GetTransitions(id_actividad);

            string transitionsID;
            if (entity.enviar_reparo)
            {
                transitionsID = listTransitions
                    .Where(x => x.name == "GenerarMemoEscritura_CorregirReparoMemoEscritura_ReparoSI")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }
            else
            {
                transitionsID = listTransitions
                    .Where(x => x.name == "GenerarMemoEscritura_VisarOperacion_ReparoNO")
                    .Select(x => x.transition_id)
                    .FirstOrDefault() ?? string.Empty;
            }

            return await WorkflowApp.AvanzarActividad(transitionsID, folio, id_usuario);
        }

        private async Task<MemoEscrituraDataDTO> BuildMemoData(long id_expediente)
        {
            List<datos_operacion_comprador> compradores = await CompradorApp.GetByExpediente(id_expediente) ?? new();
            List<datos_operacion_vendedor>  vendedores  = await VendedorApp.GetByExpediente(id_expediente) ?? new();
            datos_operacion_datos_credito   datosCredito   = await DatosCreditoApp.GetByExpediente(id_expediente);
            List<datos_operacion_propiedad> propiedades    = await PropiedadApp.GetAllByExpediente(id_expediente) ?? new();
            carga_operacion_banco_antecedente_credito cargaCredito   = await CargaCreditoApp.GetByExpediente(id_expediente);
            carga_operacion_banco_datos_operacion     cargaOperacion = await CargaOperacionApp.GetByExpediente(id_expediente);
            List<tasacion_detalle> tasaciones = await TasacionDetalleApp.GetByExpediente(id_expediente) ?? new();
            decimal valorUFHoy = await GetValorUFHoy();

            List<ControlBaseDTO> catRelacion  = await CommonApp.GetCatalogoByType(Constants.Catalogo.RelacionTitular) ?? new();
            List<ControlBaseDTO> catEstCivil  = await CommonApp.GetCatalogoByType(Constants.Catalogo.DatosOperacionEstadoCivil) ?? new();
            List<ControlBaseDTO> catTipoProp  = await CommonApp.GetCatalogoByType(Constants.Catalogo.DatosOperacionTipoPropiedad) ?? new();
            List<ControlBaseDTO> catEstProp   = await CommonApp.GetCatalogoByType(Constants.Catalogo.DatosOperacionEstadoPropiedad) ?? new();

            string codigoTitular = CodigoCatalogo(catRelacion, "Titular");

            return new MemoEscrituraDataDTO
            {
                id_expediente = id_expediente,
                encabezado = BuildEncabezado(cargaCredito, cargaOperacion, valorUFHoy, id_expediente),
                antecedentes_personales = BuildAntecedentesPersonales(compradores, vendedores, codigoTitular, catEstCivil),
                antecedentes_prestamo = BuildAntecedentesPrestamo(cargaCredito, tasaciones, valorUFHoy),
                deuda_compradores = BuildDeudaCompradores(),
                antecedentes_credito = BuildAntecedentesCredito(cargaCredito, valorUFHoy),
                seguros = BuildSeguros(tasaciones),
                impuesto_al_mutuo = datosCredito?.impuesto_a_pagar ?? 0m,
                gastos_operacionales = BuildGastosOperacionales(),
                dividendos = BuildDividendos(cargaCredito),
                medio_pago_pac = BuildMedioPago(cargaCredito),
                antecedentes_propiedad = BuildPropiedades(propiedades, catTipoProp, catEstProp),
                otros_antecedentes = BuildOtrosAntecedentes(compradores, datosCredito, codigoTitular),
                resolucion = BuildResolucion(cargaCredito)
            };
        }

        private async Task<decimal> GetValorUFHoy()
        {
            var uf = await ValorUfRepo.GetByFecha(DateTime.Today)
                  ?? await ValorUfRepo.GetMasReciente();
            return uf?.valor ?? 0m;
        }

        private static EncabezadoMemoDTO BuildEncabezado(
            carga_operacion_banco_antecedente_credito credito,
            carga_operacion_banco_datos_operacion operacion,
            decimal valorUFHoy,
            long id_expediente)
        {
            decimal monto = credito?.monto_solicitado ?? 0m;
            decimal tasa = credito?.tasa ?? 0m;
            int plazo = credito?.plazo ?? 0;
            DateTime hoy = DateTime.Now;

            decimal valorUFCalculo = credito?.factor_conversion_uf > 0
                ? credito.factor_conversion_uf ?? valorUFHoy
                : valorUFHoy;

            DateTime fechaUFCalculo = credito?.fecha_inicio != null && credito.fecha_inicio > DateTime.MinValue
                ? credito.fecha_inicio.Value
                : hoy;

            return new EncabezadoMemoDTO
            {
                nro_solicitud = id_expediente.ToString(),
                nro_mutuo = operacion?.nro_mutuo?.ToString() ?? string.Empty,
                mes_calculo = hoy.ToString("MMMM 'del' yyyy", CultureInfo.GetCultureInfo("es-CL")),
                monto_prestamo_uf = monto,
                tasa_porcentaje = tasa,
                plazo_anios = plazo,
                costo_total_credito_uf = 0m,
                cae_porcentaje = 0m,
                valor_uf_hoy = valorUFHoy,
                fecha_uf_hoy = hoy,
                valor_uf_calculo = valorUFCalculo,
                fecha_uf_calculo = fechaUFCalculo,
                oficina_origen = operacion?.propietario ?? string.Empty,
                credito_en_uf = string.Equals(credito?.moneda, "UF", StringComparison.OrdinalIgnoreCase),
                fecha_escritura_texto = "____ de ____ del ____"
            };
        }

        private static List<AntecedentePersonalMemoDTO> BuildAntecedentesPersonales(
            List<datos_operacion_comprador> compradores,
            List<datos_operacion_vendedor> vendedores,
            string codigoTitular,
            List<ControlBaseDTO> catEstadoCivil)
        {
            var rows = new List<AntecedentePersonalMemoDTO>();

            rows.AddRange(compradores
                .Where(c => MismoCodigo(c.relacion_titular, codigoTitular))
                .Select(c => new AntecedentePersonalMemoDTO
                {
                    relacion = "Comprador",
                    rut = c.rut ?? string.Empty,
                    nombre_razon_social = NombreCompleto(c.razon_social, c.nombres, c.apellido_paterno, c.apellido_materno),
                    estado_civil = DescripcionCatalogo(c.estado_civil, catEstadoCivil),
                    regimen_bienes = string.Empty
                }));

            rows.AddRange(vendedores
                .Where(v => MismoCodigo(v.relacion_titular, codigoTitular))
                .Select(v => new AntecedentePersonalMemoDTO
                {
                    relacion = "Vendedor",
                    rut = v.rut ?? string.Empty,
                    nombre_razon_social = NombreCompleto(v.razon_social, v.nombres, v.apellido_paterno, v.apellido_materno),
                    estado_civil = DescripcionCatalogo(v.estado_civil, catEstadoCivil),
                    regimen_bienes = string.Empty
                }));

            return rows;
        }

        private static AntecedentesPrestamoMemoDTO BuildAntecedentesPrestamo(
            carga_operacion_banco_antecedente_credito credito,
            List<tasacion_detalle> tasaciones,
            decimal valorUFHoy)
        {
            decimal monto = credito?.monto_solicitado ?? 0m;
            decimal precioVentaClp = credito?.precio_venta_pesos ?? 0m;
            decimal precioVentaUf = valorUFHoy > 0 ? precioVentaClp / valorUFHoy : 0m;

            tasacion_detalle tasacion = tasaciones.FirstOrDefault();
            decimal valorTasacionUf = tasacion?.valor_tasacion_uf ?? 0m;
            decimal valorTasacionClp = tasacion?.valor_tasacion_pesos ?? 0m;
            decimal valorAsegurableUf = tasacion?.monto_asegurable_uf ?? 0m;
            decimal valorAsegurableClp = tasacion?.monto_asegurable_pesos ?? 0m;
            DateTime fechaTasacion = tasacion?.fecha_informe_tasacion ?? DateTime.MinValue;

            decimal valorUfTasacion = valorTasacionUf > 0 && valorTasacionClp > 0
                ? Math.Round(valorTasacionClp / valorTasacionUf, 2)
                : valorUFHoy;

            return new AntecedentesPrestamoMemoDTO
            {
                tipo_prestamo = credito?.tipo_prestamo ?? string.Empty,
                tipo_prestamo_subproducto = credito?.codigo_producto_cartera ?? string.Empty,
                destino_prestamo = credito?.destino_credito ?? string.Empty,
                precio_venta_uf = precioVentaUf,
                precio_venta_clp = precioVentaClp,
                valor_tasacion_uf = valorTasacionUf,
                valor_tasacion_clp = valorTasacionClp,
                valor_asegurable_uf = valorAsegurableUf,
                valor_asegurable_clp = valorAsegurableClp,
                fecha_tasacion = fechaTasacion,
                valor_uf_tasacion = valorUfTasacion,
                prestamo_maximo_clp = credito?.prestamo_maximo ?? 0m,
                monto_solicitado_uf = monto,
                monto_solicitado_clp = monto * valorUFHoy,
                dividendo_pagar_uf = 0m,
                dividendo_pagar_clp = 0m,
                renta_liquida_ajustada_clp = 0m,
                tipo_comision_tasa = $"{credito?.tipo_tasa}, {credito?.variabilidad_tasa}".TrimStart(',', ' '),
                meses_sabaticos = (credito?.meses_sabaticos ?? 0) == 0 ? "No Hay" : $"{credito.meses_sabaticos} meses",
                periodo_gracia = $"{credito?.periodo_gracia ?? 0} meses"
            };
        }

        private static DeudaCompradoresMemoDTO BuildDeudaCompradores()
        {
            return new DeudaCompradoresMemoDTO
            {
                monto_total_pension_uf = 0m,
                monto_total_pension_clp = 0m,
                total_cuotas_impagas = 0,
                valor_cuota_pension_uf = 0m,
                valor_cuota_pension_clp = 0m
            };
        }

        private static AntecedentesCreditoMemoDTO BuildAntecedentesCredito(
            carga_operacion_banco_antecedente_credito credito,
            decimal valorUFHoy)
        {
            decimal monto = credito?.monto_solicitado ?? 0m;
            decimal precioVentaClp = credito?.precio_venta_pesos ?? 0m;
            decimal precioVentaUf = valorUFHoy > 0 ? precioVentaClp / valorUFHoy : 0m;
            decimal cuotaContadoUf = Math.Max(precioVentaUf - monto, 0m);
            decimal cuotaContadoClp = Math.Max(precioVentaClp - (monto * valorUFHoy), 0m);

            return new AntecedentesCreditoMemoDTO
            {
                serie_codigo_bursatil_serie = "Sin Serie",
                serie_codigo_bursatil_codigo = "Sin Código",
                monto_credito_uf = monto,
                monto_credito_clp = monto * valorUFHoy,
                monto_cuota_contado_uf = cuotaContadoUf,
                monto_cuota_contado_clp = cuotaContadoClp,
                precio_venta_uf = precioVentaUf,
                precio_venta_clp = precioVentaClp,
                plazo_anios = credito?.plazo ?? 0,
                tasa_preferencial = credito?.tasa_primer_periodo ?? credito?.tasa ?? 0m,
                tasa_estandar = credito?.tasa_segundo_periodo ?? credito?.tasa ?? 0m
            };
        }

        private static List<SeguroMemoDTO> BuildSeguros(List<tasacion_detalle> tasaciones)
        {
            tasacion_detalle tasacion = tasaciones.FirstOrDefault();
            decimal montoIncendio = tasacion?.monto_asegurable_uf ?? 0m;

            return new List<SeguroMemoDTO>
            {
                new SeguroMemoDTO { descripcion = "INCENDIO SISMO AL EDIFICIO NO SUBSIDIADO", monto_uf = montoIncendio },
                new SeguroMemoDTO { descripcion = "SEGURO DESGRAVAMEN HIPOTECARIO",           monto_uf = 0m }
            };
        }

        private static GastosOperacionalesMemoDTO BuildGastosOperacionales()
        {
            return new GastosOperacionalesMemoDTO
            {
                conservador_bienes_raices = 0m,
                escrituracion = 0m,
                estudio_titulos = 0m,
                gastos_notariales = 0m,
                servicio_inscripcion_cbr = 0m,
                tasacion = 0m,
                total_gastos_operacionales = 0m
            };
        }

        private static DividendosMemoDTO BuildDividendos(carga_operacion_banco_antecedente_credito credito)
        {
            int plazoMeses = (credito?.plazo ?? 0) * 12;
            return new DividendosMemoDTO
            {
                rangos = plazoMeses > 0
                    ? new List<RangoDividendoMemoDTO>
                    {
                        new RangoDividendoMemoDTO { del = 1, al = plazoMeses, uf = 0m }
                    }
                    : new List<RangoDividendoMemoDTO>()
            };
        }

        private static MedioPagoPACMemoDTO BuildMedioPago(carga_operacion_banco_antecedente_credito credito)
        {
            return new MedioPagoPACMemoDTO
            {
                numero_pac = credito?.numero_cuenta_gastos?.ToString() ?? string.Empty,
                tipo_medio_pago = credito?.indicador_pac ?? string.Empty
            };
        }

        private static List<AntecedentePropiedadMemoDTO> BuildPropiedades(
            List<datos_operacion_propiedad> propiedades,
            List<ControlBaseDTO> catTipoPropiedad,
            List<ControlBaseDTO> catEstadoPropiedad)
        {
            return propiedades.Select(p =>
            {
                string rol = string.Join("-", new[] { p.rol_avaluo_1, p.rol_avaluo_2 }
                    .Where(s => !string.IsNullOrWhiteSpace(s)));
                return new AntecedentePropiedadMemoDTO
                {
                    tipo = DescripcionCatalogo(p.tipo_propiedad, catTipoPropiedad),
                    direccion = ComponerDireccion(p),
                    rol_avaluo = rol,
                    estado = DescripcionCatalogo(p.estado, catEstadoPropiedad)
                };
            }).ToList();
        }

        private static OtrosAntecedentesMemoDTO BuildOtrosAntecedentes(
            List<datos_operacion_comprador> compradores,
            datos_operacion_datos_credito? datosCredito,
            string codigoTitular)
        {
            string titular = compradores
                .Where(c => MismoCodigo(c.relacion_titular, codigoTitular))
                .Select(c => NombreCompleto(c.razon_social, c.nombres, c.apellido_paterno, c.apellido_materno))
                .FirstOrDefault() ?? string.Empty;

            return new OtrosAntecedentesMemoDTO
            {
                beneficio_tributario = (datosCredito?.dfl2 ?? false) ? "Ben. Tributario DFL2" : string.Empty,
                beneficiario = titular,
                destino_fondos = string.Empty,
                hipoteca_mandato = "[No Posee Carta de Resguardo o Alzamiento]"
            };
        }

        private static ResolucionMemoDTO BuildResolucion(carga_operacion_banco_antecedente_credito credito)
        {
            decimal monto = credito?.monto_solicitado ?? 0m;
            decimal tasa = credito?.tasa ?? 0m;
            int plazo = credito?.plazo ?? 0;
            var cl = CultureInfo.GetCultureInfo("es-CL");
            string texto = $"Por {monto.ToString("N4", cl)}.- UF a la tasa del {tasa.ToString("N2", cl)}% " +
                           $"correspondiente al primer período y a un plazo de {plazo} años, siempre que el informe " +
                           $"de estudio de antecedentes técnicos y legales sean favorables";
            return new ResolucionMemoDTO { texto_aprobado = texto };
        }

        // -------------------------------------------------------------------------
        // PDF rendering
        // -------------------------------------------------------------------------

        private int CalcularProximaVersion(long id_expediente, int id_documento)
        {
            int max = ExpedienteDigitalApp.All()
                .Where(a => a.id_expediente == id_expediente
                         && a.id_documento == id_documento
                         && a.is_active
                         && a.row_status)
                .Select(a => (int?)a.version_archivo)
                .DefaultIfEmpty(0)
                .Max() ?? 0;
            return max + 1;
        }

        private byte[] RenderRdlToPdf(string reportFileName, MemoEscrituraDataDTO data, int version)
        {
            string reportsFolder = Configuration["ReportSettings:ReportsPath"] ?? "Reports";
            string reportsPath = Path.Combine(Directory.GetCurrentDirectory(), reportsFolder, reportFileName);
            if (!File.Exists(reportsPath))
                throw new FileNotFoundException($"No se encontró la plantilla RDL del memo: {reportFileName}", reportsPath);

            using FileStream rdlStream = new FileStream(reportsPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using MemoryStream output = new MemoryStream();

            ReportWriter writer = new ReportWriter();
            try { writer.LoadReport(rdlStream); }
            catch (Exception ex) { throw new InvalidOperationException($"Fallo al cargar el RDL '{reportFileName}': {ex.Message}", ex); }

            foreach (var ds in BuildDataSources(data))
                writer.DataSources.Add(ds);

            writer.SetParameters(new List<ReportParameter>
            {
                new ReportParameter { Name = "VersionMemo",     Values = new List<string> { version.ToString() } },
                new ReportParameter { Name = "FechaGeneracion", Values = new List<string> { DateTime.Now.ToString("dd/MM/yyyy HH:mm") } },
                new ReportParameter { Name = "ImpuestoAlMutuo", Values = new List<string> { data.impuesto_al_mutuo.ToString(CultureInfo.InvariantCulture) } }
            });

            try { writer.Save(output, WriterFormat.PDF); }
            catch (Exception ex) { throw new InvalidOperationException($"BoldReports falló al exportar el RDL a PDF: {ex.Message}", ex); }

            return output.ToArray();
        }

        private static IEnumerable<ReportDataSource> BuildDataSources(MemoEscrituraDataDTO d)
        {
            yield return Ds("Encabezado",             new[] { d.encabezado ?? new EncabezadoMemoDTO() });
            yield return Ds("AntecedentesPersonales", d.antecedentes_personales ?? new List<AntecedentePersonalMemoDTO>());
            yield return Ds("AntecedentesPrestamo",   new[] { d.antecedentes_prestamo ?? new AntecedentesPrestamoMemoDTO() });
            yield return Ds("DeudaCompradores",       new[] { d.deuda_compradores ?? new DeudaCompradoresMemoDTO() });
            yield return Ds("AntecedentesCredito",    new[] { d.antecedentes_credito ?? new AntecedentesCreditoMemoDTO() });
            yield return Ds("Seguros",                d.seguros ?? new List<SeguroMemoDTO>());
            yield return Ds("GastosOperacionales",    new[] { d.gastos_operacionales ?? new GastosOperacionalesMemoDTO() });
            yield return Ds("RangosDividendo",        d.dividendos?.rangos ?? new List<RangoDividendoMemoDTO>());
            yield return Ds("MedioPago",              new[] { d.medio_pago_pac ?? new MedioPagoPACMemoDTO() });
            yield return Ds("Propiedades",            d.antecedentes_propiedad ?? new List<AntecedentePropiedadMemoDTO>());
            yield return Ds("OtrosAntecedentes",      new[] { d.otros_antecedentes ?? new OtrosAntecedentesMemoDTO() });
            yield return Ds("Resolucion",             new[] { d.resolucion ?? new ResolucionMemoDTO() });
        }

        private static ReportDataSource Ds(string name, System.Collections.IEnumerable value)
            => new ReportDataSource { Name = name, Value = value };

        private static void ValidarPdf(byte[] bytes)
        {
            if (bytes == null || bytes.Length < 4)
                throw new InvalidOperationException("BoldReports devolvió un stream vacío al renderizar el memo.");
            if (bytes[0] != 0x25 || bytes[1] != 0x50 || bytes[2] != 0x44 || bytes[3] != 0x46)
            {
                string preview = System.Text.Encoding.UTF8.GetString(bytes, 0, Math.Min(bytes.Length, 256));
                throw new InvalidOperationException(
                    "El motor de reportes no devolvió un PDF válido. Revisar plantilla RDL. " +
                    $"Inicio del stream: {preview}");
            }
        }

        // -------------------------------------------------------------------------
        // Helpers
        // -------------------------------------------------------------------------

        private static string NombreCompleto(string razonSocial, string nombres, string apPaterno, string apMaterno)
        {
            if (!string.IsNullOrWhiteSpace(razonSocial)) return razonSocial.Trim();
            return string.Join(" ", new[] { apPaterno, apMaterno, nombres }
                .Where(s => !string.IsNullOrWhiteSpace(s))).Trim();
        }

        // Normaliza "01" y "001" al mismo entero para comparar sin importar el padding
        private static bool MismoCodigo(string? a, string? b)
        {
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) return false;
            if (int.TryParse(a, out int ia) && int.TryParse(b, out int ib)) return ia == ib;
            return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        }

        // Devuelve la descripción del catálogo que corresponde al código almacenado
        private static string DescripcionCatalogo(string? codigo, List<ControlBaseDTO> catalogo)
        {
            if (string.IsNullOrEmpty(codigo)) return string.Empty;
            var match = catalogo.FirstOrDefault(c => MismoCodigo(c.code, codigo));
            return match?.description ?? codigo;
        }

        // Busca el code del catálogo cuya descripción coincide con el texto dado
        private static string CodigoCatalogo(List<ControlBaseDTO> catalogo, string descripcion)
        {
            var match = catalogo.FirstOrDefault(c =>
                string.Equals(c.description, descripcion, StringComparison.OrdinalIgnoreCase));
            return match?.code ?? string.Empty;
        }

        private static string ComponerDireccion(datos_operacion_propiedad p)
        {
            var partes = new List<string>();
            if (!string.IsNullOrWhiteSpace(p.direccion)) partes.Add(p.direccion);
            if (!string.IsNullOrWhiteSpace(p.numero)) partes.Add(p.numero);
            if (!string.IsNullOrWhiteSpace(p.comuna)) partes.Add($"de la comuna de {p.comuna}");
            if (!string.IsNullOrWhiteSpace(p.region)) partes.Add($"Región {p.region}");
            return string.Join(", ", partes);
        }
    }
}
