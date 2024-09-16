using IProj.DataAccess.Interfaces.MessageBroker;
using IProj.DataAccess.Interfaces.Messages;
using IProj.DataAccess.Interfaces.Users;
using IProj.DataAccess.Repositories.Messages;
using IProj.DataAccess.Repositories.Users;
using IProj.Service.Hubs;
using IProj.Service.Services.MessageBroker;
using IProjAdmin.Web.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddCustomDbContext(builder.Configuration);
builder.Services.AddCustomControllers();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IMessageRepository, MessageRepository>();
builder.Services.AddTransient<IRabbitMqProducer, RabbitMqProducer>();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
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
                         "https://iproj.uz/Messages/Worker"
                          )
            .AllowCredentials()
            .SetIsOriginAllowedToAllowWildcardSubdomains();
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
    options.DefaultSignOutScheme = "Cookies";
}).AddCookie("Cookies")
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

var app = builder.Build();

// HTTP so'rovlarini sozlash
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.Use((context, next) =>
{
    context.Request.Scheme = "https"; return next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.MapHub<NotificationHub>("/notificationHub").RequireCors("AllowAllOrigins");

app.Run();
