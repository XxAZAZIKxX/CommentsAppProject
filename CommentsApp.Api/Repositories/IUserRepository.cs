using CommentsApp.Api.Data.Models;
using CommentsApp.Core.Utilities;

namespace CommentsApp.Api.Repositories;

public interface IUserRepository
{
    Task<User[]> GetUsersAsync();
    Task<Result<User>> GetUserAsync(Guid userId);
    Task<Result<User>> GetUserByUsernameAsync(string username);

    Task<User> AddUserAsync(User user);
    Task<Result<User>> UpdateUserAsync(Guid userId, Action<User> updateAction);

    Task<bool> DeleteUserAsync(Guid userId);

    Task<bool> IsUsernameAvailableAsync(string username);
    Task<bool> IsEmailAvailableAsync(string email);
}