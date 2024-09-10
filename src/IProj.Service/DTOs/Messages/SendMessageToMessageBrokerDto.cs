namespace IProj.Service.DTOs.Messages;

public class SendMessageToMessageBroker 
{
    public Guid MessageId { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public string ReceiverGmail { get; set; } = string.Empty;
    public string MessageContent { get; set; } = string.Empty;
    public DateTime SendTime { get; set; }
    public DateTime? ReadTime { get; set; }
    public bool? IsRead { get; set; }
}
