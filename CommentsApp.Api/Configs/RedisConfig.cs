using System.Net;
using CommentsApp.Api.Core.Extensions;
using Microsoft.AspNetCore.Connections;
using StackExchange.Redis;

namespace CommentsApp.Api.Configs;

public sealed class RedisConfig
{
    public EndPointCollection EndPoints { get; }
    public TimeSpan Lifetime { get; }
    public RedisConfig(IConfiguration configuration)
    {
        var endPoints = configuration.GetValueOrThrow<string[]>("Redis:EndPoints");
        EndPoints = new EndPointCollection(endPoints.Select(p => new DnsEndPoint(p, 6379)).ToArray<EndPoint>());
        Lifetime = TimeSpan.FromMinutes(configuration.GetValueOrThrow<long>("Redis:Lifetime"));
    }
}