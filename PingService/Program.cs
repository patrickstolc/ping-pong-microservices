using EasyNetQ;
using Newtonsoft.Json.Linq;
using PingService;
using Vault;
using Vault.Model;
using Vault.Client;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration.GetSection("Settings").Get<Settings>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddSingleton(
    (serviceProvider) =>
    {
        VaultConfiguration vaultConfig = new VaultConfiguration(config.VaultHostname);
        VaultClient vaultClient = new VaultClient(vaultConfig);

        var authResponse = vaultClient.Auth.UserpassLogin("pingpongvault", new UserpassLoginRequest("UserPass"));
        vaultClient.SetToken(authResponse.ResponseAuth.ClientToken);

        VaultResponse<KvV2ReadResponse> response = vaultClient.Secrets.KvV2Read("secret", "pingpong");
        JObject data = (JObject)response.Data.Data;
        
        Console.WriteLine(data.ToString());
        
        SecretSettings secretSettings = data.ToObject<SecretSettings>();

        return secretSettings;
    }    
);
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

Console.WriteLine("Ping Service is running...");
app.Run();