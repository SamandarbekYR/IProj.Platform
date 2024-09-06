using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
    options.DefaultSignOutScheme = "Cookies";
}).AddCookie("Cookies")
  .AddOpenIdConnect("oidc", options =>
  {
      options.Authority = "http://192.168.0.52:8888";
      options.ClientId = "oidcMVCApp";
      options.ClientSecret = "Wabase";
      options.ResponseType = "code";
      options.UsePkce = true;
      options.ResponseMode = "query";
      options.Scope.Add("weatherApi.read");
      options.Scope.Add("openid");
      options.Scope.Add("profile");
      options.Scope.Add("email");
      options.Scope.Add("role");
      options.GetClaimsFromUserInfoEndpoint = true;
      options.RequireHttpsMetadata = false;
      options.SaveTokens = true;
      options.CallbackPath = "/signin-oidc";
  }); 

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSerilogRequestLogging();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
