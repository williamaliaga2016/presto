using System.Threading.Tasks;

namespace Multibanca.Application.Interfaces.FuncTransversal
{
    /// <summary>
    /// Capacidad de notificación por correo. Actualmente no existe infraestructura SMTP
    /// en el backend (ver BBV-107_RECETA_IMPLEMENTACION.md sección 0.2): la implementación
    /// registrada hoy solo deja constancia en log. Cuando se disponga de credenciales SMTP,
    /// se reemplaza la implementación sin modificar a quienes la invocan.
    /// </summary>
    public interface IEmailNotificationApplication
    {
        Task EnviarNotificacionCobertura(long idExpediente, string correoDestino, int userId);
    }
}
