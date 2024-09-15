namespace IProj.Service.DTOs.Messages;

public class SendMessageToUserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
}
