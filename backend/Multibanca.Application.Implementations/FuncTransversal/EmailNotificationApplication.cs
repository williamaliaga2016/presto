using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Multibanca.Application.Interfaces.FuncTransversal;

namespace Multibanca.Application.Implementations.FuncTransversal
{
    /// <summary>
    /// Implementación stub: no existe infraestructura SMTP en el backend todavía.
    /// Solo deja constancia en log del intento de notificación (ver sección 0.2 de
    /// BBV-107_RECETA_IMPLEMENTACION.md). Reemplazar por un envío SMTP real cuando
    /// existan credenciales/config, sin tocar el código que invoca esta interfaz.
    /// </summary>
    public class EmailNotificationApplication : IEmailNotificationApplication
    {
        private readonly ILogger<EmailNotificationApplication> Logger;

        public EmailNotificationApplication(ILogger<EmailNotificationApplication> logger)
        {
            Logger = logger;
        }

        public Task EnviarNotificacionCobertura(long idExpediente, string correoDestino, int userId)
        {
            Logger.LogInformation(
                "[EmailNotificationApplication] Notificación de cobertura simulada (sin envío SMTP real). " +
                "Expediente={IdExpediente}, Destino={Correo}, Usuario={UserId}",
                idExpediente, correoDestino, userId);

            return Task.CompletedTask;
        }
    }
}
