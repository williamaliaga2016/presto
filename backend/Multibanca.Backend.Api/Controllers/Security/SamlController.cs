using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace Multibanca.Backend.Api.Controllers.Security
{
    [ApiController]
    public class SamlController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public SamlController(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        // GET /api/security/saml/metadata
        // Genera el XML de metadata del SP que se entrega al administrador del IDP (BBVA).
        [HttpGet("/api/security/saml/metadata")]
        public IActionResult Metadata()
        {
            var samlConfig = _config.GetSection("Saml2");
            var certPath = samlConfig["SpCertificatePath"]!;
            if (!Path.IsPathRooted(certPath))
                certPath = Path.Combine(_env.ContentRootPath, certPath);
            var cert = new X509Certificate2(certPath, samlConfig["SpCertificatePassword"]!);
            var certBase64 = Convert.ToBase64String(cert.RawData);

            var sb = new StringBuilder();
            using var writer = XmlWriter.Create(sb, new XmlWriterSettings { Indent = true });

            writer.WriteStartElement("md", "EntityDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
            writer.WriteAttributeString("entityID", samlConfig["EntityId"]);
            writer.WriteAttributeString("xmlns", "ds", null, "http://www.w3.org/2000/09/xmldsig#");

            writer.WriteStartElement("md", "SPSSODescriptor", null);
            writer.WriteAttributeString("AuthnRequestsSigned", "true");
            writer.WriteAttributeString("WantAssertionsSigned", "true");
            writer.WriteAttributeString("protocolSupportEnumeration", "urn:oasis:names:tc:SAML:2.0:protocol");

            EscribirKeyDescriptor(writer, certBase64, "signing");
            EscribirKeyDescriptor(writer, certBase64, "encryption");

            writer.WriteStartElement("md", "AssertionConsumerService", null);
            writer.WriteAttributeString("Binding", "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST");
            writer.WriteAttributeString("Location", samlConfig["AssertionConsumerServiceUrl"]);
            writer.WriteAttributeString("index", "1");
            writer.WriteEndElement();

            writer.WriteEndElement(); // SPSSODescriptor
            writer.WriteEndElement(); // EntityDescriptor

            // Flush es obligatorio antes de leer el StringBuilder.
            // Con "using var" (C# 8), el Dispose ocurre al salir del método,
            // es decir DESPUÉS de que sb.ToString() ya fue evaluado.
            writer.Flush();

            return Content(sb.ToString(), "application/xml", Encoding.UTF8);
        }

        private static void EscribirKeyDescriptor(XmlWriter w, string certB64, string use)
        {
            w.WriteStartElement("md", "KeyDescriptor", null);
            w.WriteAttributeString("use", use);
            w.WriteStartElement("ds", "KeyInfo", "http://www.w3.org/2000/09/xmldsig#");
            w.WriteStartElement("ds", "X509Data", null);
            w.WriteElementString("ds", "X509Certificate", null, certB64);
            w.WriteEndElement();
            w.WriteEndElement();
            w.WriteEndElement();
        }
    }
}
