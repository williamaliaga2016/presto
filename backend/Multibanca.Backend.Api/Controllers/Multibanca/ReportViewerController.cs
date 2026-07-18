using BoldReports.Web;
using BoldReports.Web.ReportViewer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportViewerController : Controller, IReportController
    {
        private readonly IMemoryCache _cache;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public ReportViewerController(
            IMemoryCache memoryCache,
            IWebHostEnvironment hostingEnvironment,
            IConfiguration configuration)
        {
            _cache = memoryCache;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        [HttpPost]
        public object PostReportAction([FromBody] Dictionary<string, object> jsonArray)
        {
            return ReportHelper.ProcessReport(jsonArray, this, _cache);
        }

        [HttpPost]
        public object PostFormReportAction()
        {
            return ReportHelper.ProcessReport(null, this, _cache);
        }

        [ActionName("GetResource")]
        [AcceptVerbs("GET")]
        public object GetResource(ReportResource resource)
        {
            return ReportHelper.GetResource(resource, this, _cache);
        }

        [NonAction]
        public void OnInitReportOptions(ReportViewerOptions reportOption)
        {
            string reportPath = reportOption.ReportModel.ReportPath ?? string.Empty;
            string reportFileName = Path.GetFileName(reportPath);

            if (string.IsNullOrWhiteSpace(reportFileName))
            {
                throw new FileNotFoundException("No se recibió el nombre del reporte RDL.");
            }

            string reportsPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Reports");
            string fullReportPath = Path.Combine(reportsPath, reportFileName);

            if (!System.IO.File.Exists(fullReportPath))
            {
                throw new FileNotFoundException($"No se encontró el reporte: {reportFileName}", fullReportPath);
            }

            using FileStream inputStream = new FileStream(
                fullReportPath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read
            );

            MemoryStream reportStream = new MemoryStream();
            inputStream.CopyTo(reportStream);
            reportStream.Position = 0;

            reportOption.ReportModel.Stream = reportStream;

            SetDataSourceCredentials(reportOption);
        }

        [NonAction]
        public void OnReportLoaded(ReportViewerOptions reportOption)
        {
            SetDataSourceCredentials(reportOption);
        }

        private void SetDataSourceCredentials(ReportViewerOptions reportOption)
        {
            string connectionString = _configuration.GetConnectionString("multibanca_odbc")
                ?? throw new InvalidOperationException("No existe la cadena de conexión 'multibanca_odbc'.");

            string user = _configuration["ReportDatabase:User"]
                ?? throw new InvalidOperationException("No existe ReportDatabase:User.");

            string password = _configuration["ReportDatabase:Password"]
                ?? throw new InvalidOperationException("No existe ReportDatabase:Password.");

            reportOption.ReportModel.DataSourceCredentials = new List<DataSourceCredentials>
            {
                new DataSourceCredentials
                {
                    Name = "DataSource1",
                    ConnectionString = connectionString,
                    UserId = user,
                    Password = password,
                    IntegratedSecurity = false
                }
            };
        }

        [HttpGet]
        public IActionResult TestOdbc()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("multibanca_odbc")
                    ?? throw new InvalidOperationException("No existe la cadena de conexión 'multibanca_odbc'.");

                using System.Data.Odbc.OdbcConnection connection = new System.Data.Odbc.OdbcConnection(connectionString);
                connection.Open();

                using System.Data.Odbc.OdbcCommand command = new System.Data.Odbc.OdbcCommand("SELECT 1", connection);
                object result = command.ExecuteScalar();

                return Ok(new
                {
                    status = true,
                    message = "ODBC conectado correctamente",
                    result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = false,
                    message = ex.Message,
                    detail = ex.ToString()
                });
            }
        }
    }
}