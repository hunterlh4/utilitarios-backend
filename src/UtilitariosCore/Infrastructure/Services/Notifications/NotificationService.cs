using UtilitariosCore.Domain.Dtos.Queue;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Infrastructure.Queue;
using UtilitariosCore.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace UtilitariosCore.Infrastructure.Services.Notifications;

public class NotificationService : INotificationService
{
    private readonly AzureQueueClient _azureQueueClient;
    private readonly AzureQueueSettings _queueSettings;
    private readonly SystemSettings _systemSettings;

    public NotificationService(
        AzureQueueClient azureQueueClient,
        IOptions<AzureQueueSettings> queueSettings,
        IOptions<SystemSettings> systemSettings)
    {
        _azureQueueClient = azureQueueClient;
        _queueSettings = queueSettings.Value;
        _systemSettings = systemSettings.Value;
    }

    /// <summary>
    /// Enviar mensaje de WhatsApp a un contacto
    /// </summary>
    public async Task SendWhatsAppMessageAsync(string codigoPais, string numeroCelular, string mensaje)
    {
        if (string.IsNullOrWhiteSpace(numeroCelular) || string.IsNullOrWhiteSpace(mensaje))
            return;

        WhatsAppQueueDto whatsAppMessage = new()
        {
            IdSistema = _systemSettings.IdSistemaNotificaciones,
            Mensaje = new WhatsAppMessageDto
            {
                CodigoPais = codigoPais,
                NumeroCelular = numeroCelular,
                Mensaje = mensaje
            }
        };

        _ = Task.Run(() => _azureQueueClient.SendMessageAsync(_queueSettings.Queues.WhatsApp.Name, whatsAppMessage));
    }

    /// <summary>
    /// Enviar mensaje de WhatsApp a múltiples contactos
    /// </summary>
    public async Task SendWhatsAppMessageAsync(List<(string CodigoPais, string Telefono)> contactos, string mensaje)
    {
        foreach (var contacto in contactos)
        {
            await SendWhatsAppMessageAsync(contacto.CodigoPais, contacto.Telefono, mensaje);
        }
    }

    /// <summary>
    /// Enviar correo electrónico
    /// </summary>
    public async Task SendEmailAsync(List<string> destinatarios, string asunto, string cuerpo)
    {
        if (destinatarios == null || !destinatarios.Any() || string.IsNullOrWhiteSpace(asunto) || string.IsNullOrWhiteSpace(cuerpo))
            return;

        EmailQueueDto emailMessage = new()
        {
            IdSistema = _systemSettings.IdSistemaNotificaciones,
            Mensaje = new EmailQueueContentDto
            {
                Destinatarios = destinatarios,
                EsHtml = true,
                Asunto = asunto,
                Cuerpo = cuerpo
            }
        };

        _ = Task.Run(() => _azureQueueClient.SendMessageAsync(_queueSettings.Queues.Correo.Name, emailMessage));
    }
}
