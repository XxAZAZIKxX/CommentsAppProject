using CommentsApp.Api.Shared.Requests;
using CommentsApp.Api.Shared.Responses;
using CommentsApp.Core.Utilities;

namespace CommentsApp.Api.Shared.Services;

public interface IUserService
{
    Task<Result<TokenResponse>> LoginAsync(LoginRequest request);
    Task<Result<TokenResponse>> RegisterAsync(RegisterRequest request);
}