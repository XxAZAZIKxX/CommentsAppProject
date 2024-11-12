using CommentsApp.Api.Configs;
using CommentsApp.Core.Helpers;
using StackExchange.Redis;

namespace CommentsApp.Api.Repositories.Implementations;

public class DbRefreshTokenRepository(IConnectionMultiplexer redis, RedisConfig config) : IRefreshTokenRepository
{
    public async Task<string> GenerateRefreshTokenAsync(Guid userId)
    {
        var database = redis.GetDatabase(0);
        var token = CryptoHelper.GenerateRandomString();
        var b = await database.StringSetAsync($"{userId}:{token}", token, config.Lifetime);
        if (b is false) throw new Exception("Redis didn't save the token!");
        return token;
    }

    public async Task<bool> CheckRefreshTokenAsync(Guid userId, string refreshToken)
    {
        var database = redis.GetDatabase(0);
        var value = await database.StringGetAsync($"{userId}:{refreshToken}");
        return value.HasValue;
    }
}