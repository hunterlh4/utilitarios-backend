namespace BackofficeCore.Domain.Interfaces;

public interface INotificationService
{
    Task SendWhatsAppMessageAsync(string codigoPais, string numeroCelular, string mensaje);
    Task SendWhatsAppMessageAsync(List<(string CodigoPais, string Telefono)> contactos, string mensaje);
    Task SendEmailAsync(List<string> destinatarios, string asunto, string cuerpo);
}
