using IProj.DataAccess.Interfaces.MessageBroker;
using IProj.DataAccess.Interfaces.Messages;
using IProj.DataAccess.Interfaces.Users;
using IProj.DataAccess.Repositories.Messages;
using IProj.DataAccess.Repositories.Users;
using IProj.Service.Hubs;
using IProj.Service.Services.MessageBroker;
using IProjAdmin.Web.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddCustomDbContext(builder.Configuration);
builder.Services.AddCustomControllers();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IMessageRepository, MessageRepository>();
builder.Services.AddTransient<IRabbitMqProducer, RabbitMqProducer>();
builder.Services.AddSignalR();
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
      options.RequireHttpsMetadata = false;
      options.SaveTokens = true;
      options.CallbackPath = "/signin-oidc";
  });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=Main}");

app.MapHub<NotificationHub>("/notificationHub");

app.Run();
