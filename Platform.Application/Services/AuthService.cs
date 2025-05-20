using Platform.Application.DTOs;
using Platform.Application.Models;
using Platform.Application.Repos;

namespace Platform.Application.Services
{
    public class AuthService: IAuthService
    {
        private readonly IAuthRepository _repo;

        public AuthService(IAuthRepository repo)
        {
            _repo = repo;
        }


        public async Task<LoginResponse> Login(LoginDTO loginData)
        {
            LoginResponse loginResult = await _repo.LoginAsync(loginData);
            return loginResult;
        }


        public async Task<RegistrationResponse> Register(UserToRegisterDTO userToRegister)
        {
            RegistrationResponse registrationResult = await _repo.RegisterAsync(userToRegister);
            return registrationResult;
        }
    }
}