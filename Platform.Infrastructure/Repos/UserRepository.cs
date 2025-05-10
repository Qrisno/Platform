using Platform.Application.Repos;
using Platform.Domain.Entities.Models;

namespace Platform.Infrastructure;

public class UserRepository : IUserRepository
{

    private readonly PlatformDbContext _dbContext;

    public UserRepository(PlatformDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _dbContext.Users.FindAsync(id);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await GetUserByIdAsync(id);

        if (user == null)
        {
            return false;
        }
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<User?> UpdateUserAsync(User user)
    {
        var userToUpdate = await GetUserByIdAsync(user.UserId);
        if (userToUpdate == null)
        {
            return null;
        }
        _dbContext.Users.Update(userToUpdate);
        await _dbContext.SaveChangesAsync();
        return userToUpdate;
    }


}