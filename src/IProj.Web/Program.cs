using IProj.DataAccess.Interfaces.MessageBroker;
using IProj.DataAccess.Interfaces.Messages;
using IProj.DataAccess.Interfaces.Users;
using IProj.DataAccess.Repositories.Messages;
using IProj.DataAccess.Repositories.Users;
using IProj.Service.Services.MessageBroker;
using IProj.Web.Helpers;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddCustomDbContext(builder.Configuration);
builder.Services.AddCustomControllers();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IMessageRepository, MessageRepository>();
builder.Services.AddTransient<IRabbitMqProducer, RabbitMqProducer>();
builder.Services.AddSignalR();

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
      options.Authority = "https://auth.iproj.uz";
      options.ClientId = "oidcMVCApp";
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


var logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
        .WriteTo.Console()
        .CreateLogger();
builder.Host.UseSerilog(logger);



var app = builder.Build();


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
app.UseSerilogRequestLogging();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Accaunt}/{action=Login}");

//app.MapHub<NotificationHub>("/notificationHub");

app.Run();

