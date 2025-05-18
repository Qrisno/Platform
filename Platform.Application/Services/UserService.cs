using Platform.Application.Repos;
using Platform.Domain.Entities.Models;

namespace Platform.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<User?> GetUser(int id)
        {
            return await _repo.GetUserByIdAsync(id);
        }

        public async Task<User?> UpdateUser(User user)
        {
            return await _repo.UpdateUserAsync(user);
        }
    }
}