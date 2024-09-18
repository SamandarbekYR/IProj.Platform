namespace IProjAdmin.Web.Configurations;

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
                                 "https://iproj.uz/Accaunt/Login",
                                 "https://admin.iproj.uz/Home/Index",
                                 "https://admin.iproj.uz/Admin/SendMessage",
                                 "https://iproj.uz/Messages/Worker",
                                 "https://localhost:7101/Admin/SendMessage",
                                 "https://localhost:7101/Home/Index",
                                 "https://localhost:7071/Messages/Worker",
                                 "https://localhost:7071/Accaunt/Login")
                    .AllowCredentials()
                    .SetIsOriginAllowedToAllowWildcardSubdomains();
                    });
        });
    }
}
