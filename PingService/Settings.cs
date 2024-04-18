namespace PingService;

public sealed class ServiceSettings
{
    public required string Host { get; set; } = null!;
}

public sealed class ConnectionStringSettings
{
    public required string ConnectionString { get; set; } = null!;
}

public sealed class Settings
{
    public Dictionary<string, ServiceSettings>? Services { get; set; } = null!;
    
    public Dictionary<string, ConnectionStringSettings>? ConnectionStrings { get; set; } = null!;
}