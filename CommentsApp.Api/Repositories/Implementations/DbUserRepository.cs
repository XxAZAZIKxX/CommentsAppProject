using CommentsApp.Api.Data;
using CommentsApp.Api.Data.Models;
using CommentsApp.Core.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CommentsApp.Api.Repositories.Implementations;

public class DbUserRepository(DataContext dataContext) : IUserRepository
{
    public async Task<User[]> GetUsersAsync()
    {
        return await dataContext.Users
            .AsNoTracking()
            .ToArrayAsync();
    }

    public async Task<Result<User>> GetUserAsync(Guid userId)
    {
        var user = await dataContext.Users.SingleOrDefaultAsync(p=>p.Id == userId);
        if (user is null) return new NotImplementedException();
        return user;
    }

    public async Task<Result<User>> GetUserByUsernameAsync(string username)
    {
        var user = await dataContext.Users.SingleOrDefaultAsync(p => p.Username == username);
        if (user is null) return new NotImplementedException();
        return user;
    }

    public async Task<User> AddUserAsync(User user)
    {
        var entry = await dataContext.Users.AddAsync(user);
        await dataContext.SaveChangesAsync();
        return entry.Entity;    
    }

    public async Task<Result<User>> UpdateUserAsync(Guid userId, Action<User> updateAction)
    {
        var userResult = await GetUserAsync(userId);
        if (userResult.IsFailed) return userResult.Exception;
        var user = userResult.Value;
        updateAction(user);
        await dataContext.SaveChangesAsync();
        return user;
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        var userResult = await GetUserAsync(userId);
        if (userResult.IsFailed) return false;
        dataContext.Users.Remove(userResult.Value);
        await dataContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsUsernameAvailableAsync(string username)
    {
        return await dataContext.Users.AllAsync(p => p.Username != username);
    }

    public async Task<bool> IsEmailAvailableAsync(string email)
    {
        return await dataContext.Users.AllAsync(p => p.Email != email);
    }
}