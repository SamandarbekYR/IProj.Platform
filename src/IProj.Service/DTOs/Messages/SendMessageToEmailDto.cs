namespace IProj.Service.DTOs.Messages;

public class SendMessageToEmailDto
{
    public string To { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
