using IProj.Service.DTOs.Messages;
using IProj.Service.Interfaces.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace IProj.Service.Services.Messages;

public class SendMessageToEmailService : ISendMessageToEmailService
{
    private readonly SMTPSettings _smtpSettings;
    private readonly ILogger<SendMessageToEmailService> _logger;

    public SendMessageToEmailService(IOptions<SMTPSettings> smtpSettings,
                                     ILogger<SendMessageToEmailService> logger)
    {
        _smtpSettings = smtpSettings.Value;
        _logger = logger;
    }
    public async Task SendEmailAsync(SendMessageToEmailDto messageDto)
    {
        try
        {
            var mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse(_smtpSettings.Username));
            mail.To.Add(MailboxAddress.Parse(messageDto.To));
            mail.Subject = messageDto.Title;
            mail.Body = new TextPart(TextFormat.Html)
            {
                Text = messageDto.Body
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
            await smtp.SendAsync(mail);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Habar jo'natishda xatolik yuz berdi:  {ex}");
        }
    }
}
