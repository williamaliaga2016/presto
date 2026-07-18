using Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco;
using Data.Repository.Interfaces.Repositories.Multibanca.BBVA;
using Data.Repository.Interfaces.Repositories.Multibanca.CargaOperacionBanco;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca.BBVA;
using Multibanca.Common;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca.BBVA;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;
using System.Diagnostics;
using System.Globalization;

namespace Multibanca.Application.Implementations.Multibanca.BBVA;

public class CartaAprobacionBbvaApplication : ICartaAprobacionBbvaApplication
{
    private readonly ICartaAprobacionBbvaRepository CartaRepo;
    private readonly ICargaOperacionBancoAntecedenteCompradorRepository CompradorRepo;
    private readonly ICargaOperacionBancoAntecedenteCreditoRepository CreditoRepo;
    private readonly ICargaOperacionBancoDatosOperacionRepository DatosOpRepo;
    private readonly IExpedienteDigitalApplication ExpedienteDigitalApp;
    private readonly IConfiguration Configuration;
    private readonly ILogger<CartaAprobacionBbvaApplication> Logger;

    private const string Ciudad = "Bogotá";

    public CartaAprobacionBbvaApplication(
        ICartaAprobacionBbvaRepository cartaRepo,
        ICargaOperacionBancoAntecedenteCompradorRepository compradorRepo,
        ICargaOperacionBancoAntecedenteCreditoRepository creditoRepo,
        ICargaOperacionBancoDatosOperacionRepository datosOpRepo,
        IExpedienteDigitalApplication expedienteDigitalApp,
        IConfiguration configuration,
        ILogger<CartaAprobacionBbvaApplication> logger)
    {
        CartaRepo = cartaRepo;
        CompradorRepo = compradorRepo;
        CreditoRepo = creditoRepo;
        DatosOpRepo = datosOpRepo;
        ExpedienteDigitalApp = expedienteDigitalApp;
        Configuration = configuration;
        Logger = logger;
    }

    public async Task<carta_aprobacion_bbva?> GetByExpediente(long idExpediente)
    {
        return await CartaRepo.GetByExpediente(idExpediente);
    }

    public async Task<List<carta_aprobacion_bbva>> GetHistorico(long idExpediente)
    {
        return await CartaRepo.GetHistoricoByExpediente(idExpediente);
    }

