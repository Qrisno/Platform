using Platform.Application.Repos;
using Platform.Domain.Entities.Models;

namespace Platform.Application.Services
{
    public class UserService(IUserRepository repo)
    {
        public async Task<User?> GetUser(int id)
        {
            return await repo.GetUserByIdAsync(id);
        }

        public async Task<User?> UpdateUser(User user)
        {
            return await repo.UpdateUserAsync(user);
        }
    }
}