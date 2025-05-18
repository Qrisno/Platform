using Platform.Application.DTOs;
using Platform.Application.Models;

namespace Platform.Application.Repos
{
    public interface IAuthRepository
    {
        Task<LoginResponse> LoginAsync(LoginDTO user);
        Task<RegistrationResponse> RegisterAsync(UserToRegisterDTO user);
    }
}