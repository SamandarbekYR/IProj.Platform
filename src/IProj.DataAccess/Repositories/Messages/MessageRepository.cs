using IProj.DataAccess.Data;
using IProj.DataAccess.Interfaces.Messages;
using IProj.Domain.Entities.Messages;

namespace IProj.DataAccess.Repositories.Messages;

public class MessageRepository : BaseRepository<Message>, IMessageRepository
{
    public MessageRepository(AppDbContext appDb) : base(appDb)
    { }
}
