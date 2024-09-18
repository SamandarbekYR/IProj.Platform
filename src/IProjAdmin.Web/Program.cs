using IProj.DataAccess.Interfaces.MessageBroker;
using IProj.DataAccess.Interfaces.Messages;
using IProj.DataAccess.Interfaces.Users;
using IProj.DataAccess.Repositories.Messages;
using IProj.DataAccess.Repositories.Users;
using IProj.Service.Hubs;
using IProj.Service.Services.MessageBroker;
using IProjAdmin.Web.Configurations;
using IProjAdmin.Web.Helpers;
using IProjAdmin.Web.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddCustomDbContext(builder.Configuration);
builder.Services.AddCustomControllers();
builder.Services.AddSignalR();
builder.Services.ConfigureCors();
builder.Services.ConfigureAuthentication();
builder.Host.ConfigureSerilog(builder.Configuration);
ServiceConfig.AddCustomServices(builder.Services);

builder.Services.Configure<AppSettings>
    (builder.Configuration.GetSection("AppSettings"));

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
app.UseRouting();
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.MapHub<NotificationHub>("/notificationHub").RequireCors("AllowAllOrigins");
app.Run();
