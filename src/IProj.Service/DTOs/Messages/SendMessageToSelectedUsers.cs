namespace IProj.Service.DTOs.Messages
{
    public class SendMessageToSelectedUsers
    {
        public Guid MessageId { get; set; }
        public string MessageContent { get; set; } = string.Empty;
    }
}
