using IProj.Domain.Entities.Messages;

namespace IProj.DataAccess.Interfaces.Messages;

public interface IMessageRepository : IBaseRepository<Message>
{
    Task<Message?> UpdateMessageStatus(Guid messageId, bool IsRead);
}