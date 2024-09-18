using IProj.Consumer.Configurations;
using IProj.DataAccess.Interfaces.Messages;
using IProj.DataAccess.Repositories.Messages;
using IProj.Service.DTOs.Messages;
using IProj.Service.Interfaces.Messages;
using IProj.Service.Services.MessageBroker;
using IProj.Service.Services.Messages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IMessageRepository, MessageRepository>();
builder.Services.AddTransient<ISendMessageToEmailService, SendMessageToEmailService>();
builder.Services.AddHostedService<RabbitMqConsumer>();
builder.Services.Configure<SMTPSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddCustomDbContext(builder.Configuration);
builder.Host.ConfigureSerilog(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
