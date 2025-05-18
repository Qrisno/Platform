using Platform.Application.Repos;
using Platform.Domain.Entities.Models;
using Platform.Infrastructure.Interfaces;

namespace Platform.Infrastructure
{
    public class UserRepository(IPlatformDbContext dbContext) : IUserRepository
    {
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await dbContext.Users.FindAsync(id);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            User? user = await GetUserByIdAsync(id);

            if (user == null)
            {
                return false;
            }

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<User?> UpdateUserAsync(User user)
        {
            User? userToUpdate = await GetUserByIdAsync(user.UserId);
            if (userToUpdate == null)
            {
                return null;
            }

            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
            return user;
        }
    }
}