using IProj.DataAccess.Data;
using IProj.DataAccess.Interfaces.Users;
using IProj.Domain.Entities.Users;

namespace IProj.DataAccess.Repositories.Users;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext appDb) : base(appDb)
    { }
}
