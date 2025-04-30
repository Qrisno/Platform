using Platform.Domain.Entities.Models;

namespace Platform.Application.Repos;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(int id);
    Task<bool> DeleteUserAsync(int id);
    Task<User?> UpdateUserAsync(User user);
}
