using EasyNetQ;
using PingService;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration.GetSection("Settings").Get<Settings>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddSingleton(config);
builder.Services.AddSingleton(new MessageClient(RabbitHutch.CreateBus("host=rabbitmq;port=5672;virtualHost=/;username=guest;password=guest")));
builder.Services.AddHostedService<MessageHandler>(sp => new MessageHandler(config));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
