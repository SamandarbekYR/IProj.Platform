using IProj.DataAccess.Interfaces.MessageBroker;
using IProj.DataAccess.Interfaces.Messages;
using IProj.DataAccess.Interfaces.Users;
using IProj.DataAccess.Repositories.Messages;
using IProj.DataAccess.Repositories.Users;
using IProj.Service.Services.MessageBroker;
using IProj.Web.Helpers;
using IProj.Service.Hubs;
using Serilog;
using IProj.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using IProj.Web.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddCustomDbContext(builder.Configuration);
builder.Services.AddCustomControllers();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IMessageRepository, MessageRepository>();
builder.Services.AddTransient<IRabbitMqProducer, RabbitMqProducer>();
builder.Services.AddSignalR();

builder.Services.ConfigureAuthentication();
builder.Host.ConfigureSerilog(builder.Configuration);
var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

//    // Migratsiyalarni qo'llash va ma'lumotlar bazasini yangilash
//    dbContext.Database.Migrate();
//}

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

