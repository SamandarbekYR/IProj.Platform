using IProj.Domain.Entities.Messages;
using IProj.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace IProj.DataAccess.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>()
            .Property(m => m.Id)
            .ValueGeneratedNever();
    }

    DbSet<User> users { get; set; }
    DbSet<Message> messages { get; set; }
}
