using IProj.Service.DTOs.Messages;

namespace IProj.Service.Interfaces.Messages;

public interface ISendMessageToEmailService
{
    Task SendEmailAsync(SendMessageToEmailDto messageDto);
}
