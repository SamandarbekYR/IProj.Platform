namespace IProj.Web.Configurations
{
    public static class CorsConfig
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins("https://admin.iproj.uz",
                                     "https://iproj.uz",
                                     "https://admin.iproj.uz/Admin/SendMessage",
                                     "https://iproj.uz/Messages/Worker")
                        .AllowCredentials()
                        .SetIsOriginAllowedToAllowWildcardSubdomains();
                });
            });
        }
    }
}