    public async Task<(bool success, string message)> Generar(long idExpediente, int idUsuario)
    {
        carta_aprobacion_bbva? cartaExistente = null;
        var swTotal = Stopwatch.StartNew();
        var sw = Stopwatch.StartNew();

        try
        {
            cartaExistente = await CartaRepo.GetByExpediente(idExpediente);
            Logger.LogInformation("[CartaAprobacion][{Id}] GetByExpediente: {Ms}ms", idExpediente, sw.ElapsedMilliseconds); sw.Restart();

            // Datos complementarios (no bloquean si el expediente aún no los tiene)
            List<carga_operacion_banco_antecedente_comprador_entity> compradores = new();
            carga_operacion_banco_antecedente_credito_entity? credito = null;
            carga_operacion_banco_datos_operacion_entity? datosOp = null;
            try { compradores = await CompradorRepo.GetByExpediente(idExpediente); } catch { /* opcional */ }
            Logger.LogInformation("[CartaAprobacion][{Id}] CompradorRepo: {Ms}ms", idExpediente, sw.ElapsedMilliseconds); sw.Restart();
            try { credito = await CreditoRepo.GetByExpediente(idExpediente); } catch { /* opcional */ }
            Logger.LogInformation("[CartaAprobacion][{Id}] CreditoRepo: {Ms}ms", idExpediente, sw.ElapsedMilliseconds); sw.Restart();
            try { datosOp = await DatosOpRepo.GetByExpediente(idExpediente); } catch { /* opcional */ }
            Logger.LogInformation("[CartaAprobacion][{Id}] DatosOpRepo: {Ms}ms", idExpediente, sw.ElapsedMilliseconds); sw.Restart();

            int modeloCarta = cartaExistente?.modelo_carta ?? 2;
            int nuevaVersion = (cartaExistente?.version ?? 0) + 1;

            // Crear o actualizar el registro en estado PENDIENTE
            carta_aprobacion_bbva carta;
            if (cartaExistente == null)
            {
                carta = new carta_aprobacion_bbva
                {
                    id_expediente = idExpediente,
                    modelo_carta = modeloCarta,
                    id_tipo_sub_producto = credito?.id_tipo_sub_producto ?? string.Empty,
                    estado = "PENDIENTE",
                    version = nuevaVersion,
                    is_active = true,
                    row_status = true,
                    created_by = idUsuario,
                    created_date = DateTime.Now,
                };
                carta = await CartaRepo.Crear(carta);
            }
            else
            {
                cartaExistente.estado = "PENDIENTE";
                cartaExistente.version = nuevaVersion;
                cartaExistente.error_detalle = null;
                cartaExistente.modified_by = idUsuario;
                cartaExistente.modified_date = DateTime.Now;
                carta = await CartaRepo.Actualizar(cartaExistente);
            }
            Logger.LogInformation("[CartaAprobacion][{Id}] Crear/Actualizar registro PENDIENTE: {Ms}ms", idExpediente, sw.ElapsedMilliseconds); sw.Restart();

            // Construir los 18 campos del Mail Merge
            var (fieldNames, fieldValues) = ConstruirCamposMailMerge(compradores, carta, credito, datosOp);

            // Determinar nombre de plantilla
            string sufijo = modeloCarta == 1 ? "1" : "2";
            string nombrePlantilla = $"CartaAprobacion_{sufijo}.docx";
            string rutaPlantilla = ObtenerRutaPlantilla(nombrePlantilla);

            // Nombre fijo por expediente — se sobreescribe en cada generación
            string nombreArchivoBase = $"CartaAprobacion_{idExpediente}";
            string tempDocxPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.docx");

            MemoryStream docxStream;
            MemoryStream pdfStream;

            using (var fileStream = new FileStream(rutaPlantilla, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var document = new WordDocument(fileStream, FormatType.Docx))
            {
                Logger.LogInformation("[CartaAprobacion][{Id}] Abrir plantilla DOCX: {Ms}ms", idExpediente, sw.ElapsedMilliseconds); sw.Restart();

                document.MailMerge.Execute(fieldNames, fieldValues);
                Logger.LogInformation("[CartaAprobacion][{Id}] MailMerge.Execute: {Ms}ms", idExpediente, sw.ElapsedMilliseconds); sw.Restart();

                // Guardar DOCX temporal (para la conversión a PDF)
                using (var tempStream = new FileStream(tempDocxPath, FileMode.Create, FileAccess.Write))
                    document.Save(tempStream, FormatType.Docx);

                // Guardar DOCX en memoria para upload
                docxStream = new MemoryStream();
                document.Save(docxStream, FormatType.Docx);
                docxStream.Position = 0;
            }
            Logger.LogInformation("[CartaAprobacion][{Id}] Guardar DOCX (temp + memoria): {Ms}ms", idExpediente, sw.ElapsedMilliseconds); sw.Restart();

            // Convertir DOCX temporal a PDF
            using (var tempReadStream = new FileStream(tempDocxPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var wordDoc = new WordDocument(tempReadStream, FormatType.Docx))
            using (var renderer = new DocIORenderer())
            {
                Logger.LogInformation("[CartaAprobacion][{Id}] Reabrir DOCX temp: {Ms}ms", idExpediente, sw.ElapsedMilliseconds); sw.Restart();

                pdfStream = new MemoryStream();
                using PdfDocument pdf = renderer.ConvertToPDF(wordDoc);
                Logger.LogInformation("[CartaAprobacion][{Id}] ConvertToPDF (DocIORenderer): {Ms}ms", idExpediente, sw.ElapsedMilliseconds); sw.Restart();

                pdf.Save(pdfStream);
                pdfStream.Position = 0;
            }
            Logger.LogInformation("[CartaAprobacion][{Id}] pdf.Save: {Ms}ms", idExpediente, sw.ElapsedMilliseconds); sw.Restart();

            // Eliminar temporal
            if (File.Exists(tempDocxPath))
                File.Delete(tempDocxPath);

            // Subir DOCX (solo almacenamiento — no se registra en expediente digital)
            string nombreDocx = $"{nombreArchivoBase}.docx";
            await ExpedienteDigitalApp.UploadToLocal(idExpediente, nombreDocx, docxStream);
            Logger.LogInformation("[CartaAprobacion][{Id}] UploadToLocal DOCX: {Ms}ms", idExpediente, sw.ElapsedMilliseconds); sw.Restart();

            // Subir PDF y registrar en expediente digital
            string nombrePdf = $"{nombreArchivoBase}.pdf";
            await ExpedienteDigitalApp.UploadToLocal(idExpediente, nombrePdf, pdfStream);
            Logger.LogInformation("[CartaAprobacion][{Id}] UploadToLocal PDF: {Ms}ms", idExpediente, sw.ElapsedMilliseconds); sw.Restart();

            await ExpedienteDigitalApp.SaveMetadataMongo(new expediente_digital
            {
                id_expediente          = idExpediente,
                id_documento           = Constants.DocumentosBBVA.CartaAprobacion,
                nombre_archivo         = nombrePdf,
                nombre_archivo_original = nombrePdf,
                comentarios            = $"Carta de Aprobación generada por sistema (v{nuevaVersion})",
            }, idUsuario);
            Logger.LogInformation("[CartaAprobacion][{Id}] SaveMetadataMongo: {Ms}ms", idExpediente, sw.ElapsedMilliseconds); sw.Restart();

            // Actualizar registro con estado GENERADO y URLs
            string baseUrl = Configuration["Server_UploadDownload:BaseUrl"] ?? string.Empty;
            carta.estado = "GENERADO";
            carta.nombre_archivo_docx = nombreDocx;
            carta.nombre_archivo_pdf = nombrePdf;
            carta.url_docx = string.IsNullOrEmpty(baseUrl)
                ? nombreDocx
                : $"{baseUrl.TrimEnd('/')}/{idExpediente}/{nombreDocx}";
            carta.url_pdf = string.IsNullOrEmpty(baseUrl)
                ? nombrePdf
                : $"{baseUrl.TrimEnd('/')}/{idExpediente}/{nombrePdf}";
            carta.error_detalle = null;
            carta.modified_by = idUsuario;
            carta.modified_date = DateTime.Now;
            await CartaRepo.Actualizar(carta);
            Logger.LogInformation("[CartaAprobacion][{Id}] Actualizar registro GENERADO: {Ms}ms", idExpediente, sw.ElapsedMilliseconds); sw.Restart();

            docxStream.Dispose();
            pdfStream.Dispose();

            Logger.LogInformation("[CartaAprobacion][{Id}] TOTAL: {Ms}ms", idExpediente, swTotal.ElapsedMilliseconds);

            return (true, "Carta de Aprobación generada correctamente. Verifique en el Expediente Digital.");
        }
        catch (Exception ex)
        {
            Logger.LogWarning("[CartaAprobacion][{Id}] Falló tras {Ms}ms: {Error}", idExpediente, swTotal.ElapsedMilliseconds, ex.Message);

            // Marcar como ERROR si el registro ya fue creado
            if (cartaExistente != null)
            {
                cartaExistente.estado = "ERROR";
                cartaExistente.error_detalle = ex.Message;
                cartaExistente.modified_date = DateTime.Now;
                await CartaRepo.Actualizar(cartaExistente);
            }

            throw new Exception($"Error al generar la Carta de Aprobación: {ex.Message}", ex);
        }
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private (string[] fieldNames, string[] fieldValues) ConstruirCamposMailMerge(
        List<carga_operacion_banco_antecedente_comprador_entity> compradores,
        carta_aprobacion_bbva carta,
        carga_operacion_banco_antecedente_credito_entity? credito,
        carga_operacion_banco_datos_operacion_entity? datosOp)
    {
        string FormatMonto(decimal? monto)
        {
            if (!monto.HasValue) return string.Empty;
            string formatted = monto.Value.ToString("#,##0", CultureInfo.InvariantCulture);
            formatted = formatted.Replace(",", "TEMP").Replace(".", ",").Replace("TEMP", ".");
            return $"$ {formatted}";
        }

        string FormatFecha(DateTime? fecha)
        {
            if (!fecha.HasValue) return string.Empty;
            return fecha.Value.ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("es-ES"));
        }

        string MontoEnLetras(decimal? monto)
        {
            if (!monto.HasValue) return string.Empty;
            return ConvertirNumeroATexto(monto.Value);
        }

        string NombreCompleto(carga_operacion_banco_antecedente_comprador_entity? c)
        {
            if (!string.IsNullOrWhiteSpace(c?.nombre_completo)) return c.nombre_completo;

            return string.Join(" ", new[] { c?.nombres, c?.apellido_paterno, c?.apellido_materno }
                .Where(s => !string.IsNullOrWhiteSpace(s)));
        }

        var activos = compradores
            .Where(c => c.is_active && c.row_status)
            .OrderBy(c => c.id_carga_operacion_banco_antecedente_comprador)
            .ToList();

        carga_operacion_banco_antecedente_comprador_entity? Titular(string tipoTitular, int fallbackIndex) =>
            activos.FirstOrDefault(c => c.tipo_titular == tipoTitular)
            ?? (activos.Count > fallbackIndex ? activos[fallbackIndex] : null);

        var t1 = Titular("T1", 0);
        var t2 = Titular("T2", 1);
        var t3 = Titular("T3", 2);
        var t4 = Titular("T4", 3);

        decimal? prestamo = credito?.prestamo_maximo;

        var fieldNames = new[]
        {
            "TITULAR1", "CEDULA1",
            "TITULAR2", "CEDULA2",
            "TITULAR3", "CEDULA3",
            "TITULAR4", "CEDULA4",
            "SCORING",
            "SUBPRODUCTO",
            "VR__EN_LETRAS_PESOS",
            "VR_PRESTAMO_PESOS",
            "FINANCIACION",
            "PLAZO",
            "VIGENCIA_INMUEBLE",
            "PROYECTO",
            "Ciudad",
            "FECHA_APROBACION"
        };

        var fieldValues = new[]
        {
            NombreCompleto(t1),
            t1?.numero_identificacion ?? string.Empty,
            NombreCompleto(t2),
            t2?.numero_identificacion ?? string.Empty,
            NombreCompleto(t3),
            t3?.numero_identificacion ?? string.Empty,
            NombreCompleto(t4),
            t4?.numero_identificacion ?? string.Empty,
            datosOp?.nro_mutuo?.ToString() ?? string.Empty,
            credito?.codigo_producto_cartera ?? string.Empty,
            MontoEnLetras(prestamo),
            FormatMonto(prestamo),
            credito?.tipo_financiamiento ?? string.Empty,
            credito?.plazo.HasValue == true ? credito.plazo.Value.ToString() : string.Empty,
            string.Empty,
            datosOp?.nombre_proyecto ?? string.Empty,
            Ciudad,
            FormatFecha(credito?.fecha_inicio)
        };

        return (fieldNames, fieldValues);
    }

    private string ObtenerRutaPlantilla(string nombrePlantilla)
    {
        string baseDir = AppContext.BaseDirectory;
        string ruta = Path.Combine(baseDir, "Areas", "FirstProcess", "Files", nombrePlantilla);

        if (!File.Exists(ruta))
            throw new FileNotFoundException($"Plantilla no encontrada: {ruta}");

        return ruta;
    }

    // ── Conversión número → letras ────────────────────────────────────

    private static string ConvertirNumeroATexto(decimal numero)
    {
        if (numero == 0) return "CERO PESOS";

        long entero = (long)Math.Floor(numero);
        int decimales = (int)Math.Round((numero - entero) * 100);

        string texto = NumeroATexto(entero).ToUpper().Trim() + " PESOS";
        if (decimales > 0)
            texto += $" CON {decimales}/100";

        return texto;
    }

    private static string NumeroATexto(long numero)
    {
        if (numero == 0) return "cero";
        if (numero < 0) return "menos " + NumeroATexto(-numero);

        string[] unidades = { "", "un", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve",
                               "diez", "once", "doce", "trece", "catorce", "quince", "dieciséis", "diecisiete",
                               "dieciocho", "diecinueve" };
        string[] decenas = { "", "diez", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };
        string[] centenas = { "", "ciento", "doscientos", "trescientos", "cuatrocientos", "quinientos",
                               "seiscientos", "setecientos", "ochocientos", "novecientos" };

        if (numero < 20) return unidades[numero];
        if (numero < 100)
        {
            string dec = decenas[numero / 10];
            long res = numero % 10;
            return res == 0 ? dec : $"{dec} y {unidades[res]}";
        }
        if (numero == 100) return "cien";
        if (numero < 1000)
        {
            string cen = centenas[numero / 100];
            long res = numero % 100;
            return res == 0 ? cen : $"{cen} {NumeroATexto(res)}";
        }
        if (numero < 2000) return "mil " + (numero % 1000 == 0 ? "" : NumeroATexto(numero % 1000)).Trim();
        if (numero < 1_000_000)
        {
            long miles = numero / 1000;
            long resto = numero % 1000;
            string milText = NumeroATexto(miles) + " mil";
            return resto == 0 ? milText : $"{milText} {NumeroATexto(resto)}";
        }
        if (numero < 2_000_000) return "un millón " + (numero % 1_000_000 == 0 ? "" : NumeroATexto(numero % 1_000_000)).Trim();
        if (numero < 1_000_000_000)
        {
            long millones = numero / 1_000_000;
            long resto = numero % 1_000_000;
            string millText = NumeroATexto(millones) + " millones";
            return resto == 0 ? millText : $"{millText} {NumeroATexto(resto)}";
        }
        if (numero < 2_000_000_000) return "un billón " + (numero % 1_000_000_000 == 0 ? "" : NumeroATexto(numero % 1_000_000_000)).Trim();

        long billones = numero / 1_000_000_000;
        long restoB = numero % 1_000_000_000;
        string billText = NumeroATexto(billones) + " billones";
        return restoB == 0 ? billText : $"{billText} {NumeroATexto(restoB)}";
    }
}
