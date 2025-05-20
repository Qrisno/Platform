using Platform.Application.DTOs;
using Platform.Application.Models;

namespace Platform.Application.Models;

    public interface IAuthService
    {
        public Task<LoginResponse> Login(LoginDTO loginData);
        public Task<RegistrationResponse> Register(UserToRegisterDTO userToRegister);
    }
