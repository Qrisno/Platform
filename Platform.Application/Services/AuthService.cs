using Platform.Application.DTOs;
using Platform.Application.Models;
using Platform.Application.Repos;
using Platform.Domain.Entities.Models;

namespace Platform.Application.Services;

public class AuthService
{
    private IAuthRepository _repo;

    public AuthService(IAuthRepository repo)
    {
        _repo = repo;
    }


    public async Task<LoginResponse> Login(LoginDTO loginData)
    {
        var loginResult = await _repo.LoginAsync(loginData);
        return loginResult;
    }


    public async Task<RegistrationResponse> Register(UserToRegisterDTO userToRegister)
    {
        var registrationResult = await _repo.RegisterAsync(userToRegister);
        return registrationResult;
    }


}