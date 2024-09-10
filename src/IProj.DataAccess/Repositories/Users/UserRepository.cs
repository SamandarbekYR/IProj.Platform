using IProj.DataAccess.Data;
using IProj.DataAccess.Interfaces.Users;
using IProj.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace IProj.DataAccess.Repositories.Users;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private AppDbContext _appDbContext;
    private DbSet<User> _users;
    public UserRepository(AppDbContext appDb) : base(appDb)
    {
        _appDbContext = appDb;
        _users = appDb.Set<User>();
    }


    public async Task<User?> UpdateUserIsOnline(string gmail, bool isOnline)
    {
        if (string.IsNullOrEmpty(gmail))
            return null;

        var user = await _users.FirstOrDefaultAsync(g => g.Gmail == gmail);

        if (user == null)
            return null;

        if (user.IsOnline != isOnline)
        {
            user.IsOnline = isOnline;
            await _appDbContext.SaveChangesAsync();
        }

        return user;
    }
}
