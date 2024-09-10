using IProj.DataAccess.Data;
using IProj.DataAccess.Interfaces.Messages;
using IProj.Domain.Entities.Messages;
using Microsoft.EntityFrameworkCore;

namespace IProj.DataAccess.Repositories.Messages;

public class MessageRepository : BaseRepository<Message>, IMessageRepository
{
    private DbSet<Message> _messages;
    private AppDbContext _appDbContext;
    public MessageRepository(AppDbContext appDb) : base(appDb)
    {
        _messages = appDb.Set<Message>();
        _appDbContext = appDb;
    }
    public async Task<Message?> UpdateMessageStatus(Guid messageId, bool IsRead)
    {
        var message = await _messages.FirstOrDefaultAsync(i => i.Id == messageId);

        if (message is not null)
        {
            if (message.IsRead != IsRead)
            {
                message.IsRead = IsRead;
                message.ReadTime = DateTime.UtcNow.AddHours(5);
                _messages.Update(message);
                await _appDbContext.SaveChangesAsync();

                return message;
            }
        }

        return null;
    }
}
