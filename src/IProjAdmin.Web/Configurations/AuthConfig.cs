namespace IProjAdmin.Web.Configurations
{
    public static class AuthConfig
    {
        public static void ConfigureAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
                options.DefaultSignOutScheme = "Cookies";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = "https://auth.iproj.uz";
                options.ClientId = "oidcMVCAppAdmin";
                options.ClientSecret = "Wabase";
                options.ResponseType = "code";
                options.UsePkce = true;
                options.ResponseMode = "query";
                options.Scope.Add("message.read");
                options.Scope.Add("message.write");
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.Scope.Add("role");
                options.GetClaimsFromUserInfoEndpoint = true;
                options.RequireHttpsMetadata = true;
                options.SaveTokens = true;
                options.CallbackPath = "/signin-oidc";
            });
        }
    }
}
