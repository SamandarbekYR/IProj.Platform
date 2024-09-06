using IProj.Domain.Entities.Messages;
using IProj.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace IProj.DataAccess.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    DbSet<User> Users { get; set; }
    DbSet<Message> Messages { get; set; }
}
