using IProj.DataAccess.Interfaces.MessageBroker;
using IProj.DataAccess.Interfaces.Messages;
using IProj.DataAccess.Interfaces.Users;
using IProj.DataAccess.Repositories.Messages;
using IProj.DataAccess.Repositories.Users;
using IProj.Service.Services.MessageBroker;

namespace IProj.Web.Configurations;

public static class ServiceConfig
{
    public static void AddCustomServices(IServiceCollection services)
    {
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IMessageRepository, MessageRepository>();
    }
}
