namespace CommentsApp.Api.Repositories;

public interface IRefreshTokenRepository
{
    Task<string> GenerateRefreshTokenAsync(Guid userId);
    Task<bool> CheckRefreshTokenAsync(Guid userId, string refreshToken);
}