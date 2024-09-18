using Iproj.Web.Configurations;
using IProj.Web.Configurations;
using IProj.Web.Helpers;
using IProj.Web.Models;
using Serilog;

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
app.UseSerilogRequestLogging();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Accaunt}/{action=Login}");

app.Run();

