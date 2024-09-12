using IProj.Consumer.Configurations;
using IProj.DataAccess.Interfaces.Messages;
using IProj.DataAccess.Repositories.Messages;
using IProj.Service.DTOs.Messages;
using IProj.Service.Interfaces.Messages;
using IProj.Service.Services.MessageBroker;
using IProj.Service.Services.Messages;
using Serilog.Events;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IMessageRepository, MessageRepository>();
builder.Services.AddTransient<ISendMessageToEmailService, SendMessageToEmailService>();
builder.Services.AddHostedService<RabbitMqConsumer>();
builder.Services.Configure<SMTPSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddCustomDbContext(builder.Configuration);
var logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
        .WriteTo.Console()
        .CreateLogger();
builder.Host.UseSerilog(logger);
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
