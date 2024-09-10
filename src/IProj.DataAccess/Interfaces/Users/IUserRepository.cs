using IProj.Domain.Entities.Users;

namespace IProj.DataAccess.Interfaces.Users;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> UpdateUserIsOnline(string gmail, bool isOnline);
}
