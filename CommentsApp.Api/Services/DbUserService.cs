using CommentsApp.Api.Configs;
using CommentsApp.Api.Data.Models;
using CommentsApp.Api.Repositories;
using CommentsApp.Api.Shared.Requests;
using CommentsApp.Api.Shared.Responses;
using CommentsApp.Api.Shared.Services;
using CommentsApp.Core.Extensions;
using CommentsApp.Core.Helpers;
using CommentsApp.Core.Utilities;

namespace CommentsApp.Api.Services;

public class DbUserService(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    JwtConfig jwtConfig) : IUserService
{
    private const string BearerTokenFormat = "Bearer {0}";

    public async Task<Result<TokenResponse>> LoginAsync(LoginRequest request)
    {
        var userResult = await userRepository.GetUserByUsernameAsync(request.Username);
        if (userResult.IsFailed) return new NotImplementedException();
        var user = userResult.Value;
        var equals = user.PasswordHash.Equals(Convert.ToHexString(CryptoHelper.GetPbkdf2Bytes(request.PasswordHash)),
            StringComparison.InvariantCultureIgnoreCase);
        if (equals is false) return new NotImplementedException();

        var userId = user.Id;

        return new TokenResponse
        {
            UserId = userId,
            BearerToken = BearerTokenFormat.Format(jwtConfig.GenerateJwtToken(userId,
                await refreshTokenRepository.GenerateRefreshTokenAsync(userId)))
        };
    }

    public async Task<Result<TokenResponse>> RegisterAsync(RegisterRequest request)
    {
        if (await userRepository.IsEmailAvailableAsync(request.Email) is false) 
            return new NotImplementedException();
        if (await userRepository.IsUsernameAvailableAsync(request.Username) is false) 
            return new NotImplementedException();

        var user = await userRepository.AddUserAsync(new User()
        {
            Email = request.Email,
            Username = request.Username,
            VisibleName = request.VisibleName,
            PasswordHash = Convert.ToHexString(CryptoHelper.GetPbkdf2Bytes(request.PasswordHash)),
        });

        var userId = user.Id;
        return new TokenResponse()
        {
            UserId = userId,
            BearerToken = BearerTokenFormat.Format(jwtConfig.GenerateJwtToken(userId,
                await refreshTokenRepository.GenerateRefreshTokenAsync(userId)))
        };
    }
}