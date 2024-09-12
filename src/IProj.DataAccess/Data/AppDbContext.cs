using IProj.Domain.Entities.Messages;
using IProj.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace IProj.DataAccess.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
       // Database.Migrate();
        //Database.EnsureCreated();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>()
            .Property(m => m.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<User>().HasData(
            new User { Id = Guid.Parse("53aa79f4-1650-4baa-b5c7-c7a1fd9d4128"), FirstName = "Samandarbek", RoleName = "Worker", Position = "Backend Developer", Gmail = "samandarbekyr@gmail.com", IsOnline = false },
            new User { Id = Guid.Parse("6effb728-a7cd-460c-91e5-9f048185fd11"), FirstName = "Muhammadqodir", RoleName = "Owner", Position = "Desktop Developer", Gmail = "muhammadqodir5050@gmail.com", IsOnline = false },
            new User { Id = Guid.Parse("a8f7f39e-3445-433c-93d3-e6755610a5e0"), FirstName = "Samandar", RoleName = "Worker", Position = "Full-stack Developer", Gmail = "sharpistmaster@gmail.com", IsOnline = false },
            new User { Id = Guid.Parse("aae35ced-e156-4e1d-beb0-0a5d035763e0"), FirstName = "Able", RoleName = "Worker", Position = "Devops", Gmail = "able.devops@gmail.com", IsOnline = false },
            new User { Id = Guid.Parse("dd13aea5-d3fd-4afc-8fb8-7f5940766935"), FirstName = "Behruz", RoleName = "Worker", Position = "Frontend Developer", Gmail = "uzgrandmaster@gmail.com", IsOnline = false },
            new User { Id = Guid.Parse("17820355-1ee5-49c8-9cca-ea6cfb5312ca"), FirstName = "Olim", RoleName = "Worker", Position = "Project Manager", Gmail = "olim@gmail.com", IsOnline = false }
            );
    }

    DbSet<User> users { get; set; }
    DbSet<Message> messages { get; set; }
}
