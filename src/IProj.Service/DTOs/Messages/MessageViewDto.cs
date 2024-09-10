namespace IProj.Service.DTOs.Messages
{
    public class MessageViewDto
    {
        public string ReceiveName { get; set; } = string.Empty;
        public string MessageContent { get; set; } = string.Empty;
        public DateTime SendTime { get; set; }
        public DateTime? ReadTime { get; set; }
        public bool? ReadStatus { get; set; }
    }
}
