using CommentsApp.Api.Core.Extensions;

namespace CommentsApp.Api.Configs;

public sealed class DatabaseConfig(IConfiguration configuration)
{
    public string Server { get; } = configuration.GetValueOrThrow("Database:Server");
    public string User { get; } = configuration.GetValueOrThrow("Database:User");
    public string Password { get; } = configuration.GetValueOrThrow("Database:Password");
    public string Database { get; } = configuration.GetValueOrThrow("Database:Database");
}