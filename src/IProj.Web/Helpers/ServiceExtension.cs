using IProj.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
namespace IProj.Web.Helpers
{
    public static class ServiceExtension
    {
        public static void AddCustomControllers(this IServiceCollection services)
        {
            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    });
        }
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
}
