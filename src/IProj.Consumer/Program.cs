using IProj.Consumer.Configurations;
using IProj.DataAccess.Interfaces.Messages;
using IProj.DataAccess.Repositories.Messages;
using IProj.Service.DTOs.Messages;
using IProj.Service.Interfaces.Messages;
using IProj.Service.Services.MessageBroker;
using IProj.Service.Services.Messages;

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
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
