using IProj.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace IProj.Consumer.Configurations;

public static class CongiguretionDatabase
{
    public static void AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connection = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connection);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
    }
}
